<%@ Page Language="C#" MasterPageFile="~/Views/Shared/Administrador.Master" Inherits="System.Web.Mvc.ViewPage<SigOSO_PBD.Models.agregarServicioModel>" %>




<asp:Content ID="Content4" ContentPlaceHolderID="TitleContent" runat="server">
    Mantencion de servicios
</asp:Content>

<asp:Content ID="Content5" ContentPlaceHolderID="MainContent" runat="server">
    <form id="form1" method="post" style="height: auto;">
        <fieldset>
            <legend>Crear nuevo servicio</legend>
                <table style="width: 100%;">
                    <tr>
                        <td class="input-medium" style="width: 200px">
                            &nbsp;
                            Nombre Servicio</td>
                        <td>
                            &nbsp;
                            <%: Html.TextBoxFor(nvoServicio => nvoServicio.nombreServicio)%>
                            <%: Html.ValidationMessageFor(nvoServicio => nvoServicio.nombreServicio)%></td>
                    </tr>
                    <tr>
                        <td class="input-medium" style="width: 200px">
                            &nbsp;
                            Precio pizarra</td>
                        <td>
                            &nbsp;
                            <%: Html.TextBoxFor(nvoServicio => nvoServicio.precioPizarra)%>
                            <%: Html.ValidationMessageFor(nvoServicio => nvoServicio.precioPizarra)%></td>
                    </tr>
                    <tr>
                        <td class="input-medium" style="width: 200px">
                            &nbsp;
                            Factor bono</td>
                        <td>
                            &nbsp;
                            <%: Html.TextBoxFor(nvoServicio => nvoServicio.factorBono)%>
                            <%: Html.ValidationMessageFor(nvoServicio => nvoServicio.factorBono)%></td>
                    </tr>
                </table>     
                <div>
                    <%: ViewBag.respuestaPost%>
                </div>   
                <div style="height: 30px; width: 20%; margin-right: auto; margin-left: 40%;" align="center">
                    <input id="btn_agregarServicio" type="submit" value="Agregar Servicio" />
                </div>
        </fieldset>
    
        <fieldset>
            <legend>Lista Servicios</legend>
                <%if (ViewBag.tabla != null) {                      
                    Response.Write(ViewBag.tabla);                       
                }                                                     
                %>
        </fieldset>
    
    
    </form>

</asp:Content>


