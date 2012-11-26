<%@ Page Language="C#" MasterPageFile="~/Views/Shared/Administrador.Master" Inherits="System.Web.Mvc.ViewPage" %>
<asp:Content ID="Content4" ContentPlaceHolderID="TitleContent" runat="server">
    <MARQUEE  id="indices_economicos"></MARQUEE >       
    BIENVENIDO
</asp:Content>

<asp:Content ID="Content5" ContentPlaceHolderID="MainContent" runat="server">
    <div>
        Formulario de administrador<br />
        <img alt="Logo de SigOSO" src="/Content/imagenes/logo.png" />
    </div> 

     

    <script src="http://www.bci.cl/common/include/valores.js"></script>
    <script type="text/javascript">
        //abreviación para document.getElementById
        function g(id) {
            return document.getElementById(id);
        }
        //función que obtiene el valor deseado
        //formatear_numero es una función definida en el archivo del bci
        function valor(indice) {
            return formatear_numero(arrValores[indice].valor2);
        }
        //función que se ejecuta al cargar el DOM, esto no asegura que los objetos existirán al momento de establecer los valores
        window.onload = function () {
            //En variables con el nombre de cada indicador establecemos el indice del arreglo
            //esto lo hacemos revisando el archivo del bci e identificando los indicadores que nos interesan.
            var uf = 4, usd = 6, utm = 5, euro = 8, yen = 9;
            if (typeof (arrValores) != "undefined") {
                g('indices_economicos').innerHTML = "";
                g('indices_economicos').innerHTML += "valor uf: " + valor(uf) + "            ";
                g('indices_economicos').innerHTML += "valor utm: " + valor(utm) + "            ";
                g('indices_economicos').innerHTML += "valor dolar: " + valor(usd) + "            ";
                g('indices_economicos').innerHTML += "valor euro: " + (valor(euro).toString().replace(",", ".") * valor(usd).toString().replace(",", ".")).toFixed(2).toString().replace(".", ",") + "    ";
            }
        }
        
    
    
    </script>






</asp:Content>


