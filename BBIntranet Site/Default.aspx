<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="Default.aspx.cs" Inherits="_Default" 
Title="Beefbooster Intranet Site - Home Page" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <asp:Menu ID="mnuMain" runat="server" Orientation="Horizontal" 
    Width="608px" BackColor="#F7F6F3" DynamicHorizontalOffset="2" 
    Font-Names="Verdana" Font-Size="0.8em" ForeColor="#7C6F57" 
    StaticSubMenuIndent="10px">
        <DynamicHoverStyle BackColor="#7C6F57" ForeColor="White" />
        <DynamicMenuItemStyle HorizontalPadding="5px" VerticalPadding="2px" />
        <DynamicMenuStyle BackColor="#F7F6F3" />
        <DynamicSelectedStyle BackColor="#5D7B9D" />
        <Items>
            <asp:MenuItem NavigateUrl="~/ASReml.aspx" Text="ASReml" Value="ASReml"></asp:MenuItem>
            <asp:MenuItem Text="Calving" Value="Calving">
                <asp:MenuItem NavigateUrl="~/Rpt011_Calving_Qualifying.aspx" Text="Bull Calf Qualifying" Value="Bull Calf Qualifying">
                </asp:MenuItem>
            </asp:MenuItem>
            <asp:MenuItem Text="Weaning" Value="Weaning">
                <asp:MenuItem NavigateUrl="~/Rpt022_CowProductionPage.aspx" Text="Cow Production" Value="Cow Production">
                </asp:MenuItem>
                <asp:MenuItem NavigateUrl="~/Rpt020_BullCalfSelectionPage.aspx" Text="Bull Calf Selection"
                    Value="Bull Calf Selection"></asp:MenuItem>
                <asp:MenuItem NavigateUrl="~/Rpt024_HeiferSelectionPage.aspx" 
                    Text="Heifer Selection" Value="Heifer Selection"></asp:MenuItem>
            </asp:MenuItem>
            <asp:MenuItem Text="Bulltest" Value="Bulltest">
                <asp:MenuItem NavigateUrl="~/Rpt301_Teststation_Worksheet.aspx" Text="Worksheet"
                    Value="Worksheet"></asp:MenuItem>
                <asp:MenuItem NavigateUrl="~/Rpt304_Teststation_PerformanceEBV.aspx" Text="Performance - EBV"
                    Value="Performance (EBV)"></asp:MenuItem>                
                <asp:MenuItem NavigateUrl="~/Rpt310_BullBatteryPerformance.aspx" Text="Breeder Bull Battery Performance"
                    Value="Breeder Bull Battery Performance"></asp:MenuItem>
                <asp:MenuItem NavigateUrl="~/Rpt404_Sale_SireSelection.aspx" Text="Sales - Sire Selection"
                    Value="Sales - Sire Selection"></asp:MenuItem>
                <asp:MenuItem NavigateUrl="~/Rpt405_Sale_SireCertificate.aspx" Text="Sales - Certificates"
                    Value="Sales - Certificates"></asp:MenuItem>
            </asp:MenuItem>
        </Items>
        <StaticHoverStyle BackColor="#7C6F57" ForeColor="White" />
        <StaticMenuItemStyle HorizontalPadding="5px" VerticalPadding="2px" />
        <StaticSelectedStyle BackColor="#5D7B9D" />
    </asp:Menu>
</asp:Content>

