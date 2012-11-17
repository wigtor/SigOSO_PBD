<%@ Page Language="C#" MasterPageFile="~/Views/Shared/Administrador.Master" Inherits="System.Web.Mvc.ViewPage<SigOSO_PBD.Models.agregarUnidadModel>" %>




<asp:Content ID="Content4" ContentPlaceHolderID="TitleContent" runat="server">
    Mantención medidas de material
</asp:Content>

<asp:Content ID="Content5" ContentPlaceHolderID="MainContent" runat="server">


    <form id="form1" method="post">
        <fieldset>
            <legend>Ingresar nueva unidad</legend>
            <label>Nombre</label>            
                <%: Html.TextBoxFor(nvaUnidad => nvaUnidad.nombre, new { @class = "text" })%>
                <%: Html.ValidationMessageFor(nvaUnidad => nvaUnidad.nombre)%>

            <label>Abreviatura</label>
                <%: Html.TextBoxFor(nvaUnidad => nvaUnidad.abreviatura, new { @class = "text" })%>
                <%: Html.ValidationMessageFor(nvaUnidad => nvaUnidad.abreviatura)%>

            <div>
                <% if (ViewBag.respuestaPost!= null) {%>
                    <%: ViewBag.respuestaPost %>
                <%}
                    %>
            </div>

            <div style="height: 30px; width: 20%; margin-right: auto; margin-left: 40%;" align="center">
                <input id="btn_agregarUnidad" type="submit" value="Agregar Unidad" />
            </div>
        </fieldset>

        <fieldset>
            <legend>Lista de unidades</legend>
                <%if (ViewBag.tabla != null) {   
                      
                                                         
                    Response.Write(ViewBag.tabla);                       
                }                                                     
                %>
        </fieldset>

            

    </form>
</asp:Content>


