using ChallengeTecnico_Ears.IService;
using ChallengeTecnico_Ears.Models;
using Microsoft.AspNetCore.Mvc;

namespace ChallengeTecnico_Ears.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PersonController(IPersonService personService, ILogger<PersonController> logger) : ControllerBase
    {
        private readonly IPersonService _personService = personService;
        private readonly ILogger<PersonController> _logger = logger;

        [HttpGet]
        public async Task<ActionResult<List<PersonModel>>> Get()
        {
            _logger.LogInformation("Getting list of persons");
            List<PersonModel> persons;

            try
            {
                persons = await _personService.GetPersonList();

            }
            catch (Exception ex)
            {
                return StatusCode( 500, new { message = "An error occurred while obtaining the model.", detail = ex.Message });
            }           

            if (persons.Count == 0)
            {
                _logger.LogWarning("List of persons is empty");
                return NotFound();
            }            

            return Ok(persons);
        }
    }
}
