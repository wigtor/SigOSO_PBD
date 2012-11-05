<%@ Page Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage" %>

<asp:Content ID="aboutTitle" ContentPlaceHolderID="TitleContent" runat="server">
    -Acerca de
</asp:Content>

<asp:Content ID="aboutContent" ContentPlaceHolderID="MainContent" runat="server">
    <h2>Acerca de</h2>
    <p>
        Este corresponde a la entrega final del trabajo de Proyecto de base de datos 2-2012. El proyecto ha sido llamado SigOSO
    </p>
    <p>
        Haciendo una prueba de un select: "SELECT * FROM unidad_material" y se ha obtenido el primer valor resultante: <%: ViewBag.primerValor %>
    </p>
</asp:Content>
