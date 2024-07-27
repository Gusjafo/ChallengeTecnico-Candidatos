using ChallengeTecnico_Ears.Context;
using ChallengeTecnico_Ears.Models;
using ChallengeTecnico_Ears.Repository;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;

namespace TestChallenge
{
    public class UnitTest
    {
        private readonly ContextChallenge _context;
        private readonly PersonRepository _repository;
        private readonly Mock<ILogger<PersonRepository>> _mockLogger;

        public UnitTest()
        {
            // Configurar el DbContext en memoria
            var options = new DbContextOptionsBuilder<ContextChallenge>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

            _context = new ContextChallenge(options);
            _mockLogger = new Mock<ILogger<PersonRepository>>();
            _repository = new PersonRepository(_context, _mockLogger.Object);
        }

        [Fact]
        public async Task GetPersonWithOffices_ReturnsPersonsWithOffices_WhenDataIsAvailable()
        {
            // Arrange
            _context.T_Person.Add(new PersonModel
            {
                Active = true,
                EmployeeFile = 1004,
                Offices = new List<OfficeModel> { new OfficeModel { CompanyName = "Company1" } }
            });
            _context.SaveChanges();

            // Act
            var result = await _repository.GetPersonWithOffices();

            // Assert
            Assert.Single(result);
            Assert.Equal(1004, result.First().EmployeeFile);
            Assert.NotEmpty(result.First().Offices);
        }

        [Fact]
        public async Task GetPersonWithOffices_ReturnsEmptyList_WhenNoActivePersonsMatchCriteria()
        {
            // Arrange
            _context.T_Person.Add(new PersonModel
            {
                Active = false,
                EmployeeFile = 1004,
                Offices = new List<OfficeModel> { new OfficeModel { CompanyName = "Company1" } }
            });
            _context.SaveChanges();

            // Act
            var result = await _repository.GetPersonWithOffices();

            // Assert
            Assert.Empty(result);
        }

        [Fact]
        public async Task GetPersonWithOffices_ExcludesPersonsBelowFileNumber()
        {
            // Arrange
            _context.T_Person.AddRange(
                new PersonModel
                {
                    Active = true,
                    EmployeeFile = 1002,
                    Offices = new List<OfficeModel> { new OfficeModel { CompanyName = "Company1" } }
                },
                new PersonModel
                {
                    Active = true,
                    EmployeeFile = 1004,
                    Offices = new List<OfficeModel> { new OfficeModel { CompanyName = "Company2" } }
                },
                new PersonModel
                {
                    Active = true,
                    EmployeeFile = 1005,
                    Offices = new List<OfficeModel> { new OfficeModel { CompanyName = "Company3" } }
                }
            );
            _context.SaveChanges();

            // Act
            var result = await _repository.GetPersonWithOffices();

            // Assert
            Assert.Equal(2, result.Count());
            Assert.Equal(1004, result.First().EmployeeFile);
        }

        [Fact]
        public async Task GetPersonWithOffices_ReturnsActivePerson()
        {
            // Arrange
            _context.T_Person.AddRange(
                new PersonModel
                {
                    Active = true,
                    EmployeeFile = 1005,
                    Offices = new List<OfficeModel> { new OfficeModel { CompanyName = "Company1" } }
                },
                new PersonModel
                {
                    Active = true,
                    EmployeeFile = 1006,
                    Offices = new List<OfficeModel> { new OfficeModel { CompanyName = "Company2" } }
                },
                new PersonModel
                {
                    Active = false,
                    EmployeeFile = 1007,
                    Offices = new List<OfficeModel> { new OfficeModel { CompanyName = "Company3" } }
                },
                new PersonModel
                {
                    Active = false,
                    EmployeeFile = 1008,
                    Offices = new List<OfficeModel> { new OfficeModel { CompanyName = "Company4" } }
                }
            );
            _context.SaveChanges();

            // Act
            var result = await _repository.GetPersonWithOffices();

            // Assert
            Assert.Equal(2, result.Count);
            Assert.Contains(result, p => p.EmployeeFile == 1005);
            Assert.Contains(result, p => p.EmployeeFile == 1006);
        }

        [Fact]
        public async Task GetPersonWithOffices_ReturnsEmptyList_WhenNoPersonsExist()
        {
            // Act
            var result = await _repository.GetPersonWithOffices();

            // Assert
            Assert.Empty(result);
        }

        internal void Dispose()
        {
            _context.Database.EnsureDeleted();
            _context.Dispose();
        }


    }
}
