<%@ Page Language="C#" MasterPageFile="~/Views/Shared/Administrador.Master" Inherits="System.Web.Mvc.ViewPage<SigOSO_PBD.Models.Contrato>" %>




<asp:Content ID="Content4" ContentPlaceHolderID="TitleContent" runat="server">
    Mantención Contrato
</asp:Content>

<asp:Content ID="Content5" ContentPlaceHolderID="MainContent" runat="server">


    <form id="form1" method="post">
        <fieldset>
            <legend>Seleccionar el cliente</legend>
            <label>Rut cliente</label>
                <%
                    string rut_ingresado = "";
                    if (ViewBag.rutCliente != null)
                    {
                        rut_ingresado = ViewBag.rutCliente;
                    }
                %>
                <%: Html.TextBox("rutCliente", rut_ingresado, new { @class = "text" })%>
                <input id="Submit1" type="submit" name="btn_cargar" value="Cargar"/>
                <%: Html.ValidationMessage("rutCliente")%>

            <label>Nombre cliente</label>
                <%
                    string nombre_cliente = "";
                    if (ViewBag.nombreCliente != null)
                    {
                        nombre_cliente = ViewBag.nombreCliente;
                    }
                %>
                <%: Html.TextBox("nombreCliente", nombre_cliente, new { @class = "text", @readonly = "true" })%>
                <%: Html.ValidationMessage("nombreCliente")%>

            <div>
                <% if (ViewBag.respuestaPost!= null) {%>
                    <%: ViewBag.respuestaPost %>
                <%}
                    %>
            </div>
        </fieldset>
    </form>

        
        <fieldset>
            <legend>Lista de materiales genéricos</legend>
                <%if (ViewBag.tabla != null) {                      
                    Response.Write(ViewBag.tabla);                       
                }                                                     
                %>
        </fieldset>

        
</asp:Content>


