// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebApplication1.Controllers;

using Data.AccessLayer.Abstractions;
using Data.Models;

using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

[Route("api/[controller]")]
[ApiController]
public class AddressesController : ControllerBase
{
    private readonly IAddressAccessLayer addressAccessLayer;

    public AddressesController(IAddressAccessLayer addressAccessLayer)
        => this.addressAccessLayer = addressAccessLayer;


    // GET: api/<AddressesController>
    [HttpGet]
    public async Task<IActionResult> Get(CancellationToken cancellationToken)
        => this.Ok(await this.addressAccessLayer.GetCollection().ToListAsync(cancellationToken));

    // GET api/<AddressesController>/5
    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetAsync(int? id, CancellationToken cancellationToken)
    {
        if (id is < 0)
            return this.BadRequest();

        return this.Ok(await this.addressAccessLayer.GetSingleAsync(address => address.Id == id,
                                                                    cancellationToken: cancellationToken));
    }

    // POST api/<AddressesController>
    [HttpPost]
    public async Task<IActionResult> Post([FromBody] Address? address, CancellationToken cancellationToken)
    {
        if (address is null)
            return this.BadRequest();

        var idCreated = await this.addressAccessLayer.AddAsync(address, cancellationToken);
        return this.Created(string.Empty, idCreated);
    }

    // PUT api/<AddressesController>/5
    [HttpPut("{id}")]
    public void Put(int id, [FromBody] string value)
    {
    }

    // DELETE api/<AddressesController>/5
    [HttpDelete("{id}")]
    public async Task Delete(int id, CancellationToken cancellationToken) 
        => await this.addressAccessLayer.RemoveAsync(id, cancellationToken);
}
