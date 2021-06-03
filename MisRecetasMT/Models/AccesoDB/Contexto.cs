using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using MisRecetasMT.Models.Entidades;
using MisRecetasMT.Models.Usuario;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MisRecetasMT.Models.AccesoDB
{
    public class Contexto: IdentityDbContext
    {
        public Contexto(DbContextOptions<Contexto> options) : base(options)
        {

        }
        protected override void OnModelCreating(ModelBuilder builder)
        {            
            builder.Entity<RecetaDetalle>().HasIndex(s => s.IngredienteId).IsUnique(false);
            builder.Entity<RecetaDetalle>().HasIndex(s => s.RecetaId).IsUnique(false);
            base.OnModelCreating(builder);
        }
        public DbSet<UsuarioIdentity> UsuarioIdentity { get; set; }
        public DbSet<Receta> Recetas { get; set; }
        public DbSet<Ingrediente> Ingredientes { get; set; }
        public DbSet<MisRecetasMT.Models.Entidades.TipoIngrediente> TipoIngrediente { get; set; }
        public DbSet<MisRecetasMT.Models.Entidades.RecetaDetalle> RecetaDetalle { get; set; }

    }
}
