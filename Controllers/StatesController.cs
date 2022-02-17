#nullable disable
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TodoApi.Models;

namespace TodoApi.Controllers
{
    [Route("api/states")]
    [ApiController]
    public class StatesController : ControllerBase
    {
        private readonly StatesContext _context;

        public StatesController(StatesContext context)
        {
            _context = context;
        }

        // GET: api/TodoItems
        [HttpGet]
        public async Task<ActionResult<IEnumerable<StatesItem>>> GetStates()
        {
            return await _context.StatesItems.ToListAsync();
        }

        // GET: api/TodoItems/5
        [HttpGet("{id}")]
        public async Task<ActionResult<StatesItem>> GetTodoItem(long id)
        {
            var StatesItem = await _context.StatesItems.FindAsync(id);

            if (StatesItem == null)
            {
                return NotFound();
            }

            return StatesItem;
        }

        // PUT: api/TodoItems/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutStatesItem(long id, StatesItem StatesItem)
        {
            if (id != StatesItem.Id)
            {
                return BadRequest();
            }

            _context.Entry(StatesItem).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!StatesItemExists(id))
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

        // POST: api/TodoItems
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<StatesItem>> PostStatesItem(StatesItem StatesItem)
        {
            _context.StatesItems.Add(StatesItem);
            await _context.SaveChangesAsync();

            //return CreatedAtAction("GetTodoItem", new { id = todoItem.Id }, todoItem);
            return CreatedAtAction(nameof(GetStates), new { id = StatesItem.Id }, StatesItem);
        }

        // DELETE: api/TodoItems/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteStatesItem(long id)
        {
            var StatesItem = await _context.StatesItems.FindAsync(id);
            if (StatesItem == null)
            {
                return NotFound();
            }

            _context.StatesItems.Remove(StatesItem);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool StatesItemExists(long id)
        {
            return _context.StatesItems.Any(e => e.Id == id);
        }
    }
}
