using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MyMvcMovie.Data;
using MyMvcMovie.Models;
using PagedList;

namespace MyMvcMovie.Controllers
{
    public class MoviesController : Controller
    {
        private readonly MyMvcMovieContext _context;

        public MoviesController(MyMvcMovieContext context)
        {
            _context = context;
        }

		// GET: Movies
		public async Task<IActionResult> Index(string sortOrder, string searchString)
		{
			// Сохраняем текущий поиск в ViewData, чтобы он не пропадал после сортировки
			ViewData["CurrentFilter"] = searchString;

			// Параметры для переключения сортировки
			ViewData["TitleSort"] = string.IsNullOrEmpty(sortOrder) ? "title_desc" : "";
			ViewData["DateSort"] = sortOrder == "Date" ? "date_desc" : "Date";
			ViewData["GenreSort"] = sortOrder == "Genre" ? "genre_desc" : "Genre";

			var movies = from m in _context.Movie
						 select m;

			// Применяем фильтрацию (поиск)
			if (!string.IsNullOrEmpty(searchString))
			{
				movies = movies.Where(s => s.Title!.ToUpper().Contains(searchString.ToUpper()));
			}

			// Применяем сортировку
			switch (sortOrder)
			{
				case "title_desc": movies = movies.OrderByDescending(m => m.Title); break;
				case "Date": movies = movies.OrderBy(m => m.ReleaseDate); break;
				case "date_desc": movies = movies.OrderByDescending(m => m.ReleaseDate); break;
				case "Genre": movies = movies.OrderBy(m => m.Genre); break;
				case "genre_desc": movies = movies.OrderByDescending(m => m.Genre); break;
				default: movies = movies.OrderBy(m => m.Title); break;
			}

			return View(await movies.AsNoTracking().ToListAsync());
		}

		// GET: Movies/Details/5
		public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var movie = await _context.Movie
                .FirstOrDefaultAsync(m => m.Id == id);
            if (movie == null)
            {
                return NotFound();
            }

            return View(movie);
        }

        // GET: Movies/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Movies/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Title,ReleaseDate,Genre,URL,Brief,Poster,photo, Price, Rating")] Movie movie)
        {
            if (ModelState.IsValid)
            {
                _context.Add(movie);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(movie);
        }

        // GET: Movies/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var movie = await _context.Movie.FindAsync(id);
            if (movie == null)
            {
                return NotFound();
            }
            return View(movie);
        }

        // POST: Movies/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Title,ReleaseDate,Genre,URL,Brief,Poster,photo, Price, Rating")] Movie movie)
        {
            if (id != movie.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(movie);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MovieExists(movie.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(movie);
        }

        // GET: Movies/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var movie = await _context.Movie
                .FirstOrDefaultAsync(m => m.Id == id);
            if (movie == null)
            {
                return NotFound();
            }

            return View(movie);
        }

        // POST: Movies/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var movie = await _context.Movie.FindAsync(id);
            if (movie != null)
            {
                _context.Movie.Remove(movie);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool MovieExists(int id)
        {
            return _context.Movie.Any(e => e.Id == id);
        }
    }
}
