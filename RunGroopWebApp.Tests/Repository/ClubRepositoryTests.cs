using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using RunGroopWebApp.Data;
using RunGroopWebApp.Data.Enum;
using RunGroopWebApp.Models;
using RunGroopWebApp.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RunGroopWebApp.Tests.Repository
{
    public class ClubRepositoryTests

    {
        private async Task<ApplicationDbContext> GetDbContext()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            var databaseContext = new ApplicationDbContext(options);
            databaseContext.Database.EnsureCreated();
            if (await databaseContext.Clubs.CountAsync() < 0)
            {
                for (int i = 0; i < 10; i++)
                {
                    databaseContext.Clubs.Add(
                        new Club()
                        {
                            Title = "Running Club 1",
                            Image = "https://media.istockphoto.com/id/1324624694/photo/fitness-woman-running-training-for-marathon-on-sunny-coast-trail.jpg?s=170667a&w=0&k=20&c=OZO37MLisOxGCOIGJhxFNrLtOgjeC5r67Z7Xlrg1yu4=",
                            Description = "This is the desc for Running Club 1",
                            ClubCategory = ClubCategory.City,
                            Address = new Address()
                            {
                                Street = "123 Main St",
                                City = "Charlotte",
                                State = "Nc",
                            }
                        });
                    await databaseContext.SaveChangesAsync();
                }
            }
            return databaseContext;
        }

        [Fact]
        public async void ClubRepository_Add_ReturnsBool()
        {
            // Arrange
            var club = new Club()
            {
                Title = "Running Club 1",
                Image = "https://media.istockphoto.com/id/1324624694/photo/fitness-woman-running-training-for-marathon-on-sunny-coast-trail.jpg?s=170667a&w=0&k=20&c=OZO37MLisOxGCOIGJhxFNrLtOgjeC5r67Z7Xlrg1yu4=",
                Description = "This is the desc for Running Club 1",
                ClubCategory = ClubCategory.City,
                Address = new Address()
                {
                    Street = "123 Main St",
                    City = "Charlotte",
                    State = "Nc",
                }
            };

            var dbContext = await GetDbContext();
            var clubRepository = new ClubRepository(dbContext);

            // Act
            var result = clubRepository.Add(club);

            // Assert
            result.Should().BeTrue();
        }

        [Fact]
        public async void ClubRepository_GetByIdAsync_ReturnsClub()
        {
            // Arrange
            var id = 1;
            var dbContext = await GetDbContext();
            var clubRepository = new ClubRepository(dbContext);
            // dbContext.Clubs.AsNoTracking();

            // Act
            var result = clubRepository.GetByIdAsync(id);

            // Assert
            result.Should().NotBeNull();
            result.Should().BeOfType<Task<Club>>();
        }
    }
}
