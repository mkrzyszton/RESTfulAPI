using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Products.Models;
using Swashbuckle.AspNetCore.Annotations;

namespace Products.Models
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private static readonly string[] ProductsNames = new[]
        {
            "Bułki", "Ser", "Woda", "Ziemniaki"
        };

        private static readonly decimal[] ProductsPrices = new[]
        {
            0.99m, 3.99m, 1.5m, 0.3m
        };

        private static readonly int[] ProductsAmount = new[]
        {
            5, 2, 6, 10
        };

        private readonly ProductsContext _context;
        private object[] id;

        public ProductsController(ProductsContext context)
        {
            _context = context;
        }

        // GET: api/Products
        [HttpGet]
        public async Task<IEnumerable<Products>> GetProductsAsync()
        {
            var products = await _context.Products.FindAsync(id);

            if (products == null)
            {
                return Enumerable.Range(1, 4).Select(index => new Products
                {
                    Id = index,
                    Name = ProductsNames[index - 1],
                    Price = ProductsPrices[index - 1],
                    Amount = ProductsAmount[index - 1]
                })
                .ToArray();
            }

            return (IEnumerable<Products>)products;
        }

        // GET: api/Products/5
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [SwaggerOperation("Zwraca zadanie o podanym {id}.", "Używa EF FindAsync()")]
        [SwaggerResponse(404, "Nie znaleziono zadania o podanym {id}")]
        public async Task<ActionResult<Products>> GetProducts(
        [SwaggerParameter("Podaj nr zadnia które chcesz odczytać", Required = true)]
            long id)
        {
            var products = await _context.Products.FindAsync(id);

            if (products == null)
            {
                return NotFound();
            }

            return products;
        }


        // PUT: api/Products/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutProducts(long id, Products products)
        {
            if (id != products.Id)
            {
                return BadRequest();
            }

            _context.Entry(products).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProductsExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Products
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Products>> PostProducts(Products products)
        {
            _context.Products.Add(products);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetProducts", new { id = products.Id }, products);
        }

        // DELETE: api/Products/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProducts(long id)
        {
            var products = await _context.Products.FindAsync(id);
            if (products == null)
            {
                return NotFound();
            }

            _context.Products.Remove(products);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ProductsExists(long id)
        {
            return _context.Products.Any(e => e.Id == id);
        }
    }
}
