using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebRest.EF.Data;
using WebRest.EF.Models;

namespace WebRestAPI.Controllers.UD;

[ApiController]
[Route("api/[controller]")]
public class CustomerAddressController : ControllerBase, iController<CustomerAddress>
{
    private WebRestOracleContext _context;
    private readonly IMapper _mapper;

    public CustomerAddressController(WebRestOracleContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    [HttpGet]
    [Route("Get")]
    public async Task<IActionResult> Get()
    {
        List<CustomerAddress> lst = null;
        lst = await _context.CustomerAddress.ToListAsync();
        return Ok(lst);
    }

    [HttpGet]
    [Route("Get/{ID}")]
    public async Task<IActionResult> Get(string ID)
    {
        var itm = await _context.CustomerAddress.Where(x => x.CustomerAddressId == ID).FirstOrDefaultAsync();
        return Ok(itm);
    }

    [HttpDelete]
    [Route("Delete/{ID}")]
    public async Task<IActionResult> Delete(string ID)
    {
        var itm = await _context.CustomerAddress.Where(x => x.CustomerAddressId == ID).FirstOrDefaultAsync();
        _context.CustomerAddress.Remove(itm);
        await _context.SaveChangesAsync();
        return Ok();
    }

    [HttpPut]
    public async Task<IActionResult> Put([FromBody] CustomerAddress _CustomerAddress)
    {
        var trans = _context.Database.BeginTransaction();

        try
        {
            var itm = await _context.CustomerAddress.AsNoTracking()
                .Where(x => x.CustomerAddressId == _CustomerAddress.CustomerAddressId)
                .FirstOrDefaultAsync();

            if (itm != null)
            {
                itm = _mapper.Map<CustomerAddress>(_CustomerAddress);
                _context.CustomerAddress.Update(itm);
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
    public async Task<IActionResult> Post([FromBody] CustomerAddress _CustomerAddress)
    {
        var trans = _context.Database.BeginTransaction();

        try
        {
            _CustomerAddress.CustomerAddressId = Guid.NewGuid().ToString().ToUpper().Replace("-", "");
            _context.CustomerAddress.Add(_CustomerAddress);
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