using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using WebNetCore3.Areas.Categorias.Models;
using WebNetCore3.Areas.Cursos.Models;
using WebNetCore3.Areas.Inscripciones.Models;

namespace WebNetCore3.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<TCategoria> _TCategoria { get; set; }
        public DbSet<TCursos> _TCursos { get; set; }
        public DbSet<Inscripcion> _TInscripcion { get; set; }
    }
}
