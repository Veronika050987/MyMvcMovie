using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MyMvcMovie.Models;

namespace MyMvcMovie.Data
{
    public class MyMvcMovieContext : DbContext
    {
        public MyMvcMovieContext (DbContextOptions<MyMvcMovieContext> options)
            : base(options)
        {
        }

        public DbSet<MyMvcMovie.Models.Movie> Movie { get; set; } = default!;
    }
}
