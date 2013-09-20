<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="Rpt024_HeiferSelectionPage.aspx.cs" Inherits="Rpt024_HeiferSelectionPage" Title="Breeder Heifer Calf Selection" %>
<%@ Register Src="UserControls/Report024.ascx" TagName="Report024" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <uc1:Report024  runat="server" />
    <asp:ScriptManager runat="server" /> 
</asp:Content>

