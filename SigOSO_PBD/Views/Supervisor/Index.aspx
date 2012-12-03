
<%@ Page Language="C#" MasterPageFile="~/Views/Shared/Supervisor.Master" Inherits="System.Web.Mvc.ViewPage" %>


<asp:Content ID="Content4" ContentPlaceHolderID="TitleContent" runat="server">
    <MARQUEE  id="indices_economicos" onmouseover="this.stop();" onmouseout="this.start();">
        
    </MARQUEE >       
    
    Bienvenido usuario<% Response.Write(" '" + User.Identity.Name + "'"); %>
    <HR><br>
    
    
    <!--#include virtual="../../Servicios/indicesEconomicos.html"-->


</asp:Content>

<asp:Content ID="Content5" ContentPlaceHolderID="MainContent" runat="server">
        <div class="sitio_en_construccion"></div> 


</asp:Content>

