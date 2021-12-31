using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace MisRecetasMT.ViewModels.Usuario
{
    public class UsuarioModel
    {
        [Required(ErrorMessage = "El email es requerido")]
        [EmailAddress(ErrorMessage = "Email invalido")]
        public string Email { get; set; }

        [DisplayName("Contraseña")]
        [Required(ErrorMessage = "La contraseña es requerida")]        
        [StringLength(16, ErrorMessage = "El {0} debe tener al menos {2} y maximo {1} caracteres.", MinimumLength = 8)]
        public string Password { get; set; }

        [Required(ErrorMessage = "El nombre es requerido")]
        [RegularExpression(@"^[a-zA-Z\s]+$", ErrorMessage = "Utilice caracteres solamente")]
        [StringLength(25, ErrorMessage = "El {0} debe tener al menos {2} y maximo {1} caracteres.", MinimumLength = 2)]
        public string Nombre { get; set; }

        [DisplayName("Cédula")]
        [Required(ErrorMessage = "La Cédula es requerida")]
        [RegularExpression("^[0-9]*$", ErrorMessage = "La cédula debe contener solo números")]
        
        [Range(1000, 9000000000000000000, ErrorMessage = "Rango de la cédula invalido")]
        public long Cedula { get; set; }
        public int Rol { get; set; }

        public string Registro { get; set; }

    }
}
