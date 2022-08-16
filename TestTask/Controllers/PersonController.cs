using Microsoft.AspNetCore.Mvc;
using TestTask.Models;
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

            // deserialize string into object of type Person using own deserializer (see more
            // details in Minimal requirements section)
            // DO NOT use 3rd party libraries for deserialization like Json.NET or Microsoft
            // insert or update Person entity in database
            // return entity id

            //            Call Save api endpoint with next json string:
            //            {
            //            firstName: ‘Ivan’,
            //            lastName: ‘Petrov’,
            //            address:
            //                {
            //                city: ‘Kiev’,
            //                 addressLine: prospect “Peremogy” 28 / 7,
            //                }
            //            }
            //// NOTE that there is no “ (quote) char in ‘addressLine’ field value before
            //‘prospect’ and after ‘7’, so your json deserializer should handle such cases


            //Use the result of previous GetAll api for calling Save api endpoint.
        }
    }
}
