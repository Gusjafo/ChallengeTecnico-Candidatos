using ChallengeTecnico_Ears.Context;
using ChallengeTecnico_Ears.IRepository;
using ChallengeTecnico_Ears.Models;
using Microsoft.EntityFrameworkCore;

namespace ChallengeTecnico_Ears.Repository
{
    public class PersonRepository(ContextChallenge dbContext, ILogger<PersonRepository> logger) : IPersonRepository
    {
        private readonly ContextChallenge _context = dbContext;
        private readonly ILogger<PersonRepository> _logger = logger;  

        public async Task<List<PersonModel>> GetPersonWithOffices()
        {
            List<PersonModel> persons = [];

            try
            {
                persons = await _context.T_Person.Where(p => p.Active && p.EmployeeFile > 1003)
                                                 .Include(p => p.Offices)
                                                 .ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError("An error occurred while getting active persons and offices: {}", ex.Message);
                _logger.LogError("{}", ex.InnerException);
                throw;
            }

            return persons;
        }
    }
}
