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


        <div class="fieldsetInterno">
        <fieldset>
            <legend>Datos de contrato</legend>
                <label>Fecha inicio contrato</label>
            
                    <%: Html.DropDownList("dia_ini_contrato", (List<SelectListItem>)ViewBag.listaDias, new { @style = "width: 55px;" })%>
                    <%: Html.DropDownList("mes_ini_contrato", (List<SelectListItem>)ViewBag.listaMeses, new { @style = "width: 130px;" })%>
                    <%: Html.ValidationMessageFor(nvoContrato => nvoContrato.dia_ini_contrato)%>
                    <%: Html.ValidationMessageFor(nvoContrato => nvoContrato.mes_ini_contrato)%>

                    <%: Html.TextBoxFor(nvoTrabajador => nvoTrabajador.agno_ini_contrato, new { @class = "text", @style = "width: 60px;" })%>
                    <%: Html.ValidationMessageFor(nvoTrabajador => nvoTrabajador.agno_ini_contrato)%>



                <label>Fecha caducidad contrato</label>
                    <%: Html.CheckBox("tieneTermino_contrato", false) %>
                    <%: Html.DropDownList("dia_caducidad_contrato", (List<SelectListItem>)ViewBag.listaDias, new { @style = "width: 55px;", @readonly=true })%>
                    <%: Html.DropDownList("mes_caducidad_contrato", (List<SelectListItem>)ViewBag.listaMeses, new { @style = "width: 130px;", @readonly = true })%>
                    <%: Html.ValidationMessageFor(nvoContrato => nvoContrato.dia_caducidad_contrato)%>
                    <%: Html.ValidationMessageFor(nvoContrato => nvoContrato.mes_caducidad_contrato)%>

                    <%: Html.TextBoxFor(nvoContrato => nvoContrato.agno_caducidad_contrato, new { @class = "text", @style = "width: 60px;", @readonly = true })%>
                    <%: Html.ValidationMessageFor(nvoContrato => nvoContrato.agno_caducidad_contrato)%>
                
                <div class="fieldsetInterno">
                <fieldset>
                    <legend>Servicios a prestar en contrato</legend>
                    
                    <label>Seleccione un servicio para agregar</label>

                        <%: Html.DropDownListFor(nvoContrato => nvoContrato.servicioSeleccionado, (List<SelectListItem>)ViewBag.listaServicios, new { @style = "width: 70%;", @onchange = "submit()" })%>

                    <label>Precio referencia del servicio</label>
                        <%: Html.TextBox("precioReferencia", (string)ViewBag.precioReferencia, new { @class = "text", @style = "width: 100px;", @readonly = "true" })%>
                        
                    <label>Precio acordado en contrato</label>
                        <%: Html.TextBox("precioPorContrato", (string)ViewBag.precioReferencia, new { @class = "text", @style = "width: 100px;"})%>


                    <label>Condición para servicio</label>
                        <%: Html.TextArea("condicion_servicio", new { @style="max-width: 600px; min-width: 600px;"})%>

                    <div style="height: 30px; width: 20%; margin-right: auto; margin-left: 40%;"  align="center">
                        <input id="btn_agregarServicio" type="submit" name="btn_agregarServicio" value="Agregar servicio" />
                    </div>

                </fieldset>
                </div>

                <div class="fieldsetInterno">
                <fieldset>
                    <legend>Servicios del contrato</legend>
                    <table style="width: 100%">
                        <tr>
                            <td>
                                <b>Servicio</b></td>
                            <td>
                                <b>Precio a cobrar</b></td>
                            <td>
                                <b>Acción</b></td>
                        </tr>
                        <%  if (ViewBag.listaServiciosAgregados != null)
                            {
                                List<SigOSO_PBD.Models.ServicioListado> resultados = (List<SigOSO_PBD.Models.ServicioListado>)ViewBag.listaServiciosAgregados;
                                foreach (SigOSO_PBD.Models.ServicioListado temp in resultados)
                                {
                                    Response.Write("<tr>");
                
                                    Response.Write("<td>");
                                    Response.Write(temp.id_servicio); //debe ir el nombre
                                    Response.Write("</td>");

                                    Response.Write("<td>");
                                    Response.Write(temp.precio_acordado);
                                    Response.Write("</td>");

                                    Response.Write("<td>");
                                    Response.Write("<input id=\"Submit1\" name=\"quitar_" + temp.id_servicio + "\" type=\"submit\" value=\"Quitar\" />");
                                    Response.Write("</td>");

                                    Response.Write("</tr>");
                                }

                            }
                        %>
    </table>

                    
                </fieldset>
                </div>

                
        </fieldset>
        </div>
        <div style="height: 30px; width: 20%; margin-right: auto; margin-left: 40%;"  align="center">
            <input id="btn_agregarContrato" type="button" name="btn_agregarContrato" value="Agregar contrato" />
        </div>
    </form>    
</asp:Content>


