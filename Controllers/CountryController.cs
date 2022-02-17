#nullable disable
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CountryApi.Models;

namespace CountryApi.Controllers
{
    [Route("api/countries")]
    [ApiController]
    public class CountryController : ControllerBase
    {
        private readonly CountryContext _context;

        public CountryController(CountryContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<CountryItem>>> GetCountry()
        {
            return await _context.Country.ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<CountryItem>> GetCountry(long id)
        {
            var country = await _context.Country.FindAsync(id);

            if (country == null)
            {
                return NotFound();
            }

            return country;
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutCountry(long id, CountryItem country)
        {
            if (id != country.Id)
            {
                return BadRequest();
            }

            _context.Entry(country).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CountryExists(id))
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

        [HttpPost]
        public async Task<ActionResult<CountryItem>> PostCountry(CountryItem country)
        {
            _context.Country.Add(country);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetCountry), new { id = country.Id }, country);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCountry(long id)
        {
            var country = await _context.Country.FindAsync(id);
            if (country == null)
            {
                return NotFound();
            }

            _context.Country.Remove(country);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool CountryExists(long id)
        {
            return _context.Country.Any(e => e.Id == id);
        }
    }
}
