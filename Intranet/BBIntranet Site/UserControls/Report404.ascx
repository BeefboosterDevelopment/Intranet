<%@ Control Language="C#" AutoEventWireup="true" CodeFile="Report404.ascx.cs" Inherits="Report404" %>
<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a"
    Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>
<table id="tblParameters" style="border-top-style: ridge; border-right-style: ridge;
    border-left-style: ridge; border-bottom-style: ridge">
    <tr>
        <td style="text-align: left; font-weight: bold; height: 21px;" >
            Sire Selection Sheets
        </td>
    </tr>
    <tr>
        <td style="text-align: right">
            <asp:Label ID="lblYear" runat="server" Style="font-weight: bold" Text="Year Born"></asp:Label>
        </td>
        <td>
            <asp:DropDownList ID="ddlYearBorn" runat="server" Width="90px">
                <asp:ListItem Selected="True" Value="-1">Current</asp:ListItem>
                <asp:ListItem>2014</asp:ListItem>
                <asp:ListItem>2013</asp:ListItem>
                <asp:ListItem>2012</asp:ListItem>
                <asp:ListItem>2011</asp:ListItem>
                <asp:ListItem>2010</asp:ListItem>
                <asp:ListItem>2009</asp:ListItem>
            </asp:DropDownList>
        </td>
        <td style="text-align: left">
            <asp:Label ID="lblStrain" runat="server" Font-Bold="True" Text="Strain"></asp:Label>
        </td>
        <td style="text-align: left">
            <asp:DropDownList ID="ddlStrain" runat="server">
                <asp:ListItem>M1</asp:ListItem>
                <asp:ListItem selected="true">M2</asp:ListItem>
                <asp:ListItem>M3</asp:ListItem>
                <asp:ListItem>M4</asp:ListItem>
                <asp:ListItem>TX</asp:ListItem>
            </asp:DropDownList>
        </td>
        <td />
        <td />
        
    </tr>
    <tr>
        <td colspan="2" style="text-align: left">
            Reliability Score Settings
        </td>
    </tr>
    <tr>
        <td colspan="2">
            <asp:Label ID="Label1" runat="server" Text="Low less than " ForeColor="Red" />
            <asp:TextBox ID="tbMedReliability" Text="0.245" runat="server" Style="text-align: left;
                width: 45px" />
            <asp:Label ID="Label2" runat="server" Text="High more than" ForeColor="DarkGreen" />
            <asp:TextBox ID="tbHighReliability" Text="0.600" runat="server" Style="text-align: left;
                width: 45px" />
        </td>
    </tr>
    <tr>
        <td colspan="2" style="text-align: left">
            <asp:Label ID="Label3" runat="server" Text="Medium is between Low and High" ForeColor="Black" />
        </td>
    </tr>
    <tr>
        <td style="text-align: right">
            <asp:RadioButtonList ID="rdoOutputType" runat="server" RepeatDirection="Horizontal">
                <asp:ListItem>Excel</asp:ListItem>
                <asp:ListItem Selected="True">PDF</asp:ListItem>
            </asp:RadioButtonList>
        </td>
        <td>
            <asp:Button ID="btnViewReport" runat="server" OnCommand="GenerateReport" Text="Generate Report"/>
        </td>
    </tr>
</table>
<br />
<div id="SireSelectionSheetsReport">
    <rsweb:ReportViewer ID="rv" runat="server" Visible="false" EnableViewState="False" />
</div>
