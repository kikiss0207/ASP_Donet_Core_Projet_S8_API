using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Projet_CSharp.Models;

namespace Projet_CSharp.Data
{
    public class Projet_CSharpContext : DbContext
    {
        public Projet_CSharpContext (DbContextOptions<Projet_CSharpContext> options)
            : base(options)
        {
        }

        public DbSet<Projet_CSharp.Models.Continent> Continent { get; set; } = default!;

        public DbSet<Projet_CSharp.Models.Countrie> Countrie { get; set; } = default!;

        public DbSet<Projet_CSharp.Models.Pop> Pop { get; set; } = default!;
    }
}
