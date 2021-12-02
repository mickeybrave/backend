
using Backend.DAL;
using Backend.Model;
using System.Linq;
using Xunit;

namespace Backend.Tests
{
    public class PriceRepository_tests
    {
      

       

        [Fact]
        public async void PriceRepository_integration_has_data_test()
        {
            PriceUpdater<Price> PriceDataUpdater = new PriceUpdater<Price>();
            DataRepository<Price> rep = new DataRepository<Price>(PriceDataUpdater);

            var allDataTask = await rep.GetAllTask();
            Assert.True(allDataTask.Any());

            var allData = await rep.GetAllTask();
            Assert.True(allData.Any());

        }
      


    }
}
