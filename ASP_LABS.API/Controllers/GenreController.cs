using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ASP_LABS.API.Data;
using ASP_LABS.Domain.Entities;
using ASP_LABS.API.Services.GenreService;
using ASP_LABS.Domain.Models;
using Microsoft.AspNetCore.Authorization;

namespace ASP_LABS.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GenreController : ControllerBase
    {
        private readonly IGenreService _service;

        public GenreController(IGenreService service)
        {
			_service = service;
        }

		// GET: api/Genre
		[HttpGet]
        public async Task<ResponseData<ListModel<Genre>>> GetGenreSet()
        {
            var response = await _service.GetGenreListAsync();
            return response;
        }
/*
        // GET: api/Genre/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Genre>> GetGenre(int id)
        {
          if (_context.GenreSet == null)
          {
              return NotFound();
          }
            var genre = await _context.GenreSet.FindAsync(id);

            if (genre == null)
            {
                return NotFound();
            }

            return genre;
        }

        // PUT: api/Genre/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutGenre(int id, Genre genre)
        {
            if (id != genre.Id)
            {
                return BadRequest();
            }

            _context.Entry(genre).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!GenreExists(id))
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

        // POST: api/Genre
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Genre>> PostGenre(Genre genre)
        {
          if (_context.GenreSet == null)
          {
              return Problem("Entity set 'AppDbContext.GenreSet'  is null.");
          }
            _context.GenreSet.Add(genre);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetGenre", new { id = genre.Id }, genre);
        }

        // DELETE: api/Genre/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteGenre(int id)
        {
            if (_context.GenreSet == null)
            {
                return NotFound();
            }
            var genre = await _context.GenreSet.FindAsync(id);
            if (genre == null)
            {
                return NotFound();
            }

            _context.GenreSet.Remove(genre);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool GenreExists(int id)
        {
            return (_context.GenreSet?.Any(e => e.Id == id)).GetValueOrDefault();
        }*/
    }
}
