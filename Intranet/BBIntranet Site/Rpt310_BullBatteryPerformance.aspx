<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="Rpt310_BullBatteryPerformance.aspx.cs" Inherits="Rpt310_BullBatteryPerformance" Title="Breeder Bull Battery Performance Report" %>
<%@ Register Src="UserControls/Report310.ascx" TagName="Report310" TagPrefix="uc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <uc1:Report310 runat="server" />
    <asp:ScriptManager runat="server" /> 
</asp:Content>

