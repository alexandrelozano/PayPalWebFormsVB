<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="paypalcheckout.aspx.vb" Inherits="PayPalWebFormsVB.paypalcheckout" %>

<!DOCTYPE html>
<html>
<head>
    <title>Checkout page</title>
    <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.0.2/dist/css/bootstrap.min.css" rel="stylesheet" integrity="sha384-EVSTQN3/azprG1Anm3QDgpJLIm9Nao0Yz1ztcQTwFspd3yD65VohhpuuCOmLASjC" crossorigin="anonymous" />
    <meta name="viewport" content="width=device-width, initial-scale=1">
</head>
<body>
    <script src="https://cdn.jsdelivr.net/npm/bootstrap@5.0.2/dist/js/bootstrap.bundle.min.js" integrity="sha384-MrcW6ZMFYlzcLA8Nl+NtUVF0sA7MsXsP1UyJoMp4YLEuNSfAP+JcXn/tWtIaxVXM" crossorigin="anonymous"></script>

    <script src="https://code.jquery.com/jquery-3.6.1.min.js" integrity="sha256-o88AwQnZB+VDvE9tvIXrMQaPlFFSUTR+nldQm1LuPXQ=" crossorigin="anonymous"></script>
    <!-- Replace "test" with your own sandbox Business account app client ID -->
    <script src="https://www.paypal.com/sdk/js?client-id=<%=PayPal_ClientId() %>&currency=<%= PayPal_currency() %>&locale=<%= PayPal_locale() %>"></script>

    <form id="form1" runat="server">
        <div id="divPagina" class="container" role="main">

            <div class="row">
                <div class="col-sm-12">
                    &nbsp;
                </div>
            </div>

            <div class="row">
                <div class="col-sm-12">
                    <div class="page-header">
                        <h1>Checkout page</h1>
                        <h3>ACME Company</h3>
                    </div>
                </div>
            </div>

            <div class="card">
                <div class="card-header">
                    Payment details
                </div>
                <div class="card-body">
                    <div class="row">
                        <div class="col-sm-12">
                            <%= OrderDescription() %>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-sm-12">
                            Total: <%= OrderAmount() %>
                        </div>
                    </div>
                    <div class="row">
                        &nbsp;
                    </div>
                    <div class="row">
                        <div class="col-sm-12">
                            <%= OrderState() %>
                        </div>
                    </div>
                    <div class="row">
                        &nbsp;
                    </div>
                    <div class="row">
                        <!-- Set up a container element for the button -->
                        <div id="paypal-button-container" <%= PayPalButtonHidden() %>></div>
                    </div>
                </div>
            </div>

            <div class="row">
                <div class="col-sm-12">
                    &nbsp;
                </div>
            </div>

            <div class="row">
                <div class="col-sm-12">
                    <p class="text-muted">Contact info, cookies info, payment policy, etc</p>
                </div>
            </div>
        </div>


    </form>

    <script>
        paypal.Buttons({

            env: 'sandbox',

            style: {
                layout: 'vertical',
                size: 'responsive',
                shape: 'pill',
                color: 'blue',
                label: 'pay'
            },

            // Order is created on the server and the order id is returned
            createOrder: (data, actions) => {

                var p;
                var order = CreateOrder();
                order.done(function (data) {
                    var a = JSON.parse(data.d);
                    p = a.id;
                });

                return p;
            },

            // Finalize the transaction on the server after payer approval
            onApprove: (data, actions) => {

                var approve = ApproveOrder(data.orderID);
                approve.done(function (data) {
                    if (data.d == "OK") {
                        actions.redirect(window.location.href);
                    } else {
                        alert(data.d);
                    }
                });

            }
        }).render('#paypal-button-container');

        function CreateOrder() {
            return $.ajax({
                type: "POST",
                async: false,
                url: "ws.asmx/CreateOrder",
                data: "{ 'CustomId': '<%=CustomId%>' }",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                error:
                    function (XMLHttpRequest, textStatus, errorThrown) {
                        alert('Error at CreateOrder: ' + textStatus + errorThrown);
                    },
            });
        };

        function ApproveOrder(orderId) {
            return $.ajax({
                type: "POST",
                async: false,
                url: "ws.asmx/ApproveOrder",
                data: "{ 'CustomId': '<%=CustomId%>', 'OrderId': '" + orderId + "' }",
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    error:
                        function (XMLHttpRequest, textStatus, errorThrown) {
                            alert('Error at ApproveOrder: ' + textStatus + errorThrown);
                        },
                });
        };
    </script>
</body>
</html>

