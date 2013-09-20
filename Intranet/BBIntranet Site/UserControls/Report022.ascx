<%@ Control Language="C#" AutoEventWireup="true" CodeFile="Report022.ascx.cs" Inherits="UserControls_Report022" %>
<%@ Register Src="BBHerdYearSelector.ascx" TagName="BBHerdYearSelector" TagPrefix="uc1" %>
<%--<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a"
    Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>--%>
<%@ Register assembly="Microsoft.ReportViewer.WebForms, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a" namespace="Microsoft.Reporting.WebForms" tagprefix="rsweb" %>
	
	&nbsp;<asp:Panel ID="Panel1" runat="server" Height="229px" Width="486px">
	
	<table id="tblParameters" cellspacing="1" cellpadding="1" border="1" >
        <tr>
        <td align="center" colspan="2" style="font-weight: bold; font-size: large">
            Cow Production Report</td>
        </tr>
       <tr>
            <td colspan="2" style="text-align: left; height: 86px;">
                <uc1:BBHerdYearSelector ID="BBHerdYearSelector1" runat="server" />
            </td>
        </tr>

        <tr>
            <td colspan="2" style="text-align: left">
                <asp:Label ID="lblSortOrder" runat="server" Text="Sort Order" Width="86px" />
                <asp:DropDownList ID="ddlSortOrder" runat="server">
                    <asp:ListItem Selected="True" Value="0">Age Of Dam</asp:ListItem>
                    <asp:ListItem Value="1">Value Indicator</asp:ListItem>
                </asp:DropDownList>
            </td>
        </tr>
        
		<tr>
			<td colspan="2" style="text-align: center" align="left">
                <asp:RadioButtonList ID="rdoOutputType" runat="server" EnableViewState="False" RepeatDirection="Horizontal"
                    Width="111px">
                    <asp:ListItem>Excel</asp:ListItem>
                    <asp:ListItem Selected="True">PDF</asp:ListItem>
                </asp:RadioButtonList>
			    <asp:button id="btnViewReport22" runat="server" Width="120px" Height="24px" Text="Generate Report" OnCommand="GenerateReport">
			    </asp:button>
			</td>
		</tr>
	</table>
    <rsweb:ReportViewer ID="rv" runat="server" Height="31px" Width="490px" Font-Names="Verdana" Font-Size="8pt" Visible="False">
        <LocalReport DisplayName="Cow Performance">
        </LocalReport>
    </rsweb:ReportViewer>
</asp:Panel>
