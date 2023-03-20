using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Projet_CSharp.Data;
using Projet_CSharp.Models;

namespace Projet_CSharp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CountriesController : ControllerBase
    {
        private readonly Projet_CSharpContext _context;

        public CountriesController(Projet_CSharpContext context)
        {
            _context = context;
        }

        // GET: api/Countries
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Countrie>>> GetCountrie()
        {
          if (_context.Countrie == null)
          {
              return NotFound();
          }
            return await _context.Countrie.Include("List_pop").ToListAsync();
        }

        // GET: api/Countries/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Countrie>> GetCountrie(int id)
        {
          if (_context.Countrie == null)
          {
              return NotFound();
          }
            var countrie = await _context.Countrie.Include(c => c.List_pop).FirstOrDefaultAsync(c => c.Id == id);

            if (countrie == null)
            {
                return NotFound();
            }

            return countrie;
        }

        // PUT: api/Countries/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCountrie(int id, Countrie countrie)
        {
            if (id != countrie.Id)
            {
                return BadRequest();
            }

            _context.Entry(countrie).State = EntityState.Modified;

            // Retirer l'attribut Id_Continent de la liste des propriétés à modifier
            _context.Entry(countrie).Property(x => x.Id_Continent).IsModified = false;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CountrieExists(id))
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

        // POST: api/Countries
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        // POST: api/Countries
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Countrie>> PostCountrie(Countrie countrie)
        {
            if (_context.Continent == null)
            {
                return Problem("Entity set 'Projet_CSharpContext.Continent' is null.");
            }

            var continent = await _context.Continent.Include(c => c.List_country).FirstOrDefaultAsync(c => c.Id == countrie.Id_Continent);
            if (continent == null)
            {
                return BadRequest("Continent not found.");
            }

            if (continent.List_country == null)
            {
                continent.List_country = new List<Countrie>();
            }
            continent.List_country.Add(countrie);

            _context.Countrie.Add(countrie);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetCountrie", new { id = countrie.Id }, countrie);
        }

        // DELETE: api/Countries/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCountrie(int id)
        {
            var countrie = await _context.Countrie.FindAsync(id);
            if (countrie == null)
            {
                return NotFound();
            }

            // Supprimer tous les populations liées à ce continent
            var pop = await _context.Pop.Where(c => c.Id_country == id).ToListAsync();
            if (pop.Count > 0)
            {
                _context.Pop.RemoveRange(pop);
            }

            _context.Countrie.Remove(countrie);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool CountrieExists(int id)
        {
            return (_context.Countrie?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
