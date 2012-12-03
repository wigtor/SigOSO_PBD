<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Supervisor.Master" Inherits="System.Web.Mvc.ViewPage<SigOSO_PBD.Models.AsigMatSupervisorModels>" %>

<asp:Content ID="Content4" ContentPlaceHolderID="TitleContent" runat="server">
    Asignación materiales
</asp:Content>

<asp:Content ID="Content5" ContentPlaceHolderID="MainContent" runat="server">
<form id="form1" method="post">    
<script type="text/javascript";>
    function cargar(objeto) {
        var id = objeto.id.split('_')[1];
        var posicion;
        var cargo = false;
        var i;
        for (i = 0; i < var_id_solicitud_material.length; i++) {

            if (var_id_solicitud_material[i].toString() == id.toString()) {
                
                posicion = i;
                cargo = true;
                break;
            }
        }
        if (cargo) {


            $('#id_trabajo_interno').val(var_id_trabajo_interno[posicion]);
            $('#fecha').val(var_fecha[posicion]);
            $('#material_solicitado').val(var_material_solicitado[posicion]);
            $('#cantidad_solicitada').val(var_cantidad_solicitada[posicion]);
            $('#glosa_material').val(var_glosa_material[posicion]);
            $('#detalle_solicitud').val(var_detalle_solicitud[posicion]);
            $('#id_solicitud').val(var_id_solicitud_material[posicion]);
            
        }




    }

</script>
    <fieldset>
        <legend>Solicitudes de material</legend>
        <% string tabla = "";
           tabla += "<div style='width: 100%; max-height:350px; overflow: auto;'>";
           tabla += "<table align='center' style='width: 100%;' >";
           tabla += "<tr>";
           tabla += "<td><b>Fecha Solicitud</b></td>";
           tabla += "<td><b>RUT</b></td>";
           tabla += "<td><b>Material</b></td>";
           tabla += "<td><b>Cantidad</b></td>";
           tabla += "<td><b>Cargar</b></td>";
           tabla += "</tr>";              
           if (ViewBag.fecha_solicitud_material != null)
           {
               for (int i = 0; i < ViewBag.fecha_solicitud_material.Count; i++)
               {
                   tabla += "<tr>";
                   tabla += "<td><b>" + ((List<string>)ViewBag.fecha_solicitud_material)[i] + "</b></td>";
                   tabla += "<td><b>" + ((List<string>)ViewBag.rut_trabajador)[i] + "</b></td>";
                   tabla += "<td><b>" + ((List<string>)ViewBag.nombre_tipo_material )[i] + "</b></td>";
                   tabla += "<td><b>" + ((List<string>)ViewBag.cantidad_solicitada)[i] + "</b></td>";
                   tabla += "<td><b>" + "<input id='cargar_" + ((List<string>)ViewBag.id_solicitud_material)[i] + "' type='button' value='cargar' name='cargar_" + ((List<string>)ViewBag.id_solicitud_material)[i] + "' onclick=cargar(this)>" + "</b></td>";
                   tabla += "</tr>"; 
               }
               string script = "<script>";
               script += "var var_id_trabajo_interno =  new Array();";
               script += "var var_fecha =  new Array();";
               script += "var var_material_solicitado =  new Array();";
               script += "var var_cantidad_solicitada =  new Array();";
               script += "var var_glosa_material =  new Array();";
               script += "var var_detalle_solicitud =  new Array();";
               script += "var var_id_solicitud_material =  new Array();";
               for (int i = 0; i < ViewBag.fecha_solicitud_material.Count; i++)
               {
                   script += "var_id_trabajo_interno.push('" + ((List<string>)ViewBag.id_trabajo_interno)[i] + "');";
                   script += "var_fecha.push('" + ((List<string>)ViewBag.fecha_solicitud_material)[i] + "');";
                   script += "var_material_solicitado.push('" + ((List<string>)ViewBag.nombre_tipo_material)[i] + "');";
                   script += "var_cantidad_solicitada.push('" + ((List<string>)ViewBag.cantidad_solicitada)[i] + "');";
                   script += "var_glosa_material.push('" + ((List<string>)ViewBag.glosa_tipo_material)[i] + "');";
                   script += "var_detalle_solicitud.push('" + ((List<string>)ViewBag.comentarios_jefe_cuadrilla)[i] + "');";
                   script += "var_id_solicitud_material.push('" + ((List<string>)ViewBag.id_solicitud_material)[i] + "');";                   
               }
               script += "</script>";
               Response.Write(script);    
           }       
            tabla += "</table>";
            tabla += "</div>";
            Response.Write(tabla);
             
                   
            %>
     </fieldset>

     <fieldset>
        <legend>Datos solicitud</legend>
        
        <label>ID Trabajo Interno</label>  
        <%: Html.TextBoxFor(respuesta => respuesta.id_trabajo_interno, new { @class = "text", @disabled = "disabled" })%>

        <label>Fecha</label>  
        <%: Html.TextBoxFor(respuesta => respuesta.fecha, new { @class = "text", @disabled = "disabled" })%>
        
        <label>Material</label>  
        <%: Html.TextBoxFor(respuesta => respuesta.material_solicitado, new { @class = "text", @disabled = "disabled" })%>

        <label>Cantidad Solicitada</label>  
        <%: Html.TextBoxFor(respuesta => respuesta.cantidad_solicitada, new { @class = "text", @disabled = "disabled" })%>

        <label>Glosa material</label>  
        <%: Html.TextBoxFor(respuesta => respuesta.glosa_material, new { @class = "text", @disabled = "disabled" })%>

        <label>Detalle Solicitud</label>  
        <%: Html.TextAreaFor(respuesta => respuesta.detalle_solicitud, new { @class = "text", @disabled = "disabled", @style = "width: 280px; height: 90px;"})%>

        <label>Cantidad aprobada</label>  
        <%: Html.TextBoxFor(respuesta => respuesta.cantidad_asignada, new { @class = "text"})%>

        <label>Detalle cantidad aprobada</label>  
        <%: Html.TextAreaFor(respuesta => respuesta.detalle_respuesta, new { @class = "text", @style = "width: 280px; height: 90px;" })%>


        <%: Html.TextBoxFor(respuesta => respuesta.id_solicitud, new { @class = "text", @style = "display:none"})%>


        <input type="submit" name="btn_respuesta" value="Enviar respuesta" style="display:block;"/>
     </fieldset>
</form>    
</asp:Content>


