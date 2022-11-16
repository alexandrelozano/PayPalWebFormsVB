Imports System.ComponentModel
Imports System.Web.Services
Imports System.Web.Services.Protocols
Imports Newtonsoft.Json

' Para permitir que se llame a este servicio web desde un script, usando ASP.NET AJAX, quite la marca de comentario de la línea siguiente.
<System.Web.Script.Services.ScriptService()>
<System.Web.Services.WebService(Namespace:="http://tempuri.org/")> _
<System.Web.Services.WebServiceBinding(ConformsTo:=WsiProfiles.BasicProfile1_1)> _
<ToolboxItem(False)> _
Public Class ws
    Inherits System.Web.Services.WebService

    <WebMethod()>
    Public Function CreateOrder(CustomId As String) As String

        Dim http = Utils.GetPaypalHttpClient()
        Dim accessToken = Utils.GetPayPalAccessTokenAsync(http)
        Dim order = ""
        Dim currency = Web.Configuration.WebConfigurationManager.AppSettings("PayPal_currency")

        order = Utils.PayPalCreateOrder(http, accessToken, CustomId, currency, Utils.OrderAmount(CustomId), Utils.OrderDescription(CustomId))

        Return order

    End Function

    <WebMethod()>
    Public Function ApproveOrder(CustomId As String, OrderId As String) As String

        Dim ret = ""

        Try
            Dim http = Utils.GetPaypalHttpClient()
            Dim accessToken = Utils.GetPayPalAccessTokenAsync(http)

            Dim jsonorder = Utils.PayPalCapturePayment(http, accessToken, OrderId)

            Dim order = JsonConvert.DeserializeObject(Of PayPalOrder)(jsonorder)

            ' TODO: With CustomId and order check integrity and register in you system
            ret = "OK"

        Catch ex As Exception
            Debug.Print(ex.Message & vbCrLf & ex.StackTrace)
        End Try

        Return ret

    End Function

End Class