<%@ Control Language="C#" AutoEventWireup="true" CodeFile="Report310.ascx.cs" Inherits="UserControls.Report310" %>
<%@ Register Src="BBBreederSelector.ascx" TagName="BBBreederSelector" TagPrefix="uc1" %>
<%@ Register assembly="Microsoft.ReportViewer.WebForms, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a" namespace="Microsoft.Reporting.WebForms" tagprefix="rsweb" %>
<style type="text/css">
    .style1
    {
        width: 923px;
        font-size: larger
    }
</style>
<table id="tblParameters" style="border-bottom-style: ridge; border-left-style: ridge; border-right-style: ridge; border-top-style: ridge; height: 181px; width: 400px;" >
    <tr>
        <td style="font-weight: bold; text-align: center;" class="style1">
            Breeder Bull Battery Performance Report
        </td>
    </tr>
    <tr>
        <td colspan="2">&nbsp;</td>
    </tr>    
    <tr>
        <td>
            <uc1:BBBreederSelector id="ucBBBreederSelector1" runat="server">
            </uc1:BBBreederSelector>
        </td>
    </tr>
    <tr>
        <td colspan="2">&nbsp;</td>
    </tr>    
    <tr>
        <td>
            <uc1:BBBreederSelector id="ucBBBreederSelector2" runat="server">
            </uc1:BBBreederSelector>
        </td>
    </tr>
    <tr>
        <td colspan="2">&nbsp;</td>
    </tr>

    <tr>
        <td colspan="2" style="text-align: left">&nbsp;</td>
    </tr>     
    <tr><td colspan="2" style="text-align: center; font-size: large">Reliability Score Settings</td></tr>
    <tr>
        <td colspan="2" >
            <asp:Label  runat="server" Text="Low: less than " ForeColor="Blue"/>
            <asp:TextBox ID="tbMedReliability" Text="0.25" runat="server" style="text-align: left; width:45px" />
            <asp:Label  runat="server" Text="High: more than" ForeColor="Red"/>
            <asp:TextBox ID="tbHighReliability" Text="0.70" runat="server" style="text-align: left; width:45px"/>
        </td>
    </tr>

    <tr>
        <td colspan="2" style="text-align: left">
           <asp:Label  runat="server" Text="Medium: between Low and High" ForeColor="Orange"/>
        </td>
    </tr>

    <tr>
        <td colspan="2" style="text-align: left">&nbsp;</td>
    </tr>
                    



                    
    <tr>
        <td colspan="2" style="text-align: center;">
            <asp:RadioButtonList ID="rdoOutputType" runat="server" RepeatDirection="Horizontal" >
                <asp:ListItem>Excel</asp:ListItem>
                <asp:ListItem Selected="True">PDF</asp:ListItem>
            </asp:RadioButtonList>
            <asp:Button runat="server" OnCommand="GenerateReport"
                        Text="Generate Report" />
        </td>
    </tr>
</table>            
<div id="batteryReport" >
    <rsweb:ReportViewer ID="rv" runat="server" visible="false" height="13px" width="409px" Font-Names="Verdana" Font-Size="8pt" EnableViewState="False">
    </rsweb:ReportViewer>
    &nbsp;
</div>