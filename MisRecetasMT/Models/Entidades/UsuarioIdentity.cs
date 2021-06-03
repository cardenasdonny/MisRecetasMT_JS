using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MisRecetasMT.Models.Usuario
{
    public class UsuarioIdentity: IdentityUser
    {
        
        [Required(ErrorMessage = "La cédula es requerida")]
        [RegularExpression("^[0-9]*$", ErrorMessage = "La cédula debe contener solo números")] 
        public long Cedula { get; set; }     

        [Required(ErrorMessage = "El nombre es requerido")]
        [StringLength(20, ErrorMessage = "El {0} debe tener al menos {2} y maximo {1} caracteres.", MinimumLength = 8)]
        public string Nombre { get; set; }
        public int Rol { get; set; }
    }
}
