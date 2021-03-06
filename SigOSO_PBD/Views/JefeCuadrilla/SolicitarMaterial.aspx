﻿<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/JefeCuadrilla.Master" Inherits="System.Web.Mvc.ViewPage<SigOSO_PBD.Models.solicitudMaterialModels>" %>

<asp:Content ID="Content4" ContentPlaceHolderID="TitleContent" runat="server">
    <script type="text/javascript" src="../../Scripts/solicitudMaterial.js"></script>
    Solicitud de material
</asp:Content>

<asp:Content ID="Content5" ContentPlaceHolderID="MainContent" runat="server">
<form id="form1" method="post">
    <div class="fieldsetInterno">
    <fieldset>
        <legend>Información orden de trabajo interno asignado</legend>
            <%if (ViewBag.NumeroOrden != null)
                {
                    Response.Write(ViewBag.NumeroOrden);                       
            }                                                     
            %>
            <div class="fieldsetInterno">
            <fieldset>
                <legend>Materiales asignados</legend>


                    <table>
                        <tr>
                            <td><b>
                            Nombre material</b></td> <!-- incluye la unidad -->
                            <td><b>
                            Cantidad asignada</b></td>
                            <td><b>
                            Cantidad disponible</b></td>
                        </tr>

                        <% if (ViewBag.MaterialesAsignados != null)
                        {
                            List<SigOSO_PBD.Models.materialSolicitado> lst = (List<SigOSO_PBD.Models.materialSolicitado>)ViewBag.MaterialesAsignados;
                            foreach(SigOSO_PBD.Models.materialSolicitado temp in lst) {
                                Response.Write("<tr>");
                                
                                
                                Response.Write("<td>");
                                Response.Write(temp.nombreMaterial);
                                Response.Write("</td>");

                                Response.Write("<td>");
                                Response.Write(temp.cantidadAsignada + " " + temp.abrevUnidad);
                                Response.Write("</td>");

                                Response.Write("<td>");
                                Response.Write(temp.cantidadDisponible + " " + temp.abrevUnidad);
                                Response.Write("</td>");

                                
                                Response.Write("</tr>");
                            }                    
                        }                                                     
                        %>
                    </table>
            </fieldset>
            </div>
    </fieldset>
    </div>

    <div class="fieldsetInterno">
    <fieldset>
        <legend>Solicitud de materiales</legend>
        <label>Nombre material</label>
        <%if (ViewBag.SolicitudDeMateriales != null)
          {
              Response.Write(ViewBag.SolicitudDeMateriales);                       
        }                                                     
        %>
        <%: Html.ValidationMessageFor(servicio => servicio.tipo)%>
        <div>Cantidad</div>
        <%: Html.TextBoxFor(servicio => servicio.cantidad, new { @class = "text" })%>
        <%: Html.ValidationMessageFor(servicio => servicio.cantidad)%>
        <%: Html.TextBoxFor(servicio => servicio.id, new { @id = "id_servicio_a_agregar_oculta", @style = "display:none;" })%>

        <%: Html.TextBoxFor(servicio => servicio.tipo, new { @id = "tipo_oculta", @style = "display:none;" })%>
        
        <%: Html.TextBoxFor(servicio => servicio.unidad, new { @id = "unidad_oculta", @style = "display:none;" })%>

        <div id="unidad"></div>
        <fieldset>
        <legend>Detalle dematerial</legend>
            <%: Html.TextBoxFor(servicio => servicio.detalle, new { @class = "text" })%>            
        </fieldset>
        <input id="Submit1" type="submit" name="btn_agregar_servicio" value="agregar"/>
        <fieldset>
        <legend>Materiales solicitados</legend>
            <% string tabla = "";
               tabla +="<table align='center' style='width: 100%'>";
               tabla +="<tr>";
               tabla +="<td><b>Nombre Material</b></td>";
               tabla +="<td><b>cantidad</b></td>"; 
               tabla +="<td><b>unidad</b></td>"; 
               tabla +="<td><b>eliminar</b></td>"; 
               tabla +="</tr>";
               if (ViewBag.listaAgregadosMaterialSolicitado!= null)
               {
                   List<SigOSO_PBD.Models.solicitudMaterialModels> resultados = (List<SigOSO_PBD.Models.solicitudMaterialModels>)ViewBag.listaAgregadosMaterialSolicitado;
                   for (int i = 0; i < resultados.Count; i++)
                   {
                       tabla += "<tr>";
                       tabla += "<td><b>" + resultados[i].tipo + "</b></td>";
                       tabla += "<td><b>" + resultados[i].cantidad + "</b></td>";
                       tabla += "<td><b>" + resultados[i].unidad + "</b></td>";
                       tabla += "<td><b>" + "<input id='btn_eliminar' type='submit' value='eliminar' name='eliminar_" + resultados[i].id + "'>" + "</b></td>";                                                     
                       tabla += "</tr>";
                   }
               }               
               tabla +="</table>";
               Response.Write(tabla);                                                                                             
            %>

        </fieldset>
        <input type="submit" name="btn_solicitar" value="guardar"/>
    </fieldset>
    </div>

    
</form>
</asp:Content>

