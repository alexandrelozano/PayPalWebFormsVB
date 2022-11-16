Public Class paypalcheckout
    Inherits System.Web.UI.Page

    Protected Property CustomId As String

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        ' TODO: retrieve from request or session the CustomId
        CustomId = "1234567"

    End Sub

    Protected Function PayPal_locale() As String

        Return Web.Configuration.WebConfigurationManager.AppSettings("PayPal_locale")

    End Function

    Protected Function PayPal_currency() As String

        Return Web.Configuration.WebConfigurationManager.AppSettings("PayPal_currency")

    End Function

    Protected Function PayPal_ClientId() As String

        Return Utils.GetPayPalClientId()

    End Function

    Protected Function OrderAmount() As String

        Return Format(Utils.OrderAmount(CustomId), "0.00") & " " & Web.Configuration.WebConfigurationManager.AppSettings("PayPal_currency")

    End Function

    Protected Function OrderDescription() As String

        Return Utils.OrderDescription(CustomId)

    End Function

    Protected Function OrderState() As String

        Dim message As String

        If Utils.PaymentCompleted(CustomId) Then
            message = "Payment completed"
        Else
            message = "Pending payment"
        End If

        Return message

    End Function

    Protected Function PayPalButtonHidden() As String

        Dim ret = ""

        If Utils.PaymentCompleted(CustomId) Then
            ret = "hidden"
        End If

        Return ret

    End Function

End Class