<%@ Page Language="C#" MasterPageFile="~/Views/Shared/Administrador.Master" Inherits="System.Web.Mvc.ViewPage<SigOSO_PBD.Models.agregarTrabajadorModel>" %>


<asp:Content ID="Content4" ContentPlaceHolderID="TitleContent" runat="server">
    Modificar trabajador
</asp:Content>

<asp:Content ID="Content5" ContentPlaceHolderID="MainContent" runat="server">
    <form id="form1" method="post">
        <label>Nombre</label>
              
            <%: Html.TextBoxFor(nvoTrabajador => nvoTrabajador.nombre, new { @class = "text", @readonly="true" })%>
            <%: Html.ValidationMessageFor(nvoTrabajador => nvoTrabajador.nombre)%>
        <label>Abreviatura o iniciales</label>
            
            <%: Html.TextBoxFor(nvoTrabajador => nvoTrabajador.iniciales, new { @class = "text" })%>
            <%: Html.ValidationMessageFor(nvoTrabajador => nvoTrabajador.iniciales)%>
            
        <label>Rut</label>
              
            <%: Html.TextBoxFor(nvoTrabajador => nvoTrabajador.rut, new { @class = "text" })%>
            <input id="Submit1" type="submit" name="btn_submit" value="Cargar"/>
            <%: Html.ValidationMessageFor(nvoTrabajador => nvoTrabajador.rut)%>


        <label>Perfil de trabajador</label>

            <%: Html.DropDownList("id_perfil", (List<SelectListItem>)ViewBag.listaPerfiles, new { @style = "width: 42%;" })%>
            
        <label>Correo</label>
            
            <%: Html.TextBoxFor(nvoTrabajador => nvoTrabajador.correo, new { @class = "text" })%>
            <%: Html.ValidationMessageFor(nvoTrabajador => nvoTrabajador.correo)%>
        
        <label>Comuna</label>

            <%: Html.TextBoxFor(nvoTrabajador => nvoTrabajador.comuna, new { @class = "text" })%>
            <%: Html.ValidationMessageFor(nvoTrabajador => nvoTrabajador.comuna)%>

        <label>Direccón</label>

            <%: Html.TextBoxFor(nvoTrabajador => nvoTrabajador.direccion, new { @class = "text" })%>
            <%: Html.ValidationMessageFor(nvoTrabajador => nvoTrabajador.direccion)%>

        <label>Teléfono 1</label>
            
            <%: Html.TextBoxFor(nvoTrabajador => nvoTrabajador.telefono1, new { @class = "text" })%>
            <%: Html.ValidationMessageFor(nvoTrabajador => nvoTrabajador.telefono1)%>

        <label>Teléfono 2</label>
            
            <%: Html.TextBoxFor(nvoTrabajador => nvoTrabajador.telefono2, new { @class = "text" })%>
            <%: Html.ValidationMessageFor(nvoTrabajador => nvoTrabajador.telefono2)%>

        <label>Fecha inicio contrato</label>
            
            <%: Html.TextBoxFor(nvoTrabajador => nvoTrabajador.dia_ini_contrato, new { @class = "text", @readonly = "readonly", @style = "width: 55px;" })%>
            <%: Html.TextBoxFor(nvoTrabajador => nvoTrabajador.mes_ini_contrato, new { @class = "text", @readonly = "readonly", @style = "width: 130px;" })%>
            <%: Html.TextBoxFor(nvoTrabajador => nvoTrabajador.agno_ini_contrato, new { @class = "text", @readonly = "readonly", @style = "width: 60px;" })%>

        <label>Fecha Fin contrato</label>
            
            <%: Html.DropDownList("dia_fin_contrato", (List<SelectListItem>)ViewBag.listaDias, new { @style = "width: 55px;" })%>
            <%: Html.DropDownList("mes_fin_contrato", (List<SelectListItem>)ViewBag.listaMeses, new { @style = "width: 130px;" })%>
            <%: Html.ValidationMessageFor(nvoTrabajador => nvoTrabajador.dia_fin_contrato)%>
            <%: Html.ValidationMessageFor(nvoTrabajador => nvoTrabajador.mes_fin_contrato)%>

            <%: Html.TextBoxFor(nvoTrabajador => nvoTrabajador.agno_fin_contrato, new { @class = "text", @style = "width: 60px;" })%>
            <%: Html.ValidationMessageFor(nvoTrabajador => nvoTrabajador.agno_fin_contrato)%>

        <label>¿Está activo?</label>
            <%
                bool activo = false;
                if (ViewBag.trabajadorActivo != null) {
                    if ((bool)ViewBag.trabajadorActivo) {
                        activo = true;
                    }
                }
            %>

            <%: Html.CheckBox("es_activo", activo)%>
            


    <div style="height: 30px; width: 20%; margin-right: auto; margin-left: 40%;" 
        align="center">
        <input id="btn_agregarTrabajador" type="submit" name="btn_submit" value="Guardar cambios" />
        </div>
    </form>
</asp:Content>


