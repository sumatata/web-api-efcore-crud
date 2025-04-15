using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebRest.EF.Data;
using WebRest.EF.Models;

namespace WebRestAPI.Controllers.UD;

[ApiController]
[Route("api/[controller]")]
public class CustomerController : ControllerBase, iController<Customer>
{
    private WebRestOracleContext _context;
    // Create a field to store the mapper object
    private readonly IMapper _mapper;

    public CustomerController(WebRestOracleContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    [HttpGet]
    [Route("Get")]
    public async Task<IActionResult> Get()
    {

        List<Customer> lst = null;
        lst = await _context.Customer.ToListAsync();
        return Ok(lst);
    }


    [HttpGet]
    [Route("Get/{ID}")]
    public async Task<IActionResult> Get(string ID)
    {
        var itm = await _context.Customer.Where(x => x.CustomerId == ID).FirstOrDefaultAsync();
        return Ok(itm);
    }


    [HttpDelete]
    [Route("Delete/{ID}")]
    public async Task<IActionResult> Delete(string ID)
    {
        var itm = await _context.Customer.Where(x => x.CustomerId == ID).FirstOrDefaultAsync();
        _context.Customer.Remove(itm);
        await _context.SaveChangesAsync();
        return Ok();
    }

    [HttpPut]
    public async Task<IActionResult> Put([FromBody] Customer _Customer)
    {
        var trans = _context.Database.BeginTransaction();

        try
        {
            var itm = await _context.Customer.AsNoTracking()
            .Where(x => x.CustomerId == _Customer.CustomerId)
            .FirstOrDefaultAsync();


            if (itm != null)
            {
                itm = _mapper.Map<Customer>(_Customer);
    
                _context.Customer.Update(itm);
                await _context.SaveChangesAsync();
                trans.Commit();

            }
        }
        catch (Exception ex)
        {
            trans.Rollback();
            return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
        }

        return Ok();

    }

    [HttpPost]
    public async Task<IActionResult> Post([FromBody] Customer _Customer)
    {
        var trans = _context.Database.BeginTransaction();

        try
        {
            _Customer.CustomerId = Guid.NewGuid().ToString().ToUpper().Replace("-", "");
            _context.Customer.Add(_Customer);
            await _context.SaveChangesAsync();
            trans.Commit();
        }
        catch (Exception ex)
        {
            trans.Rollback();
            return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
        }

        return Ok();
    }

}