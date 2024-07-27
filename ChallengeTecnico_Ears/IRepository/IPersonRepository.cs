using ChallengeTecnico_Ears.Models;

namespace ChallengeTecnico_Ears.IRepository
{
    public interface IPersonRepository
    {
        Task<List<PersonModel>> GetPersonWithOffices();
    }
}
