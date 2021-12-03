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

        private readonly ICalculationService _calculationService;

        public PricesController(ICalculationService calculationService)
        {
            this._calculationService = calculationService;
        }
        // GET: api/<PricesController>
        [HttpGet]
        public async Task<IActionResult> GetAllPrices()
        {
            var results = await _calculationService.GetAllPricesAsync();
            return Ok(results);
        }




        // GET api/<PricesController>/5

        [HttpGet("{code}")]
        public async Task<IActionResult> GetPrice(string code)
        {
            var result = await _calculationService.GetPriceAsync(code);

            if (result.ComplexResult.ResultType == ResultType.NotFound)
            {
                return NotFound(result.ComplexResult.Message);
            }
            return Ok(result.Result);
        }



        [HttpGet("{result}/GetResult")]
        public IActionResult GetResult(string result)
        {
            return Ok("Calculation result: " + result);
        }

        [Route("/error")]
        public IActionResult Error() => Problem();


        [HttpPost("scan")]
        public async Task<IActionResult> PostScanCalculateProduct([FromBody] ProductScan singleScan)
        {

            var serviceResult = await _calculationService.CalculatePrice(singleScan);


            switch (serviceResult.ComplexResult.ResultType)
            {
                case ResultType.OK:
                    return RedirectToAction("GetResult", new { result = serviceResult.Result });

                case ResultType.BadRequest:
                case ResultType.UnknownError:
                    return BadRequest(serviceResult.ComplexResult.Message);
                case ResultType.NotFound:
                    return NotFound(serviceResult.ComplexResult.Message);
                case ResultType.NoContent:
                default:
                    return NoContent();
            }

        }
       
        [HttpPost]
        public async Task<IActionResult> PostCalculateProduct([FromBody] string codesSequence)
        {

            var serviceResult = await _calculationService.CalculatePrice(codesSequence);


            switch (serviceResult.ComplexResult.ResultType)
            {
                case ResultType.OK:
                    return RedirectToAction("GetResult", new { result = serviceResult.Result });

                case ResultType.BadRequest:
                case ResultType.UnknownError:
                    return BadRequest(serviceResult.ComplexResult.Message);
                case ResultType.NotFound:
                    return NotFound(serviceResult.ComplexResult.Message);
                case ResultType.NoContent:
                default:
                    return NoContent();
            }

        }


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
