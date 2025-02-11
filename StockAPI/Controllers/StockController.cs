using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StockAPI.Data;
using StockAPI.DTos.Stock;
using StockAPI.Mappers;

namespace StockAPI.Controllers
{
    //[Route("/api/[Controller]")]
    [Route("api/stock")]
    public class StockController : Controller
    {
        private readonly ApplicationDbContext _context;
        public StockController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            //var stocks = _context.Stock.ToList().Select(s => s.ToStockDto()); // ToList is deferred execution - it actually executes the sql.
            var stocks = await _context.Stock.ToListAsync();
            var stocksDto = stocks.Select(s => s.ToStockDto());
            return Ok(stocks);
        }

        [HttpGet("{id}")] // use model binding to extract the id from the route
        public async Task<IActionResult> GetById([FromRoute] int id)
        {
            var stock = await _context.Stock.FindAsync(id);
            if (stock == null)
            {
                return NotFound();
            }
            else
            {
                return Ok(stock.ToStockDto());
            }
        }

        [HttpPost]
        public IActionResult Create([FromBody] CreateStockRequestDto stockDto)
        {
            var stockModel = stockDto.ToStockFromCreateDTO();
            _context.Stock.Add(stockModel);
            _context.SaveChanges();
            return CreatedAtAction(nameof(GetById), new { id = stockModel.Id }, stockModel.ToStockDto()); // create and call the GetById then return the data from the response dto
        }

        [HttpPut]
        [Route("{id}")]
        public IActionResult Update([FromRoute] int id, [FromBody] UpdateStockRequestDto updateDto)
        {
            var stockModel = _context.Stock.FirstOrDefault(s => s.Id == id);

            if (stockModel == null)
            {
                return NotFound();
            }

            stockModel.Symbol = updateDto.Symbol;
            stockModel.CompanyName = updateDto.CompanyName;
            stockModel.Purchase = updateDto.Purchase;
            stockModel.LastDiv = updateDto.LastDiv;
            stockModel.Industry = updateDto.Industry;
            stockModel.MarketCap = updateDto.MarketCap;

            _context.SaveChanges();

            return Ok(stockModel.ToStockDto());
        }

        [HttpDelete]
        [Route("{id}")]
        public IActionResult Delete([FromRoute] int id)
        {
            var stock = _context.Stock.FirstOrDefault(s => s.Id == id);
            
            if (stock == null)
            {
                return NotFound();
            }

            _context.Stock.Remove(stock);
            _context.SaveChanges();

            return NoContent();
        }
    }
}
