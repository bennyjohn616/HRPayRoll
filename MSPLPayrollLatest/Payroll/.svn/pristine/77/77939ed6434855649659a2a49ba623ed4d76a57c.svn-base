<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="Payroll.Default" %>

<%@ Register Src="~/report_viewer.ascx" TagName="reportviewer" TagPrefix="uc1" %>
<html>
<head>
    <script type="text/javascript">
        function printDiv(divID) {

            //Get the HTML of div
            var divElements = document.getElementById(divID).innerHTML;
            //Get the HTML of whole page
            var oldPage = document.body.innerHTML;

            //Reset the page's HTML with div's HTML only
            document.body.innerHTML =
              "<html><head><title></title></head><body>" +
              divElements + "</body>";

            //Print Page
            window.print();

            //Restore orignal HTML
            document.body.innerHTML = oldPage;


        }
    </script>
</head>
<body>
    <div id="content" class="span10">
        <form runat="server">
        <asp:ScriptManager ID="ScriptManager1" runat="server">
        </asp:ScriptManager>
        <asp:UpdatePanel runat="server" ID="pnl_report">
            <ContentTemplate>
                <div style="width:1050px;">
                    <uc1:reportviewer ReportTitle="Default" ReportName="Default Name" runat="server"
                        ID="rptDemo" />
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
        </form>
    </div>
</body>
