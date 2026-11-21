using Microsoft.AspNetCore.Mvc;
using SparkySparky.Services;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace SparkySparky.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MetalLimbController : ControllerBase
    {
        private readonly IMetalLimbService metalLimbService;

        public MetalLimbController(IMetalLimbService metalLimbService)
        {
            this.metalLimbService = metalLimbService;
        }

        // GET: api/<MetalLimbController>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            this.metalLimbService.SensorIsAlive();
            return new string[] { "SensorIsAlive", "Yes" };
        }

        // GET api/<MetalLimbController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            throw new NotImplementedException();
            return "value";
        }

        // POST api/<MetalLimbController>
        [HttpPost]
        public void Post([FromBody] string value)
        {
            throw new NotImplementedException();
        }

        // PUT api/<MetalLimbController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
            throw new NotImplementedException();
        }

        // DELETE api/<MetalLimbController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
            throw new NotImplementedException();
        }
    }
}
