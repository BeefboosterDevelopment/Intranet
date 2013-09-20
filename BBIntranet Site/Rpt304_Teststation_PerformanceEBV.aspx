<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="Rpt304_Teststation_PerformanceEBV.aspx.cs" Inherits="Rpt304_Teststation_PerformanceEBV" Title="Teststation - Performance Report 2008 EBV Version" %>
<%@ Register Src="UserControls/Report304.ascx" TagName="Report304" TagPrefix="uc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <uc1:Report304 runat="server" />
    <asp:ScriptManager runat="server" /> 
</asp:Content>

