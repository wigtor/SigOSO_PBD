<%@ Page Language="C#" MasterPageFile="~/Views/Shared/Administrador.Master" Inherits="System.Web.Mvc.ViewPage<SigOSO_PBD.Models.OrdenTrabajo>" %>




<asp:Content ID="Content4" ContentPlaceHolderID="TitleContent" runat="server">
    Ingresar orden de trabajo
</asp:Content>

<asp:Content ID="Content5" ContentPlaceHolderID="MainContent" runat="server">
    
    <script type="text/javascript">
        function cambiaTieneFechaFinal(checkbox) {
            var dia = document.getElementById('dia_caducidad_contrato');
            var mes = document.getElementById('mes_caducidad_contrato');
            var agno = document.getElementById('agno_caducidad_contrato');
            if (checkbox.checked == true) {
                dia.readOnly = false;
                mes.readOnly = false;
                agno.readOnly = false;
            }
            else {
                dia.readOnly = true;
                mes.readOnly = true;
                agno.readOnly = true;
            }

        }
    </script>

    <form id="form1" method="post">
        <div class="fieldsetInterno">
        <fieldset>
            <legend>Información del contrato vinculado a la orden de trabajo</legend>
            
            <!-- RUT DEL CLIENTE -->
            <%: Html.LabelFor(nvaOT => nvaOT.rutCliente) %>
                <%: Html.DropDownListFor(nvaOT => nvaOT.rutCliente, (List<SelectListItem>)ViewBag.listaRuts, new { @class = "text" })%>
                <%: Html.ValidationMessageFor(nvoContrato => nvoContrato.rutCliente)%>

            <!-- NOMBRE DEL CLIENTE (SOLO LECTURA)-->
            <%: Html.LabelFor(nvaOT => nvaOT.nombre_cliente) %>
                <%: Html.TextBoxFor(nvaOT => nvaOT.nombre_cliente, new { @class = "text", @readonly = "true" })%>
                <%: Html.ValidationMessageFor(nvaOT => nvaOT.nombre_cliente)%>

            <!-- N° DE CONTRATO -->
            <%: Html.LabelFor(nvaOT => nvaOT.contrato) %>
                <%: Html.DropDownListFor(nvaOT => nvaOT.contrato, (List<SelectListItem>)ViewBag.listaContratos, new { @class = "text" })%>
                <%: Html.ValidationMessageFor(nvoContrato => nvoContrato.contrato)%>

            <!-- DESCRIPCIÓN DEL CONTRATO -->
            <%: Html.LabelFor(nvaOT => nvaOT.descripcion_contrato) %>
                <%: Html.TextAreaFor(nvaOT => nvaOT.descripcion_contrato, new { @class = "text", @readonly = "true" })%>
                <%: Html.ValidationMessageFor(nvaOT => nvaOT.descripcion_contrato)%>
        </fieldset>
        </div>


        <div class="fieldsetInterno">
        <fieldset>
            <legend>Datos de la orden de trabajo</legend>
                <!-- CIUDAD DE EJECUCIÓN DE LA OT -->
                <%: Html.LabelFor(nvaOT => nvaOT.ciudad_ot) %>
                    <%: Html.TextBoxFor(nvaOT => nvaOT.ciudad_ot, new { @class = "text" })%>
                    <%: Html.ValidationMessageFor(nvaOT => nvaOT.ciudad_ot)%>
                <%: Html.LabelFor(nvaOT => nvaOT.comuna_ot) %>
                    <%: Html.TextBoxFor(nvaOT => nvaOT.comuna_ot, new { @class = "text" })%>
                    <%: Html.ValidationMessageFor(nvaOT => nvaOT.comuna_ot)%>

                <label>Fecha de inicio</label>
                    <%: Html.DropDownList("dia_ini_contrato", (List<SelectListItem>)ViewBag.listaDias, new { @style = "width: 55px;" })%>
                    <%: Html.DropDownList("mes_ini_contrato", (List<SelectListItem>)ViewBag.listaMeses, new { @style = "width: 130px;" })%>
                    <%: Html.ValidationMessageFor(nvoContrato => nvoContrato.dia_ini_contrato)%>
                    <%: Html.ValidationMessageFor(nvoContrato => nvoContrato.mes_ini_contrato)%>

                    <%: Html.TextBoxFor(nvoTrabajador => nvoTrabajador.agno_ini_contrato, new { @class = "text", @style = "width: 60px;" })%>
                    <%: Html.ValidationMessageFor(nvoTrabajador => nvoTrabajador.agno_ini_contrato)%>



                <label>Fecha caducidad contrato</label>
                    <%: Html.CheckBox("tieneTermino_contrato", false, new { @onchange="cambiaTieneFechaFinal(this)"})%>
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
                        <%: Html.ValidationMessageFor(nvoContrato => nvoContrato.servicioSeleccionado)%>

                    <label>Precio referencia del servicio</label>
                        <%: Html.TextBox("precioReferencia", (string)ViewBag.precioReferencia, new { @class = "text", @style = "width: 100px;", @readonly = "true" })%>
                        
                    <label>Precio acordado en contrato</label>
                        <%: Html.TextBox("precioPorContrato", (string)ViewBag.precioReferencia, new { @class = "text", @style = "width: 100px;"})%>
                        <%: Html.ValidationMessage("precioPorContrato")%>

                    <label>Condición para servicio</label>
                        <%: Html.TextArea("condicion_servicio", new { @style="max-width: 600px; min-width: 600px;"})%>

                    <div style="height: 30px; width: 20%; margin-right: auto; margin-left: 40%;"  text-align="center">
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
                                <b>N° servicio</b></td>
                            <td>
                                <b>Nombre del servicio</b></td>
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
                                    Response.Write(temp.nombre_servicio); //debe ir el nombre
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

            

            <div class="fieldsetInterno">
                <fieldset>
                    <legend>Descripción del contrato</legend>
                    <%: Html.TextAreaFor(nvoContrato => nvoContrato.breve_descripcion, new { @style="max-width: 600px; min-width: 600px;"})%>
                </fieldset>
            </div>    
        </fieldset>
        </div>
        <div style="height: 30px; width: 20%; margin-right: auto; margin-left: 40%;"  text-align="center">
            <input id="btn_agregarContrato" type="submit" name="btn_agregarContrato" value="Agregar contrato" />
        </div>
    </form>    
</asp:Content>


