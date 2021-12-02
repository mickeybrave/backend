using Backend.DAL;
using Backend.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Backend.Pricing.BL
{
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

        Task<DataResult<IEnumerable<Price>>> GetAllAsync();
        Task<DataResult<Price>> GetItemAsync(string code);

    }
    public class CalculationService : ICalculationService
    {
        private readonly IDataRepository<Price> _dataRepository;

        public CalculationService(IDataRepository<Price> dataRepository)
        {
            _dataRepository = dataRepository;
        }

        public async Task<DataResult<Price>> GetItemAsync(string code)
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

        public async Task<DataResult<IEnumerable<Price>>> GetAllAsync()
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
    }
}
