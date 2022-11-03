// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebApplication1.Controllers;
using Microsoft.AspNetCore.Mvc;

[Route("api/[controller]")]
[ApiController]
public class CompaniesController : ControllerBase
{
    // GET: api/<CompaniesController>
    [HttpGet]
    public IEnumerable<string> Get()
    {
        return new string[] { "value1", "value2" };
    }

    // GET api/<CompaniesController>/5
    [HttpGet("{id}")]
    public string Get(int id)
    {
        return "value";
    }

    // POST api/<CompaniesController>
    [HttpPost]
    public void Post([FromBody] string value)
    {
    }

    // PUT api/<CompaniesController>/5
    [HttpPut("{id}")]
    public void Put(int id, [FromBody] string value)
    {
    }

    // DELETE api/<CompaniesController>/5
    [HttpDelete("{id}")]
    public void Delete(int id)
    {
    }
}
