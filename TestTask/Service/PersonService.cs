using System.Text.RegularExpressions;
using TestTask.Data.Models;
using TestTask.Repository.Interfaces;

namespace TestTask.Service
{
    public class PersonService
    {
        private readonly IPersonRepository personRepository;
        private readonly IAddressRepository addressRepository;
        private readonly JsonSerializerCustom<Person> jsonSerializerCustom;
        public PersonService(IPersonRepository personRepository, IAddressRepository addressRepository, JsonSerializerCustom<Person> jsonSerializerCustom)
        {
            this.personRepository = personRepository;
            this.addressRepository = addressRepository;
            this.jsonSerializerCustom = jsonSerializerCustom;
        }

        public async Task<string> GetAllPersons(string? order)
        {
            var allPerson = await personRepository.GetAllAsync();
            if (allPerson.Count() > 0)
            {
                if (order != null && Regex.Matches(order, "\":").Count >= 1)
                {
                    var dictOrder = jsonSerializerCustom.ConvertJsonToDictionary(order);
                    foreach (var pair in dictOrder)
                    {
                        if (pair.Key.ToLower() == "city")
                            allPerson = allPerson.Where(x => x.Address.City == pair.Value);
                        else if (pair.Key.ToLower() == "firstname")
                            allPerson = allPerson.Where(x => x.FirstName == pair.Value);
                        else if (pair.Key.ToLower() == "lastname")
                            allPerson = allPerson.Where(x => x.LastName == pair.Value);
                    }
                }
                string result = jsonSerializerCustom.ConvertListObjectToJson(allPerson.ToList());

                return result;

            }
            else return "No Person";
        }

        public async Task<long> SavePersonsAsync(string jsonString)
        {
            Person person = jsonSerializerCustom.ConvertFromJsonToObject(jsonString, typeof(Person)) as Person;
            if (person != null)
            {
                if (person.Address != null)
                {
                    if (person.Address.Id == 0)
                    {
                        Address address = await addressRepository.CreateAsync(person.Address);
                        await addressRepository.SaveAsync();
                        person.AddressId = address.Id;
                    }
                    else
                    {
                        Address address = await addressRepository.GetByIdAsync(person.Address.Id);
                        if (address != null)
                        {
                            address.City = person.Address.City;
                            address.AddressLine = person.Address.AddressLine;

                            addressRepository.Update(address);

                            await addressRepository.SaveAsync();

                            person.AddressId = address.Id;
                        }
                        else
                        {
                            throw new Exception("Address Id is not correct");
                        }
                    }
                }
                if (person.Id == 0)
                {
                    Person newPerson = await personRepository.CreateAsync(person);
                    await personRepository.SaveAsync();
                    person.Id = newPerson.Id;
                }
                else
                {
                    Person getPerson = await personRepository.GetByIdAsync(person.Id);
                    if (getPerson != null)
                    {
                        getPerson.FirstName = person.FirstName;
                        getPerson.LastName = person.LastName;
                        getPerson.AddressId = person.AddressId;
                        personRepository.Update(getPerson);
                        await personRepository.SaveAsync();
                    }
                    else
                    {
                        throw new Exception("Person Id is not correct");
                    }
                }
                return person.Id;
            }
            else
            {
                throw new Exception("Json string wasn't correct");
            }
        }
    }
}
