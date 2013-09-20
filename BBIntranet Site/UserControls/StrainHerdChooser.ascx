<%@ Control Language="C#" AutoEventWireup="true" CodeFile="StrainHerdChooser.ascx.cs" Inherits="UserControls_StrainHerdChooser" %>
<asp:Panel runat="server" Direction="LeftToRight" Height="15px"
    HorizontalAlign="Left" Width="215px">
    <asp:Label  runat="server" Text="Strain"></asp:Label><asp:DropDownList
        ID="ddlStrain" runat="server" AutoPostBack="True" DataSourceID="odsStrain" DataTextField="StrainCode"
        DataValueField="StrainCode" OnSelectedIndexChanged="ddlStrain_SelectedIndexChanged"
        Width="49px">
    </asp:DropDownList>
    &nbsp;
    <asp:Label runat="server" Text="Herd"></asp:Label><asp:DropDownList
        ID="ddlHerd" runat="server" DataSourceID="odsHerd" DataTextField="Code" DataValueField="SN"
        Width="55px">
    </asp:DropDownList></asp:Panel>

<asp:ObjectDataSource ID="odsHerd" runat="server" OldValuesParameterFormatString="original_{0}" SelectMethod="GetHerds" TypeName="Beefbooster.Web.BBDataHelper">
    <SelectParameters>
        <asp:Parameter Name="inStrain" Type="String" />
    </SelectParameters>
</asp:ObjectDataSource>


<asp:ObjectDataSource ID="odsStrain" runat="server" SelectMethod="GetStrains"
    TypeName="Beefbooster.Web.BBDataHelper" OldValuesParameterFormatString="original_{0}">
</asp:ObjectDataSource>


