<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="StockChatter.aspx.cs" Inherits="Stockify.Api.StockChatter" %>

<div id="page-wrapper">
    <h1 style="color:gray; text-align: center;">Stockify!</h1>
    <hr class="brace">
    <table class="table-fill" id="table">
        <thead>
            <tr>
                <th class="text-left">Rank</th>
                <th class="text-left">Ticker</th>
                <th class="text-left">Company</th>
            </tr>
        </thead>
        <tbody class="table-hover" id="tableBody">
        </tbody>
    </table>
</div>
<script type="text/javascript" src="/scripts/jquery-1.10.2.js"></script>
<script type="text/javascript" src="/scripts/jquery-1.10.2.min.js"></script>
<script type="text/javascript" src="/scripts/bootstrap.js"></script>
<script type="text/javascript" src="/scripts/bootstrap.min.js"></script>
<script type="text/javascript" src="https://cdn.datatables.net/1.10.11/js/jquery.dataTables.min.js"></script>
<script type="text/javascript" src="https://cdn.datatables.net/1.10.11/js/dataTables.bootstrap.min.js"></script>
<link rel="stylesheet" href="/content/bootstrap.css" />
<link rel="stylesheet" href="/content/bootstrap.min.css" />
<link rel="stylesheet" href="/content/stockify.css" />
<link rel="stylesheet" href="/content/hstockify.css" />
<script>


    $body = $("body");
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
                var stocks = result.reverse();
                var table = document.getElementById("table");
                for (var i = 0; i < stocks.length; i++) {
                    var row = table.insertRow(1);
                    var cell = row.insertCell(0);
                    cell.innerHTML = stocks[i].rank;
                    var cell2 = row.insertCell(1);
                    cell2.innerHTML = stocks[i].ticker.toUpperCase();
                    var cell3 = row.insertCell(2);
                    cell3.innerHTML = stocks[i].name;
                }
            }
        });
    }
</script>
</asp:Content>
