using Backend.DAL;
using Backend.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Backend.Pricing.BL
{
    public interface ICalculationService
    {
        Task<IEnumerable<Price>> GetAlPrices();
        Task<Price> GePriceTask(string code);
    }
    public class CalculationService : ICalculationService
    {
        private readonly IDataRepository<Price> _dataRepository;

        public CalculationService(IDataRepository<Price> dataRepository)
        {
            _dataRepository = dataRepository;
        }

        public Task<IEnumerable<Price>> GetAlPrices()
        {
            return _dataRepository.GetAllTask();
        }

        public Task<Price> GePriceTask(string code)
        {
            return _dataRepository.GetTask(code);
        }
    }
}
