<%@ Page Language="C#" MasterPageFile="~/Views/Shared/Administrador.Master" Inherits="System.Web.Mvc.ViewPage" %>
<asp:Content ID="Content4" ContentPlaceHolderID="TitleContent" runat="server">
    <MARQUEE  id="indices_economicos" onmouseover="this.stop();" onmouseout="this.start();">
        
    </MARQUEE >       
    BIENVENIDO

    

<script src="http://www.bci.cl/common/include/valores.js"></script>
        <script type="text/javascript">
            if (typeof (arrValores) != "undefined") {
                var escritura = "";
                function formatear_numero2(numero) {
                    var nroFormateado = '';
                    var indice = 0;
                    var band = false;
                    var numero2 = new String(numero);
                    for (i = 0; i <= numero2.length && indice <= 2; i++) {
                        if (numero2.charAt(i) == "." || numero2.charAt(i) == ",") {
                            nroFormateado = formato_miles(nroFormateado) + ",";
                            band = true;
                        } else {
                            nroFormateado = nroFormateado + numero2.charAt(i);
                        }
                        if (band)
                            indice++;
                    }
                    return nroFormateado;
                }
                escritura += 'UF';
                if (typeof (arrValores) != "undefined") {
                    if (arrValores[4].valor1 > 0) {
                        escritura += '<img src="../../Content/imagenes/indicadores-arrow-green.png" />';
                    }
                    if (arrValores[4].valor1 < 0) {
                        escritura += '<img src="../../Content/imagenes/indicadores-arrow-red.png" />';
                    }
                    if (arrValores[4].valor1 == 0) {
                        escritura += '<img src="../../Content/imagenes/indicadores-arrow-yellow.png" />';
                    }
                } else {
                    escritura += '<img src="../../Content/imagenes/indicadores-arrow-yellow.png" />';
                }
                if (typeof (arrValores) != "undefined")
                    if (typeof (arrValores[4]) == "object")
                        escritura += formatear_numero(arrValores[4].valor2) + " &nbsp&nbsp&nbsp ";
                escritura += "UTM";
                if (typeof (arrValores) != "undefined") {
                    if (arrValores[5].valor1 > 0) {
                        escritura += '<img src="../../Content/imagenes/indicadores-arrow-green.png" />';
                    }
                    if (arrValores[5].valor1 < 0) {
                        escritura += '<img src="../../Content/imagenes/indicadores-arrow-red.png" />';
                    }
                    if (arrValores[5].valor1 == 0) {
                        escritura += '<img src="../../Content/imagenes/indicadores-arrow-yellow.png" />';
                    }
                } else {
                    escritura += '<img src="../../Content/imagenes/indicadores-arrow-yellow.png" />';
                }
                if (typeof (arrValores) != "undefined")
                    if (typeof (arrValores[5]) == "object")
                        escritura += formatear_numero(arrValores[5].valor2) + " &nbsp&nbsp&nbsp ";
                escritura += "<img src='../../Content/imagenes/chile.png' /> IPSA ";
                if (typeof (arrValores) != "undefined") {
                    if (arrValores[19].valor1 > 0) {
                        escritura += '<img src="../../Content/imagenes/indicadores-arrow-green.png" />';
                    }
                    if (arrValores[19].valor1 < 0) {
                        escritura += '<img src="../../Content/imagenes/indicadores-arrow-red.png" />';
                    }
                    if (arrValores[19].valor1 == 0) {
                        escritura += '<img src="../../Content/imagenes/indicadores-arrow-yellow.png" />';
                    }
                } else {
                    escritura += '<img src="../../Content/imagenes/indicadores-arrow-yellow.png" />';
                }
                if (typeof (arrValores) != "undefined")
                    if (typeof (arrValores[19]) == "object")
                        escritura += formatear_numero(arrValores[19].valor2) + " &nbsp&nbsp&nbsp ";
                escritura += "<img src='../../Content/imagenes/usa.png' /> D&oacute;lar Observado";
                if (typeof (arrValores) != "undefined") {
                    if (arrValores[55].valor1 > 0) {
                        escritura += '<img src="../../Content/imagenes/indicadores-arrow-green.png" />';
                    }
                    if (arrValores[55].valor1 < 0) {
                        escritura += '<img src="../../Content/imagenes/indicadores-arrow-red.png" />';
                    }
                    if (arrValores[55].valor1 == 0) {
                        escritura += '<img src="../../Content/imagenes/indicadores-arrow-yellow.png" />';
                    }
                } else {
                    escritura += '<img src="../../Content/imagenes/indicadores-arrow-yellow.png" />';
                }
                if (typeof (arrValores) != "undefined")
                    if (typeof (arrValores[55]) == "object")
                        escritura += formatear_numero(arrValores[55].valor2) + " &nbsp&nbsp&nbsp ";
                escritura += "<img src='../../Content/imagenes/euro.png' /> Euro";
                if (typeof (arrValores) != "undefined") {
                    if (arrValores[8].valor1 > 0) {
                        escritura += '<img src="../../Content/imagenes/indicadores-arrow-green.png" />';
                    }
                    if (arrValores[8].valor1 < 0) {
                        escritura += '<img src="../../Content/imagenes/indicadores-arrow-red.png" />';
                    }
                    if (arrValores[8].valor1 == 0) {
                        escritura += '<img src="../../Content/imagenes/indicadores-arrow-yellow.png" />';
                    }
                } else {
                    escritura += '<img src="../../Content/imagenes/indicadores-arrow-yellow.png" />';
                }
                if (typeof (arrValores) != "undefined") {
                    if ((typeof (arrValores[8]) == "object") && (typeof (arrValores[55]) == "object")) {

                        var xRealValor = parseFloat(arrValores[55].valor2) / (parseFloat(1) / parseFloat(arrValores[8].venda));
                        escritura += formatear_numero2(xRealValor) + " &nbsp&nbsp&nbsp ";
                    }
                }
                document.getElementById('indices_economicos').innerHTML = escritura;
            } else {
            document.getElementById('indices_economicos').innerHTML = "Error al cargar los indicadores monetarios";
            }
		</script>
</asp:Content>

<asp:Content ID="Content5" ContentPlaceHolderID="MainContent" runat="server">
    <div>
        Formulario de administrador<br />
        <img alt="Logo de SigOSO" src="/Content/imagenes/logo.png" />
    </div> 
<!--    <script src="http://www.bci.cl/common/include/valores.js"></script>
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
    -->





</asp:Content>


