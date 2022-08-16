using Microsoft.EntityFrameworkCore;
using TestTask.Data.Models;
using TestTask.Models;
using TestTask.Repository.Interfaces;

namespace TestTask.Repository
{
    public class PersonRepository : IPersonRepository
    {
        private readonly ApiDBContext context;

        public PersonRepository(ApiDBContext _context)
        {
            context = _context;
        }
        public async Task<Person> CreateAsync(Person person)
        {
            await context.Person.AddAsync(person);
            return person;
        }

        public async Task<Person> GetByIdAsync(long id)
        {
            var person = await context.Person.FirstOrDefaultAsync(x => x.Id == id);

            return person;
        }

        public async Task<IEnumerable<Person>> GetAllAsync()
        {
            return context.Person.Select(person => new Person
            {
                Id = person.Id,
                FirstName = person.FirstName,
                LastName = person.LastName,
                Address = person.Address,
            }).ToListAsync().Result;
        }

        public void Update(Person person)
        {
            context.Entry(person).State = EntityState.Modified;
        }
        public Task SaveAsync()
        {
            return context.SaveChangesAsync();
        }
    }
}
