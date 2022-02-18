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
    [Route("api/states")]
    [ApiController]
    public class StateController : ControllerBase
    {
        private readonly CountryContext _context;

        public StateController(CountryContext context)
        {
            _context = context;
        }

        // GET: api/states
        // list of all states
        [HttpGet]
        public async Task<ActionResult<IEnumerable<State>>> GetStates()
        {
            return await _context.States.ToListAsync();
        }

        // POST: api/states
        // add new state
        [HttpPost]
        public async Task<ActionResult<State>> PostState(State state)
        {
            _context.States.Add(state);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetStates), new { id = state.Id }, state);
        }

    }
}
