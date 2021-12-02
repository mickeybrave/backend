using Backend.DAL;
using Backend.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
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
        private readonly IDataRepository<Price> _dataRepository;

        public PricesController(IDataRepository<Price> dataRepository)
        {

            //PriceUpdater<Price> PriceDataUpdater = new PriceUpdater<Price>();
            //  _PriceRepository = new DataRepository<Price>(dataSourceJsonPath, PriceDataUpdater);
            _dataRepository = dataRepository;
        }
        // GET: api/<PricesController>
        [HttpGet]
        public IEnumerable<Price> Get()
        {
            return _dataRepository.GetAll();
        }

        // GET api/<PricesController>/5
        [HttpGet("{id}")]
        public Price Get(string id)
        {
            return _dataRepository.Get(id);
        }

        [Route("/error")]
        public IActionResult Error() => Problem();

        [HttpPost]
        public async Task<ActionResult<Price>> PostPrices(Price Price)
        {
            //if (Price == null || Price.AmountDollars > ConstThreashold)
            //{
            //    return StatusCode(500, new ErrorData { Code = 500, Message = $"Client Id = {Price.ProductCode} does not have sufficient funds." }.ToString());
            //}
            //await _dataRepository.UpdateTask(Price);

            return CreatedAtAction(nameof(Get), new { id = Price.ProductCode }, Price);
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
