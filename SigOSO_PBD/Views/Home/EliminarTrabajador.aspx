<%@ Page Language="C#" MasterPageFile="~/Views/Shared/Administrador.Master" Inherits="System.Web.Mvc.ViewPage<SigOSO_PBD.Models.agregarTrabajadorModel>" %>

<script runat="server">

    protected void Button1_Click(object sender, EventArgs e)
    {
        
    }
</script>
<asp:Content ID="Content4" ContentPlaceHolderID="TitleContent" runat="server">
    Eliminar trabajador
</asp:Content>

<asp:Content ID="Content5" ContentPlaceHolderID="MainContent" runat="server">
    <form id="form1" method="post">
        <label>Nombre</label>
              
            <%: Html.TextBoxFor(nvoTrabajador => nvoTrabajador.nombre, new { @class = "text", @readonly="true" })%>
            <%: Html.ValidationMessageFor(nvoTrabajador => nvoTrabajador.nombre)%>
        <label>Abreviatura o iniciales</label>
            
            <%: Html.TextBoxFor(nvoTrabajador => nvoTrabajador.iniciales, new { @class = "text", @readonly = "true" })%>
            <%: Html.ValidationMessageFor(nvoTrabajador => nvoTrabajador.iniciales)%>
            
        <label>Rut</label>
              
            <%: Html.TextBoxFor(nvoTrabajador => nvoTrabajador.rut, new { @class = "text" })%>
            <input id="Submit1" type="submit" name="btn_submit" value="Cargar"/>
            <%: Html.ValidationMessageFor(nvoTrabajador => nvoTrabajador.rut)%>

           
        <label>Perfil de trabajador</label>

            <%: Html.DropDownList("id_perfil", (List<SelectListItem>)ViewBag.listaPerfiles, new { @style = "width: 42%;",@readonly = "true" })%>
        
            
        <label>Correo</label>
            
            <%: Html.TextBoxFor(nvoTrabajador => nvoTrabajador.correo, new { @class = "text", @readonly = "true" })%>
            <%: Html.ValidationMessageFor(nvoTrabajador => nvoTrabajador.correo)%>
        
        <label>Comuna</label>

            <%: Html.TextBoxFor(nvoTrabajador => nvoTrabajador.comuna, new { @class = "text", @readonly = "true" })%>
            <%: Html.ValidationMessageFor(nvoTrabajador => nvoTrabajador.comuna)%>

        <label>Direccón</label>

            <%: Html.TextBoxFor(nvoTrabajador => nvoTrabajador.direccion, new { @class = "text", @readonly = "true" })%>
            <%: Html.ValidationMessageFor(nvoTrabajador => nvoTrabajador.direccion)%>

        <label>Teléfono 1</label>
            
            <%: Html.TextBoxFor(nvoTrabajador => nvoTrabajador.telefono1, new { @class = "text", @readonly = "true" })%>
            <%: Html.ValidationMessageFor(nvoTrabajador => nvoTrabajador.telefono1)%>

        <label>Teléfono 2</label>
            
            <%: Html.TextBoxFor(nvoTrabajador => nvoTrabajador.telefono2, new { @class = "text", @readonly = "true" })%>
            <%: Html.ValidationMessageFor(nvoTrabajador => nvoTrabajador.telefono2)%>
           


    <div style="height: 30px; width: 20%; margin-right: auto; margin-left: 40%;" 
        align="center">
        <input id="btn_eliminarTrabajador" type="submit" name="btn_submit" value="Eliminar" />
        </div>
    </form>
</asp:Content>


