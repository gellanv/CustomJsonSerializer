using Microsoft.EntityFrameworkCore;
using TestTask.Data.Models;
using TestTask.Models;
using TestTask.Repository.Interfaces;

namespace TestTask.Repository
{
    public class AddressRepository : IAddressRepository
    {
        private readonly ApiDBContext context;

        public AddressRepository(ApiDBContext context)
        {
            this.context = context;
        }

        public async Task<Address> CreateAsync(Address address)
        {
            await context.Address.AddAsync(address);
            return address;
        }

        public async Task<Address> GetByIdAsync(long id)
        {
            var address = await context.Address.FirstOrDefaultAsync(x => x.Id == id);

            return address;
        }

        public void Update(Address address)
        {
            context.Entry(address).State = EntityState.Modified;
        }

        public Task SaveAsync()
        {
            return context.SaveChangesAsync();
        }
    }
}