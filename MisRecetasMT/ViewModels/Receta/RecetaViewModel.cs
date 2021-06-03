using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace MisRecetasMT.ViewModels.Receta
{
    public class RecetaViewModel
    {
        public int RecetaId { get; set; }
        public List<Ingredientes> Ingredientes { get; set; }        
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public string NombreImagen { get; set; }
        public IFormFile Imagen { get; set; }
        public bool Estado { get; set; }
        public string UsuarioId { get; set; }
    }
    
    public class Ingredientes
    {
        public int IngredienteId { get; set; }
        public int Cantidad { get; set; }
    }
    
}
