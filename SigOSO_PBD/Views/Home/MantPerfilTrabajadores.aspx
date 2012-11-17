<%@ Page Language="C#" MasterPageFile="~/Views/Shared/Administrador.Master" Inherits="System.Web.Mvc.ViewPage<SigOSO_PBD.Models.perfilTrabajadorModel>" %>




<asp:Content ID="Content4" ContentPlaceHolderID="TitleContent" runat="server">
    Mantención Perfil de trabajadores
</asp:Content>

<asp:Content ID="Content5" ContentPlaceHolderID="MainContent" runat="server">


    <form id="form1" method="post">
        <div class="fieldsetInterno">
        <fieldset>
            <legend>Crear perfil trabajador</legend>
            <label>Nombre del cargo</label>
                <%: Html.TextBoxFor(nvoPerfil => nvoPerfil.nombre, new { @class = "text" })%>
                <%: Html.ValidationMessageFor(nvoPerfil => nvoPerfil.nombre)%>
            
            <div style="height: 30px; width: 20%; margin-right: auto; margin-left: 40%;" align="center">
                <input id="btn_agregarUnidad" type="submit" value="Agregar Unidad" />
            </div>
        </fieldset>
        </div>

        <div class="fieldsetInterno">
        <fieldset>
            <legend>Lista de cargos o perfiles</legend>
                <%if (ViewBag.tabla != null) {                      
                    Response.Write(ViewBag.tabla);                       
                }                                                     
                %>
        </fieldset>
        </div>
            

    </form>
</asp:Content>


