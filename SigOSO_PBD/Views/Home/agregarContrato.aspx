<%@ Page Language="C#" MasterPageFile="~/Views/Shared/Administrador.Master" Inherits="System.Web.Mvc.ViewPage<SigOSO_PBD.Models.Contrato>" %>




<asp:Content ID="Content4" ContentPlaceHolderID="TitleContent" runat="server">
    Mantención Contrato
</asp:Content>

<asp:Content ID="Content5" ContentPlaceHolderID="MainContent" runat="server">


    <form id="form1" method="post">
        <div class="fieldsetInterno">
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
                <%: Html.TextBoxFor(nvoContrato => nvoContrato.rutCliente, new { @class = "text" })%>
                <input id="Submit1" type="submit" name="btn_cargar" value="Cargar"/>
                <%: Html.ValidationMessageFor(nvoContrato => nvoContrato.rutCliente)%>

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
        </fieldset>
        </div>

            <div>
                <% if (ViewBag.respuestaPost!= null) {%>
                    <%: ViewBag.respuestaPost %>
                <%}
                    %>
            </div>
        
    

        <div class="fieldsetInterno">
        <fieldset>
            <legend>Datos de contrato</legend>
                <label>Fecha inicio contrato</label>
            
                    <%: Html.DropDownList("dia_ini_contrato", (List<SelectListItem>)ViewBag.listaDias, new { @style = "width: 55px;" })%>
                    <%: Html.DropDownList("mes_ini_contrato", (List<SelectListItem>)ViewBag.listaMeses, new { @style = "width: 130px;" })%>
                    <%: Html.ValidationMessageFor(nvoTrabajador => nvoTrabajador.dia_ini_contrato)%>
                    <%: Html.ValidationMessageFor(nvoTrabajador => nvoTrabajador.mes_ini_contrato)%>

                    <%: Html.TextBoxFor(nvoTrabajador => nvoTrabajador.agno_ini_contrato, new { @class = "text", @style = "width: 60px;" })%>
                    <%: Html.ValidationMessageFor(nvoTrabajador => nvoTrabajador.agno_ini_contrato)%>



                <label>Fecha caducidad contrato</label>
            
                    <%: Html.DropDownList("dia_caducidad_contrato", (List<SelectListItem>)ViewBag.listaDias, new { @style = "width: 55px;" })%>
                    <%: Html.DropDownList("mes_caducidad_contrato", (List<SelectListItem>)ViewBag.listaMeses, new { @style = "width: 130px;" })%>
                    <%: Html.ValidationMessageFor(nvoTrabajador => nvoTrabajador.dia_caducidad_contrato)%>
                    <%: Html.ValidationMessageFor(nvoTrabajador => nvoTrabajador.mes_caducidad_contrato)%>

                    <%: Html.TextBoxFor(nvoTrabajador => nvoTrabajador.agno_caducidad_contrato, new { @class = "text", @style = "width: 60px;" })%>
                    <%: Html.ValidationMessageFor(nvoTrabajador => nvoTrabajador.agno_caducidad_contrato)%>
                
                <div class="fieldsetInterno">
                <fieldset>
                    <legend>Servicios a prestar en contrato</legend>
                    
                    <label>Seleccione un servicio para agregar</label>
                        <%: Html.DropDownList("listaServicios", (List<SelectListItem>)ViewBag.listaServicios, new { @style = "width: 70%;" })%>

                    <label>Precio referencia del servicio</label>
                        <%: Html.TextBox("precioReferencia", "", new { @class = "text", @style = "width: 100px;", @readonly = "true" })%>
                        
                    <label>Precio acordado en contrato</label>
                        <%: Html.TextBox("precioPorContrato", "", new { @class = "text", @style = "width: 100px;"})%>


                    <label>Condición para servicio</label>
                        <%: Html.TextArea("condicion_servicio") %>

                    <div style="height: 30px; width: 20%; margin-right: auto; margin-left: 40%;"  align="center">
                        <input id="btn_agregarCondicion" type="button" name="btn_agregarCondicion" value="Agregar servicio" />
                    </div>

                </fieldset>
                </div>

                <div class="fieldsetInterno">
                <fieldset>
                    <legend>Servicios del contrato</legend>
                    <%if (ViewBag.tabla != null) {                      
                        Response.Write(ViewBag.tabla);                       
                    }                                                     
                    %>
                </fieldset>
                </div>

                
        </fieldset>
        </div>

    </form>    
</asp:Content>


