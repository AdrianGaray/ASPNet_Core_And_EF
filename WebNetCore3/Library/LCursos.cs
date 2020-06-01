using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebNetCore3.Areas.Cursos.Models;
using WebNetCore3.Areas.Inscripciones.Models;
using WebNetCore3.Data;
using WebNetCore3.Models;

namespace WebNetCore3.Library
{
    public class LCursos
    {
        private Uploadimage _image;
        private ApplicationDbContext context;
        private IWebHostEnvironment environment;

        public LCursos(ApplicationDbContext context, IWebHostEnvironment environment)
        {
            this.context = context;
            this.environment = environment;
            _image = new Uploadimage();
        }

        // Metodo que retorna una tarea
        // la palabra "async" significa que este metodo se va a sincronizar con otro metodo que va a realizar 
        // una tarea indicando que este metodo tiene q esperar y sincronizarce hasta que ese metodo termine 
        // la tarea este metodo va a proseguir ejecutanto su procedimiento
        public async Task<IdentityError> RegistrarCursoAsync(DataPaginador<TCursos> model)
        {
            IdentityError identityError;
            try
            {
                if (model.Input.CursoID.Equals(0))
                {
                    // MODO DE RESGISTRAR UN CURSO
                    // await le indica que este metodo tiene que ser esperado por
                    var imageByte = await _image.ByteAvatarImageAsync(model.AvatarImage, environment);
                    var curso = new TCursos
                    {
                        Curso = model.Input.Curso,
                        Informacion = model.Input.Informacion,
                        Horas = model.Input.Horas,
                        Costo = model.Input.Costo,
                        Estado = model.Input.Estado,
                        Image = imageByte,
                        CategoriaID = model.Input.CategoriaID
                    };

                    // agrega un curso a la tabla TCurso de la BD.
                    context.Add(curso);
                    context.SaveChanges();
                }
                else
                {
                    // MODO DE ACTUALIZAR UN CURSO
                    byte[] imageByte;
                    if (model.AvatarImage != null)
                    {
                        // contriene una imagen que hemos cargado
                        // y se actualiza la imagen del curso
                        imageByte = await _image.ByteAvatarImageAsync(model.AvatarImage, environment);
                    }
                    else
                    {
                        // no se actuliza la imagen del curso
                        // y le propocionamos la misma imagen
                        imageByte = model.Input.Image;
                    }
                    var curso = new TCursos
                    {
                        CursoID = model.Input.CursoID,
                        Curso = model.Input.Curso,
                        Informacion = model.Input.Informacion,
                        Horas = model.Input.Horas,
                        Costo = model.Input.Costo,
                        Estado = model.Input.Estado,
                        Image = imageByte,
                        CategoriaID = model.Input.CategoriaID
                    };

                    // Actualiza el curso en la BD
                    context.Update(curso);
                    context.SaveChanges();
                }

                identityError = new IdentityError { Code = "Done" };
            }
            catch (Exception e)
            {
                identityError = new IdentityError
                {
                    Code = "Error",
                    Description = e.Message
                };
            }
            return identityError;
        }

        internal List<TCursos> getTCursos(string search)
        {
            List<TCursos> listCursos;
            if (search == null)
            {
                listCursos = context._TCursos.ToList();
            }
            else
            {
                listCursos = context._TCursos.Where(c => c.Curso.StartsWith(search)).ToList();
            }
            return listCursos;
        }

        internal IdentityError UpdateEstado(int id)
        {
            IdentityError identityError;
            try
            {
                var curso = context._TCursos.Where(c => c.CursoID.Equals(id)).ToList().ElementAt(0);
                curso.Estado = curso.Estado ? false : true;
                context.Update(curso);
                context.SaveChanges();
                identityError = new IdentityError { Description = "Done" };
            }
            catch (Exception e)
            {
                identityError = new IdentityError
                {
                    Code = "Error",
                    Description = e.Message
                };
            }
            return identityError;
        }

        internal IdentityError DeleteCurso(int cursoID)
        {
            IdentityError identityError;
            try
            {
                var curso = new TCursos
                {
                    CursoID = cursoID
                };
                context.Remove(curso);
                context.SaveChanges();
                identityError = new IdentityError { Description = "Done" };
            }
            catch (Exception e)
            {
                identityError = new IdentityError
                {
                    Code = "Error",
                    Description = e.Message
                };
            }
            return identityError;
        }

        // metodo que llamada desde la pantalla de detalle
        public DataCurso getTCurso(int id)
        {
            DataCurso dataCurso = null;
            // se crea la consulta a dos tablas relacionadas
            var query = context._TCategoria.Join(context._TCursos,
                c => c.CategoriaID,
                t => t.CategoriaID,
                (c, t) => new {
                    c.CategoriaID,
                    c.Categoria,
                    t.CursoID,
                    t.Curso,
                    t.Informacion,
                    t.Horas,
                    t.Costo,
                    t.Estado,
                    t.Image
                }).Where(d => d.CursoID.Equals(id)).ToList();
            
            // verifica si tiene registros
            if (!query.Count.Equals(0))
            {
                // se obtiene el ultimo objeto de la coleccion
                var data = query.Last();
                dataCurso = new DataCurso
                {
                    CursoID = data.CursoID,
                    Curso = data.Curso,
                    Informacion = data.Informacion,
                    Horas = data.Horas,
                    Costo = data.Costo,
                    Estado = data.Estado,
                    Image = data.Image,
                    Categoria = data.Categoria,
                };
            }

            return dataCurso;
        }

        public IdentityError Inscripcion(string idUser, int cursoID)
        {
            IdentityError identityError;
            try
            {
                var cursoInscripcion = context._TInscripcion.Where(
                    c => c.CursoID.Equals(cursoID) &&
                    c.EstudianteID.Equals(idUser)).ToList();

                if (cursoInscripcion.Count.Equals(0))
                {
                    var curso = getTCurso(cursoID);
                    var inscripcion = new Inscripcion
                    {
                        EstudianteID = idUser,
                        Fecha = DateTime.Now,
                        Pago = curso.Costo,
                        CursoID = curso.CursoID
                    };

                    context.Add(inscripcion);
                    context.SaveChanges();

                    identityError = new IdentityError { Description = "Done" };
                }
                else
                {
                    identityError = new IdentityError { Description = "Ya está suscrito en el curso" };
                }
            }
            catch (Exception e)
            {
                identityError = new IdentityError
                {
                    Code = "Error",
                    Description = e.Message
                };
            }
            return identityError;
        }

        public List<DataCurso> Inscripciones(string idUser, String search)
        {
            List<DataCurso> cursos = new List<DataCurso>();
            
            var inscripciones = context._TInscripcion.Where(c => c.EstudianteID.Equals(idUser)).ToList();
           
            if (!inscripciones.Count.Equals(0))
            {
                inscripciones.ForEach(c => {
                    if (search == null || search.Equals(""))
                    {
                        var query = context._TCategoria.Join(context._TCursos,
                            c => c.CategoriaID,
                            t => t.CategoriaID,
                            (c, t) => new {
                                c.CategoriaID,
                                c.Categoria,
                                t.CursoID,
                                t.Curso,
                                t.Informacion,
                                t.Horas,
                                t.Costo,
                                t.Estado,
                                t.Image
                            }).Where(d => d.CursoID.Equals(c.CursoID)).ToList();
                        
                        if (!query.Count.Equals(0))
                        {
                            // estamos obteniendo datos de la query
                            var data = query.Last();
                            
                            cursos.Add(new DataCurso
                            {
                                CursoID = data.CursoID,
                                Curso = data.Curso,
                                Informacion = data.Informacion,
                                Horas = data.Horas,
                                Costo = data.Costo,
                                Estado = data.Estado,
                                Image = data.Image,
                                Categoria = data.Categoria,
                            });
                        }
                    }
                    else
                    {
                        var query = context._TCategoria.Join(context._TCursos,
                            c => c.CategoriaID,
                            t => t.CategoriaID,
                            (c, t) => new
                            {
                                c.CategoriaID,
                                c.Categoria,
                                t.CursoID,
                                t.Curso,
                                t.Informacion,
                                t.Horas,
                                t.Costo,
                                t.Estado,
                                t.Image
                            }).Where(d => d.CursoID.Equals(c.CursoID) && d.Curso.StartsWith(search)).ToList();
                        
                        if (!query.Count.Equals(0))
                        {
                            var data = query.Last();
                            
                            cursos.Add(new DataCurso
                            {
                                CursoID = data.CursoID,
                                Curso = data.Curso,
                                Informacion = data.Informacion,
                                Horas = data.Horas,
                                Costo = data.Costo,
                                Estado = data.Estado,
                                Image = data.Image,
                                Categoria = data.Categoria,
                            });
                        }
                    }
                });
            }
            return cursos;
        }
    }
}
