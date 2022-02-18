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
        public async Task<ActionResult<IEnumerable<StateDTO>>> GetStates()
        {
            return await _context.States.Select(s => new StateDTO()
            {
                Code = s.Code,
                Name = s.Name,
                Id = s.Id
            }).ToListAsync();
        }

        // POST: api/states
        // add new state
        [HttpPost]
        public async Task<ActionResult<StateDTO>> PostState(StateDTO _StateDTO)
        {
            _context.States.Add(new State(){
                Code = _StateDTO.Code,
                Name = _StateDTO.Name,
                CountryId = _StateDTO.CountryId
            });
            await _context.SaveChangesAsync();

            var _ReturnState = _context.States.Where(s => s.Code == _StateDTO.Code).Select(s => new StateDTO()
            {
                Code = s.Code,
                Name = s.Name,
                Id = s.Id
            }).FirstOrDefault();

            return CreatedAtAction(nameof(GetStates), new { code = _StateDTO.Code }, _ReturnState);
        }

    }
}
