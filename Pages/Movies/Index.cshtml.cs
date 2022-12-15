using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using RazorPagesMovie.Models;

namespace RazorPagesMovie.Pages.Movies
{
    public class IndexModel : PageModel
    {
        private readonly RazorPagesMovieContext _context;

        public IndexModel(RazorPagesMovieContext context)
        {
            _context = context;
        }

        public IList<Movie> Movie { get;set; } = default!;
        [BindProperty(SupportsGet = true)]
        public string? TitleFilter { get; set; }
        public SelectList? Genres { get; set; }
        [BindProperty(SupportsGet = true)]
        public string? GenreFilter { get; set; }


        public async Task OnGetAsync()
        {
            var movies = from m in _context.Movie
                         select m;
            IQueryable<string> genreQuery = from m in _context.Movie
                                            orderby m.Genre
                                            select m.Genre;

            if(!string.IsNullOrEmpty(TitleFilter)) {
                movies = movies.Where(s => s.Title.ToLower().Contains(TitleFilter.ToLower()));
            }

            if(!string.IsNullOrEmpty(GenreFilter)) {
                movies = movies.Where(x => x.Genre == GenreFilter);
            }
            Genres = new SelectList(await genreQuery.Distinct().ToListAsync());
            Movie = await movies.ToListAsync();
        }
    }
}
