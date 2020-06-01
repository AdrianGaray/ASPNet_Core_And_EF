using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using WebNetCore3.Areas.Cursos.Models;

namespace WebNetCore3.Areas.Inscripciones.Models
{
    public class Inscripcion
    {
        [Key]
        public int InscripcionID { get; set; }
        public string EstudianteID { get; set; }
        public DateTime Fecha { get; set; }
        public Decimal Pago { get; set; }
        public int CursoID { get; set; }
        public TCursos Curso { get; set; }
    }
}
