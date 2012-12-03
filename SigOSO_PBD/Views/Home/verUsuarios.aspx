<%@ Page Language="C#" MasterPageFile="~/Views/Shared/Administrador.Master" Inherits="System.Web.Mvc.ViewPage<SigOSO_PBD.Models.RegisterModel>" %>




<asp:Content ID="Content4" ContentPlaceHolderID="TitleContent" runat="server">
    Ver usuarios del sistema
</asp:Content>

<asp:Content ID="Content5" ContentPlaceHolderID="MainContent" runat="server">


    <form id="form1" method="post">
        <div class="fieldsetInterno"> 
        <fieldset>
            <legend>Usuarios</legend>
                <table>
                    <tr>
                        <td>
                            <%: Html.LabelFor(user => user.UserName) %>
                        </td>
                        <td>
                            <%: Html.LabelFor(user => user.Email) %>
                        </td>
                        <td>
                            <%: Html.LabelFor(user => user.tipoUsuario) %>
                        </td>
                        <td>
                            <%: Html.Label("Acción") %>
                        </td>
                    </tr>
                    <%
                        string rol;
                        if (ViewBag.listaUsuarios != null)
                        {
                            MembershipUserCollection lst = (MembershipUserCollection)ViewBag.listaUsuarios;
                            foreach (MembershipUser temp in lst)
                            {
                                Response.Write("<tr>");

                                Response.Write("<td>&nbsp;");
                                Response.Write(temp.UserName);
                                Response.Write("</td>");

                                Response.Write("<td>&nbsp;");
                                Response.Write(temp.Email);
                                Response.Write("</td>");

                                Response.Write("<td>&nbsp;");
                                rol = Roles.GetRolesForUser(temp.UserName)[0];
                                Response.Write(rol);
                                Response.Write("</td>");

                                Response.Write("<td>&nbsp;");
                                Response.Write("<input id=\"quitar_" + temp.UserName + "\" type=\"submit\" value=\"Eliminar cuenta\" name=\"quitar_" + temp.UserName + "\">");
                                Response.Write("</td>");
                                
                                Response.Write("</tr>");

                            }
                        }
                    %>
                </table>
        </fieldset>
        </div>

    </form>
</asp:Content>


