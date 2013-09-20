<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" 
       CodeFile="Rpt405_Sale_SireCertificate.aspx.cs" Inherits="Rpt405_Sale_SireCertificate" Title="Sire Certificates" %>
<%@ Register Src="UserControls/Report405.ascx" TagName="Report405" TagPrefix="uc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <uc1:Report405  runat="server" />
    <asp:ScriptManager runat="server" /> 
</asp:Content>