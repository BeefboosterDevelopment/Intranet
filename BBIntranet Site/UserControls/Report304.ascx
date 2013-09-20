<%@ Control Language="C#" AutoEventWireup="true" CodeFile="Report304.ascx.cs" Inherits="Report304" %>
<%@ Register Src="BBStrainHerdYearSelector.ascx" TagName="BBStrainHerdYearSelector" TagPrefix="uc1" %>
<%@ Register assembly="Microsoft.ReportViewer.WebForms, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a" namespace="Microsoft.Reporting.WebForms" tagprefix="rsweb" %>
<table id="tblParameters" style="border-top-style: ridge; border-right-style: ridge; border-left-style: ridge; border-bottom-style: ridge; width: 400px; height: 181px;" >
    <tr>
        <td style="text-align: center; font-weight: bold;">
            Test Station Performance Report
        </td>
    </tr>
    <tr>
        <td colspan="2" style="text-align: left">&nbsp;</td>
    </tr>    
    <tr>
        <td>
            <uc1:BBStrainHerdYearSelector id="ucBBStrainHerdYearSelector" runat="server">
            </uc1:BBStrainHerdYearSelector>
        </td>
    </tr>
    <tr>
        <td colspan="2" style="text-align: left">&nbsp;</td>
    </tr>    
    <tr>
        <td colspan="2" >
            <asp:Label runat="server" Text="Sort Order" Width="86px" />
            <asp:DropDownList ID="ddlSortOrder" runat="server">
                <asp:ListItem Value="0">Feedlot Id</asp:ListItem>
                <asp:ListItem Selected="True" Value="1">Selection Index</asp:ListItem>
            </asp:DropDownList>
        </td>
    </tr>
    <tr>
        <td colspan="2" style="text-align: left">&nbsp;</td>
    </tr>     
    <tr><td colspan="2" style="text-align: left">Reliability Score Settings</td></tr>
    <tr>
        <td colspan="2" >
            <asp:Label runat="server" Text="Low less than " ForeColor="Blue"/>
            <asp:TextBox ID="tbMedReliability" Text="0.25" runat="server" style="text-align: left; width:45px" />
            <asp:Label runat="server" Text="High more than" ForeColor="Red"/>
            <asp:TextBox ID="tbHighReliability" Text="0.70" runat="server" style="text-align: left; width:45px"/>
        </td>
    </tr>

    <tr>
        <td colspan="2" style="text-align: left">
           <asp:Label runat="server" Text="Medium is between Low and High" ForeColor="Orange"/>
        </td>
    </tr>

    <tr>
        <td colspan="2" style="text-align: left">&nbsp;</td>
    </tr>
                    
    <tr>
        <td colspan="2" style="text-align: center; ">
            <asp:RadioButtonList ID="rdoOutputType" runat="server" RepeatDirection="Horizontal" >
                <asp:ListItem>Excel</asp:ListItem>
                <asp:ListItem Selected="True">PDF</asp:ListItem>
            </asp:RadioButtonList>
            <asp:Button runat="server" OnCommand="GenerateReport"
                Text="Generate Report" />
        </td>
    </tr>
</table>            
<div id="performanceReport" >
    <rsweb:ReportViewer ID="rv" runat="server" visible="false" height="13px" width="409px" Font-Names="Verdana" Font-Size="8pt" EnableViewState="False">
    </rsweb:ReportViewer>
    &nbsp;
</div>