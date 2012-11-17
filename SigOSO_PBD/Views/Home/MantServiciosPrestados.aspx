<%@ Page Language="C#" MasterPageFile="~/Views/Shared/Administrador.Master" Inherits="System.Web.Mvc.ViewPage<SigOSO_PBD.Models.agregarServicioModel>"%>




<asp:Content ID="Content4" ContentPlaceHolderID="TitleContent" runat="server">
    Mantencion de servicios
</asp:Content>

<asp:Content ID="Content5" ContentPlaceHolderID="MainContent" runat="server">
     


    <form action="" id="form1" method="post" style="height: auto;">
        <fieldset class="condetenedor_agregar_servicio">
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
                
                <label>Visibilidad</label>
                <% bool vis1=false;
                    if(ViewBag.VisibilidadServicio!=null){
                       vis1=(bool)ViewBag.VisibilidadServicio;
                       
                   } %>  
                    <%: Html.CheckBox("visibilidad", vis1)%>
                <div style="height: 30px; width: 20%; margin-right: auto; margin-left: 40%;" align="center">
                    <input id="btn_agregar_servicio" name="btn_submit" type="submit" value="Agregar Servicio" />
                </div>
        </fieldset>

        <fieldset class="condetenedor_modificar_servicio">
            <legend>Modificar servicio</legend>
                <label>Nombre Servicio</label>            
                    <%: Html.TextBoxFor(nvoServicio => nvoServicio.nombreServicio)%>
                    <%: Html.ValidationMessageFor(nvoServicio => nvoServicio.nombreServicio)%>

                <label>Precio pizarra</label>  
                    <%: Html.TextBoxFor(nvoServicio => nvoServicio.precioPizarra)%>
                    <%: Html.ValidationMessageFor(nvoServicio => nvoServicio.precioPizarra)%>

                <label>Factor bono</label>    
                    <%: Html.TextBoxFor(nvoServicio => nvoServicio.factorBono)%>
                    <%: Html.ValidationMessageFor(nvoServicio => nvoServicio.factorBono)%>

                <label>Visibilidad</label>
                <% bool vis2=false;
                    if(ViewBag.VisibilidadServicio!=null){
                       vis2=(bool)ViewBag.VisibilidadServicio;
                       
                   } %>  
                    <%: Html.CheckBox("visibilidad", vis2)%>
                <div>
                    <%: ViewBag.respuestaPost%>
                </div>   
                <div style="height: 30px; width: 20%; margin-right: auto; margin-left: 40%;" align="center">
                    <input id="btn_modificar_servicio" name="btn_submit" type="submit" value="Guardar cambios" />
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
     <%if (ViewBag.ScriptOcultar != null)
       {
           Response.Write(ViewBag.ScriptOcultar);                       
    }                                                     
    %>
</asp:Content>


