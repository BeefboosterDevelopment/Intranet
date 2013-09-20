<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="Rpt011_Calving_Qualifying.aspx.cs" Inherits="Rpt011_Calving_Qualifying" Title="Preliminary (Pre Weaning) Bull Calf Qualifying Report" %>
<%@ Register src="UserControls/Report011.ascx" tagname="Report011" tagprefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <uc1:Report011 runat="server" />
    <asp:ScriptManager runat="server" /> 
</asp:Content>

