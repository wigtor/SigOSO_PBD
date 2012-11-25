<%@ Page Language="C#" MasterPageFile="~/Views/Shared/Administrador.Master" Inherits="System.Web.Mvc.ViewPage<SigOSO_PBD.Models.agregarServicioModel>"%>




<asp:Content ID="Content4" ContentPlaceHolderID="TitleContent" runat="server">
    Mantencion de servicios
</asp:Content>

<asp:Content ID="Content5" ContentPlaceHolderID="MainContent" runat="server">
     


    <form action="" id="form1" method="post" style="height: auto;">
        <fieldset>
            <legend class="condetenedor_agregar_servicio">Crear nuevo servicio</legend>
            <legend class="condetenedor_modificar_servicio">Modificar servicio</legend>
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
                <% bool vis1=false;
                    if(ViewBag.VisibilidadServicio!=null){
                       vis1=(bool)ViewBag.VisibilidadServicio;
                       
                   } %>  
                    <%: Html.CheckBox("visibilidad1", vis1)%>
                <div style="height: 30px; width: 20%; margin-right: auto; margin-left: 40%;" align="center" class="condetenedor_agregar_servicio">
                    <input id="btn_agregar_servicio" name="btn_submit" type="submit" value="Agregar Servicio" />
                </div>
                <div style="height: 30px; width: 20%; margin-right: auto; margin-left: 40%;" align="center" class="condetenedor_modificar_servicio">
                    <input id="btn_modificar_servicio" name="btn_submit" type="submit" value="Guardar cambios" />
                </div>
        </fieldset>



        <div class="fieldsetInterno">
        <fieldset>
            <legend>Lista Servicios</legend>
                <%if (ViewBag.tabla != null) {                      
                    Response.Write(ViewBag.tabla);                       
                }                                                     
                %>
        </fieldset>        
        </div>
    <%if (ViewBag.id_servicio != null)
      {
          Response.Write(ViewBag.id_servicio);                       
    }                                                     
    %>

    </form>

     <%if (ViewBag.ScriptOcultar != null)
       {
           Response.Write(ViewBag.ScriptOcultar);                       
    }                                                     
    %>
</asp:Content>


