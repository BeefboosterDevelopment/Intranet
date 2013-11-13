<%@ Control Language="C#" AutoEventWireup="true" CodeFile="Report011.ascx.cs" Inherits="UserControls_Report011" %>
<%@ Register assembly="Microsoft.ReportViewer.WebForms, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a" namespace="Microsoft.Reporting.WebForms" tagprefix="rsweb" %>

<%--<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a"
    Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>--%>
    
<table style="width: 508px; background-color: #ffffcc;" border="1">
    <tr>
        <td align="center" colspan="2" style="font-weight: bold; font-size: large">
            11 Preliminary (Pre Weaning) Bull Calf Qualifying Report</td>
    </tr>
    <tr>
        <td style="height: 24px">
            Herd</td>
        <td style="width: 161px; height: 24px">
            <asp:DropDownList ID="ddlHerd" runat="server" Width="215px">
                <asp:ListItem Selected="True" Value="5">AB - Antelope Butte Ranch</asp:ListItem>
            </asp:DropDownList></td>
    </tr>
    <tr>
        <td>
            Calving Year</td>
        <td style="width: 161px">
            <asp:DropDownList ID="ddlYear" runat="server" Width="71px">
                <asp:ListItem>2005</asp:ListItem>
                <asp:ListItem>2006</asp:ListItem>
                <asp:ListItem>2007</asp:ListItem>
                <asp:ListItem>2008</asp:ListItem>
                <asp:ListItem>2009</asp:ListItem>
                <asp:ListItem>2010</asp:ListItem>
                <asp:ListItem>2011</asp:ListItem>
                <asp:ListItem>2012</asp:ListItem>
                <asp:ListItem Selected="True">2013</asp:ListItem>
                <asp:ListItem>2014</asp:ListItem>
            </asp:DropDownList></td>
    </tr>
    
    <tr>
        <td colspan="2" style="text-align: center; height: 44px;" ><asp:Button ID="btnDefaults" runat="server" Text="Set Remaining Defaults" OnCommand="SetDefaults" /></td>
    </tr>

    <tr>
        <td>
            Include Pulled Calves</td>
        <td style="width: 161px">
            <asp:CheckBox ID="cbIncludePulled" runat="server" /></td>
    </tr>
    <tr>
        <td style="height: 24px">
            Minimum BWT</td>
        <td style="height: 24px; width: 161px;">
            <asp:TextBox ID="tbMinBWT" runat="server" Width="80px"></asp:TextBox></td>
    </tr>
    <tr>
        <td>
            Maximum BWT</td>
        <td style="width: 161px">
            <asp:TextBox ID="tbMaxBWT" runat="server" Width="79px"></asp:TextBox></td>
    </tr>
    <tr>
        <td style="height: 25px">
            Max Birth Date - Month</td>
        <td style="height: 25px; width: 161px;">
            <asp:DropDownList ID="ddlMinBDateMonth" runat="server" Width="83px">
                <asp:ListItem Value="4">March</asp:ListItem>
                <asp:ListItem Value="4">April</asp:ListItem>
                <asp:ListItem Selected="True" Value="5">May</asp:ListItem>
                <asp:ListItem Value="6">June</asp:ListItem>
                <asp:ListItem Value="7">July</asp:ListItem>
            </asp:DropDownList></td>
    </tr>
    <tr>
        <td style="height: 25px">
            Max Birth Date - Day</td>
        <td style="height: 25px; width: 161px;">
            <asp:DropDownList ID="ddlMinBDateDay" runat="server" Width="50px">
                <asp:ListItem>1</asp:ListItem>
                <asp:ListItem>2</asp:ListItem>
                <asp:ListItem>3</asp:ListItem>
                <asp:ListItem>4</asp:ListItem>
                <asp:ListItem>5</asp:ListItem>
                <asp:ListItem>6</asp:ListItem>
                <asp:ListItem>7</asp:ListItem>
                <asp:ListItem>8</asp:ListItem>
                <asp:ListItem>9</asp:ListItem>
                <asp:ListItem>10</asp:ListItem>
                <asp:ListItem>11</asp:ListItem>
                <asp:ListItem>12</asp:ListItem>
                <asp:ListItem>13</asp:ListItem>
                <asp:ListItem>14</asp:ListItem>
                <asp:ListItem Selected="True">15</asp:ListItem>
                <asp:ListItem>16</asp:ListItem>
                <asp:ListItem>17</asp:ListItem>
                <asp:ListItem>18</asp:ListItem>
                <asp:ListItem>19</asp:ListItem>
                <asp:ListItem>20</asp:ListItem>
                <asp:ListItem>21</asp:ListItem>
                <asp:ListItem>22</asp:ListItem>
                <asp:ListItem>23</asp:ListItem>
                <asp:ListItem>24</asp:ListItem>
                <asp:ListItem>25</asp:ListItem>
                <asp:ListItem>26</asp:ListItem>
                <asp:ListItem>27</asp:ListItem>
                <asp:ListItem>28</asp:ListItem>
                <asp:ListItem>29</asp:ListItem>
                <asp:ListItem>30</asp:ListItem>
                <asp:ListItem>31</asp:ListItem>
                <asp:ListItem></asp:ListItem>
            </asp:DropDownList></td>
    </tr>
    <tr>
        <td>
            Include Heifer Calves</td>
        <td style="width: 161px">
            <asp:CheckBox ID="cbIncludeHeiferCalves" runat="server" /></td>
    </tr>
    <tr>
        <td style="height: 22px">
            Scope</td>
        <td style="height: 22px; width: 161px;">
            <asp:DropDownList ID="ddlReportScope" runat="server" Width="180px">
            </asp:DropDownList></td>
    </tr>
    <tr>
        <td colspan="2" style="text-align: center">
            <asp:Button ID="btnRun" runat="server" Text="Generate Report"
                OnCommand="GenerateReport"  /></td>
    </tr>
</table>
                
<rsweb:ReportViewer ID="rv011" runat="server" Width="797px" Font-Names="Verdana"
    Font-Size="8pt" Height="395px" ShowCredentialPrompts="False" ShowDocumentMapButton="False"
    ShowFindControls="False" ShowParameterPrompts="False" ShowPromptAreaButton="False">
</rsweb:ReportViewer>
