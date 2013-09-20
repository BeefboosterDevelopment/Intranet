<%@ Control Language="C#" AutoEventWireup="true" CodeFile="Report303.ascx.cs" Inherits="Report303" %>
<%@ Register Src="BBStrainHerdYearSelector.ascx" TagName="BBStrainHerdYearSelector" TagPrefix="uc1" %>
<%@ Register assembly="Microsoft.ReportViewer.WebForms, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a" namespace="Microsoft.Reporting.WebForms" tagprefix="rsweb" %>
<table id="tblParameters" style="border-top-style: ridge; border-right-style: ridge; border-left-style: ridge; border-bottom-style: ridge; width: 400px; height: 181px;" >
    <tr>
        <td align="center" colspan="1" style="font-weight: bold; height: 21px; width: 505px;">
            Test Station Performance Report - 2008 Data Version</td>
    </tr>
    <tr>
        <td valign="middle" style="width: 505px; height: 47px;" align="center">
            <uc1:BBStrainHerdYearSelector id="ucBBStrainHerdYearSelector" runat="server">
            </uc1:BBStrainHerdYearSelector>
            &nbsp;&nbsp;&nbsp;
        </td>
    </tr>
    <tr>
        <td align="center" style="width: 505px; height: 44px" valign="middle">
            <asp:RadioButtonList ID="rdoOutputType" runat="server" RepeatDirection="Horizontal"
                Width="111px">
                <asp:ListItem>Excel</asp:ListItem>
                <asp:ListItem Selected="True">PDF</asp:ListItem>
            </asp:RadioButtonList>
            <asp:Button ID="btnViewReport" runat="server" OnCommand="GenerateReport"
                Text="Generate Report" Width="120px"/></td>
    </tr>
</table>            
<div id="performanceReport" >
    <rsweb:ReportViewer ID="rv" runat="server" visible="false" height="13px" width="409px" Font-Names="Verdana" Font-Size="8pt" EnableViewState="False">
    </rsweb:ReportViewer>
    &nbsp;
</div>