<%@ Page Language="C#" MasterPageFile="~/Views/Shared/Administrador.Master" Inherits="System.Web.Mvc.ViewPage<SigOSO_PBD.Models.DatosAuditoriaModel>" %>




<asp:Content ID="Content4" ContentPlaceHolderID="TitleContent" runat="server">
    Ver auditoría
</asp:Content>

<asp:Content ID="Content5" ContentPlaceHolderID="MainContent" runat="server">


    <form id="form1" method="post">
        <div class="fieldsetInterno"> 
        <fieldset>
            <legend>Datos de consulta</legend>
            <label>Nombre Tabla</label>
                <%: Html.DropDownList("nombreTabla", (List<SelectListItem>)ViewBag.listaTablas, new { @style = "width: 170px;" })%>

            <label>Operación</label>
                <%: Html.DropDownList("operacion", (List<SelectListItem>)ViewBag.listaOperaciones, new { @style = "width: 130px;" })%>

            <label>Desde</label>
                <%: Html.DropDownList("dia_ini", (List<SelectListItem>)ViewBag.listaDias, new { @style = "width: 55px;" })%>
                <%: Html.DropDownList("mes_ini", (List<SelectListItem>)ViewBag.listaMeses, new { @style = "width: 130px;" })%>
                <%: Html.ValidationMessageFor(datosAuditoria => datosAuditoria.dia_ini)%>
                <%: Html.ValidationMessageFor(datosAuditoria => datosAuditoria.mes_ini)%>

                <%: Html.TextBoxFor(datosAuditoria => datosAuditoria.agno_ini, new { @class = "text", @style = "width: 60px;" })%>
                <%: Html.ValidationMessageFor(nvoTrabajador => nvoTrabajador.agno_ini)%>


            <label>Hasta</label>
                <%: Html.DropDownList("dia_fin", (List<SelectListItem>)ViewBag.listaDias, new { @style = "width: 55px;" })%>
                <%: Html.DropDownList("mes_fin", (List<SelectListItem>)ViewBag.listaMeses, new { @style = "width: 130px;" })%>
                <%: Html.ValidationMessageFor(datosAuditoria => datosAuditoria.dia_fin)%>
                <%: Html.ValidationMessageFor(datosAuditoria => datosAuditoria.mes_fin)%>

                <%: Html.TextBoxFor(datosAuditoria => datosAuditoria.agno_fin, new { @class = "text", @style = "width: 60px;" })%>
                <%: Html.ValidationMessageFor(nvoTrabajador => nvoTrabajador.agno_fin)%>

            <label>Mostrar máximo:</label>
            <%: Html.DropDownList("cuantosVer", (List<SelectListItem>)ViewBag.cuantosDatosVer, new { @style = "width: 100px;" })%>
            resultados
        </fieldset>

            
        
            <div style="height: 30px; width: 20%; margin-right: auto; margin-left: 40%;" align="center">
                <input id="btn_cargarr" type="submit" name="btn_cargar" value="Cargar" />
            </div>

        </div>



        <div class="fieldsetInterno"> 
        <fieldset>
            <legend>resultados</legend>

            <table style="width: 100%;">
                <tr>
                    <td>
                        &nbsp;
                        <b>Nombre tabla</b>
                    </td>
                    <td>
                        &nbsp;
                        <b>Operación</b>
                    </td>
                    <td>
                        &nbsp;
                        <b>Timestamp</b>
                    </td>
                    <td>
                        &nbsp;
                        <b>Datos antes</b>
                    </td>
                    <td>
                        &nbsp;
                        <b>Datos después</b>
                    </td>
                </tr>
                <%  if (ViewBag.tablaLogs != null)
                    {
                        List<SigOSO_PBD.Models.DatoLogForTabla> tabla = (List<SigOSO_PBD.Models.DatoLogForTabla>)ViewBag.tablaLogs;
                        if (tabla.Count == 0) {
                            Response.Write("<tr>");

                            Response.Write("<td>&nbsp; La busqueda no arrojó resultados");
                            Response.Write("</td>");
                            Response.Write("</tr>");
                        }
                        
                        foreach (SigOSO_PBD.Models.DatoLogForTabla temp in tabla)
                        {
                            Response.Write("<tr>");
                            
                            Response.Write("<td>&nbsp;");
                            Response.Write(temp.nombreTabla);
                            Response.Write("</td>");
                            
                            Response.Write("<td>&nbsp;");
                            Response.Write(temp.operacion);
                            Response.Write("</td>");

                            Response.Write("<td>&nbsp;");
                            Response.Write(temp.timestamp);
                            Response.Write("</td>");

                            Response.Write("<td>&nbsp;");
                            Response.Write("<textarea cols=\"15\" rows=\"3\" width=\"160px\">");
                            Response.Write(temp.datosAntes);
                            Response.Write("</textarea>");
                            Response.Write("</td>");

                            Response.Write("<td>&nbsp;");
                            Response.Write("<textarea cols=\"15\" rows=\"3\" width=\"160px\">");
                            Response.Write(temp.datosDespues);
                            Response.Write("</textarea>");
                            Response.Write("</td>");
                            
                            Response.Write("</tr>");
                        }     
                    }
                %>

            </table>
        </fieldset>
        <div>
            <%
                if (ViewBag.mensaje != null)
                {
                    Response.Write(ViewBag.mensaje);
                }
                    %>
        </div>
        </div>
    </form>
</asp:Content>


