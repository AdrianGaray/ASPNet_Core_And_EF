using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using WebNetCore3.Areas.Cursos.Models;

namespace WebNetCore3.Areas.Categorias.Models
{
    public class TCategoria
    {
        [Key]
        public int CategoriaID { get; set; }
        [Required(ErrorMessage = "El campo Categoria es obligatorio.")]
        public string Categoria { get; set; }
        [Required(ErrorMessage = "El campo descripcion es obligatorio.")]
        public string Descripcion { get; set; }
        public Boolean Estado { get; set; } = true;
        public ICollection<TCursos> Cursos { get; set; }
    }
}
