using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebRest.EF.Data;
using WebRest.EF.Models;

namespace WebRestAPI.Controllers.UD;

[ApiController]
[Route("api/[controller]")]
public class ProductController : ControllerBase, iController<Product>
{
    private WebRestOracleContext _context;
    private readonly IMapper _mapper;

    public ProductController(WebRestOracleContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    [HttpGet]
    [Route("Get")]
    public async Task<IActionResult> Get()
    {
        List<Product> lst = null;
        lst = await _context.Product.ToListAsync();
        return Ok(lst);
    }

    [HttpGet]
    [Route("Get/{ID}")]
    public async Task<IActionResult> Get(string ID)
    {
        var itm = await _context.Product.Where(x => x.ProductId == ID).FirstOrDefaultAsync();
        return Ok(itm);
    }

    [HttpDelete]
    [Route("Delete/{ID}")]
    public async Task<IActionResult> Delete(string ID)
    {
        var itm = await _context.Product.Where(x => x.ProductId == ID).FirstOrDefaultAsync();
        _context.Product.Remove(itm);
        await _context.SaveChangesAsync();
        return Ok();
    }

    [HttpPut]
    public async Task<IActionResult> Put([FromBody] Product _Product)
    {
        var trans = _context.Database.BeginTransaction();

        try
        {
            var itm = await _context.Product.AsNoTracking()
                .Where(x => x.ProductId == _Product.ProductId)
                .FirstOrDefaultAsync();

            if (itm != null)
            {
                itm = _mapper.Map<Product>(_Product);
                _context.Product.Update(itm);
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
    public async Task<IActionResult> Post([FromBody] Product _Product)
    {
        var trans = _context.Database.BeginTransaction();

        try
        {
            _Product.ProductId = Guid.NewGuid().ToString().ToUpper().Replace("-", "");
            _context.Product.Add(_Product);
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