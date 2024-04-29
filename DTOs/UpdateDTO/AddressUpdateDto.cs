namespace BackendBookstore.DTOs.UpdateDTO
{
    public class AddressUpdateDto
    {
        public int AddressId { get; set; }

        public string Street { get; set; } = null!;

        public string City { get; set; } = null!;

        public string PostalCode { get; set; } = null!;

        public int? OrdersId { get; set; }
    }
}
