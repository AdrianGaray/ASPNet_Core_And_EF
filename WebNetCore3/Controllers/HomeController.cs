using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using WebNetCore3.Areas.Cursos.Models;
using WebNetCore3.Data;
using WebNetCore3.Library;
using WebNetCore3.Models;

namespace WebNetCore3.Controllers
{
    public class HomeController : Controller
    {
        private LCursos _curso;
        private static DataPaginador<TCursos> models;
        private static DataCurso _dataCurso;
        private SignInManager<IdentityUser> _signInManager;
        private UserManager<IdentityUser> _userManager;
        private static IdentityError identityError;
        /*public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }*/

        /* Registro de Roles
        IServiceProvider _serviceProvider;
        public HomeController(IServiceProvider serviceProvider)
        {
             _serviceProvider = serviceProvider;
        }

        */

        // se hace inyeccion del Objeto ApplicationDbContext, sobre el constructor
        public HomeController(ApplicationDbContext context, UserManager<IdentityUser> userManager,
            SignInManager<IdentityUser> signInManager, IServiceProvider serviceProvider)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _curso = new LCursos(context, null);
        }

        public IActionResult Index(int id, String filtrar)
        {
            Object[] objects = new Object[3];
            var data = _curso.getTCursos(filtrar);
            if (0 < data.Count)
            {
                var url = Request.Scheme + "://" + Request.Host.Value;
                // Visualiza 10 paginas
                // no se pasa ninguna area
                // "Home" nombre del control
                // "Index" nombre de la accion
                // objects = new LPaginador<TCursos>().paginador(data, id, 10, "", "Home", "Index", url);
                objects = new LPaginador<TCursos>().paginador(data, id, 10, "", "Home", "Index", url);
            }
            else
            {
                // si no se obtiene datos
                objects[0] = "No hay datos que mostrar";
                objects[1] = "No hay datos que mostrar";
                objects[2] = new List<TCursos>();
            }

            models = new DataPaginador<TCursos>
            {
                List = (List<TCursos>)objects[2],
                Pagi_info = (String)objects[0],
                Pagi_navegacion = (String)objects[1],
                Input = new TCursos(),
            };

            if (identityError != null)
            {
                // significa que contiene datos
                models.Pagi_info = identityError.Description;
                identityError = null;
            }
            //await CreateRolesAsync(_serviceProvider);
            return View(models);
        }

        /* Registro de Roles
        public async Task <IActionResult> Index()
        {
            await CreateRolesAsync(_serviceProvider);
            return View();
        }
        */

        public IActionResult Detalles(int id)
        {
            var model = _curso.getTCurso(id);
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Obtener(int cursoID, int vista)
        {
            if (_signInManager.IsSignedIn(User))
            {
                var user = await _userManager.GetUserAsync(User);
                var idUser = await _userManager.GetUserIdAsync(user);

                var data = _curso.Inscripcion(idUser, cursoID);

                if (data.Description.Equals("Done"))
                {
                    // Controlador/ Accion / Area
                    return Redirect("/Inscripciones/Inscripciones/Index?area=Inscripciones");
                }
                else
                {
                    identityError = data;

                    // vista = 1 estamos en la Vista Home/Index Principal.
                    // vista = 2 viene de la Vista Home/Detalle.
                    if (vista.Equals(1))
                    {
                        return Redirect("/Home/Index");
                    }
                    else
                    {
                        _dataCurso = _curso.getTCurso(cursoID);
                        _dataCurso.ErrorMessage = data.Description;

                        return View("Detalles", _dataCurso);
                    }
                }
            }
            else
            {
                return Redirect("/Identity/Account/Login");
            }

        } 

        public IActionResult Privacy()
        {
            return View(models);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        private async Task CreateRolesAsync(IServiceProvider serviceProvider)
        {
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var userManager = serviceProvider.GetRequiredService<UserManager<IdentityUser>>();
            String[] rolesName = { "Admin", "Student" };
            foreach (var item in rolesName)
            {
                // Registra los roles del array roleNmae y lo guarda en la tabla AspNetRole
                var roleExist = await roleManager.RoleExistsAsync(item);
                if (!roleExist)
                {
                    await roleManager.CreateAsync(new IdentityRole(item));
                }
            }

            // id del usuario de la tabla AspNetUsers, para proporcinarle un rol
            //var user = await userManager.FindByIdAsync("6a5fc1bb-3aa1-4d0f-91c6-a23f057c8592");
            var user = await userManager.FindByIdAsync("1185b098-8841-4ea0-b9ec-8a8ff9aad518");
            // 1185b098-8841-4ea0-b9ec-8a8ff9aad518
            await userManager.AddToRoleAsync(user, "Admin");
        }
    }
}
