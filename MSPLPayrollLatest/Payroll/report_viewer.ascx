<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="report_viewer.ascx.cs" Inherits="Payroll.report_viewer" Debug="true" %>
<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a"
    Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">

<head>
    <title></title>
</head>
<body>

<div style="background-color:White;width:95%;margin: auto;" >
  <rsweb:ReportViewer ID="ReportViewer1" ShowBackButton="true"  ShowPrintButton="true" ShowRefreshButton="false" BorderWidth="2px"
        runat="server" Width="100%" Height="140%">
    </rsweb:ReportViewer>
    </div>
</body>
</html>