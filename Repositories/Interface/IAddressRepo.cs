using BackendBookstore.Models;

namespace BackendBookstore.Repositories.Interface
{
    public interface IAddressRepo
    {
        void Create(Address address);
        void Update(Address address);
        void Delete(int addressId);
        Address FindAddressById(int addressId);
        IEnumerable<Address> GetAddresses();
        bool SaveChanges();
        IEnumerable<Order> GetOrdersForAddress(int addressId);


    }
}
