using Microsoft.AspNetCore.Identity;
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
            builder.Seed();


            //create user
            var appUser = new UsuarioIdentity
            {
                Id = "02174cf0–9412–4cfe-afbf-59f706d72cf6",
                Email = "mariaisabel200.trivio@gmail.com",
                NormalizedEmail = "MARIAISABEL200.TRIVIO@GMAIL.COM",
                Cedula = 1234567890,
                UserName = "mariaisabel200.trivio@gmail.com",
                NormalizedUserName = "MARIAISABEL200.TRIVIO@GMAIL.COM",
                EmailConfirmed = true,
                Nombre = "Admin",
                Rol = 1
           };

            //set user password
            PasswordHasher<UsuarioIdentity> ph = new PasswordHasher<UsuarioIdentity>();
            appUser.PasswordHash = ph.HashPassword(appUser, "admin");
            //seed user
            builder.Entity<UsuarioIdentity>().HasData(appUser);

            
            
        }
        public DbSet<UsuarioIdentity> UsuarioIdentity { get; set; }
        public DbSet<Receta> Recetas { get; set; }
        public DbSet<Ingrediente> Ingredientes { get; set; }
        public DbSet<MisRecetasMT.Models.Entidades.TipoIngrediente> TipoIngrediente { get; set; }
        public DbSet<MisRecetasMT.Models.Entidades.RecetaDetalle> RecetaDetalle { get; set; }

    }
}
