Public Class PayPalCreateOrder

    Public intent As String
    Public purchase_units As purchase_unit()
    Public application_context As application_ctx

    Public Class purchase_unit

        Public amount As amount
        Public custom_id As String
        Public description As String
        Public invoice_id As String

    End Class

    Public Class amount

        Public currency_code As String
        Public value As String

    End Class

    Public Class application_ctx

        Public shipping_preference As String

    End Class

End Class
