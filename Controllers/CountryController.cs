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
        public async Task<ActionResult<IEnumerable<CountryDTO>>> GetAllCountries()
        {
            return await _context.Country.Select(c => new CountryDTO(){
                Code = c.Code,
                Name = c.Name,
                Id = c.Id
            }).ToListAsync();
                                    
        }

        // GET: api/countries/<CountryCode>/states
        // list of all states for a country
        [HttpGet("{code}/states")]
        public async Task<ActionResult<IEnumerable<StateDTO>>> GetCountryStates(string code)
        {
            var countrySelected = await _context.Country.Where(c => c.Code == code).Select(c => c).FirstOrDefaultAsync();

            var countrysStates = from s in countrySelected.States
                                    select new StateDTO(){
                                        Code = s.Code,
                                        Name = s.Name,
                                        Id = s.Id
                                    };
            return countrysStates.ToList();

        }
        // GET: api/countries/<CountryCode>
        // single country by code
        [HttpGet("{code}")]
        public async Task<ActionResult<CountryDTO>> GetCountry(string code)
        {
            var country = await _context.Country.Where(c => c.Code == code).Select(c => new CountryDTO(){
                Code = c.Code,
                Name = c.Name,
                Id = c.Id
            }).FirstOrDefaultAsync();

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
            _context.Country.Add(country);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetCountry), new { id = country.Id }, country);
        }
    }
}
