using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MisRecetasMT.Models.Entidades
{
    public class TipoIngrediente
    {
        [Key]
        public int TipoIngredienteId { get; set; }  
        
        [DisplayName("Tipo de ingrediente")]
        [Required(ErrorMessage = "El nombre del tipo de ingrediente es requerido")]
        [RegularExpression(@"^[a-zA-Z\s]+$", ErrorMessage = "Utilice caracteres solamente")]
        [StringLength(25, ErrorMessage = "El {0} debe tener al menos {2} y maximo {1} caracteres.", MinimumLength = 2)]
        public string NombreTipoIngrediente { get; set; }
        
        [DisplayName("Habilitado/Deshabilitado")]
        public bool EstadoTipoIngrediente { get; set; }
        
        //public virtual List<Ingrediente> Ingredientes { get; set; }

    }
}
