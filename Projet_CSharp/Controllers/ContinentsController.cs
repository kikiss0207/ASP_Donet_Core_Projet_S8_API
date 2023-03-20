using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
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
    public class ContinentsController : ControllerBase
    {
        private readonly Projet_CSharpContext _context;

        public ContinentsController(Projet_CSharpContext context)
        {
            _context = context;
        }

        // GET: api/Continents
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Continent>>> GetContinent()
        {
          if (_context.Continent == null)
          {
              return NotFound();
          }
            return await _context.Continent.Include("List_country.List_pop").ToListAsync();
        }

        // GET: api/Continents/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Continent>> GetContinent(int id)
        {
          if (_context.Continent == null)
          {
              return NotFound();
          }
            var continent = await _context.Continent.Include(c => c.List_country).FirstOrDefaultAsync(c => c.Id == id);

            if (continent == null)
            {
                return NotFound();
            }

            return continent;
        }

        // GET: api/Countries/1/population/2022
        [HttpGet("{id}/population/{year}")]
        public async Task<ActionResult<String>> GetContinentPopulation(int id, int year)
        {
            var countries = await _context.Countrie.Include(c => c.List_pop)
                                           .Where(c => c.Id_Continent == id)
                                           .ToListAsync();
            int population = 0;
            var namem = await _context.Continent
                                .Where(c => c.Id == id)
                                .Select(c => c.Name)
                                .FirstOrDefaultAsync();
            foreach (var country in countries)
            {
                var pop = country.List_pop.FirstOrDefault(p => p.Annee == year);
                if (pop != null)
                {
                    population += pop.Nbre_pop;
                }
            }
            return String.Concat("La population du Continent ", namem, " est de :", population);
        }

        // PUT: api/Continents/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutContinent(int id, Continent continent)
        {
            if (id != continent.Id)
            {
                return BadRequest();
            }

            _context.Entry(continent).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ContinentExists(id))
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

        // POST: api/Continents
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Continent>> PostContinent(Continent continent)
        {
          if (_context.Continent == null)
          {
              return Problem("Entity set 'Projet_CSharpContext.Continent'  is null.");
          }
            _context.Continent.Add(continent);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetContinent", new { id = continent.Id }, continent);
        }

        // DELETE: api/Continents/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteContinent(int id)
        {
            var continent = await _context.Continent.FindAsync(id);
            if (continent == null)
            {
                return NotFound();
            }

            // Supprimer tous les pays liés à ce continent
            var countries = await _context.Countrie.Where(c => c.Id_Continent == id).ToListAsync();
            if (countries.Count > 0)
            {
                _context.Countrie.RemoveRange(countries);
            }

            _context.Continent.Remove(continent);
            await _context.SaveChangesAsync();

            return NoContent();
        }

      


private bool ContinentExists(int id)
        {
            return (_context.Continent?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
