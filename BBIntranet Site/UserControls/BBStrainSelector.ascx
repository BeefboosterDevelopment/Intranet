<%@ Control Language="C#" AutoEventWireup="true" CodeFile="BBStrainSelector.ascx.cs" Inherits="UserControls_BBStrainSelector" %>
<div>
    <asp:Label ID="lblStrain" runat="server" Width="50px">Strain</asp:Label>
    <asp:DropDownList ID="ddlStrain" runat="server" DataSourceID="odsStrainList" DataTextField="StrainCode" 
        DataValueField="StrainCode" AutoPostBack="True">
    </asp:DropDownList>
    <asp:ObjectDataSource ID="odsStrainList" runat="server" SelectMethod="GetStrains"
        TypeName="Beefbooster.Web.BBDataHelper" OldValuesParameterFormatString="original_{0}">
    </asp:ObjectDataSource>
</div>