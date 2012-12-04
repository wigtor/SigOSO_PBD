<%@ Page Language="C#" MasterPageFile="~/Views/Shared/JefeBodega.Master" Inherits="System.Web.Mvc.ViewPage<SigOSO_PBD.Models.retiroMaterialModel>" %>


<asp:Content ID="Content4" ContentPlaceHolderID="TitleContent" runat="server">
    Retiro de material
</asp:Content>

<asp:Content ID="Content5" ContentPlaceHolderID="MainContent" runat="server">

<form id="form1" method="post">    
    <fieldset>
        <legend>Datos orden de trabjo interna</legend>
        <label>RUT jefe cuadrilla</label>        
        <%: Html.TextBoxFor(retiroMaterial => retiroMaterial.RUT, new { @id = "input_RUT" })%>
        <input type="submit" name="btn_cargar" value="cargar"/>
        <%: Html.ValidationMessageFor(retiroMaterial => retiroMaterial.RUT)%>
        <label>Nombre jefe de cuadrilla</label>
        <%: Html.TextBoxFor(retiroMaterial => retiroMaterial.NomJefeCuadrilla, new { @id = "input_NomJefeCuadrilla", @disabled = "disabled" })%>
        <%: Html.ValidationMessageFor(retiroMaterial => retiroMaterial.NomJefeCuadrilla)%>
        <label>N° orden interna</label>
        <%: Html.TextBoxFor(retiroMaterial => retiroMaterial.NumOrdenInterna, new { @id = "input_NumOrdenInterna", @disabled = "disabled" })%>
        <%: Html.ValidationMessageFor(retiroMaterial => retiroMaterial.NumOrdenInterna)%>
        <label>Detalle orden de trabajo</label>
        <%: Html.TextAreaFor(retiroMaterial => retiroMaterial.DetalleOrdenTrabajo, new { @id = "input_DetalleOrdenTrabajo", @disabled = "disabled", @style = "min-width:290px; max-width:290px; min-height:75px; max-height:75px;" })%>
        <%: Html.ValidationMessageFor(retiroMaterial => retiroMaterial.DetalleOrdenTrabajo)%>

        <script type="text/javascript">
            var id_trabajo_interno = <% Response.Write("\""+ViewBag.id_trabajo_interno+"\""); %>;
            if(id_trabajo_interno!=""){
                var nombre_trabajador = <% Response.Write("\""+ViewBag.nombre_trabajador+"\""); %>;
                var glosa_ti = <% Response.Write("\""+ViewBag.glosa_ti+"\""); %>;
                $("#input_NomJefeCuadrilla").val(nombre_trabajador);
                $("#input_NumOrdenInterna").val(id_trabajo_interno);
                $("#input_DetalleOrdenTrabajo").val(glosa_ti);
            }
        </script>
    </fieldset>
    <fieldset>
        <legend>Detalles materiales</legend>
        <%  string tabla = "";
            tabla += "<table align='center' style='width: 100%'>";
            tabla += "<tr>";
            tabla += "<td><b>Nombre Material</b></td>";
            tabla += "<td><b>Asignado</b></td>";
            tabla += "<td><b>Disponible</b></td>";
            tabla += "<td><b>Cantidad a retirar</b></td>";
            tabla += "</tr>";
            
            
            if(ViewBag.nombre_material!=null){
                for (int i = 0; i<((List<string>)ViewBag.nombre_material).Count;i++ )
                {
                    tabla += "<tr>";
                    tabla += "<td><b>" + ((List<string>)ViewBag.nombre_material)[i] + "</b></td>";
                    tabla += "<td><b>" + ((List<string>)ViewBag.asignado)[i] + " " + ((List<string>)ViewBag.unidad)[i] + "</b></td>";
                    tabla += "<td>" + ((List<string>)ViewBag.disponible)[i] + " " + ((List<string>)ViewBag.unidad)[i] + "<b>Disponible</b></td>";
                    tabla += "<td><b>Cantidad a retirar</b></td>";
                    tabla += "</tr>";
                }    
            }
            tabla += "</table>";
            Response.Write(tabla);
             %>
    </fieldset>
</form>    
</asp:Content>


