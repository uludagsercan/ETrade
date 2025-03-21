# NET Core ile Microservice Mimarisi ve MassTransit Kullanarak Event Tabanlı İletişim

### Genel Açıklama

Event tabanlı mimariyi kullanarak üç adet mikroservis geliştirilmiştir. Mikroservisler sırasıyla:

1.⁠ ⁠*Order Microservice*
2.⁠ ⁠*Stock Microservice*
3.⁠ ⁠*Payment Microservice*

olarak tanımlanmıştır.

RabbitMQ mesaj kuyruğu ve MassTransit kütüphanesi kullanılarak mikroservislerin haberleşmesi sağlanmıştır.

### Mikroservislerin Görevleri

### 1. Order Microservice
```csharp
        public static void AddInfrastructureService(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddMassTransit(x =>
            {
                x.AddConsumer<StockNotAvailableEventConsumer>();
                x.AddConsumer<PaymentCompletedEventConsumer>();
                x.AddConsumer<PaymentFailedEventConsumer>();
                x.UsingRabbitMq((context, cfg) =>
                {
                    cfg.Host(configuration.GetConnectionString("RabbitMQ"));
                    cfg.ReceiveEndpoint("etrade.order.stock-not-available-event-queue", e =>
                    {
                        e.ConfigureConsumer<StockNotAvailableEventConsumer>(context);

                    });
                    cfg.ReceiveEndpoint("etrade.order.payment-completed-event-queue", e =>
                    {
                        e.ConfigureConsumer<PaymentCompletedEventConsumer>(context);
                    });
                    cfg.ReceiveEndpoint("etrade.order.payment-failed-event-queue", e =>
                    {
                        e.ConfigureConsumer<PaymentFailedEventConsumer>(context);
                    });
                });
            });
```

•⁠  ⁠Sipariş oluşturulduğunda, siparişi ilk olarak durumunu "Suspend" (Askıda) olacak şekilde veritabanına kaydedildi.
```csharp
        public async Task<bool> CreateOrderAsync(CreateOrderDto createOrderDto)
        {
            var order = createOrderDto.Adapt<Order>();

            await orderWriteRepository.AddAsync(order);
            var result = await orderWriteRepository.SaveAsync();
            if (result < 0)
                return false;
            OrderCreatedEvent orderCreatedEvent = new(order.Id, order.CustomerName, order.OrderDetails.Adapt<List<BusShared.Events.OrderDetail>>());
            await busService.PublishAsync(orderCreatedEvent);
            return true;
        }
```

•⁠  ⁠Sipariş başarıyla kaydedildikten sonra, diğer mikroservislere bildirmek amacıyla "OrderCreatedEvent" isimli bir event oluşturup kuyruğa gönderildi.
```csharp
  await busService.PublishAsync(orderCreatedEvent);
```

•⁠  ⁠Payment Microservice'den gelen "PaymentCompletedEvent" eventini dinleyerek sipariş durumunu "Completed" (Tamamlandı) olarak güncellendi.
```csharp
    public class PaymentCompletedEventConsumer(IOrderService orderService) : IConsumer<PaymentCompletedEvent>
    {
        public async Task Consume(ConsumeContext<PaymentCompletedEvent> context)
        {
            await orderService.UpdateOrderStatusAsync(new UpdateOrderDto(context.Message.OrderId, OrderStatus.Completed));
        }
    }
```

### 2. Stock Microservice
```csharp
            services.AddMassTransit(x =>
            {
                x.AddConsumer<OrderCreatedEventConsumer>();
                x.UsingRabbitMq((context, cfg) =>
                {
                    cfg.Host(configuration.GetConnectionString("RabbitMQ"));
                    cfg.ReceiveEndpoint("etrade.order.order-created-event-queue", e =>
                    {
                        e.ConfigureConsumer<OrderCreatedEventConsumer>(context);
                    });
                });
            });
```
•⁠  ⁠"OrderCreatedEvent" eventini dinleyerek siparişte istenen ürünlerin stok durumları kontrol edildi.
```csharp
    public class OrderCreatedEventConsumer(IPublishEndpoint publishEndpoint) : IConsumer<OrderCreatedEvent>
    {
        public async Task Consume(ConsumeContext<OrderCreatedEvent> context)
        {
            var orderCreatedEvent = context.Message;
            var stockService = context.GetServiceOrCreateInstance<IStockService>();
            foreach (var item in orderCreatedEvent.OrderDetails)
            {
                var isStockAvailable = await stockService.IsStockAvailableAsync(item.ProductId.ToString(), item.Quantity);
                if (!isStockAvailable)
                {
                    await publishEndpoint.Publish(new StockNotAvailableEvent(orderCreatedEvent.OrderId, "Stock is not available"));
                }
                else
                {
                    await publishEndpoint.Publish(new StockReservedEvent(orderCreatedEvent.OrderId));
                }
            }
        }
    }
```
•⁠  ⁠Eğer stok mevcutsa:<br>
    - Stok miktarını güncelleyip ilgili kaydı veritabanına kaydedildi.<br>
    - Sonrasında ödeme sürecinin başlatılması amacıyla "StockReservedEvent" eventini yayınlandı.
```csharp
      await publishEndpoint.Publish(new StockReservedEvent(orderCreatedEvent.OrderId));
```

•⁠  ⁠Eğer stok yeterli değilse:<br>
    - "StockNotAvailableEvent" eventini yayınlayarak siparişin başarısız olduğunu Order Microservice'e bildirildi.<br>
```csharp
      await publishEndpoint.Publish(new StockNotAvailableEvent(orderCreatedEvent.OrderId, "Stock is not available"));
```

### 3. Payment Microservice
```csharp
            services.AddMassTransit(x =>
            {
                x.AddConsumer<StockNotAvailableEventConsumer>();
                x.AddConsumer<PaymentCompletedEventConsumer>();
                x.AddConsumer<PaymentFailedEventConsumer>();
                x.UsingRabbitMq((context, cfg) =>
                {
                    cfg.Host(configuration.GetConnectionString("RabbitMQ"));
                    cfg.ReceiveEndpoint("etrade.order.stock-not-available-event-queue", e =>
                    {
                        e.ConfigureConsumer<StockNotAvailableEventConsumer>(context);

                    });
                    cfg.ReceiveEndpoint("etrade.order.payment-completed-event-queue", e =>
                    {
                        e.ConfigureConsumer<PaymentCompletedEventConsumer>(context);
                    });
                    cfg.ReceiveEndpoint("etrade.order.payment-failed-event-queue", e =>
                    {
                        e.ConfigureConsumer<PaymentFailedEventConsumer>(context);
                    });
                });
            });
```

•⁠  ⁠"StockReservedEvent" eventini dinleyerek ödeme işlemleri gerçekleştirildi.
```csharp
    public class StockReservedEventConsumer(IBusService busService) : IConsumer<StockReservedEvent>
    {
        public async Task Consume(ConsumeContext<StockReservedEvent> context)
        {
            var stockReservedEvent = context.Message;
            //payment service will be called here
            if (true)
            {
                PaymentCompletedEvent paymentCompletedEvent = new PaymentCompletedEvent(stockReservedEvent.OrderId);
                await busService.PublishAsync(paymentCompletedEvent);
            }
            else
            {
                PaymentFailedEvent paymentFailedEvent = new PaymentFailedEvent(stockReservedEvent.OrderId, "Payment failed");
                await busService.PublishAsync(paymentFailedEvent);
            }


        }
    }
```
•⁠  ⁠Ödeme başarılı gerçekleşirse:<br>
    - "PaymentCompletedEvent" eventini göndererek Order Microservice'e siparişin başarılı tamamlandığı bildirildi.
```csharp
    public class PaymentCompletedEventConsumer(IOrderService orderService) : IConsumer<PaymentCompletedEvent>
    {
        public async Task Consume(ConsumeContext<PaymentCompletedEvent> context)
        {
            await orderService.UpdateOrderStatusAsync(new UpdateOrderDto(context.Message.OrderId, OrderStatus.Completed));
        }
    }
```
•⁠  ⁠Ödeme başarısız olursa:<br>
    - "PaymentFailedEvent" eventini göndererek siparişin başarısız olduğunu Order Microservice'e bildirildi.
```csharp
    public class PaymentFailedEventConsumer(IOrderService orderService) : IConsumer<PaymentFailedEvent>
    {
        public async Task Consume(ConsumeContext<PaymentFailedEvent> context)
        {
            await orderService.UpdateOrderStatusAsync(new UpdateOrderDto(context.Message.OrderId, OrderStatus.PaymentFailed));
        }
    }
```


