using ChallengeTecnico_Ears.IRepository;
using ChallengeTecnico_Ears.IService;
using ChallengeTecnico_Ears.Models;

namespace ChallengeTecnico_Ears.Services
{
    public class PersonService(IPersonRepository personRepository) : IPersonService
    {
        private readonly IPersonRepository _personRepository = personRepository;
                
        public async Task<List<PersonModel>> GetPersonList()
        {
            return await _personRepository.GetPersonWithOffices();
        }
    }
}
