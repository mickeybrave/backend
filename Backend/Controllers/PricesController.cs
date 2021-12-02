using Backend.Model;
using Backend.Pricing.BL;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;


// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Backend.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class PricesController : ControllerBase
    {
        private const int ConstThreashold = 1000;

        //IDataRepository<Price> _PriceRepository;
        private readonly IConfiguration _config;
        private readonly ICalculationService _calculationService;

        public PricesController(ICalculationService calculationService)
        {
            this._calculationService = calculationService;
        }
        // GET: api/<PricesController>
        [HttpGet]
        public async Task<IActionResult> GetTodoItems()
        {
            var results = await _calculationService.GetAllAsync();
            return Ok(results);
        }


        //[HttpGet("Output")]
        //public async Task<IEnumerable<Price>> GetOutput()
        //{
        //    return await _calculationService.GetAlPrices();
        //}

        // GET api/<PricesController>/5

        [HttpGet("{code}")]
        public async Task<IActionResult> Get(string code)
        {
            var result = await _calculationService.GetItemAsync(code);

            if (result.ComplexResult.ResultType == ResultType.NotFound)
            {
                return NotFound(result.ComplexResult.Message);
            }
            return Ok(result.Result);
        }




        [Route("/error")]
        public IActionResult Error() => Problem();

        //[HttpPost]
        //public async Task<ActionResult<Price>> PostCalculateProduct(string productCode)
        //{
        //    //if (Price == null || Price.AmountDollars > ConstThreashold)
        //    //{
        //    //    return StatusCode(500, new ErrorData { Code = 500, Message = $"Client Id = {Price.ProductCode} does not have sufficient funds." }.ToString());
        //    //}
        //    //await _dataRepository.UpdateTask(Price);

        //    return CreatedAtAction(nameof(Get), new { id = Price.ProductCode }, Price);
        //}

        // PUT api/<PricesController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] Price Price)
        {
        }

        // DELETE api/<PricesController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
