<%@ Page Language="C#" MasterPageFile="~/Views/Shared/Administrador.Master" Inherits="System.Web.Mvc.ViewPage<SigOSO_PBD.Models.RegisterModel>" %>

<asp:Content ID="registerTitle" ContentPlaceHolderID="TitleContent" runat="server">
    Registrar nuevo usuario
</asp:Content>

<asp:Content ID="registerContent" ContentPlaceHolderID="MainContent" runat="server">
    <div>
        Las contraseñas requeridas deben tener un mínimo de <%: Membership.MinRequiredPasswordLength %> caracteres de longitud.
    </div>

    <script src="<%: Url.Content("~/Scripts/jquery.validate.min.js") %>" type="text/javascript"></script>
    <script src="<%: Url.Content("~/Scripts/jquery.validate.unobtrusive.min.js") %>" type="text/javascript"></script>

    <% using (Html.BeginForm("Register", "Home", FormMethod.Post, new { @id="form1"})) { %>
        <%: Html.ValidationSummary(true, "La creación de la cuenta no ha sido exitosa. Por favor corrija los errores e intente nuevamente.") %>
        <div>
            <div class="fieldsetInterno">
            <fieldset>
                <legend>Información de cuenta</legend>
                
                <div class="editor-label">
                    <%: Html.LabelFor(m => m.UserName) %>
                </div>
                <div class="editor-field">
                    <%: Html.TextBoxFor(m => m.UserName) %>
                    <%: Html.ValidationMessageFor(m => m.UserName) %>
                </div>

                <div class="editor-label">
                    <%: Html.LabelFor(m => m.rut) %>
                </div>
                <div class="editor-field">
                    <%: Html.TextBoxFor(m => m.rut) %>
                    <%: Html.ValidationMessageFor(m => m.rut)%>
                </div>
                
                <div class="editor-label">
                    <%: Html.LabelFor(m => m.Email) %>
                </div>
                <div class="editor-field">
                    <%: Html.TextBoxFor(m => m.Email) %>
                    <%: Html.ValidationMessageFor(m => m.Email) %>
                </div>
                
                <div class="editor-label">
                    <%: Html.LabelFor(m => m.Password) %>
                </div>
                <div class="editor-field">
                    <%: Html.PasswordFor(m => m.Password) %>
                    <%: Html.ValidationMessageFor(m => m.Password) %>
                </div>
                
                <div class="editor-label">
                    <%: Html.LabelFor(m => m.ConfirmPassword) %>
                </div>
                <div class="editor-field">
                    <%: Html.PasswordFor(m => m.ConfirmPassword) %>
                    <%: Html.ValidationMessageFor(m => m.ConfirmPassword) %>
                </div>

                <div class="editor-field">
                    <%: Html.DropDownListFor(m => m.tipoUsuario, (List<SelectListItem>)ViewBag.listaTiposUsuarios) %>
                    <%: Html.ValidationMessageFor(m => m.tipoUsuario)%>
                </div>
                
                <p>
                    <input type="submit" value="Register" />
                </p>
            </fieldset>
            </div>
        </div>

    <% } %>
</asp:Content>
