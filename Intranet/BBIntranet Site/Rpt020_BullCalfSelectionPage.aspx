<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="Rpt020_BullCalfSelectionPage.aspx.cs" Inherits="Rpt020_BullCalfSelectionPage" Title="Breeder Bull Calf Selection" %>
<%@ Register Src="UserControls/Report020.ascx" TagName="Report020" TagPrefix="uc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <asp:ScriptManager runat="server" /> 
    <div style="width: 933px; height: 657px">
        <uc1:Report020 runat="server" />
    </div>
</asp:Content>

