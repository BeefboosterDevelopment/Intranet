<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="Rpt302_Teststation_PerformancePage.aspx.cs" Inherits="Rpt302_Teststation_PerformancePage" Title="Teststation - Performance Report" %>

<%@ Register Src="UserControls/Report302.ascx" TagName="Report302" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    &nbsp;<uc1:Report302 ID="Report302_1" runat="server" />
</asp:Content>

