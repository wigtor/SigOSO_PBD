<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/JefeCuadrilla.Master" Inherits="System.Web.Mvc.ViewPage<SigOSO_PBD.Models.solicitudMaterialModels>" %>

<asp:Content ID="Content4" ContentPlaceHolderID="TitleContent" runat="server">
    <script type="text/javascript" src="../../Scripts/solicitudMaterial.js"></script>
    Solicitud de material
</asp:Content>

<asp:Content ID="Content5" ContentPlaceHolderID="MainContent" runat="server">
<form id="form1" method="post">
    <%
    if (ViewBag.cantidad_solicitada!=null)
    {
        string tabla = "";
        tabla += "<table align='center' style='width: 100%'>";
        tabla += "<tr>";
        tabla += "<td><b>fecha_solicitud_material</b></td>";
        tabla += "<td><b>nombre_material</b></td>";
        tabla += "<td><b>cantidad solicitada</b></td>";
        tabla += "<td><b>comentario solicitud</b></td>";
        tabla += "<td><b>fecha respuesta</b></td>";
        tabla += "<td><b>cantidad aprobada</b></td>";       
        tabla += "<td><b>comentario supervisor</b></td>";
        tabla += "</tr>";
        for (int i = 0; i < ((List<string>)ViewBag.cantidad_solicitada).Count; i++) {
            tabla += "<td><b>" + ((List<string>)ViewBag.fecha_solicitud_material)[i] + "</b></td>";
            tabla += "<td><b>" + ((List<string>)ViewBag.nombre_tipo_material)[i] + "</b></td>";
            tabla += "<td><b>" + ((List<string>)ViewBag.cantidad_solicitada)[i] + "</b></td>";
            tabla += "<td><b>" + ((List<string>)ViewBag.comentarios_jefe_cuadrilla)[i]+ "</b></td>";
            if (((List<string>)ViewBag.revisada)[i]=="False"){
                tabla += "<td><b>" + "-----" + "</b></td>";
                tabla += "<td><b>" + "-----" + "</b></td>";
                tabla += "<td><b>" + "-----" + "</b></td>";            
            }else{
                tabla += "<td><b>" + ((List<string>)ViewBag.fecha_respuesta)[i] + "</b></td>";
                tabla += "<td><b>" + ((List<string>)ViewBag.cantidad_aprobada_material)[i] + "</b></td>";
                tabla += "<td><b>" + ((List<string>)ViewBag.comentarios_supervisor)[i] + "</b></td>";
            }
            
            
            


            
            
            tabla += "</tr>";
        }
        tabla += "</table>";
        Response.Write(tabla);
    }





        
     %>

    
                                

</form>
</asp:Content>

