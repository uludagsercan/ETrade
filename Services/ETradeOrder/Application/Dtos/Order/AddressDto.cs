

namespace Application.Dtos.Order
{
    public record AddressDto(
        string Province,
        string District,
        string Street,
        string ZipCode,
        string Line);
}