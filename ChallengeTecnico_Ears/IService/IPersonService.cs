using ChallengeTecnico_Ears.Models;
using System.Net;
namespace ChallengeTecnico_Ears.IService
{
    public interface IPersonService
    {
        Task<List<PersonModel>> GetPersonList();
    }
}
