#nullable disable
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CountryApi.Models;

// ToListAsync might be able to be replaced with .Any(),
// although it's not async (if that matters)
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

        // GET: api/countries
        // list of all countries
        [HttpGet("countries")]
        public async Task<ActionResult<IEnumerable<Country>>> GetCountry()
        {
            return await _context.Country.ToListAsync();
        }
        // GET: api/states
        // list of all states
        [HttpGet("states")]
        public async Task<ActionResult<IEnumerable<State>>> GetStates()
        {
            return await _context.StatesItem.ToListAsync();
        }

        // GET: api/countries/<CountryCode>/states
        // list of all states for a country
        [HttpGet("countries/{code}/states")]
        public async Task<ActionResult<IEnumerable<State>>> GetCountryStates(string code)
        {
            var allStates = await _context.StatesItem.ToListAsync();
            var allCountries = await _context.Country.ToListAsync();

            var country = allCountries.Where(c => c.Code == code).Select(c => c.Id).FirstOrDefault();

            var countryStates = allStates.Where(s => s.countryId == country);

            return countryStates.ToList();

        }
        // GET: api/countries/<CountryCode>
        // single country by code
        [HttpGet("countries/{code}")]
        public async Task<ActionResult<Country>> GetCountry(string code)
        {
            var allCountries = await _context.Country.ToListAsync();
            var country = allCountries.Where(c => c.Code == code).Select(c => c).FirstOrDefault();

            if (country == null)
            {
                return NotFound();
            }

            return country;
        }

        // PUT: api/countries/<CountryCode>
        // update country by code
        [HttpPut("countries/{code}")]
        public async Task<IActionResult> PutCountry(string code, Country country)
        {
            if (code != country.Code)
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
                if (!CountryExists(code))
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

        // POST: api/states/<statecode>
        // update state by code
        // ! untested, as webui doesn't implement this
        [HttpPut("states/{code}")]
        public async Task<IActionResult> PutState(string code, State state)
        {
            if (code != state.Code)
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
                if (!StateExists(code))
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

        // POST: api/countries
        // add new country
        [HttpPost("countries")]
        public async Task<ActionResult<Country>> PostCountry(Country country)
        {
            _context.Country.Add(country);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetCountry), new { id = country.Id }, country);
        }

        // POST: api/states
        // add new state
        [HttpPost("states")]
        public async Task<ActionResult<Country>> PostState(State state)
        {
            _context.StatesItem.Add(state);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetStates), new { id = state.Id }, state);
        }

        // DELETE: api/countries/<CountryCode>
        // delete country by code
        // ! untested, as webui doesn't implement this
        [HttpDelete("countries/{code}")]
        public async Task<IActionResult> DeleteCountry(string code)
        {
            var allCountries = await _context.Country.ToListAsync();
            var country = allCountries.Where(c => c.Code == code).Select(c => c).FirstOrDefault();

            if (country == null)
            {
                return NotFound();
            }

            _context.Country.Remove(country);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // DELETE: api/states/<statecode>
        // delete state by code
        // ! untested, as webui doesn't implement this
        [HttpDelete("states/{code}")]
        public async Task<IActionResult> DeleteState(string code)
        {
            var allStates = await _context.StatesItem.ToListAsync();
            var state = allStates.Where(c => c.Code == code).Select(c => c).FirstOrDefault();

            if (state == null)
            {
                return NotFound();
            }

            _context.StatesItem.Remove(state);
            await _context.SaveChangesAsync();

            return NoContent();
        }
        // check if country exists by it's countrycode
        private bool CountryExists(string code)
        {
            return _context.Country.Any(e => e.Code == code);
        }
        // check if state exists by it's statecode
        private bool StateExists(string code)
        {
            return _context.StatesItem.Any(e => e.Code == code);
        }
    }
}
