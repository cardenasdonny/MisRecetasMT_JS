using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace MisRecetasMT.Models.Entidades
{
    public class RecetaDetalle
    {
        [Key]
        public int RecetaDetalleId { get; set; }

        public int RecetaId { get; set; }
        public int IngredienteId { get; set; }

        [ForeignKey("RecetaId")]
        public virtual Receta Receta { get; set; }

        [ForeignKey("IngredienteId")]
        public virtual Ingrediente Ingrediente { get; set; }

        public int Cantidad { get; set; }
    }
}
