class Cursos extends Uploadpicture {
    constructor() {
        super(); // hace referencia al metodo cosntructor de la clase Uploadpicture
        this.Image = null;
        this.CursoID = 0;
    }
    
    RegistrarCurso() {

        var data = new FormData(); // formatea los datos, que se obtiene del modal. Y se guarda como una coleccion.
        // -- Coleccion de Datos, donde almacenamos las siguientes propiedades
        data.append('Input.Curso', $("#curNombre").val());
        data.append('Input.Informacion', $("#curDescripcion").val());
        data.append('Input.Horas', $("#curHoras").val());
        data.append('Input.Costo', $("#curCosto").val());
        //data.append('Input.Estado', $("#curEstado").checked);
        data.append('Input.Estado', document.getElementById("curEstado").checked);
        data.append('Input.CategoriaID', $("#curCatargoria").val());

        console.log($("#curCatargoria").val());

        data.append('Input.Image', this.Image);
        data.append('Input.CursoID', $("#curCursoID").val());

        console.log(data);
        
        $.each($('input[type=file]')[0].files, (i, file) => {
            data.append('AvatarImage', file);
        });

        if (window.location.pathname.includes("Index"))
        {
            $.ajax({
                url: "GetCurso", 
                data: data,                
                cache: false,
                contentType: false,
                processData: false,
                type: 'POST', 
                success: (result) => { 
                    try {                       
                        var item = JSON.parse(result);
                        if (item.Code == "Done") {
                            //window.location.href = "Cursos";
                            window.location.href = "Index";
                        } else {
                            document.getElementById("mensaje").innerHTML = item.Description;
                        }
                    } catch (e) {
                        document.getElementById("mensaje").innerHTML = result;
                    }
                    console.log(result);
                }
            });
        }
        else {
            // --
            // se invoca el metodo AJAX. Se envia al servidor por POST
            $.ajax({
                url: "Cursos/GetCurso", // Nombre de metodo de accion que esta en CursoController
                data: data, // Informacion que vamos a enviar por POST
                // como se envia la informacion de imagenes se desabilita estas opciones
                cache: false,
                contentType: false,
                processData: false,
                // --
                type: 'POST', // peticion por POST
                // se asigna a una informacion anonima
                success: (result) => { // esta funciona obtiene el resultado que no envie el servidor
                    try {
                        // convertir el dato de tipo string (result), en una coleccion de objetos (item)
                        var item = JSON.parse(result);
                        if (item.Code == "Done") {
                            //window.location.href = "Cursos"; // Carga nuevamente la vista
                            window.location.href = "Cursos/Index";
                        } else {
                            document.getElementById("mensaje").innerHTML = item.Description;
                        }
                    } catch (e) {
                        document.getElementById("mensaje").innerHTML = result;
                    }
                    console.log(result);
                }
            });
        }
    }

    EditCurso(curso, cat) {
        console.info("EditCurso");
        let j = 1;

        // se obtiene informacion de los Input del Modal
        $("#curNombre").val(curso.Curso);
        $("#curDescripcion").val(curso.Informacion); 
        $("#curHoras").val(curso.Horas);
        $("#curCosto").val(curso.Costo);
        $("#curEstado").prop("checked", curso.Estado);
        $("#curCursoID").val(curso.CursoID);       

        this.Image = curso.Image; // guardo la imagen que viene en curso en la propiedad this.Image
        
        document.getElementById("cursoImage").innerHTML = "<img class='cursoImage' src='data:image/jpeg;base64," + curso.Image + "' />";

        let x = document.getElementById("curCatargoria"); // se obtiene la categoria id seleccionada del combo
        // -
        x.options.length = 0;

        for (var i = 0; i < cat.length; i++) {
            if (cat[i].Value == curso.CategoriaID) {

                x.options[0] = new Option(cat[i].Text, cat[i].Value);
                //x.selectedIndex = 0;
                j = i;
            } else {

                x.options[i] = new Option(cat[i].Text, cat[i].Value);
            }

        }
        
        x.options[j] = new Option(cat[0].Text, cat[0].Value);
        x.options.selectedIndex = 0;
        console.log(curso);
        console.log(cat);
    }

    GetCurso(curso) {
        document.getElementById("titleCurso").innerHTML = curso.Curso;
        this.CursoID = curso.CursoID;
    }

    EliminarCurso() {
        console.log("Esta dentro del evento EliminarCurso()")

        $.post(
            "Cursos/EliminarCurso",
            { CursoID: this.CursoID },
            (response) => {
                try {
                    var item = JSON.parse(response);
                    if (item.Description == "Done") {
                        console.info("Respuesta del Servidor: " + item.Description);
                        //window.location.href = "Cursos";
                        window.location.href = "Cursos/Index";
                    } else {
                        document.getElementById("mensajeEliminar").innerHTML = item.Description;
                        console.warn(item.Description);
                    }
                } catch (e) {
                    document.getElementById("mensajeEliminar").innerHTML = response;
                    console.log("Esta dentro el Catch");
                    console.Error(response);
                }
                console.log(response);
            }
        );
    }

    Restablecer() {
        $("#curNombre").val("");
        $("#curDescripcion").val("");
        $("#curHoras").val("");
        $("#curCosto").val("");
        $("#curEstado").prop("checked", false);
        $("#curCursoID").val(""); 
        document.getElementById("cursoImage").innerHTML = ['<img class="cursoImage" src="',
            "/Images/logo-google.png", '"/>'].join('');
    }
}
