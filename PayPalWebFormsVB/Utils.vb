Imports System.Net.Http
Imports System.Net.Http.Headers
Imports Newtonsoft.Json

Public Class Utils

    Public Shared Function GetPayPalClientId() As String

        Return Web.Configuration.WebConfigurationManager.AppSettings("PayPal_ClientId")

    End Function

    Public Shared Function GetPayPalSecret() As String

        Return Web.Configuration.WebConfigurationManager.AppSettings("PayPal_Secret")

    End Function

    Public Shared Function PaymentCompleted(CustomId As String) As Boolean

        ' TODO: check in your system if this order is already paid
        Return False

    End Function

    Public Shared Function OrderAmount(CustomId As String) As Double

        ' TODO: retrieve fom your system the amount
        Return 10.5

    End Function

    Public Shared Function OrderDescription(CustomId As String) As String

        ' TODO: retrieve fom your system the order description
        Return "A sample product description"

    End Function

    Public Shared Function GetPaypalHttpClient() As HttpClient

        Dim API_URL = Web.Configuration.WebConfigurationManager.AppSettings("PayPal_API_URL")
        Dim http = New HttpClient()

        http.BaseAddress = New Uri(API_URL)
        http.Timeout = TimeSpan.FromSeconds(30)

        Return http

    End Function

    Public Shared Function GetPayPalAccessTokenAsync(http As HttpClient) As PaypalAccessToken

        Dim accessToken As PaypalAccessToken = Nothing

        Try
            Dim clientId = GetPayPalClientId()
            Dim secret = GetPayPalSecret()

            Dim str = $"{clientId}:{secret}"
            Dim bytes() As Byte = Encoding.GetEncoding("iso-8859-1").GetBytes(str)
            Dim request = New HttpRequestMessage(HttpMethod.Post, "/v1/oauth2/token")
            request.Headers.Authorization = New AuthenticationHeaderValue("Basic", Convert.ToBase64String(bytes))

            Dim form = New Dictionary(Of String, String)
            form.Add("grant_type", "client_credentials")

            request.Content = New FormUrlEncodedContent(form)

            Dim response = http.SendAsync(request)

            Dim content = response.Result.Content.ReadAsStringAsync()
            accessToken = JsonConvert.DeserializeObject(Of PaypalAccessToken)(content.Result)
        Catch ex As Exception
            Debug.Print(ex.Message)
        End Try

        Return accessToken

    End Function

    Public Shared Function PayPalCreateOrder(http As HttpClient, token As PaypalAccessToken, custom_id As String, currency_code As String, amount As Double, description As String) As String

        Dim ret As String = ""

        Try
            Dim clientId = GetPayPalClientId()
            Dim secret = GetPayPalSecret()

            Dim str = $"{clientId}:{secret}"
            Dim bytes() As Byte = Encoding.GetEncoding("iso-8859-1").GetBytes(str)
            Dim request = New HttpRequestMessage(HttpMethod.Post, "/v2/checkout/orders")
            request.Headers.Authorization = New AuthenticationHeaderValue("Bearer", token.access_token)

            Dim order = New PayPalCreateOrder
            order.intent = "CAPTURE"

            Dim appcontext = New PayPalCreateOrder.application_ctx
            appcontext.shipping_preference = "NO_SHIPPING"

            order.application_context = appcontext

            Dim purchaseunit = New PayPalCreateOrder.purchase_unit
            purchaseunit.custom_id = custom_id
            purchaseunit.description = description
            purchaseunit.amount = New PayPalCreateOrder.amount
            purchaseunit.amount.value = Replace(Format(amount, "0.00"), ",", ".")
            purchaseunit.amount.currency_code = currency_code

            order.purchase_units = {purchaseunit}

            Dim json = JsonConvert.SerializeObject(order)
            Dim strcontent = New StringContent(json, Encoding.UTF8, "application/json")
            request.Content = strcontent

            Dim response = http.SendAsync(request)

            Dim content = response.Result.Content.ReadAsStringAsync().Result

            ret = content

        Catch ex As Exception
            Debug.Print(ex.Message)
        End Try

        Return ret

    End Function

    Public Shared Function PayPalCapturePayment(http As HttpClient, token As PaypalAccessToken, orderId As String) As String

        Dim ret As String = ""

        Try
            Dim clientId = GetPayPalClientId()
            Dim secret = GetPayPalSecret()

            Dim str = $"{clientId}:{secret}"
            Dim bytes() As Byte = Encoding.GetEncoding("iso-8859-1").GetBytes(str)
            Dim request = New HttpRequestMessage(HttpMethod.Post, $"/v2/checkout/orders/{orderId}/capture")
            request.Headers.Authorization = New AuthenticationHeaderValue("Bearer", token.access_token)
            Dim strcontent = New StringContent("", Encoding.UTF8, "application/json")
            request.Content = strcontent

            Dim response = http.SendAsync(request)

            Dim content = response.Result.Content.ReadAsStringAsync().Result

            ret = content

        Catch ex As Exception
            Debug.Print(ex.Message)
        End Try

        Return ret

    End Function

End Class
