<%@ Page Language="C#" MasterPageFile="~/Views/Shared/Administrador.Master" Inherits="System.Web.Mvc.ViewPage" %>

<asp:Content ID="Content4" ContentPlaceHolderID="TitleContent" runat="server">
    Agregar nuevo cliente 
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
                Giro</td>
            <td >
                &nbsp;
                <input id="Text10" type="text" /></td>

        </tr>
        <tr>
            <td class="input-medium" style="width: 200px">
                &nbsp;
                Ciudad</td>
            <td>
                &nbsp;
                <input id="Text3" type="text" /></td>
        </tr>
        <tr>
            <td class="input-medium" style="width: 200px">
                &nbsp;
                Comuna</td>
            <td>
                &nbsp;
                <input id="Text11" type="text" /></td>
        </tr>
        <tr>
            <td class="input-medium" style="width: 200px">
                &nbsp; Dirección</td>
            <td>
                &nbsp;
                <input id="Text4" type="text" /></td>
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
        
         
         <tr>
            <td class="input-medium" style="width: 200px">
                &nbsp;
                Correo electrónico</td>
            <td>
                &nbsp;
                <input id="Text12" type="text" /></td>
        </tr>
        
         
    </table>


    <div style="height: 30px; width: 20%; margin-right: auto; margin-left: 40%;" 
        align="center">
        <asp:Button ID="btn_agregarCliente" runat="server" Text="Agregar cliente" /></div>
    </form>
</asp:Content>


