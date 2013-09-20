<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="Rpt301_Teststation_Worksheet.aspx.cs" Inherits="Rpt301_Teststation_Worksheet" Title="Teststation - Worksheet" %>
<%@ Register Src="UserControls/Report301.ascx" TagName="Report301" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <uc1:Report301 runat="server" />
    <asp:ScriptManager runat="server" /> 
</asp:Content>

