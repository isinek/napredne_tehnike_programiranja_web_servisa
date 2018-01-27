using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OAuthLab.DAL.Entities;
using OAuthLab.JwtAPI.Controllers;
using Xunit;

namespace OAuthLab.Tests.UnitTests
{
    public class Suppliers_Should
    {
        DbContextOptions<StoreSampleContext> _dbContextOptions;

        public Suppliers_Should()
        {
            _dbContextOptions = new DbContextOptionsBuilder<StoreSampleContext>()
                            .UseInMemoryDatabase(databaseName: "Test_database")
                            .Options;
        }

        // Testiranje dodavanja novih dobavljaca u bazu podataka
        [Fact]
        public async void PostSupplier()
        {   
            using (var context = new StoreSampleContext(_dbContextOptions))
            {
                var suppliersAPI = new SuppliersController(context);
                for (int i = 0; i < 10; ++i)
                {
                    Supplier tmpSupplier = new Supplier();
                    tmpSupplier.CompanyName = $"Kompanija { i + 1 }";
                    tmpSupplier.City = "Zagreb";
                    tmpSupplier.Country = "Hrvatska";
                    tmpSupplier.Phone = $"01 234 567{ i }";
                    var result = await suppliersAPI.PostSupplier(tmpSupplier);
                    var badRequest = result as BadRequestObjectResult;

                    Assert.Null(badRequest);    // Ako API ne vraca BadRequest, to znaci da je poziv uspjesan
                }
            }
        }

        // Testiranje dohvata dobavljaca
        [Fact]
        public async void GetSupplier()
        {
            // Dodavanje dobavljaca prije dohvacanja
            using (var context = new StoreSampleContext(_dbContextOptions))
            {
                var suppliersAPI = new SuppliersController(context);
                for (int i = 0; i < 10; ++i)
                {
                    Supplier tmpSupplier = new Supplier();
                    tmpSupplier.CompanyName = $"Kompanija { i + 1 }";
                    tmpSupplier.City = "Zagreb";
                    tmpSupplier.Country = "Hrvatska";
                    tmpSupplier.Phone = $"01 234 567{ i }";
                    suppliersAPI.PostSupplier(tmpSupplier).Wait();
                }
            }

            using (var context = new StoreSampleContext(_dbContextOptions))
            {
                var suppliersAPI = new SuppliersController(context);
                var result = await suppliersAPI.GetSupplier(5);
                var okResult = result as OkObjectResult;

                // Ako je rezultat Ok i status kod je 200, tada je poziv uspjesan
                Assert.NotNull(okResult);
                Assert.Equal(200, okResult.StatusCode);

                // Ako je dohvacen dobavljac sa ispravnim brojem telefona, poziv je uspjesan
                Supplier supplier = okResult.Value as Supplier;
                Assert.NotNull(supplier);
                Assert.Equal("01 234 5674", supplier.Phone);
            }
        }
    }
}
