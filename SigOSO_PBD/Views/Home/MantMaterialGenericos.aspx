<%@ Page Language="C#" MasterPageFile="~/Views/Shared/Administrador.Master" Inherits="System.Web.Mvc.ViewPage<SigOSO_PBD.Models.MaterialGenericoModel>" %>




<asp:Content ID="Content4" ContentPlaceHolderID="TitleContent" runat="server">
    Mantención materiales genéricos
</asp:Content>

<asp:Content ID="Content5" ContentPlaceHolderID="MainContent" runat="server">


    <form id="form1" method="post">
        <div class="fieldsetInterno">
        <fieldset>
            <legend>Agregar nuevo material</legend>
            <label>Nombre</label>
                <%: Html.TextBoxFor(nvoMat => nvoMat.nombre, new { @class = "text" })%>
                <%: Html.ValidationMessageFor(nvoMat => nvoMat.nombre)%>

            <label>¿En qué se mide?</label>
                <%: Html.DropDownList("id_unidad", (List<SelectListItem>)ViewBag.lista_unidades, new { @style = "width: 42%;" })%>

            <label>Glosa material</label>
                <%: Html.TextAreaFor(nvoMat => nvoMat.glosa_material, new { @class = "text" })%>
                <%: Html.ValidationMessageFor(nvoMat => nvoMat.glosa_material)%>

            <div>
                <% if (ViewBag.respuestaPost!= null) {%>
                    <%: ViewBag.respuestaPost %>
                <%}
                    %>
            </div>

            <div style="height: 30px; width: 20%; margin-right: auto; margin-left: 40%;" align="center">
                <input id="btn_agregarMaterial" type="submit" value="Agregar Material" />
            </div>
        </fieldset>
        </div>

        <div class="fieldsetInterno">
        <fieldset>
            <legend>Lista de materiales genéricos</legend>
                <%if (ViewBag.tabla != null) {                      
                    Response.Write(ViewBag.tabla);                       
                }                                                     
                %>
        </fieldset>
        </div>
            

    </form>
</asp:Content>


