using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebRest.EF.Data;
using WebRest.EF.Models;

namespace WebRestAPI.Controllers.UD;

[ApiController]
[Route("api/[controller]")]
public class ProductStatusController : ControllerBase, iController<ProductStatus>
{
    private WebRestOracleContext _context;
    private readonly IMapper _mapper;

    public ProductStatusController(WebRestOracleContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    [HttpGet]
    [Route("Get")]
    public async Task<IActionResult> Get()
    {
        List<ProductStatus> lst = null;
        lst = await _context.ProductStatus.ToListAsync();
        return Ok(lst);
    }

    [HttpGet]
    [Route("Get/{ID}")]
    public async Task<IActionResult> Get(string ID)
    {
        var itm = await _context.ProductStatus.Where(x => x.ProductStatusId == ID).FirstOrDefaultAsync();
        return Ok(itm);
    }

    [HttpDelete]
    [Route("Delete/{ID}")]
    public async Task<IActionResult> Delete(string ID)
    {
        var itm = await _context.ProductStatus.Where(x => x.ProductStatusId == ID).FirstOrDefaultAsync();
        _context.ProductStatus.Remove(itm);
        await _context.SaveChangesAsync();
        return Ok();
    }

    [HttpPut]
    public async Task<IActionResult> Put([FromBody] ProductStatus _ProductStatus)
    {
        var trans = _context.Database.BeginTransaction();

        try
        {
            var itm = await _context.ProductStatus.AsNoTracking()
                .Where(x => x.ProductStatusId == _ProductStatus.ProductStatusId)
                .FirstOrDefaultAsync();

            if (itm != null)
            {
                itm = _mapper.Map<ProductStatus>(_ProductStatus);
                _context.ProductStatus.Update(itm);
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
    public async Task<IActionResult> Post([FromBody] ProductStatus _ProductStatus)
    {
        var trans = _context.Database.BeginTransaction();

        try
        {
            _ProductStatus.ProductStatusId = Guid.NewGuid().ToString().ToUpper().Replace("-", "");
            _context.ProductStatus.Add(_ProductStatus);
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