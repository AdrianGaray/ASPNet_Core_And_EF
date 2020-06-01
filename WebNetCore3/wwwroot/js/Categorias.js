
class Categorias {
    constructor() {
        this.CategoriaID = 0;
    }
    RegistrarCategoria() {
        console.log("Evento = RegistrarCategoria()");
        /*console.log("window.location.pathname = '" + window.location.pathname + "'");
        console.log("window.location.href = '" + window.location.href + "'");*/

        if (window.location.pathname.includes("Index")) {
            $.post(
                "GetCategorias",
                $(".formCategoria").serialize(),
                (response) => {
                    try {
                        var item = JSON.parse(response);
                        if (item.Code == "Done") {
                            console.info("Respuesta del Servidor: {{item.Code}}");
                            window.location.href = "Index";
                            //window.location.href = "Categorias/Index";
                            //window.location.href = "Categorias/";
                        } else {
                            document.getElementById("mensaje").innerHTML = item.Description;
                        }
                    } catch (e) {
                        document.getElementById("mensaje").innerHTML = response;
                        console.log("Esta dentro el Catch");
                        console.Error(response);
                    }
                    console.log(response);
                }
            );
        } else {
            $.post(
                "Categorias/GetCategorias",
                $(".formCategoria").serialize(),
                (response) => {
                    try {
                        var item = JSON.parse(response);
                        if (item.Code == "Done") {
                            console.info("Respuesta del Servidor: " + item.Code);
                            window.location.href = "Categorias/Index";
                        } else {
                            document.getElementById("mensaje").innerHTML = item.Description;
                            console.warn(item.Code);
                        }
                    } catch (e) {
                        document.getElementById("mensaje").innerHTML = response;
                        console.log("Esta dentro el Catch");
                        console.Error(response);
                    }
                    console.log(response);
                }
            );
        }
    }

    EditCategoria(data) {
        console.log("Esta dentro del evento EditCategoria()")

        document.getElementById("catNombre").value = data.Categoria;
        document.getElementById("catDescripcion").value = data.Descripcion;
        document.getElementById("catEstado").checked = data.Estado;
        document.getElementById("catCategoriaID").value = data.CategoriaID;

        console.log(data);
    }

    GetCategoria(data) {
        console.log("Esta dentro del evento GetCategoria()")
        document.getElementById("titleCategoria").innerHTML = data.Categoria;
        this.CategoriaID = data.CategoriaID;
    }

    EliminarCategoria() {
        console.log("Esta dentro del evento EliminarCategoria()")

        $.post(
            "Categorias/EliminarCategoria",
            { CategoriaID: this.CategoriaID },
            (response) => {
                try {
                    var item = JSON.parse(response);
                    if (item.Description == "Done") {
                        console.info("Respuesta del Servidor: " + item.Description);
                        //window.location.href = "Categoria";
                        window.location.href = " Categorias/Index";
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
        console.log("Esta dentro del evento Restablecer()")

        document.getElementById("catNombre").value = "";
        document.getElementById("catDescripcion").value = "";
        document.getElementById("catEstado").checked = false;
        document.getElementById("catCategoriaID").value = 0;
    }
}
