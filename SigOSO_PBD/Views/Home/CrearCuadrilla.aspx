<%@ Page Language="C#" MasterPageFile="~/Views/Shared/Administrador.Master" Inherits="System.Web.Mvc.ViewPage<SigOSO_PBD.Models.buscarTrabajadorModel>" %>




<asp:Content ID="Content4" ContentPlaceHolderID="TitleContent" runat="server">
    Crear Cuadrilla
</asp:Content>

<asp:Content ID="Content5" ContentPlaceHolderID="MainContent" runat="server">
    

    <form id="form1" method="post">
        <div class="fieldsetInterno">
        <fieldset>
            <legend>Buscar trabajador</legend>
            <label>Rut trabajador</label>
                <%
                    string rut_ingresado = "";
                    if (ViewBag.rut_trabajador != null)
                    {
                        rut_ingresado = ViewBag.rut_trabajador;
                    }
                %>
                <%: Html.TextBoxFor(nvoTrabajador => nvoTrabajador.rut_trabajador, new { @class = "text" })%>
                <input id="Submit1" type="submit" name="btn_cargar" value="Cargar"/>
                <%: Html.ValidationMessageFor(nvoTrabajador => nvoTrabajador.rut_trabajador)%>

            <label>Nombre trabajador</label>
                
                <%: Html.TextBoxFor(nvoTrabajador => nvoTrabajador.nombre_trabajador, new { @class = "text" })%>
                <%: Html.ValidationMessage("nombreCliente")%>
        </fieldset>
        </div>

        <div>
            Seleccione un trabajador y presione el botón 'agregar' para agregarlos a la lista de trabajadores que conformarán la cuadrilla
            El primer trabajador que seleccione para la cuadrilla será asignado como jefe de esta.
        </div>
        
        <div class="fieldsetInterno">
        <fieldset>
            <legend>Resultados de búsqueda de trabajadores</legend>
                <table align="center" style="width: 100%">
                <tr>
                    <td>
                        <b>Rut</b></td>
                    <td>
                        <b>Nombre</b></td>
                    <td>
                        <b>Teléfonos</b></td>
                    <td>
                        <b>Estado</b></td>
                    <td>
                        <b>Acción</b></td>
                </tr>
                <%
                    if (ViewBag.listaTrabajadores != null)
                    {
                        List<SigOSO_PBD.Models.ListarTrabajadorModel> resultados = (List<SigOSO_PBD.Models.ListarTrabajadorModel>)ViewBag.listaTrabajadores;
                        foreach (SigOSO_PBD.Models.ListarTrabajadorModel temp in resultados)
                        {
                            Response.Write("<tr>");
                
                            Response.Write("<td>");
                            Response.Write(temp.rut);
                            Response.Write("</td>");

                            Response.Write("<td>");
                            Response.Write(temp.nombre);
                            Response.Write("</td>");

                            Response.Write("<td>");
                            Response.Write(temp.telefono1 + "</br>"+temp.telefono2);
                            Response.Write("</td>");

                            Response.Write("<td>");
                            Response.Write(temp.estado);
                            Response.Write("</td>");

                            Response.Write("<td>");
                            Response.Write("<input id=\"editar_" + temp.rut + "\" name=\"editar_" + temp.rut + "\" type=\"button\" >");
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
            <legend>Trabajadores que integrarán la cuadrilla</legend>
                


        </fieldset>
        </div>

    </form>    
</asp:Content>


