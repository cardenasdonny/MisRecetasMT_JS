using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MisRecetasMT.Models.Entidades
{
    public class Usuario
    {
        [Key]    
        public int UsuarioId { get; set; }

        [Required(ErrorMessage = "La cédula es requerida")]
        //[RegularExpression("^[0-9]*$", ErrorMessage = "La cédula debe contener solo números")]
        public int Cedula { get; set; }
        
        [Required(ErrorMessage = "El nombre es requerido")]
        public string Nombre { get; set; }
        
        [Required(ErrorMessage = "El email es requerido")]
        [EmailAddress(ErrorMessage = "Email invalido")]
        public string Email { get; set; }
        
        [Required(ErrorMessage = "El password es requerido")]
        [StringLength(16, ErrorMessage = "El {0} debe tener al menos {2} y maximo {1} caracteres.", MinimumLength = 8)]
        public string Password { get; set; }
    }
}
