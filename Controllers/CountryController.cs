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
            return await _context.Countries.Select(c => new CountryDTO()
            {
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
            // country.code cannot include a number
            if (code.Any(char.IsDigit))
            {
                return BadRequest("Country code cannot include a number");
            }

            var query = from s in _context.States
                        where s.Country.Code == code
                        select new StateDTO()
                        {
                            Code = s.Code,
                            Name = s.Name,
                            Id = s.Id,
                            CountryId = s.CountryId
                        };

            return await query.ToListAsync();

        }
        // GET: api/countries/<CountryCode>
        // single country by code
        [HttpGet("{code}")]
        public async Task<ActionResult<CountryDTO>> GetCountry(string code)
        {
            // country.code cannot include a number
            if (code.Any(char.IsDigit))
            {
                return BadRequest("Country code cannot include a number");
            }

            var country = await _context.Countries.Where(c => c.Code == code).Select(c => new CountryDTO()
            {
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
        public async Task<ActionResult<CountryDTO>> PostCountry(CountryDTO _CountryDTO)
        {            
            // country code/name cannot be null
            if (string.IsNullOrEmpty(_CountryDTO.Code) || string.IsNullOrEmpty(_CountryDTO.Name))
            {
                return BadRequest("Country code or name cannot be null");
            }

            // country code/name cannot include a number
            if (_CountryDTO.Code.Any(char.IsDigit) || _CountryDTO.Name.Any(char.IsDigit))
            {
                return BadRequest("Country code or name cannot include a number");
            }

            // lets not accept duplicate countries
            if (await _context.Countries.AnyAsync(c => c.Code == _CountryDTO.Code))
            {
                return BadRequest("Country already exists");
            }


            await _context.Countries.AddAsync(new Country()
            {
                Code = _CountryDTO.Code,
                Name = _CountryDTO.Name
            });
            await _context.SaveChangesAsync();

            var _ReturnCountry = _context.Countries.Where(c => c.Code == _CountryDTO.Code).Select(c => new CountryDTO()
            {
                Code = c.Code,
                Name = c.Name,
                Id = c.Id
            }).FirstOrDefault();

            return CreatedAtAction(nameof(GetCountry), new { code = _CountryDTO.Code }, _ReturnCountry);
        }
    }
}
