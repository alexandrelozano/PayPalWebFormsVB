Public Class Address
    Public Property country_code As String
End Class

Public Class Amount
    Public Property currency_code As String
    Public Property value As String
End Class

Public Class Capture
    Public Property id As String
    Public Property status As String
    Public Property amount As Amount
    Public Property final_capture As Boolean
    Public Property seller_protection As SellerProtection
    Public Property seller_receivable_breakdown As SellerReceivableBreakdown
    Public Property custom_id As String
    Public Property links As List(Of Link)
    Public Property create_time As DateTime
    Public Property update_time As DateTime
End Class

Public Class GrossAmount
    Public Property currency_code As String
    Public Property value As String
End Class

Public Class Link
    Public Property href As String
    Public Property rel As String
    Public Property method As String
End Class

Public Class Name
    Public Property given_name As String
    Public Property surname As String
End Class

Public Class NetAmount
    Public Property currency_code As String
    Public Property value As String
End Class

Public Class Payer
    Public Property name As Name
    Public Property email_address As String
    Public Property payer_id As String
    Public Property address As Address
End Class

Public Class Payments
    Public Property captures As List(Of Capture)
End Class

Public Class PaymentSource
    Public Property paypal As Paypal
End Class

Public Class Paypal
    Public Property email_address As String
    Public Property account_id As String
    Public Property name As Name
    Public Property address As Address
End Class

Public Class PaypalFee
    Public Property currency_code As String
    Public Property value As String
End Class

Public Class PurchaseUnit
    Public Property reference_id As String
    Public Property payments As Payments
End Class

Public Class PayPalOrder
    Public Property id As String
    Public Property status As String
    Public Property payment_source As PaymentSource
    Public Property purchase_units As List(Of PurchaseUnit)
    Public Property payer As Payer
    Public Property links As List(Of Link)
End Class

Public Class SellerProtection
    Public Property status As String
End Class

Public Class SellerReceivableBreakdown
    Public Property gross_amount As GrossAmount
    Public Property paypal_fee As PaypalFee
    Public Property net_amount As NetAmount
End Class

