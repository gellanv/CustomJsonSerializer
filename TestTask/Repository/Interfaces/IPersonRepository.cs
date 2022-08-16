using TestTask.Data.Models;

namespace TestTask.Repository.Interfaces
{
    public interface IPersonRepository
    {
        Task<Person> CreateAsync(Person person);
        Task<Person> GetByIdAsync(long id);
        Task<IEnumerable<Person>> GetAllAsync();
        void Update(Person person);
        Task SaveAsync();
    }
}
