﻿<%@ Page Language="C#" MasterPageFile="~/Views/Shared/Administrador.Master" Inherits="System.Web.Mvc.ViewPage<SigOSO_PBD.Models.logModel>" %>




<asp:Content ID="Content4" ContentPlaceHolderID="TitleContent" runat="server">
    Configurar auditoría
</asp:Content>

<asp:Content ID="Content5" ContentPlaceHolderID="MainContent" runat="server">


    <form id="form1" method="post">
        <div class="fieldsetInterno"> 
            
            <table style="width: 100%;">
                <tr>
                    <td>
                        &nbsp;
                        <b>Nombre tabla</b>
                    </td>
                    <td>
                        &nbsp;
                        <b>Registro activo</b>
                    </td>
                    <td>
                        &nbsp;
                        <b>INSERT</b>
                    </td>
                    <td>
                        &nbsp;
                        <b>UPDATE</b>
                    </td>
                    <td>
                        &nbsp;
                        <b>DELETE</b>
                    </td>
                </tr>
                <%  if (ViewBag.tabla != null) {
                        List<SigOSO_PBD.Models.logModel> tabla = (List<SigOSO_PBD.Models.logModel>)ViewBag.tabla;
                            
                        foreach (SigOSO_PBD.Models.logModel temp in tabla) {
                            Response.Write("<tr>");
                            
                            Response.Write("<td>&nbsp;");
                            Response.Write(temp.nombre);
                            Response.Write("</td>");
                            
                            Response.Write("<td>&nbsp;");
                            %>
                                <%: Html.CheckBox("nombreTabla___"+temp.nombre+"___GEN", temp.general_activo) %>
                            <%
                            Response.Write("</td>");

                            Response.Write("<td>&nbsp;");
                            %>
                                <%: Html.CheckBox("nombreTabla___" + temp.nombre + "___INS", temp.insert_activo)%>
                            <%
                            Response.Write("</td>");

                            Response.Write("<td>&nbsp;");
                            %>
                                <%: Html.CheckBox("nombreTabla___" + temp.nombre + "___UPD", temp.update_activo)%>
                            <%
                            Response.Write("</td>");

                            Response.Write("<td>&nbsp;");
                            %>
                                <%: Html.CheckBox("nombreTabla___" + temp.nombre + "___DEL", temp.delete_activo)%>
                            <%
                            Response.Write("</td>");
                            
                            Response.Write("</tr>");
                        }     
                    }
                %>

            </table>
            <div>
                <%
                    if (ViewBag.mensaje != null)
                    {
                        Response.Write(ViewBag.mensaje);
                    }
                     %>
            </div>
            <div style="height: 30px; width: 20%; margin-right: auto; margin-left: 40%;" align="center">
                <input id="btn_guardar" type="submit" name="btn_guardar" value="guardar cambios" />
            </div>
        </div>
    </form>
</asp:Content>


