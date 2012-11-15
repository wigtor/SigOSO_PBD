<%@ Page Language="C#" MasterPageFile="~/Views/Shared/Administrador.Master" Inherits="System.Web.Mvc.ViewPage<SigOSO_PBD.Models.agregarClienteModel>" %>




<asp:Content ID="Content4" ContentPlaceHolderID="TitleContent" runat="server">
    Modificar cliente 
</asp:Content>

<asp:Content ID="Content5" ContentPlaceHolderID="MainContent" runat="server">


    <form id="form1" method="post">
        <table style="width: 100%;">
            <tr>
                <td class="input-medium" style="width: 200px">
                    &nbsp;
                    Rut</td>
                <td>
                    &nbsp;
                    <input id="btn_cargarCliente" type="submit" name="btn_submit" value="Cargar"/>
                    <!--
                    <input id="btn_cargarCliente" type="button" name="btn_cargarCliente" value="Cargar" onclick="location='./ModificarCliente/?rut='"/>
                    -->
                    <br />
                    &nbsp;
                    <%: Html.TextBoxFor(nvoCliente => nvoCliente.rut)%>
                    <%: Html.ValidationMessageFor(nvoCliente => nvoCliente.rut)%>
                    
                    </td>
            </tr>
            <tr>
                <td class="input-medium" style="width: 200px">
                    &nbsp;
                    Nombre</td>
                <td >
                    &nbsp;
                    <%: Html.TextBoxFor(nvoCliente => nvoCliente.nombre)%>
                    <%: Html.ValidationMessageFor(nvoCliente => nvoCliente.nombre)%></td>

            </tr>
            <tr>
                <td class="input-medium" style="width: 200px">
                    &nbsp;
                    Giro</td>
                <td >
                    &nbsp;
                    <%: Html.TextBoxFor(nvoCliente => nvoCliente.giro)%>
                    <%: Html.ValidationMessageFor(nvoCliente => nvoCliente.giro)%>

            </tr>
            <tr>
                <td class="input-medium" style="width: 200px">
                    &nbsp;
                    Ciudad</td>
                <td>
                    &nbsp;
                    <%: Html.TextBoxFor(nvoCliente => nvoCliente.ciudad)%>
                    <%: Html.ValidationMessageFor(nvoCliente => nvoCliente.ciudad)%>
            </tr>
            <tr>
                <td class="input-medium" style="width: 200px">
                    &nbsp;
                    Comuna</td>
                <td>
                    &nbsp;
                    <%: Html.TextBoxFor(nvoCliente => nvoCliente.comuna)%>
                    <%: Html.ValidationMessageFor(nvoCliente => nvoCliente.comuna)%></td>
            </tr>
            <tr>
                <td class="input-medium" style="width: 200px">
                    &nbsp; Dirección</td>
                <td>
                    &nbsp;
                    <%: Html.TextBoxFor(nvoCliente => nvoCliente.direccion)%>
                    <%: Html.ValidationMessageFor(nvoCliente => nvoCliente.direccion)%></td>
            </tr>
            <tr>
                <td class="input-medium" style="width: 200px">
                    &nbsp;
                    Teléfono 1</td>
                <td>
                    &nbsp;
                    <%: Html.TextBoxFor(nvoCliente => nvoCliente.telefono1)%>
                    <%: Html.ValidationMessageFor(nvoCliente => nvoCliente.telefono1)%></td>
            </tr>
             <tr>
                <td class="input-medium" style="width: 200px">
                    &nbsp;
                    Teléfono 2</td>
                <td>
                    &nbsp;
                    <%: Html.TextBoxFor(nvoCliente => nvoCliente.telefono2)%>
                    <%: Html.ValidationMessageFor(nvoCliente => nvoCliente.telefono2)%></td>
            </tr>
        
         
             <tr>
                <td class="input-medium" style="width: 200px">
                    &nbsp;
                    Correo electrónico</td>
                <td>
                    &nbsp;
                    <%: Html.TextBoxFor(nvoCliente => nvoCliente.correo)%>
                    <%: Html.ValidationMessageFor(nvoCliente => nvoCliente.correo)%></td>
            </tr>
        
         
        </table>

        <div>
            <% if (ViewBag.respuestaPost!= null) {%>
                <%: ViewBag.respuestaPost %>
            <%}
             %>
        </div>

        <div style="height: 30px; width: 20%; margin-right: auto; margin-left: 40%;" 
        align="center">
            <input id="btn_modificarCliente" name="btn_submit" type="submit" value="Guardar cambios" />
        </div>
    </form>
</asp:Content>

