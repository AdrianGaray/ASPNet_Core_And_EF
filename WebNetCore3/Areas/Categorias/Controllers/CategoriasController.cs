using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using WebNetCore3.Areas.Categorias.Models;
using WebNetCore3.Controllers;
using WebNetCore3.Data;
using WebNetCore3.Library;
using WebNetCore3.Models;

namespace WebNetCore3.Areas.Categorias.Controllers
{
    [Area("Categorias")]
    [Authorize(Roles = "Admin")]
    public class CategoriasController : Controller
    {
        private LCategorias _lcategoria;
        private SignInManager<IdentityUser> _signInManager;
        private static DataPaginador<TCategoria> models;
        private static IdentityError identityError;

        public CategoriasController(ApplicationDbContext context, 
                                    SignInManager<IdentityUser> signInManager)
        {
            _signInManager = signInManager;
            _lcategoria = new LCategorias(context);
        }
        
        [Authorize(Roles = "Admin")]
        public IActionResult Index(int id, String Search, int Registros)
        {
            if (_signInManager.IsSignedIn(User))
            {
                Object[] objects = new Object[3];
                var data = _lcategoria.getTCategoria(Search);

                if (0 < data.Count)
                {
                    var url = Request.Scheme + "://" + Request.Host.Value;
                    objects = new LPaginador<TCategoria>().paginador(_lcategoria.getTCategoria(Search), 
                            id, Registros, "Categorias", "Categorias/Categorias", "Index", url);
                }
                else
                {
                    objects[0] = "No hay datos que mostrar";
                    objects[1] = "No hay datos que mostrar";
                    objects[2] = new List<TCategoria>();
                }

                models = new DataPaginador<TCategoria>
                {
                    List = (List<TCategoria>)objects[2],
                    Pagi_info = (String)objects[0],
                    Pagi_navegacion = (String)objects[1],
                    Input = new TCategoria()
                };
                return View(models);
            }
            else
            {
                //return RedirectToAction(nameof(HomeController.Index), "Home");
                return Redirect("/Home/Index");
            }
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public String GetCategorias(DataPaginador<TCategoria> model)
        {
            if (model.Input.Categoria != null && model.Input.Descripcion != null)
            {
                var data = _lcategoria.RegistrarCategoria(model.Input);
                return JsonConvert.SerializeObject(data);
                //return "Hola";
            }
            else
            {
                return "Llene los campos requeridos";
            }
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public IActionResult UpdateEstado(int id)
        {
            identityError = _lcategoria.UpdateEstado(id);
            return Redirect("/Categorias/Categorias/Index?area=Categorias");
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public String EliminarCategoria(int CategoriaID)
        {
            identityError = _lcategoria.DeleteCategoria(CategoriaID);
            return JsonConvert.SerializeObject(identityError);
        }
    }
}