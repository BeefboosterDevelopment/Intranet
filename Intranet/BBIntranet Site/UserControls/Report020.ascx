<%@ Control Language="C#" AutoEventWireup="true" CodeFile="Report020.ascx.cs" Inherits="UserControls_Report020" %>
<%@ Register assembly="Microsoft.ReportViewer.WebForms, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a" namespace="Microsoft.Reporting.WebForms" tagprefix="rsweb" %>
<%@ Register Src="BBHerdYearSelector.ascx" TagName="BBHerdYearSelector" TagPrefix="uc1" %>

<style type="text/css">
    .style1
    {
        color: #FF0066;
    }
</style>

<script  type="text/javascript" language="javascript">
    function SubmitForm(param)
    {
        document.forms(0).__EVENTARGUMENT.value = param;
        document.forms(0).submit();
    }
</script>

  <table id="tblParameters" border="0" style="border-top-style: ridge; border-right-style: ridge; border-left-style: ridge; border-bottom-style: ridge; width: 674px;">
    <tr>
        <td align="center" colspan="2" style="font-weight: bold; font-size: large">
            Bull Calf Selection Report</td>
    </tr>
    <tr>
        <td colspan="2">
            <uc1:BBHerdYearSelector ID="ucBBHerdYearSelector" runat="server" />
        </td>
    </tr>

    
    <tr>
        <td colspan="2" style="text-align: center; height: 44px;" >
            <span class="style1">(Select herd &amp; year before setting defaults)</span></td>
    </tr>


    <tr>
        <td colspan="2" style="text-align: center; height: 44px;" >
        &nbsp;<asp:Button  runat="server" Text="Set Remaining Defaults" OnCommand="SetDefaults" />
        </td>
    </tr>


    <tr>
        <td style="height: 24px; width: 196px;">
            Minimum BWT</td>
        <td style="height: 24px; width: 210px;">
            <asp:TextBox ID="tbMinBWT" runat="server" Width="40px" MaxLength="3" >80</asp:TextBox></td>
    </tr>
    <tr>
        <td style="width: 196px">
            Maximum BWT</td>
        <td style="width: 210px">
            <asp:TextBox ID="tbMaxBWT" runat="server" Width="40px" MaxLength="3">130</asp:TextBox>
        </td>
    </tr>
    <tr>
        <td style="width: 196px">
            Minimum ADG B-W</td>
        <td style="width: 210px">
            <asp:TextBox ID="tbMinADG" runat="server" MaxLength="3" Width="40px">2.25</asp:TextBox></td>
    </tr>
    <tr>
        <td style="height: 25px; width: 196px;">
            Max Birth Date - Month</td>
        <td style="height: 25px; width: 210px;">
            <asp:DropDownList ID="ddlMinBDateMonth" runat="server" Width="83px">
                <asp:ListItem Value="3">March</asp:ListItem>
                <asp:ListItem Value="4">April</asp:ListItem>
                <asp:ListItem Selected="True" Value="5">May</asp:ListItem>
                <asp:ListItem Value="6">June</asp:ListItem>
                <asp:ListItem Value="7">July</asp:ListItem>
            </asp:DropDownList></td>
    </tr>
    <tr>
        <td style="height: 25px; width: 196px;">
            Max Birth Date - Day</td>
        <td style="height: 25px; width: 210px;">
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
        <td style="height: 25px; width: 196px;">
            Include Pulled Calves</td>
        <td style="width: 210px">
            <asp:CheckBox ID="cbIncludePulled" runat="server" /></td>
    </tr>
    <tr>
        <td style="width: 196px">
            Include Heifer Calves</td>
        <td style="width: 210px">
            <asp:CheckBox ID="cbIncludeHeiferCalves" runat="server" /></td>
    </tr>
    <tr>
        <td style="height: 22px; width: 196px;">
            # Of Calves To Send</td>
        <td style="height: 22px; width: 210px;">
            <asp:TextBox ID="tbNumCalves" runat="server" AccessKey=" " MaxLength="3" Width="60px">20</asp:TextBox></td>
    </tr>
    <tr>
        <td>Sort Order</td>
        <td style="height: 25px; width: 210px;">
            <asp:DropDownList ID="cboSortOrder" runat="server" Width="204px" Height="19px">
                <asp:ListItem Selected="True" Value="SI">Selection Index</asp:ListItem>
                <asp:ListItem Value="ID">Calf Id</asp:ListItem>
                <asp:ListItem Value="DAM">Age of Dam</asp:ListItem>                
            </asp:DropDownList></td>
    </tr>
    <tr>
        <td colspan="2" style="text-align: center">
            <asp:RadioButtonList ID="rdoOutputType" runat="server" EnableViewState="False" RepeatDirection="Horizontal"
                Width="111px">
                <asp:ListItem>Excel</asp:ListItem>
                <asp:ListItem Selected="True">PDF</asp:ListItem>
            </asp:RadioButtonList>
             <asp:button runat="server" Width="120px" Height="24px" Text="Generate Report" OnCommand="GenerateReport" />

            </td>
    </tr>
    </table>

  <div id="reportViewer">
      <rsweb:ReportViewer ID="rv020" runat="server" Font-Names="Verdana" Font-Size="8pt" Height="51px" Width="673px" Visible="False" />
  </div> 