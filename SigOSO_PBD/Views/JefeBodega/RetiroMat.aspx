<%@ Page Language="C#" MasterPageFile="~/Views/Shared/JefeBodega.Master" Inherits="System.Web.Mvc.ViewPage<SigOSO_PBD.Models.retiroMaterialModel>" %>


<asp:Content ID="Content4" ContentPlaceHolderID="TitleContent" runat="server">
    Retiro de material
</asp:Content>

<asp:Content ID="Content5" ContentPlaceHolderID="MainContent" runat="server">

<form id="form1" method="post">    
    <fieldset>
        <legend>Datos orden de trabjo interna</legend>
        <label>RUT jefe cuadrilla</label>        
        <%: Html.TextBoxFor(retiroMaterial => retiroMaterial.RUT, new { @id = "id_servicio_a_agregar_oculta" })%>
        <input type="submit" name="btn_cargar" value="cargar"/>
        <%: Html.ValidationMessageFor(retiroMaterial => retiroMaterial.RUT)%>
        <label>Nombre jefe de cuadrilla</label>
        <%: Html.TextBoxFor(retiroMaterial => retiroMaterial.NomJefeCuadrilla, new { @id = "id_servicio_a_agregar_oculta", @disabled = "disabled" })%>
        <%: Html.ValidationMessageFor(retiroMaterial => retiroMaterial.NomJefeCuadrilla)%>
        <label>N° orden interna</label>
        <%: Html.TextBoxFor(retiroMaterial => retiroMaterial.NumOrdenInterna, new { @id = "id_servicio_a_agregar_oculta", @disabled = "disabled" })%>
        <%: Html.ValidationMessageFor(retiroMaterial => retiroMaterial.NumOrdenInterna)%>
        <label>Detalle orden de trabajo</label>
        <%: Html.TextAreaFor(retiroMaterial => retiroMaterial.DetalleOrdenTrabajo, new { @id = "id_servicio_a_agregar_oculta", @disabled = "disabled", @style = "min-width:290px; max-width:290px; min-height:75px; max-height:75px;" })%>
        <%: Html.ValidationMessageFor(retiroMaterial => retiroMaterial.DetalleOrdenTrabajo)%>


    </fieldset>


</form>    
</asp:Content>


