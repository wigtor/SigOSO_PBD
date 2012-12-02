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
                <%: Html.DropDownListFor(nvoTrabajador => nvoTrabajador.id_contrato, (List<SelectListItem>)ViewBag.listaContratos, new { @class = "text", @style="width:300px;" })%>
                <%: Html.ValidationMessageFor(nvoTrabajador => nvoTrabajador.id_contrato)%>

            <input id="Submit1" type="submit" name="btn_cargar" value="Cargar detalles"/>

        </fieldset>
        </div>

    </form>
        
        <div class="fieldsetInterno">
        <fieldset>
            <legend>Información del contrato</legend>
                
            <%
                SigOSO_PBD.Models.Contrato contrato;
                if (ViewBag.contratoDetallado != null)
                {
                    contrato = (SigOSO_PBD.Models.Contrato)ViewBag.contratoDetallado;
                }
                else {
                    contrato = new SigOSO_PBD.Models.Contrato();
                    contrato.nombreCliente = "";
                    contrato.rutCliente = "";
                    contrato.tieneTermino = "false";
                    //contrato.dia_ini_contrato = "0";
                    
                }
            %>
                <label>Nombre cliente</label>
                    <%: Html.TextBox("nombreCliente", contrato.nombreCliente, new { @disabled = "disabled"})%>

                <label>Fecha inicio</label>
                    <%: Html.TextBox("fecha_inicio", contrato.dia_ini_contrato + "-" + contrato.mes_ini_contrato + "-" + contrato.agno_ini_contrato, new { @disabled = "disabled" })%>

                    <label>Fecha término</label>
                    <%
                        string valorFechaTerm;
                        if (contrato.tieneTermino.Contains("true"))
                        {
                            valorFechaTerm = contrato.dia_caducidad_contrato + "-" + contrato.mes_caducidad_contrato + "-" + contrato.agno_caducidad_contrato;
                        }
                        else
                        {
                            valorFechaTerm = "No tiene";
                        }
                    %>
                    <%: Html.TextBox("fecha_termino", valorFechaTerm, new { @disabled = "disabled" }) %>
                    
                    
                    <div class="fieldsetInterno">
                    <fieldset>
                        <legend>Descripción del contrato</legend>
                        <%: Html.TextArea("breve_descripcion", contrato.breve_descripcion, new { @style="width:95%;"}) %>
                    </fieldset>
                    </div>


                    <div class="fieldsetInterno">
                    <fieldset>
                        <legend>Servicios estipulados en el contrato</legend>

                        <table>
                            <tr>
                                <td><b>
                                    N° servicio</b></td>
                                <td><b>
                                    Nombre del servicio</b></td>
                                <td><b>
                                    N° Precio estipulado</b></td>
                                <td><b>
                                    Detalles</b></td>
                            </tr>
                            
                            <%
                            if (ViewBag.serviciosDelContrato != null) {
                                List<SigOSO_PBD.Models.ServicioListado> lst = ViewBag.serviciosDelContrato;
                                foreach (SigOSO_PBD.Models.ServicioListado temp in lst)
                                {
                                    Response.Write("<tr>");
                                    Response.Write("<td>");
                                    Response.Write(temp.id_servicio+"</td>");
                                    Response.Write("<td>");
                                    Response.Write(temp.nombre_servicio+"</td>");
                                    Response.Write("<td>");
                                    Response.Write(temp.precio_acordado+"</td>");
                                    Response.Write("<td>");
                                    Response.Write("<textarea cols=\"20\" id=\"descripcion_serv_" + temp.id_servicio + "\" name=\"descripcion_serv_" + temp.id_servicio + "\" rows=\"2\" style=\"width:95%;\">"+temp.descripcion+"</textarea></td>");
                                    Response.Write("</tr>");
                                }
                            }
                            %>
                        </table>
                        
                    </fieldset>
                    </div>

        </fieldset>
        </div>

</asp:Content>


