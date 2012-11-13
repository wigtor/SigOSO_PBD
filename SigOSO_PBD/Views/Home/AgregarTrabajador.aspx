<%@ Page Language="C#" MasterPageFile="~/Views/Shared/Administrador.Master" Inherits="System.Web.Mvc.ViewPage" %>

<asp:Content ID="Content4" ContentPlaceHolderID="TitleContent" runat="server">
    Bienvenido
</asp:Content>

<asp:Content ID="Content5" ContentPlaceHolderID="MainContent" runat="server">
    <form id="form1" runat="server">
    <table style="width: 100%;">
        <tr>
            <td class="input-medium" style="width: 200px">
                &nbsp;
                Nombre</td>
            <td>
                &nbsp;
                <input id="Text1" type="text" /></td>
        </tr>
        <tr>
            <td class="input-medium" style="width: 200px">
                &nbsp;
                Rut</td>
            <td >
                &nbsp;
                <input id="Text9" type="text" />
                <input id="Text2" type="text" style="width: 20px"/></td>

        </tr>
        <tr>
            <td class="input-medium" style="width: 200px">
                &nbsp;
                Correo</td>
            <td>
                &nbsp;
                <input id="Text3" type="text" /></td>
        </tr>
        <tr>
            <td class="input-medium" style="width: 200px">
                &nbsp;
                Ciudad</td>
            <td>
                &nbsp;
                <input id="Text4" type="text" /></td>
        </tr>
        <tr>
            <td class="input-medium" style="width: 200px">
                &nbsp;
                Comuna</td>
            <td>
                &nbsp;
                <input id="Text5" type="text" /></td>
        </tr>
        <tr>
            <td class="input-medium" style="width: 200px">
                &nbsp;
                Direccón</td>
            <td>
                &nbsp;
                <input id="Text6" type="text" /></td>
        </tr>
        <tr>
            <td class="input-medium" style="width: 200px">
                &nbsp;
                Teléfono 1</td>
            <td>
                &nbsp;
                <input id="Text7" type="text" /></td>
        </tr>
         <tr>
            <td class="input-medium" style="width: 200px">
                &nbsp;
                Teléfono 2</td>
            <td>
                &nbsp;
                <input id="Text8" type="text" /></td>
        </tr>
        
         
    </table>


    <div style="height: 30px; width: 20%; margin-right: auto; margin-left: 40%;" 
        align="center">
        <asp:Button ID="Button1" runat="server" Text="Agregar Trabajador" /></div>
    </form>
</asp:Content>


