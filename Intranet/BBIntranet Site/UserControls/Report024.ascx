<%@ Control Language="C#" AutoEventWireup="true" CodeFile="Report024.ascx.cs" Inherits="UserControls_Report024" %>
<%@ Register assembly="Microsoft.ReportViewer.WebForms, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a" namespace="Microsoft.Reporting.WebForms" tagprefix="rsweb" %>
<%@ Register Src="BBHerdYearSelector.ascx" TagName="BBHerdYearSelector" TagPrefix="uc1" %>

<script  type="text/javascript" language="javascript">
    function SubmitForm(param)
    {
        document.forms(0).__EVENTARGUMENT.value = param;
        document.forms(0).submit();
    }
</script>

  <table id="tblParameters" style="border-top-style: ridge; border-right-style: ridge; border-left-style: ridge; border-bottom-style: ridge; width: 674px;">
    <tr>
        <td colspan="2" style="font-weight: bold; font-size: large; text-align:center">
            Heifer Selection Report</td>
    </tr>
    <tr>
        <td colspan="2" rowspan="2">
            <uc1:BBHerdYearSelector ID="ucBBHerdYearSelector" runat="server" />
        </td>
    </tr>

    <tr>
    <td colspan="2"></td>         
    </tr>

    
    <tr>
        <td colspan="2">
            <asp:Button id="btnDefaults" runat="server" Width="132px" Height="24px" 
                Text="Set Defaults"  onclick="btnDefaults_Click" />        
        </td>
    </tr>

    <tr>
        <td>
            Minimum BWT</td>
        <td>
            <asp:TextBox ID="tbMinBWT" runat="server" Width="40px" MaxLength="3" ></asp:TextBox></td>
    </tr>
    <tr>
        <td style="width: 196px">
            Maximum BWT</td>
        <td style="width: 210px">
            <asp:TextBox ID="tbMaxBWT" runat="server" Width="40px" MaxLength="3"></asp:TextBox>
        </td>
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
            # Of Heifers To Select</td>
        <td style="height: 22px; width: 210px;">
            <asp:TextBox ID="tbNumCalves" runat="server" AccessKey=" " MaxLength="3" Width="60px"></asp:TextBox>
        </td>
    </tr>
    <tr>
        <td>Sort Order</td>
        <td>
            <asp:DropDownList ID="cboSortOrder" runat="server" Width="204px" Height="19px">
                <asp:ListItem Selected="True" Value="SI">Selection Index</asp:ListItem>
                <asp:ListItem Value="ID">Heifer Calf Id</asp:ListItem>            
            </asp:DropDownList></td>
    </tr>
    <tr>
        <td>Output Format</td>
        <td>
            <asp:RadioButtonList ID="rdoOutputType" runat="server" EnableViewState="False" RepeatDirection="Horizontal">
                <asp:ListItem>Excel</asp:ListItem>
                <asp:ListItem Selected="True">PDF</asp:ListItem>
            </asp:RadioButtonList>
        </td>
    </tr>
    <tr>
        <td colspan="2" style="text-align: center">
            <asp:button id="btnViewReport24" runat="server" Width="132px" Height="24px" 
                Text="Generate Report" OnCommand="GenerateReport" />
       </td>
    </tr>
    </table>

  <div id="reportViewer">
        <rsweb:ReportViewer ID="rv024" runat="server" Font-Names="Verdana" Font-Size="8pt" Height="51px" Width="673px" Visible="False" />
  </div> 