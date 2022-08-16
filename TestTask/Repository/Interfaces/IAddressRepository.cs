using TestTask.Data.Models;

namespace TestTask.Repository.Interfaces
{
    public interface IAddressRepository
    {
        Task<Address> CreateAsync(Address address);
        Task<Address> GetByIdAsync(long id);
        void Update(Address address);
        Task SaveAsync();
    }
}
