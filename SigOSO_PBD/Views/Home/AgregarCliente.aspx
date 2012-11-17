<%@ Page Language="C#" MasterPageFile="~/Views/Shared/Administrador.Master" Inherits="System.Web.Mvc.ViewPage<SigOSO_PBD.Models.agregarClienteModel>" %>




<asp:Content ID="Content4" ContentPlaceHolderID="TitleContent" runat="server">
    Agregar nuevo cliente 
</asp:Content>

<asp:Content ID="Content5" ContentPlaceHolderID="MainContent" runat="server">


    <form id="form1" method="post">
        <div class="fieldsetInterno"> 
            <label>Nombre</label>            
                <%: Html.TextBoxFor(nvoCliente => nvoCliente.nombre, new {@class="text"})%>
                <%: Html.ValidationMessageFor(nvoCliente => nvoCliente.nombre)%>

            <label>Rut</label>
                <%: Html.TextBoxFor(nvoCliente => nvoCliente.rut, new { @class = "text" })%>
                <%: Html.ValidationMessageFor(nvoCliente => nvoCliente.rut)%>

            <label>Giro</label>
                <%: Html.TextBoxFor(nvoCliente => nvoCliente.giro)%>
                <%: Html.ValidationMessageFor(nvoCliente => nvoCliente.giro)%>

            <label>Ciudad</label>
                <%: Html.TextBoxFor(nvoCliente => nvoCliente.ciudad)%>
                <%: Html.ValidationMessageFor(nvoCliente => nvoCliente.ciudad)%>

            <label>Comuna</label>
                <%: Html.TextBoxFor(nvoCliente => nvoCliente.comuna)%>
                <%: Html.ValidationMessageFor(nvoCliente => nvoCliente.comuna)%>

            <label>Dirección</label>
                <%: Html.TextBoxFor(nvoCliente => nvoCliente.direccion)%>
                <%: Html.ValidationMessageFor(nvoCliente => nvoCliente.direccion)%>

            <label>Teléfono 1</label>
                <%: Html.TextBoxFor(nvoCliente => nvoCliente.telefono1)%>
                <%: Html.ValidationMessageFor(nvoCliente => nvoCliente.telefono1)%>

            <label>Teléfono 2</label>
                <%: Html.TextBoxFor(nvoCliente => nvoCliente.telefono2)%>
                <%: Html.ValidationMessageFor(nvoCliente => nvoCliente.telefono2)%>

            <label>Correo electrónico</label>
                <%: Html.TextBoxFor(nvoCliente => nvoCliente.correo)%>
                <%: Html.ValidationMessageFor(nvoCliente => nvoCliente.correo)%>


            <div style="height: 30px; width: 20%; margin-right: auto; margin-left: 40%;" align="center">
                <input id="btn_agregarCliente" type="submit" value="Agregar Cliente" />
            </div>
            </div>
    </form>
</asp:Content>


