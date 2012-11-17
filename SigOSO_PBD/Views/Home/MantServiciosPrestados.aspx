<%@ Page Language="C#" MasterPageFile="~/Views/Shared/Administrador.Master" Inherits="System.Web.Mvc.ViewPage<SigOSO_PBD.Models.agregarServicioModel>" %>




<asp:Content ID="Content4" ContentPlaceHolderID="TitleContent" runat="server">
    Mantencion de servicios
</asp:Content>

<asp:Content ID="Content5" ContentPlaceHolderID="MainContent" runat="server">
    <form id="form1" method="post" style="height: auto;">
        <div class="fieldsetInterno">
        <fieldset>
            <legend>Crear nuevo servicio</legend>
                <label>Nombre Servicio</label>            
                    <%: Html.TextBoxFor(nvoServicio => nvoServicio.nombreServicio)%>
                    <%: Html.ValidationMessageFor(nvoServicio => nvoServicio.nombreServicio)%>

                <label>Precio pizarra</label>  
                    <%: Html.TextBoxFor(nvoServicio => nvoServicio.precioPizarra)%>
                    <%: Html.ValidationMessageFor(nvoServicio => nvoServicio.precioPizarra)%>

                <label>Factor bono</label>    
                    <%: Html.TextBoxFor(nvoServicio => nvoServicio.factorBono)%>
                    <%: Html.ValidationMessageFor(nvoServicio => nvoServicio.factorBono)%>

                <div>
                    <%: ViewBag.respuestaPost%>
                </div>   
                <div style="height: 30px; width: 20%; margin-right: auto; margin-left: 40%;" align="center">
                    <input id="btn_agregarServicio" type="submit" value="Agregar Servicio" />
                </div>
        </fieldset>
        </div>
    
        <div class="fieldsetInterno">
        <fieldset>
            <legend>Lista Servicios</legend>
                <%if (ViewBag.tabla != null) {                      
                    Response.Write(ViewBag.tabla);                       
                }                                                     
                %>
        </fieldset>
        </div>
    
    </form>

</asp:Content>


