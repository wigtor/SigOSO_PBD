/*
cod_producto
tipoMaterial
unidad
detalle
*/

function buscar_posicion(id) {
    var resultado;
    for (var i = 0; i < var_unidad.length; i++) {
        if (parseInt(var_cod_producto[i]) == parseInt(id)) {
            resultado = i;
            i = var_unidad.length;
        }
    }
    return resultado;
}

function cargarDatos(objeto) {
    if (objeto.value != -10) {
        objeto.value;
        var posicion = buscar_posicion(objeto.value);
        document.getElementById("detalle").innerHTML = var_detalle[parseInt(posicion)];
        $('#unidad_oculta').val(var_unidad[posicion]);
        $('#tipo_oculta').val(var_tipoMaterial[posicion]);
        $('#id_servicio_a_agregar_oculta').val(objeto.value);

    } else {
        document.getElementById("detalle").innerHTML = null;
        document.getElementById("unidad").innerHTML = null;
        document.getElementById("id_servicio_a_agregar").innerHTML = null;
    }
}