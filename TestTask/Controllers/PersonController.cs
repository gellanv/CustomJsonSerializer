using Microsoft.AspNetCore.Mvc;
using TestTask.Service;

namespace TestTask.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PersonController : ControllerBase
    {
        private readonly PersonService personService;


        public PersonController(PersonService personService)
        {
            this.personService = personService;
        }

        [HttpGet]
        public async Task<string> GetAllPersonsAsync(string? request)
        {
            var AllPerson = await personService.GetAllPersons(request);

            return AllPerson;
        }


        [HttpPost]
        public Task<long> SavePersonAsync(string json)
        {
            var result = personService.SavePersonsAsync(json);

            return result;
        }
    }
}
