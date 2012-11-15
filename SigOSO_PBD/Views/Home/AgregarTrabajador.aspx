<%@ Page Language="C#" MasterPageFile="~/Views/Shared/Administrador.Master" Inherits="System.Web.Mvc.ViewPage<SigOSO_PBD.Models.agregarTrabajadorModel>" %>

<asp:Content ID="Content4" ContentPlaceHolderID="TitleContent" runat="server">
    Bienvenido
</asp:Content>

<asp:Content ID="Content5" ContentPlaceHolderID="MainContent" runat="server">
    <form id="form1" method="post">
    <table style="width: 100%;">
        <tr>
            <td class="input-medium" style="width: 200px">
                &nbsp;
                Nombre
            </td>
            <td>
                &nbsp;
                <%: Html.TextBoxFor(nvoTrabajador => nvoTrabajador.nombre)%>
                <%: Html.ValidationMessageFor(nvoTrabajador => nvoTrabajador.nombre)%>
            </td>
        </tr>
        <tr>
            <td class="input-medium" style="width: 200px">
                &nbsp;
                Abreviatura o iniciales
            </td>
            <td>
                &nbsp;
                <%: Html.TextBoxFor(nvoTrabajador => nvoTrabajador.iniciales)%>
                <%: Html.ValidationMessageFor(nvoTrabajador => nvoTrabajador.iniciales)%>
            </td>
        </tr>
        <tr>
            <td class="input-medium" style="width: 200px">
                &nbsp;
                Rut</td>
            <td >
                &nbsp;
                <%: Html.TextBoxFor(nvoTrabajador => nvoTrabajador.rut)%>
                <%: Html.ValidationMessageFor(nvoTrabajador => nvoTrabajador.rut)%>
            </td>

        </tr>
        <tr>
            <td class="input-medium" style="width: 200px">
                &nbsp;
                Perfil de trabajador</td>
            <td >
                &nbsp;
                <!-- Cambiar por select -->




                <%: Html.TextBoxFor(nvoTrabajador => nvoTrabajador.id_perfil)%>
                <%: Html.ValidationMessageFor(nvoTrabajador => nvoTrabajador.id_perfil)%>
            </td>

        </tr>
        <tr>
            <td class="input-medium" style="width: 200px">
                &nbsp;
                Correo</td>
            <td>
                &nbsp;
                <%: Html.TextBoxFor(nvoTrabajador => nvoTrabajador.correo)%>
                <%: Html.ValidationMessageFor(nvoTrabajador => nvoTrabajador.correo)%>
            </td>
        </tr>
        <tr>
            <td class="input-medium" style="width: 200px">
                &nbsp;
                Ciudad</td>
            <td>
                &nbsp;
                <%: Html.TextBoxFor(nvoTrabajador => nvoTrabajador.ciudad)%>
                <%: Html.ValidationMessageFor(nvoTrabajador => nvoTrabajador.ciudad)%>
            </td>
        </tr>
        <tr>
            <td class="input-medium" style="width: 200px">
                &nbsp;
                Comuna</td>
            <td>
                &nbsp;
                <%: Html.TextBoxFor(nvoTrabajador => nvoTrabajador.comuna)%>
                <%: Html.ValidationMessageFor(nvoTrabajador => nvoTrabajador.comuna)%>
            </td>
        </tr>
        <tr>
            <td class="input-medium" style="width: 200px">
                &nbsp;
                Direccón</td>
            <td>
                &nbsp;
                <%: Html.TextBoxFor(nvoTrabajador => nvoTrabajador.direccion)%>
                <%: Html.ValidationMessageFor(nvoTrabajador => nvoTrabajador.direccion)%>
            </td>
        </tr>
        <tr>
            <td class="input-medium" style="width: 200px">
                &nbsp;
                Teléfono 1</td>
            <td>
                &nbsp;
                <%: Html.TextBoxFor(nvoTrabajador => nvoTrabajador.telefono1)%>
                <%: Html.ValidationMessageFor(nvoTrabajador => nvoTrabajador.telefono1)%>
            </td>
        </tr>
        <tr>
            <td class="input-medium" style="width: 200px">
                &nbsp;
                Teléfono 2</td>
            <td>
                &nbsp;
                <%: Html.TextBoxFor(nvoTrabajador => nvoTrabajador.telefono2)%>
                <%: Html.ValidationMessageFor(nvoTrabajador => nvoTrabajador.telefono2)%>
            </td>
        </tr>
        <tr>
            <td class="input-medium" style="width: 200px">
                &nbsp;
                Fecha inicio contrato</td>
            <td>
                &nbsp;
                <!-- Cambiar por calendario -->
                <%: Html.TextBoxFor(nvoTrabajador => nvoTrabajador.fecha_ini_contrato)%>
                <%: Html.ValidationMessageFor(nvoTrabajador => nvoTrabajador.fecha_ini_contrato)%>
            </td>
        </tr>
        
         
    </table>


    <div style="height: 30px; width: 20%; margin-right: auto; margin-left: 40%;" 
        align="center">
        <input id="btn_agregarTrabajador" type="submit" value="Agregar Trabajador" />
        </div>
    </form>
</asp:Content>


