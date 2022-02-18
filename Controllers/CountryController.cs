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

        // GET: api/countries
        // list of all countries
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Country>>> GetAllCountries()
        {
            return await _context.Countries.ToListAsync();
        }

        // GET: api/countries/<CountryCode>/states
        // list of all states for a country
        [HttpGet("{code}/states")]
        public async Task<ActionResult<IEnumerable<State>>> GetCountryStates(string code)
        {
            var country = await _context.Countries.FirstOrDefaultAsync(c => c.Code == code);

            return await _context.States.Where(s => s.countryId == country.Id).ToListAsync();

        }
        // GET: api/countries/<CountryCode>
        // single country by code
        [HttpGet("{code}")]
        public async Task<ActionResult<Country>> GetCountry(string code)
        {
            var country = await _context.Countries.FirstOrDefaultAsync(c => c.Code == code);

            if (country == null)
            {
                return NotFound();
            }

            return country;
        }

        // POST: api/countries
        // add new country
        [HttpPost]
        public async Task<ActionResult<Country>> PostCountry(Country country)
        {

            _context.Countries.Add(country);
            await _context.SaveChangesAsync();


            return CreatedAtAction(nameof(GetCountry), new { code = country.Code }, country);
        }
    }
}
