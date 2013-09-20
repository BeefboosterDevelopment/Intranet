<%@ Control Language="C#" AutoEventWireup="true" CodeFile="Report402.ascx.cs" Inherits="Report402" %>
<%@ Register assembly="Microsoft.ReportViewer.WebForms, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a" namespace="Microsoft.Reporting.WebForms" tagprefix="rsweb" %>
<table id="tblParameters" style="border-top-style: ridge; border-right-style: ridge; border-left-style: ridge; border-bottom-style: ridge" >
    <tr>
        <td align="left" colspan="8" style="font-weight: bold; height: 21px;">
            Sire Selection Sheets</td>
    </tr>
    <tr>
        <td align="right">
            <asp:Label ID="lblYear" runat="server" Font-Bold="True" Text="Year Born" Width="86px"></asp:Label></td>
        <td>
            <asp:DropDownList ID="ddlYearBorn" runat="server" Width="90px">
                <asp:ListItem Selected="True" Value="-1">Current</asp:ListItem>
                <asp:ListItem>2012</asp:ListItem>
                <asp:ListItem>2011</asp:ListItem>
                <asp:ListItem>2010</asp:ListItem>
                <asp:ListItem>2009</asp:ListItem>
                <asp:ListItem>2008</asp:ListItem>
                <asp:ListItem>2007</asp:ListItem>
                <asp:ListItem>2006</asp:ListItem>
                <asp:ListItem>2005</asp:ListItem>
                <asp:ListItem>2004</asp:ListItem>
            </asp:DropDownList></td>
               
        <td align="right" valign="middle">
            <asp:Label ID="lblStrain" runat="server" Font-Bold="True" Text="Strain"></asp:Label></td>
        <td valign="middle">
            <asp:DropDownList ID="ddlStrain" runat="server">
                <asp:ListItem Selected="True">M1</asp:ListItem>
                <asp:ListItem>M2</asp:ListItem>
                <asp:ListItem>M3</asp:ListItem>
                <asp:ListItem>M4</asp:ListItem>
                <asp:ListItem>TX</asp:ListItem>
            </asp:DropDownList></td>
             
        <td align="right" valign="middle">
            </td>
        <td valign="middle">
            </td>
                
        <td align="right" style="text-align: left" valign="middle">
            <asp:RadioButtonList ID="rdoOutputType" runat="server" RepeatDirection="Horizontal"
                Width="111px">
                <asp:ListItem>Excel</asp:ListItem>
                <asp:ListItem Selected="True">PDF</asp:ListItem>
            </asp:RadioButtonList>
            </td >
            <td >
            <asp:Button ID="btnViewReport" runat="server" OnCommand="GenerateReport"
                Text="Generate Report" Width="120px"/>
                </td>
    </tr>
</table>            
<br />
<div id="SireSelectionSheetsReport" >
    <rsweb:ReportViewer ID="rv" runat="server" visible="false" height="3px" width="581px" Font-Names="Verdana" Font-Size="8pt" EnableViewState="False">
    </rsweb:ReportViewer>
</div>