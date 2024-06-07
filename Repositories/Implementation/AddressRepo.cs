using BackendBookstore.Models;
using BackendBookstore.Repositories.Interface;
using Microsoft.EntityFrameworkCore;

namespace BackendBookstore.Repositories.Implementation
{
    public class AddressRepo : IAddressRepo
    {
        private readonly PostgresContext _context;
        public AddressRepo(PostgresContext context)
        {
            _context = context;
        }

        public void Create(Address address)
        {
            if (address != null)
                _context.Addresses.Add(address);

        }

        public void Delete(int addressId)
        {
            var address = FindAddressById(addressId);
            if (address != null)
            {
                _context.Addresses.Remove(address);
            }
        }

        public Address FindAddressById(int addressId)
        {
            var address = _context.Addresses.FirstOrDefault(a => a.AddressId == addressId);
            if (address != null)
                return address;
            else
                throw new ArgumentException($"Address with ID {addressId} not found.", nameof(addressId));
        }

        public void Update(Address address)
        {

        }

        public IEnumerable<Address> GetAddresses()
        {
            return _context.Addresses.ToList();
        }

        public bool SaveChanges()
        { 
            return (_context.SaveChanges() >= 0);
        }

        public IEnumerable<Order> GetOrdersForAddress(int addressId)
        {
            return _context.Orders
                           .Include(o => o.Addresses)
                           .Where(o => o.Addresses.Any(a => a.AddressId == addressId))
                           .ToList();
        }

        public Address FindOrCreateAddress(string street, string city, string postalCode)
        {
            var existingAddress = _context.Addresses.FirstOrDefault(a => a.Street == street && a.City == city && a.PostalCode == postalCode);

            if (existingAddress != null)
            {
                return existingAddress;
            }
            else
            {
                var newAddress = new Address
                {
                    Street = street,
                    City = city,
                    PostalCode = postalCode
                };

                _context.Addresses.Add(newAddress);
                _context.SaveChanges();

                return newAddress;
            }
        }
    }
}
