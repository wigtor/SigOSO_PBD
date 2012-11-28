<%@ Page Language="C#" MasterPageFile="~/Views/Shared/Administrador.Master" Inherits="System.Web.Mvc.ViewPage<SigOSO_PBD.Models.OrdenTrabajoModel>" %>




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
            
            <!-- RUT DEL CLIENTE Y SU NOMBRE -->
            <%: Html.LabelFor(nvaOT => nvaOT.cliente)%>
                <%: Html.DropDownListFor(nvaOT => nvaOT.cliente, (List<SelectListItem>)ViewBag.listaRuts, new { @class = "text", @onchange="submit()" })%>
                <%: Html.ValidationMessageFor(nvoContrato => nvoContrato.cliente)%>

            <!-- N° DE CONTRATO -->
            <%: Html.LabelFor(nvaOT => nvaOT.contrato) %>
                <%: Html.DropDownListFor(nvaOT => nvaOT.contrato, (List<SelectListItem>)ViewBag.listaContratos, new { @class = "text" })%>
                <%: Html.ValidationMessageFor(nvoContrato => nvoContrato.contrato)%>

            <!-- DESCRIPCIÓN DEL CONTRATO -->
            <%: Html.LabelFor(nvaOT => nvaOT.descripcion_contrato) %>
                <%: Html.TextAreaFor(nvaOT => nvaOT.descripcion_contrato, new { @class = "text", @style = "max-width: 600px; min-width: 600px;", @readonly = "true" })%>
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

                <!-- COMUNA DE EJECUCIÓN DE LA OT -->
                <%: Html.LabelFor(nvaOT => nvaOT.comuna_ot) %>
                    <%: Html.TextBoxFor(nvaOT => nvaOT.comuna_ot, new { @class = "text" })%>
                    <%: Html.ValidationMessageFor(nvaOT => nvaOT.comuna_ot)%>
                
                <!-- DIRECCIÓN DE EJECUCIÓN DE LA OT -->
                <%: Html.LabelFor(nvaOT => nvaOT.direccion_ot) %>
                    <%: Html.TextBoxFor(nvaOT => nvaOT.direccion_ot, new { @class = "text" })%>
                    <%: Html.ValidationMessageFor(nvaOT => nvaOT.direccion_ot)%>

                <label>Fecha de inicio</label>
                    <%: Html.DropDownList("dia_ini_ot", (List<SelectListItem>)ViewBag.listaDias, new { @style = "width: 55px;" })%>
                    <%: Html.DropDownList("mes_ini_ot", (List<SelectListItem>)ViewBag.listaMeses, new { @style = "width: 130px;" })%>
                    <%: Html.ValidationMessageFor(nvoContrato => nvoContrato.dia_ini_ot)%>
                    <%: Html.ValidationMessageFor(nvoContrato => nvoContrato.mes_ini_ot)%>

                    <%: Html.TextBoxFor(nvoTrabajador => nvoTrabajador.agno_ini_ot, new { @class = "text", @style = "width: 60px;" })%>
                    <%: Html.ValidationMessageFor(nvoTrabajador => nvoTrabajador.agno_ini_ot)%>



                <label>Fecha de plazo máximo</label>
                    <%: Html.DropDownList("dia_fin_ot", (List<SelectListItem>)ViewBag.listaDias, new { @style = "width: 55px;" })%>
                    <%: Html.DropDownList("mes_fin_ot", (List<SelectListItem>)ViewBag.listaMeses, new { @style = "width: 130px;" })%>
                    <%: Html.ValidationMessageFor(nvoContrato => nvoContrato.dia_fin_ot)%>
                    <%: Html.ValidationMessageFor(nvoContrato => nvoContrato.mes_fin_ot)%>

                    <%: Html.TextBoxFor(nvoContrato => nvoContrato.agno_fin_ot, new { @class = "text", @style = "width: 60px;" })%>
                    <%: Html.ValidationMessageFor(nvoContrato => nvoContrato.agno_fin_ot)%>
                
                <div class="fieldsetInterno">
                <fieldset>
                    <legend>Servicios a prestar</legend>
                    
                    <label>Seleccione un servicio para agregar</label>
                        <%: Html.DropDownListFor(nvoContrato => nvoContrato.servicioSeleccionado, (List<SelectListItem>)ViewBag.listaServicios, new { @style = "width: 70%;", @onchange = "submit()" })%>
                        <%: Html.ValidationMessageFor(nvoContrato => nvoContrato.servicioSeleccionado)%>

                    <%: Html.LabelFor(nvaOT => nvaOT.precioReferenciaContrato)%>
                        <%: Html.TextBoxFor(nvoContrato => nvoContrato.precioReferenciaContrato, new { @class = "text", @style = "width: 100px;", @readonly = "true" })%>
                    
                    <%: Html.LabelFor(nvaOT => nvaOT.cantidadDelServicio)%>
                        <%: Html.TextBoxFor(nvoContrato => nvoContrato.cantidadDelServicio, new { @class = "text", @style = "width: 100px;" })%>
                        <%: Html.ValidationMessageFor(nvaOT => nvaOT.cantidadDelServicio)%>
                        
                    <%: Html.LabelFor(nvaOT => nvaOT.precioFinal)%>
                        <%: Html.TextBoxFor(nvaOT => nvaOT.precioFinal, new { @class = "text", @style = "width: 100px;" })%>
                        <%: Html.ValidationMessageFor(nvaOT => nvaOT.precioFinal)%>

                    <%: Html.LabelFor(nvaOT => nvaOT.breve_descripcion)%>
                        <%: Html.TextAreaFor(nvaOT => nvaOT.breve_descripcion, new { @style = "max-width: 600px; min-width: 600px;" })%>

                    <div style="height: 30px; width: 20%; margin-right: auto; margin-left: 40%;"  text-align="center">
                        <input id="btn_agregarServicio" type="submit" name="btn_agregarServicio" value="Agregar trabajo" />
                    </div>

                </fieldset>
                </div>

                <div class="fieldsetInterno">
                <fieldset>
                    <legend>Trabajos de la orden</legend>
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

            
        </fieldset>
        </div>
        <div style="height: 30px; width: 20%; margin-right: auto; margin-left: 40%;"  text-align="center">
            <input id="btn_agregarContrato" type="submit" name="btn_agregarContrato" value="Agregar contrato" />
        </div>
    </form>    
</asp:Content>


