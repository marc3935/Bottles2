using Bottles;
using Microsoft.EntityFrameworkCore;

namespace BottlesUnitTest
{
    public class UnitTest1
    {
        private bool useDatabase = false;
        private IBottleRepository repo;
        public UnitTest1()
        {
            if (useDatabase)
            {
                var optionsBuilder = new DbContextOptionsBuilder<BottlesDbContext>();

                // https://learn.microsoft.com/en-us/aspnet/core/security/app-secrets
                optionsBuilder.UseSqlServer(Secrets.ConnectionStringSimply);

                // connection string structure
                //   "Data Source=mssql7.unoeuro.com;Initial Catalog=FROM simply.com;Persist Security Info=True;User ID=FROM simply.com;Password=DB PASSWORD FROM simply.com;TrustServerCertificate=True"
                BottlesDbContext _dbContext = new(optionsBuilder.Options);

                // clean database table: remove all rows
                _dbContext.Database.ExecuteSqlRaw("TRUNCATE TABLE dbo.Bottles");
                repo = new BottleRepositoryDatabaseEf(_dbContext);
            }
            else
            {
                repo = new BottleRepositoryList(includeTestData: false);
            }
        }


        [Fact]
        public void TestAddBottle()
        {
            // Arrange
            Bottle newBottle = new Bottle { Name = "Pepsi", Volume = 600.0 };
            // Act
            Bottle addedBottle = repo.AddBottle(newBottle);
            // Assert
            Assert.NotNull(addedBottle);
            Assert.Equal("Pepsi", addedBottle.Name);
            Assert.Equal(600.0, addedBottle.Volume);

            IEnumerable<Bottle> b = repo.GetBottles();
            Assert.Single(b);
        }


        [Fact]
        public void TestGetById()
        {
            // Arrange
            repo.AddBottle(new Bottle { Name = "Pepsi", Volume = 600.0 });
            repo.AddBottle(new Bottle { Name = "Coca Cola", Volume = 500.0 });
            // Act
            Bottle? bottle = repo.GetById(1);
            // Assert
            Assert.NotNull(bottle);
            Assert.Equal("Pepsi", bottle.Name);
            Assert.Equal(600.0, bottle.Volume);
        }

        [Fact]
        public void DeleteById()
        {
            // Arrange
            repo.AddBottle(new Bottle { Name = "Pepsi", Volume = 600.0 });
            repo.AddBottle(new Bottle { Name = "Coca Cola", Volume = 500.0 });
            // Act
            Bottle? deletedBottle = repo.DeleteById(1);
            // Assert
            Assert.NotNull(deletedBottle);
            Assert.Equal("Pepsi", deletedBottle.Name);
            Assert.Equal(600.0, deletedBottle.Volume);
            IEnumerable<Bottle> b = repo.GetBottles();
            Assert.Single(b);
        }

        [Fact]
        public void UpdateById()
            {
            // Arrange
            repo.AddBottle(new Bottle { Name = "Pepsi", Volume = 600.0 });
            repo.AddBottle(new Bottle { Name = "Coca Cola", Volume = 500.0 });
            // Act
            Bottle? updatedBottle = repo.UpdateById(2, new Bottle { Name = "Pepsi Max", Volume = 650.0 });
            // Assert
            Assert.NotNull(updatedBottle);
            Assert.Equal("Pepsi Max", updatedBottle.Name);
            Assert.Equal(650.0, updatedBottle.Volume);
        }


    }
}
