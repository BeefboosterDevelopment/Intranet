<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="Rpt404_Sale_SireSelection.aspx.cs" Inherits="Rpt404_Sale_SireSelection" Title="Sale Day - Client Sire Selection Report" %>
<%@ Register Src="UserControls/Report404.ascx" TagName="Report404" TagPrefix="uc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <uc1:Report404  runat="server" />
    <asp:ScriptManager runat="server" /> 
</asp:Content>