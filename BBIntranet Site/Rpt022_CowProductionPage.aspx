<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="Rpt022_CowProductionPage.aspx.cs" Inherits="Rpt022_CowProductionPage" Title="Cow Production Page" %>
<%@ Register Src="UserControls/Report022.ascx" TagName="Report022" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <div style="width: 940px; height: 578px">
        <uc1:Report022 runat="server" />
        <asp:ScriptManager runat="server" /> 
    </div>
</asp:Content>

