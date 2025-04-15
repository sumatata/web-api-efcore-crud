using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebRest.EF.Data;
using WebRest.EF.Models;

namespace WebRestAPI.Controllers.UD;

[ApiController]
[Route("api/[controller]")]
public class GenderController : ControllerBase, iController<Gender>
{
    private WebRestOracleContext _context;
    private readonly IMapper _mapper;

    public GenderController(WebRestOracleContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    [HttpGet]
    [Route("Get")]
    public async Task<IActionResult> Get()
    {
        List<Gender> lst = null;
        lst = await _context.Gender.ToListAsync();
        return Ok(lst);
    }

    [HttpGet]
    [Route("Get/{ID}")]
    public async Task<IActionResult> Get(string ID)
    {
        var itm = await _context.Gender.Where(x => x.GenderId == ID).FirstOrDefaultAsync();
        return Ok(itm);
    }

    [HttpDelete]
    [Route("Delete/{ID}")]
    public async Task<IActionResult> Delete(string ID)
    {
        var itm = await _context.Gender.Where(x => x.GenderId == ID).FirstOrDefaultAsync();
        _context.Gender.Remove(itm);
        await _context.SaveChangesAsync();
        return Ok();
    }

    [HttpPut]
    public async Task<IActionResult> Put([FromBody] Gender _Gender)
    {
        var trans = _context.Database.BeginTransaction();

        try
        {
            var itm = await _context.Gender.AsNoTracking()
                .Where(x => x.GenderId == _Gender.GenderId)
                .FirstOrDefaultAsync();

            if (itm != null)
            {
                itm = _mapper.Map<Gender>(_Gender);
                _context.Gender.Update(itm);
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
    public async Task<IActionResult> Post([FromBody] Gender _Gender)
    {
        var trans = _context.Database.BeginTransaction();

        try
        {
            _Gender.GenderId = Guid.NewGuid().ToString().ToUpper().Replace("-", "");
            _context.Gender.Add(_Gender);
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