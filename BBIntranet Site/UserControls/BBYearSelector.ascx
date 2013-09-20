<%@ Control Language="C#" AutoEventWireup="true" CodeFile="BBYearSelector.ascx.cs" Inherits="UserControls_BBYearSelector" %>
<div>
    <asp:Label ID="lblYear" runat="server" Width="50px">Year</asp:Label>
    <asp:DropDownList ID="ddlYear" runat="server" DataSourceID="odsYearList" DataTextField="YearNumberAndLetter" 
        DataValueField="YearNumber" OnDataBound="ddlYear_DataBound">
    </asp:DropDownList>
    <asp:ObjectDataSource ID="odsYearList" runat="server" SelectMethod="GetCalfYearLetters"
        TypeName="Beefbooster.Web.BBDataHelper" OldValuesParameterFormatString="original_{0}">
        <SelectParameters>
            <asp:Parameter DefaultValue="2010" Name="startingYear" Type="Int32" />
        </SelectParameters>
    </asp:ObjectDataSource>
</div>