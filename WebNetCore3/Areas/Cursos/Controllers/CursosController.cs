using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using WebNetCore3.Areas.Cursos.Models;
using WebNetCore3.Data;
using WebNetCore3.Library;
using WebNetCore3.Models;

namespace WebNetCore3.Areas.Cursos.Controllers
{
    [Area("Cursos")]
    [Authorize(Roles = "Admin")]
    public class CursosController : Controller
    {
        private LCursos _curso;
        private LCategorias _lcategoria;
        private SignInManager<IdentityUser> _signInManager;
        private static DataPaginador<TCursos> models;
        private static IdentityError identityError;

        public CursosController(ApplicationDbContext context, SignInManager<IdentityUser> signInManager, IWebHostEnvironment environment)
        {
            _signInManager = signInManager;
            _lcategoria = new LCategorias(context);
            _curso = new LCursos ( context, environment );
        }

        [Authorize(Roles = "Admin")]
        public IActionResult Index(int id, String Search, int Registros)
        {
            if (_signInManager.IsSignedIn(User))
            {
                Object[] objects = new Object[3];
                var data = _curso.getTCursos(Search);
                if (0 < data.Count)
                {
                    var url = Request.Scheme + "://" + Request.Host.Value;
                    objects = new LPaginador<TCursos>().paginador(data
                     , id, Registros, "Cursos", "Cursos/Cursos", "Index", url);
                }
                else
                {
                    objects[0] = "No hay datos que mostrar";
                    objects[1] = "No hay datos que mostrar";
                    objects[2] = new List<TCursos>();
                }

                models = new DataPaginador<TCursos>
                {
                    List = (List<TCursos>)objects[2],
                    Pagi_info = (String)objects[0],
                    Pagi_navegacion = (String)objects[1],
                    Categorias = _lcategoria.getTCategoria(),
                    Input = new TCursos()
                };

                if (identityError != null)
                {
                    models.Pagi_info = identityError.Description;
                    identityError = null;
                }

                return View(models);
            }
            else
            {
                return Redirect("/Home/Index");
            }
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public String GetCurso(DataPaginador<TCursos> model)
        {
            if (model.Input.Curso != null && model.Input.Informacion != null && model.Input.CategoriaID > 0)
            {
                if (model.Input.Horas.Equals(0))
                {
                    return "Ingrese la cantidad de horas del curso.";
                }
                else
                {
                    if (model.Input.Equals(0.00M))
                    {
                        return "Ingrese el costo del curso.";
                    }
                    else
                    {
                        var data = _curso.RegistrarCursoAsync(model);
                        return JsonConvert.SerializeObject(data.Result);
                    }
                }
            }
            else
            {
                return "Llene los campos requeridos."; 
            }
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public IActionResult UpdateEstado(int id)
        {
            identityError = _curso.UpdateEstado(id);
            //return Redirect("/Cursos/Cursos?area=Cursos");
            return Redirect("/Cursos/Cursos/Index?area=Cursos");
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public String EliminarCurso(int CursoID)
        {
            identityError = _curso.DeleteCurso(CursoID);
            return JsonConvert.SerializeObject(identityError);
        }
    }
}