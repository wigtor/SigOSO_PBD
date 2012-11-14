<%@ Page Language="C#" MasterPageFile="~/Views/Shared/Administrador.Master" Inherits="System.Web.Mvc.ViewPage" %>




<asp:Content ID="Content4" ContentPlaceHolderID="TitleContent" runat="server">
    Agregar nuevo cliente 
</asp:Content>

<asp:Content ID="Content5" ContentPlaceHolderID="MainContent" runat="server">


    <form id="form1" method="post">
        <table style="width: 100%;">
            <tr>
                <td class="input-medium" style="width: 200px">
                    &nbsp;
                    Nombre</td>
                <td>
                    &nbsp;
                    <input id="nombre" name="nombre" type="text" />
                </td>
            </tr>
            <tr>
                <td class="input-medium" style="width: 200px">
                    &nbsp;
                    Rut</td>
                <td >
                    &nbsp;
                    <input id="rut" name="rut" type="text" />
                    <input id="digito_verificador" type="text" style="width: 20px"/></td>

            </tr>
            <tr>
                <td class="input-medium" style="width: 200px">
                    &nbsp;
                    Giro</td>
                <td >
                    &nbsp;
                    <input id="giro" name="giro" type="text" /></td>

            </tr>
            <tr>
                <td class="input-medium" style="width: 200px">
                    &nbsp;
                    Ciudad</td>
                <td>
                    &nbsp;
                    <input id="ciudad" name="ciudad" type="text" /></td>
            </tr>
            <tr>
                <td class="input-medium" style="width: 200px">
                    &nbsp;
                    Comuna</td>
                <td>
                    &nbsp;
                    <input id="comuna" name="comuna" type="text" /></td>
            </tr>
            <tr>
                <td class="input-medium" style="width: 200px">
                    &nbsp; Dirección</td>
                <td>
                    &nbsp;
                    <input id="direccion" name="direccion" type="text" /></td>
            </tr>
            <tr>
                <td class="input-medium" style="width: 200px">
                    &nbsp;
                    Teléfono 1</td>
                <td>
                    &nbsp;
                    <input id="telefono1" name="telefono1" type="text" /></td>
            </tr>
             <tr>
                <td class="input-medium" style="width: 200px">
                    &nbsp;
                    Teléfono 2</td>
                <td>
                    &nbsp;
                    <input id="telefono2" name="telefono2" type="text" /></td>
            </tr>
        
         
             <tr>
                <td class="input-medium" style="width: 200px">
                    &nbsp;
                    Correo electrónico</td>
                <td>
                    &nbsp;
                    <input id="correo" name="correo" type="text" /></td>
            </tr>
        
         
        </table>


        <div style="height: 30px; width: 20%; margin-right: auto; margin-left: 40%;" 
        align="center">
            <input id="btn_agregarCliente" type="submit" value="Agregar Cliente" />
        </div>
        <div>
            <%: ViewBag.respuestaPost %>
        </div>
    </form>
</asp:Content>


