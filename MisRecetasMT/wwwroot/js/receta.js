$("#agregar").click(function () {    
    var isValid = true;

    //console.log(document.getElementById("estado").value);

    if (document.getElementById("nombre").value == '') {
        $('#nombre').siblings('span.error').text('Ingrese el nombre');
        isValid = false;
    } else {
        $('#nombre').siblings('span.error').text('');
    }

    if (document.getElementById("cantidad").value == '') {
        $('#cantidad').siblings('span.error').text('Ingrese la cantidad');
        isValid = false;
    } else {
        $('#nombre').siblings('span.error').text('');
    }

    if (document.getElementById("descripcion").value == '') {
        $('#descripcion').siblings('span.error').text('Ingrese la descripcion');
        isValid = false;
    } else {
        $('#descripcion').siblings('span.error').text('');
    }


    if (document.getElementById("ingrediente").value == '') {
        $('#ingrediente').siblings('span.error').text('Seleccione un ingrediente');
        isValid = false;
    } else {
        $('#ingrediente').siblings('span.error').text('');
    }

    if ($("#nombre").val().trim() == '') {
        $('#nombre').siblings('span.error').text('Debe escribir un nombre para la receta');
        isValid = false;
    } else {
        $('#nombre').siblings('span.error').text('');
    }

    if (isValid) {

        var ingrediente = document.getElementById("ingrediente").value;
        //console.log(ingrediente);
        var $filaNueva = $("#filaPrincipal").clone().removeAttr('id');
        $('.ingrediente', $filaNueva).val(ingrediente);
        $('#agregar', $filaNueva).addClass('remove').html('Eliminar').removeClass('btn-success').addClass('btn-danger');
        $('#ingrediente', '#cantidad', $filaNueva).attr('disabled', true);
        $('#ingrediente', '#cantidad', $filaNueva).removeAttr("id");
        $("span.error", $filaNueva).remove();
        $("#RecetaIngredientes").append($filaNueva[0]);
    }  
});

$("#RecetaIngredientes").on("click", ".remove", function () {
    $(this).parents("tr").remove();
});


$("#submit").click(function () {

    Swal.fire({
        title: "Guardar receta",
        text: "¿Estás seguro que desea guardar la receta?",
        showDenyButton: true,
        showCancelButton: true,
        confirmButtonText: 'Guardar',
        denyButtonText: 'No guardar',
    }).then((result) => {
        if (result.isConfirmed) {  
            var isValid = true;
            var itemList = [];
            $("#RecetaIngredientes tr").each(function () {
                var item = {
                    IngredienteId: parseInt($('select.ingrediente', this).val()),
                    Cantidad: parseInt($('#cantidad', this).val())
                }
                itemList.push(item);

            });
            if (itemList.length == 0) {
                $('#recetaMensaje').text('¡Agrega ingredientes!')
                isValid = false;
            }

            //var fileInput = document.getElementById('imagen');
            //var file = fileInput.files[0];
            var formData = new FormData();
            formData.append('file', file);

            if (isValid) {
                /*
                var data = {
                    Items: itemList,
                    Nombre: document.getElementById("nombre").value,
                    Descripcion: document.getElementById("descripcion").value,
                    Imagen: formData
                }
                */

                $("#submit").attr("disabled", true);
                $("#submit").html("Espere un momento...");


                var imagen = $('input[type=file]');

                //stop submit the form, we will post it manually.
                event.preventDefault();

                // Get form
                var form = $('#myform')[0];

                // Create an FormData object
                var formData = new FormData(form);

                formData.append("file", imagen[0].files[0]);

                var data = {
                    Nombre: "Dony",
                    Descripcion: "descripcion",
                    //Imagen: formData
                }

                //console.log(data);

                fetch("Recetas/Create", {
                    method: 'POST', // *GET, POST, PUT, DELETE, etc.
                    mode: 'cors', // no-cors, *cors, same-origin
                    cache: 'no-cache', // *default, no-cache, reload, force-cache, only-if-cached
                    credentials: 'same-origin', // include, *same-origin, omit
                    headers: {
                        //'Content-Type': 'application/json'
                         'Content-Type': 'application/x-www-form-urlencoded',
                    },
                    //body: JSON.stringify(data)
                    body: formData
                }).then(resp => resp.json())
                    .then(resp => {
                        //console.log(resp);
                        if (resp.data == "ok") {
                            Swal.fire('¡Receta guardada con éxito!', '', 'success')
                            setTimeout(
                                function () {
                                    location.reload();
                                }, 3000);

                            
                        } else {
                            
                            Swal.fire('¡Error al guardar!', '', 'info')
                        }
                    })
                    .catch(err => {
                        console.log(data.err);
                        Swal.fire('¡Error al guardar!', '', 'info')
                    })
            }

        } else if (result.isDenied) {
            Swal.fire('¡Receta no guardada!', '', 'info')
        }
    })   
}); //submit