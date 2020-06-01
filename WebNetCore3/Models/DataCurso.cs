using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebNetCore3.Models
{
    // se utiliaz para almacenar la informacion que viene de la tabla curso y categiria. Dependiendo del curso seleccionado
    public class DataCurso
    {
        public int CursoID { get; set; }
        public string Curso { get; set; }
        public string Informacion { get; set; }
        public byte Horas { get; set; }
        public decimal Costo { get; set; }
        public Boolean Estado { get; set; }
        public byte[] Image { get; set; }
        public string Categoria { get; set; }

        public string ErrorMessage { get; set; }
    }
}
