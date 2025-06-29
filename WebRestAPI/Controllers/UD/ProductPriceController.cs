using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebRest.EF.Data;
using WebRest.EF.Models;

namespace WebRestAPI.Controllers.UD;

[ApiController]
[Route("api/[controller]")]
public class ProductPriceController : ControllerBase, iController<ProductPrice>
{
    private WebRestOracleContext _context;
    private readonly IMapper _mapper;

    public ProductPriceController(WebRestOracleContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    [HttpGet]
    [Route("Get")]
    public async Task<IActionResult> Get()
    {
        List<ProductPrice> lst = null;
        lst = await _context.ProductPrice.ToListAsync();
        return Ok(lst);
    }

    [HttpGet]
    [Route("Get/{ID}")]
    public async Task<IActionResult> Get(string ID)
    {
        var itm = await _context.ProductPrice.Where(x => x.ProductPriceId == ID).FirstOrDefaultAsync();
        return Ok(itm);
    }

    [HttpDelete]
    [Route("Delete/{ID}")]
    public async Task<IActionResult> Delete(string ID)
    {
        var itm = await _context.ProductPrice.Where(x => x.ProductPriceId == ID).FirstOrDefaultAsync();
        _context.ProductPrice.Remove(itm);
        await _context.SaveChangesAsync();
        return Ok();
    }

    [HttpPut]
    public async Task<IActionResult> Put([FromBody] ProductPrice _ProductPrice)
    {
        var trans = _context.Database.BeginTransaction();

        try
        {
            var itm = await _context.ProductPrice.AsNoTracking()
                .Where(x => x.ProductPriceId == _ProductPrice.ProductPriceId)
                .FirstOrDefaultAsync();

            if (itm != null)
            {
                itm = _mapper.Map<ProductPrice>(_ProductPrice);
                _context.ProductPrice.Update(itm);
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
    public async Task<IActionResult> Post([FromBody] ProductPrice _ProductPrice)
    {
        var trans = _context.Database.BeginTransaction();

        try
        {
            _ProductPrice.ProductPriceId = Guid.NewGuid().ToString().ToUpper().Replace("-", "");
            _context.ProductPrice.Add(_ProductPrice);
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