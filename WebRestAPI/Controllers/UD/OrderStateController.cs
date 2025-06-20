using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebRest.EF.Data;
using WebRest.EF.Models;

namespace WebRestAPI.Controllers.UD;

[ApiController]
[Route("api/[controller]")]
public class OrderStateController : ControllerBase, iController<OrderState>
{
    private WebRestOracleContext _context;
    private readonly IMapper _mapper;

    public OrderStateController(WebRestOracleContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    [HttpGet]
    [Route("Get")]
    public async Task<IActionResult> Get()
    {
        List<OrderState> lst = null;
        lst = await _context.OrderState.ToListAsync();
        return Ok(lst);
    }

    [HttpGet]
    [Route("Get/{ID}")]
    public async Task<IActionResult> Get(string ID)
    {
        var itm = await _context.OrderState.Where(x => x.OrderStateId == ID).FirstOrDefaultAsync();
        return Ok(itm);
    }

    [HttpDelete]
    [Route("Delete/{ID}")]
    public async Task<IActionResult> Delete(string ID)
    {
        var itm = await _context.OrderState.Where(x => x.OrderStateId == ID).FirstOrDefaultAsync();
        _context.OrderState.Remove(itm);
        await _context.SaveChangesAsync();
        return Ok();
    }

    [HttpPut]
    public async Task<IActionResult> Put([FromBody] OrderState _OrderState)
    {
        var trans = _context.Database.BeginTransaction();

        try
        {
            var itm = await _context.OrderState.AsNoTracking()
                .Where(x => x.OrderStateId == _OrderState.OrderStateId)
                .FirstOrDefaultAsync();

            if (itm != null)
            {
                itm = _mapper.Map<OrderState>(_OrderState);
                _context.OrderState.Update(itm);
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
    public async Task<IActionResult> Post([FromBody] OrderState _OrderState)
    {
        var trans = _context.Database.BeginTransaction();

        try
        {
            _OrderState.OrderStateId = Guid.NewGuid().ToString().ToUpper().Replace("-", "");
            _context.OrderState.Add(_OrderState);
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