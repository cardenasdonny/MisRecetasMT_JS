using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MisRecetasMT.Models.AccesoDB;
using MisRecetasMT.Models.Entidades;
using MisRecetasMT.Models.Usuario;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MisRecetasMT.Models
{
    public static class ModelBuilderExtensions
    {       

        public static void Seed(this ModelBuilder modelBuilder)
        {

            modelBuilder.Entity<TipoIngrediente>().HasData(
                

                new TipoIngrediente
                {
                    TipoIngredienteId = 1,
                    EstadoTipoIngrediente = true,
                    NombreTipoIngrediente = "Verduras"
                },
                new TipoIngrediente
                {
                    TipoIngredienteId = 2,
                    EstadoTipoIngrediente = true,
                    NombreTipoIngrediente = "Carnes frías"
                },
                new TipoIngrediente
                {
                    TipoIngredienteId = 3,
                    EstadoTipoIngrediente = true,
                    NombreTipoIngrediente = "Lácteos"
                }
            ); 

            modelBuilder.Entity<Ingrediente>().HasData(
                
                new Ingrediente
                {
                    IngredienteId = 1,
                    TipoIngredienteId = 1,
                    EstadoIngrediente = true,
                    NombreIngrediente = "Cebolla",    
                },
                new Ingrediente
                {
                    IngredienteId = 2,
                    TipoIngredienteId = 1,
                    EstadoIngrediente = true,
                    NombreIngrediente = "Papa",
                },
                new Ingrediente
                {
                    IngredienteId = 3,
                    TipoIngredienteId = 2,
                    EstadoIngrediente = true,
                    NombreIngrediente = "Salchicha",
                },
                new Ingrediente
                {
                    IngredienteId = 4,
                    TipoIngredienteId = 3,
                    EstadoIngrediente = true,
                    NombreIngrediente = "Queso",
                },
                new Ingrediente
                {
                    IngredienteId =5,
                    TipoIngredienteId = 3,
                    EstadoIngrediente = true,
                    NombreIngrediente = "Leche",
                },
                new Ingrediente
                {
                    IngredienteId = 6,
                    TipoIngredienteId = 2,
                    EstadoIngrediente = true,
                    NombreIngrediente = "Salchichón",
                }

            );
        }
    }
}
