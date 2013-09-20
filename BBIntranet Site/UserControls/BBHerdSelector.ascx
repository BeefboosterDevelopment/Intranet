<%@ Control Language="C#" AutoEventWireup="true" CodeFile="BBHerdSelector.ascx.cs" Inherits="BBHerdSelector" %>
    <nobr><asp:Label id="lblHerd" Text="Herd" width="50px" runat="server"></asp:Label> 
<asp:DropDownList id="ddlHerd" runat="server" OnDataBound="ddlHerd_DataBound" DataValueField="SN" AutoPostBack="True" Height="19px" Width="213px" DataTextField="Description" DataSourceID="odsHerdlist"></asp:DropDownList></nobr><br />
<asp:ObjectDataSource ID="odsHerdlist" runat="server" OldValuesParameterFormatString="original_{0}" SelectMethod="GetHerds" TypeName="Beefbooster.Web.BBDataHelper">
    <SelectParameters>
        <asp:Parameter Name="inStrain" Type="String" />
    </SelectParameters>
</asp:ObjectDataSource>
