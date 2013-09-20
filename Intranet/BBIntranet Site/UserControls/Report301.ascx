<%@ Control Language="C#" AutoEventWireup="true" CodeFile="Report301.ascx.cs" Inherits="Report301" %>
<%@ Register assembly="Microsoft.ReportViewer.WebForms, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a" namespace="Microsoft.Reporting.WebForms" tagprefix="rsweb" %>

          <table id="tblParameters" border="0" style="border-top-style: ridge; border-right-style: ridge; border-left-style: ridge; border-bottom-style: ridge">
                <tr>
                    <td align="left" colspan="5" style="font-weight: bold; height: 21px;">
                        Test Station Obeservations - Worksheets</td>
                </tr>
                <tr>
                    <td align="right" style="width: 8px; height: 47px">
                        <strong>
                        Year</strong></td>
                    <td style="width: 43px; height: 47px">
                        <asp:DropDownList ID="ddlYearBorn" runat="server" Width="70px" EnableViewState="False">
                            <asp:ListItem Selected="True" Value="-1">Current</asp:ListItem>
                            <asp:ListItem>2011</asp:ListItem>
                            <asp:ListItem>2010</asp:ListItem>
                            <asp:ListItem>2009</asp:ListItem>
                            <asp:ListItem>2008</asp:ListItem>
                            <asp:ListItem>2007</asp:ListItem>
                        </asp:DropDownList></td>
              
                    <td align="right" style="width: 16px; height: 47px" valign="middle">
                        <strong>
                        Strain</strong></td>
                    <td style="height: 47px" valign="middle">
                        <asp:DropDownList ID="ddlStrain" runat="server" EnableViewState="False">
                            <asp:ListItem Selected="True">M1</asp:ListItem>
                            <asp:ListItem>M2</asp:ListItem>
                            <asp:ListItem>M3</asp:ListItem>
                            <asp:ListItem>M4</asp:ListItem>
                            <asp:ListItem>TX</asp:ListItem>
                        </asp:DropDownList></td>
               
                    <td align="right" style="width: 16px; height: 47px;" valign="middle">
                        <strong>
                        Style</strong></td>
                    <td valign="middle" style="height: 47px">
                        <asp:DropDownList ID="ddlReportStyle" runat="server" EnableViewState="False">
                            <asp:ListItem Selected="True">Weight</asp:ListItem>
                            <asp:ListItem>Feet and Legs</asp:ListItem>
                            <asp:ListItem>Off Test</asp:ListItem>
                            <asp:ListItem>BSE</asp:ListItem>
                        </asp:DropDownList></td>
                        
                    <td align="right" style="text-align: left; height: 47px;" valign="middle">
                        <asp:RadioButtonList ID="rdoOutputType" runat="server" RepeatDirection="Horizontal"
                            Width="111px" EnableViewState="False">
                            <asp:ListItem>Excel</asp:ListItem>
                            <asp:ListItem Selected="True">PDF</asp:ListItem>
                        </asp:RadioButtonList>
                    </td >

                    <td align="right" colspan="2" style="text-align: left; height: 47px;">
                        <asp:Button ID="btnViewReport" runat="server" Height="24px" OnCommand="GenerateReport"
                            Text="Generate Report" Width="120px"  />
                    </td>
                    
                </tr>
            </table>
            <div id="worksheetReport">
                <rsweb:ReportViewer ID="rvWorksheet" runat="server" visible="false" Font-Names="Verdana" Font-Size="8pt" Height="42px" Width="609px" EnableViewState="False">
                </rsweb:ReportViewer>
		    </div>