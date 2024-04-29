namespace BackendBookstore.DTOs.CreateDTO
{
    public class AddressCreateDto
    {
        public string Street { get; set; } = null!;

        public string City { get; set; } = null!;

        public string PostalCode { get; set; } = null!;

        public int? OrdersId { get; set; }
    }
}
