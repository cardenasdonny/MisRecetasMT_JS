using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace MisRecetasMT.Models.Entidades
{
    public class Ingrediente
    {
        [Key]
        public int IngredienteId { get; set; }
        [Required(ErrorMessage = "El nombre del ingrediente es requerido")]
        [DisplayName("Nombre del ingrediente")]
        [RegularExpression(@"^[a-zA-Z\s]+$", ErrorMessage = "Utilice caracteres solamente")]
        [StringLength(25, ErrorMessage = "El {0} debe tener al menos {2} y maximo {1} caracteres.", MinimumLength = 2)]
        public string NombreIngrediente { get; set; }        

        [DisplayName("Habilitado/Deshabilitado")]
        public bool EstadoIngrediente { get; set; }
        public string NombreImagen { get; set; }
        [NotMapped]
        [DisplayName("Subir imagen")]
        public  IFormFile Imagen { get; set; }

        [Required(ErrorMessage = "El tipo de ingrediente es requerido")]
        [DisplayName("Tipo de ingrediente")]
        public int TipoIngredienteId { get; set; }        
        public virtual TipoIngrediente TipoIngrediente { get; set; }        
        public virtual List<RecetaDetalle> RecetaDetalles { get; set; }
    }
}
