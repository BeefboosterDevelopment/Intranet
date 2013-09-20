<%@ Control Language="C#" AutoEventWireup="true" CodeFile="BBBreederSelector.ascx.cs" Inherits="UserControls.BBBreederSelector" %>

<asp:Label Text="Breeder:" width="80px" runat="server"></asp:Label> 

<asp:DropDownList id="ddlBreeder" runat="server" OnDataBound="ddlBreeder_DataBound" DataValueField="AccountNo" AutoPostBack="True" Height="19px" Width="250px" DataTextField="Description" DataSourceID="odsBreederlist"></asp:DropDownList><br />

<asp:ObjectDataSource ID="odsBreederlist" runat="server" OldValuesParameterFormatString="original_{0}" SelectMethod="GetBreeders" TypeName="Beefbooster.Web.BBDataHelper">
</asp:ObjectDataSource>
