using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Cors;
using Microsoft.EntityFrameworkCore;
using MyMovies.Models;

namespace MyMovies.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [EnableCors("AllowOrigin")]
    public class MovieItemsController : ControllerBase
    {
        private readonly MovieContext _context;

        public MovieItemsController(MovieContext context)
        {
            _context = context;
        }

        // GET: api/MovieItems
        // Showing all values from Database
        [HttpGet]
        public async Task<ActionResult<IEnumerable<MovieItem>>> GetMovieItems()
        {
            return await _context.MovieItems.ToListAsync();
        }

        // GET: api/MovieItems/5
        // Showing existing value from Database
        [HttpGet("{id}")]
        public async Task<ActionResult<MovieItem>> GetMovieItem(long id)
        {
            var movieItem = await _context.MovieItems.FindAsync(id);

            if (movieItem == null)
            {
                return NotFound();
            }

            return movieItem;
        }

        // PUT: api/MovieItems/5
        // Editing existing value from Database
        [HttpPut("{id}")]
        public async Task<IActionResult> PutMovieItem(long id, MovieItem movieItem)
        {
            if (id != movieItem.Id)
            {
                return BadRequest();
            }

            _context.Entry(movieItem).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!MovieItemExists(id))
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

        // POST: api/MovieItems
        // Adding new Movie Value to Database
        [HttpPost]
        public async Task<ActionResult<MovieItem>> PostMovieItem(MovieItem movieItem)
        {

            _context.MovieItems.Add(movieItem);
            await _context.SaveChangesAsync();

            return CreatedAtAction("PostMovieItem", new { id = movieItem.Id }, movieItem);
        }

        // DELETE: api/MovieItems/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<MovieItem>> DeleteMovieItem(long id)
        {
            var movieItem = await _context.MovieItems.FindAsync(id);
            if (movieItem == null)
            {
                return NotFound();
            }

            _context.MovieItems.Remove(movieItem);
            await _context.SaveChangesAsync();

            return movieItem;
        }

        private bool MovieItemExists(long id)
        {
            return _context.MovieItems.Any(e => e.Id == id);
        }
    }
}
