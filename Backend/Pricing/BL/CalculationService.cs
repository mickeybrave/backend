using Backend.DAL;
using Backend.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Backend.Pricing.BL
{
    public struct ProductScan
    {
        public string CodesSingle { get; set; }
        public bool IsNewScan { get; set; }
    }

    public class ComplexResult
    {
        public string Message { get; set; }
        public ResultType ResultType { get; set; }
    }
    public enum ResultType
    {
        OK,
        BadRequest,
        NotFound,
        NoContent,
        UnknownError
    }
    public class DataResult<T>
    {
        public T Result { get; private set; }
        public ComplexResult ComplexResult { get; private set; }

        public DataResult(T result, ComplexResult complexResult)
        {
            Result = result;
            ComplexResult = complexResult;
        }
    }

    public interface ICalculationService
    {

        Task<DataResult<IEnumerable<Price>>> GetAllPricesAsync();
        Task<DataResult<Price>> GetPriceAsync(string code);
        Task<DataResult<double>> CalculatePrice(string codesSequence);

        Task<DataResult<double>> CalculatePrice(ProductScan productScan);

    }
    public class CalculationService : ICalculationService
    {
        private readonly IDataRepository<Price> _dataRepository;

        public CalculationService(IDataRepository<Price> dataRepository)
        {
            _dataRepository = dataRepository;
        }

        public async Task<DataResult<Price>> GetPriceAsync(string code)
        {
            try
            {
                var res = await _dataRepository.GetTask(code);

                if (res == null)
                {
                    return new DataResult<Price>(null, new ComplexResult { Message = $"Id={code} was not found in the system", ResultType = ResultType.NotFound });
                }


                return new DataResult<Price>(res, new ComplexResult { Message = null, ResultType = ResultType.OK });
            }
            catch (Exception ex)
            {
                return new DataResult<Price>(null, new ComplexResult { Message = ex.Message, ResultType = ResultType.UnknownError });
            }
        }

        public async Task<DataResult<IEnumerable<Price>>> GetAllPricesAsync()
        {
            try
            {
                var res = await _dataRepository.GetAllTask();
                return new DataResult<IEnumerable<Price>>(res, new ComplexResult { Message = null, ResultType = ResultType.OK });
            }
            catch (Exception ex)
            {
                return new DataResult<IEnumerable<Price>>(null, new ComplexResult { Message = ex.Message, ResultType = ResultType.UnknownError });
            }
        }


        public async Task<DataResult<double>> CalculatePrice(string codesSequence)
        {
            try
            {
                var charsArray = codesSequence.ToCharArray();
                List<ProductScan> productScans = new List<ProductScan>();

                for (int i = 0; i < charsArray.Length; i++)
                {
                    if (i == 0)
                    {
                        productScans.Add(new ProductScan { IsNewScan = true, CodesSingle = charsArray[i].ToString() });
                    }
                    else
                    {
                        productScans.Add(new ProductScan { IsNewScan = false, CodesSingle = charsArray[i].ToString() });
                    }

                }

                DataResult<double> finalPriceResult = new DataResult<double>(0, new ComplexResult());
                //Act
                foreach (var scannedProduct in productScans)
                {
                    finalPriceResult = await this.CalculatePrice(scannedProduct);
                }

                return new DataResult<double>(finalPriceResult.Result, new ComplexResult { Message = null, ResultType = ResultType.OK });
            }
            catch (Exception ex)
            {
                return new DataResult<double>(0, new ComplexResult { Message = ex.Message, ResultType = ResultType.UnknownError });
            }

        }


        private List<Price> _scannedPrices = new List<Price>();
        public async Task<DataResult<double>> CalculatePrice(ProductScan productScan)
        {
            try
            {
                var allPricesTableResult = await _dataRepository.GetAllTask();

                if (!allPricesTableResult.Any())
                {
                    return new DataResult<double>(0, new ComplexResult { Message = "No pricese data was found in the system", ResultType = ResultType.NotFound });
                }

                var priceDataFound = allPricesTableResult.FirstOrDefault(f => f.ProductCode == productScan.CodesSingle);
                if (priceDataFound == null)
                {
                    return new DataResult<double>(0, new ComplexResult { Message = $"Product with code = {productScan.CodesSingle} was found in the system", ResultType = ResultType.NotFound });
                }

                if (productScan.IsNewScan)
                {
                    _scannedPrices = new List<Price>();
                }

                return new DataResult<double>(CalculateTotalPrice(_scannedPrices, priceDataFound), new ComplexResult { Message = null, ResultType = ResultType.OK });
            }
            catch (Exception ex)
            {
                return new DataResult<double>(0, new ComplexResult { Message = ex.Message, ResultType = ResultType.UnknownError });
            }
        }

        private double CalculateTotalPrice(List<Price> currentPricesList, Price newPriceImport)
        {
            currentPricesList.Add(newPriceImport);
            return currentPricesList.GroupBy(g => g.ProductCode).Select(s =>
              {
                  var countByType = s.Count();
                  var amountByType = s.Sum(s => s.Amount);
                  var currentPrice = s.FirstOrDefault(f => f.ProductCode == s.Key && f.Pack.Exists);

                  if (currentPrice == null)
                  {
                      return new
                      {
                          Group = s.Key,
                          Amount = s.Sum(s => s.Amount),
                          Count = s.Count()
                      };
                  }

                  var countOfVolumes = countByType / currentPrice.Pack.Count;
                  var countOfSingleItems = countByType % currentPrice.Pack.Count;

                  var priceOfVolumes = countOfVolumes * currentPrice.Pack.Amount;
                  var priceOfSingleProducts = countOfSingleItems * currentPrice.Amount;

                  return new
                  {
                      Group = s.Key,
                      Amount = priceOfVolumes + priceOfSingleProducts,
                      Count = countByType
                  };


              }).Sum(s => s.Amount);
        }

     
    }
}
