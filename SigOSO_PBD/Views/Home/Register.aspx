<%@ Page Language="C#" MasterPageFile="~/Views/Shared/Administrador.Master" Inherits="System.Web.Mvc.ViewPage<SigOSO_PBD.Models.RegisterModel>" %>

<asp:Content ID="registerTitle" ContentPlaceHolderID="TitleContent" runat="server">
    Registrar nuevo usuario
</asp:Content>

<asp:Content ID="registerContent" ContentPlaceHolderID="MainContent" runat="server">
    <div>
        Las contraseñas requeridas deben tener un mínimo de <%: Membership.MinRequiredPasswordLength %> caracteres de longitud.<br>
        Si el usuario que desea agregar es un trabajador jefe de cuadrilla entonces su nombre de usuario debe ser su Rut, según el rut que usted seleccione de entre los trabajadores.
    </div>

    <script src="<%: Url.Content("~/Scripts/jquery.validate.min.js") %>" type="text/javascript"></script>
    <script src="<%: Url.Content("~/Scripts/jquery.validate.unobtrusive.min.js") %>" type="text/javascript"></script>
    <script type="text/javascript">
        function cambioTipoUsuario(selector) {
            if (selector.value == "Jefe_cuadrilla") {
                $("#rut").removeAttr("disabled");
                $("#UserName").attr("readOnly", "true");

                var rut = $("#rut").val();
                if (rut == "-1") {
                    $("#UserName").val("");
                }
                else {
                    $("#UserName").val(rut);
                }
                $("#rut").val("-1");
            }
            else {
                $("#UserName").removeAttr("readOnly");
                $("#rut").attr("disabled", "disabled");
                $("#rut").val("-1");
            }
        }

        function cambioRutUsuario() {
            var rut = $("#rut").val();
            if (rut == "-1") {
                $("#UserName").val("");
            }
            else {
                $("#UserName").val(rut);
            }
        }
    </script>

    <% using (Html.BeginForm("Register", "Home", FormMethod.Post, new { @id="form1"})) { %>
        <%: Html.ValidationSummary(true, "La creación de la cuenta no ha sido exitosa. Por favor corrija los errores e intente nuevamente.") %>
        <div>
            <div class="fieldsetInterno">
            <fieldset>
                <legend>Información de cuenta</legend>
                
                <div class="editor-field">
                    <label>Tipo de usuario</label>
                    <%: Html.DropDownListFor(m => m.tipoUsuario, (List<SelectListItem>)ViewBag.listaTiposUsuarios, new { @onchange = "cambioTipoUsuario(this)" })%>
                    <%: Html.ValidationMessageFor(m => m.tipoUsuario)%>
                </div>


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
                    <%: Html.DropDownListFor(m => m.rut, (List<SelectListItem>)ViewBag.listaRuts, new { @disabled = "disabled", @onchange = "cambioRutUsuario()" })%>
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

                
                <p>
                    <input type="submit" value="Registrar usuario" />
                </p>
            </fieldset>
            </div>
        </div>

    <% } %>
</asp:Content>
