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
    [Route("api")]
    [ApiController]
    public class CountryController : ControllerBase
    {
        private readonly CountryContext _context;

        public CountryController(CountryContext context)
        {
            _context = context;
        }

        [HttpGet("countries")]
        public async Task<ActionResult<IEnumerable<CountryItem>>> GetCountry()
        {
            return await _context.Country.ToListAsync();
        }

        [HttpGet("states")]
        public async Task<ActionResult<IEnumerable<StatesItem>>> GetStates()
        {
            return await _context.StatesItem.ToListAsync();
        }

        [HttpGet("countries/{id}/states")]
        public async Task<ActionResult<IEnumerable<StatesItem>>> GetCountryStates(long id)
        {
            var allStates = await _context.StatesItem.ToListAsync();

            var countryStates = allStates.Where(s => s.countryId == id);

            return countryStates.ToList();

        }

        [HttpGet("countries/{id}")]
        public async Task<ActionResult<CountryItem>> GetCountry(long id)
        {
            var country = await _context.Country.FindAsync(id);

            if (country == null)
            {
                return NotFound();
            }

            return country;
        }


        [HttpPut("countries/{id}")]
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

        [HttpPut("states/{id}")]
        public async Task<IActionResult> PutState(long id, StatesItem state)
        {
            if (id != state.Id)
            {
                return BadRequest();
            }

            _context.Entry(state).State = EntityState.Modified;

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

        [HttpPost("countries")]
        public async Task<ActionResult<CountryItem>> PostCountry(CountryItem country)
        {
            _context.Country.Add(country);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetCountry), new { id = country.Id }, country);
        }

        [HttpPost("states")]
        public async Task<ActionResult<CountryItem>> PostState(StatesItem state)
        {
            _context.StatesItem.Add(state);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetStates), new { id = state.Id }, state);
        }

        [HttpDelete("countries/{id}")]
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

        [HttpDelete("states/{id}")]
        public async Task<IActionResult> DeleteState(long id)
        {
            var state = await _context.StatesItem.FindAsync(id);
            if (state == null)
            {
                return NotFound();
            }

            _context.StatesItem.Remove(state);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool CountryExists(long id)
        {
            return _context.Country.Any(e => e.Id == id);
        }

        private bool StateExists(long id)
        {
            return _context.StatesItem.Any(e => e.Id == id);
        }
    }
}
