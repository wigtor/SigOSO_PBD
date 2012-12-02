<%@ Page Language="C#" MasterPageFile="~/Views/Shared/Administrador.Master" Inherits="System.Web.Mvc.ViewPage<SigOSO_PBD.Models.Contrato>" %>




<asp:Content ID="Content4" ContentPlaceHolderID="TitleContent" runat="server">
    Ver contratos
</asp:Content>

<asp:Content ID="Content5" ContentPlaceHolderID="MainContent" runat="server">
    

    <form id="form1" method="post">
        <div class="fieldsetInterno">
        <fieldset>
            <legend>Lista de contratos</legend>
            <div>
                Seleccione un contrato y luego presione el botón "Cargar detalles".
            </div>
            <label>N° de contrato</label>
                <%: Html.DropDownListFor(nvoTrabajador => nvoTrabajador.id_contrato, (List<SelectListItem>)ViewBag.listaContratos, new { @class = "text" })%>
                <%: Html.ValidationMessageFor(nvoTrabajador => nvoTrabajador.id_contrato)%>

            <input id="Submit1" type="submit" name="btn_cargar" value="Cargar detalles"/>

        </fieldset>
        </div>

        
        <div class="fieldsetInterno">
        <fieldset>
            <legend>Detalles del contrato</legend>
            

        </fieldset>
        </div>

    </form>
</asp:Content>


