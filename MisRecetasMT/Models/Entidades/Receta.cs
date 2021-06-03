using MisRecetasMT.Models.Usuario;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace MisRecetasMT.Models.Entidades
{
    public class Receta
    {
        [Key]
        public int RecetaId { get; set; }              
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        [DisplayName("Habilitada/Deshabilitada")]
        public bool Estado { get; set; }
        public DateTime Fecha { get; set; }
        public string NombreImagen { get; set; }
        [DisplayName("Usuario")]
        public string UsuarioId { get; set; }
        public UsuarioIdentity Usuario { get; set; }
        public virtual List<RecetaDetalle> RecetaDetalles { get; set; }
    }
}
