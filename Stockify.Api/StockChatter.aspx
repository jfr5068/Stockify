<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="StockChatter.aspx.cs" Inherits="Stockify.Api.StockChatter" %>
    <div id="page-wrapper">
        <div class="row">
            <div class="col-lg-12">
                <h1 class="page-header">Stock Chatter</h1>
            </div>
        </div>
        <div class="panel-body">
            <div class="panel-body" id="panel" style="display: block;">
                <table class="table table-hover table-striped" id="table" style="width: 100%">
                    <thead>
                        <tr>
                            <th>Company</th>
                            <th>Rank</th>
                        </tr>
                    </thead>
                    <tbody style="cursor: pointer;">
                    </tbody>
                </table>
            </div>
        </div>
    </div>
    <script type="text/javascript" src="/scripts/jquery-1.10.2.js"></script>
    <script type="text/javascript" src="/scripts/jquery-1.10.2.min.js"></script>
    <script type="text/javascript" src="/scripts/bootstrap.js"></script>
    <script type="text/javascript" src="/scripts/bootstrap.min.js"></script>
    <script type="text/javascript" src="https://cdn.datatables.net/1.10.11/js/jquery.dataTables.min.js"></script>
    <script type="text/javascript" src="https://cdn.datatables.net/1.10.11/js/dataTables.bootstrap.min.js"></script>
    <link rel="stylesheet" href="/content/bootstrap.css" />
    <link rel="stylesheet" href="/content/bootstrap.min.css" />
    <script>

        
        $body = $("body");
        var table = $('#table').DataTable();
        getData();

        $(document).on({
            ajaxStart: function () { $body.addClass("loading"); },
            ajaxStop: function () { $body.removeClass("loading"); }
        });

        document.body.style.overflow = 'scroll';
        var config;
        config = {
            mode: "text/html",
            htmlMode: true,
            lineNumbers: true,
            autofocus: true,
            readOnly: true
        };

        function getData() {
            $.ajax({
                type: "GET",
                url: "/api/Stock",
                contentType: "application/json; charset=utf-8",
                dataType: 'json',
                error: function (XMLHttpRequest, textStatus, errorThrown) {
                    alert("An error occured.");
                },
                success: function (result) {
                    var stocks = result;
                    for (var i = 0; i < stocks.length; i++) {
                        var newRow = table.row.add([
                            stocks[i].name,
                            stocks[i].rank
                        ]);
                    }
                    table.draw();
                }
            });
        }
    </script>
</asp:Content>
