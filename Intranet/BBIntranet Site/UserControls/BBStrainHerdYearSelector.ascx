<%@ Control Language="C#" AutoEventWireup="true" CodeFile="BBStrainHerdYearSelector.ascx.cs" Inherits="UserControls_StrainHerdYearSelector" %>

<div>
    <table>
        <tr>
            <td style="width: 52px" valign="top">
                Strain</td>
            <td valign="top" align="left" style="width: 220px">
                <asp:DropDownList ID="ddlStrain" runat="server" DataSourceID="odsStrainList" AutoPostBack="True" DataTextField="StrainCode" DataValueField="StrainCode" Width="56px">
                </asp:DropDownList><asp:ObjectDataSource ID="odsStrainList" runat="server" OldValuesParameterFormatString="original_{0}"
                    SelectMethod="GetStrains" TypeName="Beefbooster.Web.BBDataHelper"></asp:ObjectDataSource>
            </td>
        </tr>
        <tr>
            <td style="width: 52px; height: 11px;" valign="top">
                Herd&nbsp;
            </td>
            <td align="left" style="width: 220px; height: 11px" valign="top">
                <asp:DropDownList ID="ddlHerd" runat="server" DataSourceID="odsHerdlist" DataTextField="Description" DataValueField="SN" Width="200px" OnDataBound="ddlHerd_DataBound">
                </asp:DropDownList>
                <asp:ObjectDataSource ID="odsHerdlist" runat="server" OldValuesParameterFormatString="original_{0}"
                    SelectMethod="GetHerds" TypeName="Beefbooster.Web.BBDataHelper">
                    <SelectParameters>
                        <asp:ControlParameter ControlID="ddlStrain" Name="inStrain" PropertyName="SelectedValue"
                            Type="String" />
                    </SelectParameters>
                </asp:ObjectDataSource>
            </td>
        </tr>
        <tr>
            <td align="left">
                Year</td>
            <td style="width: 220px" align="left">
                <asp:DropDownList ID="ddlYear" runat="server" DataSourceID="odsYearList" DataTextField="YearNumberAndLetter" 
                    DataValueField="YearNumber" OnDataBound="ddlYear_DataBound">
                </asp:DropDownList>
            </td>
        </tr>
    </table>
</div>
<asp:ObjectDataSource ID="odsYearList" runat="server" SelectMethod="GetCalfYearLetters"
    TypeName="Beefbooster.Web.BBDataHelper" OldValuesParameterFormatString="original_{0}">
    <SelectParameters>
        <asp:Parameter DefaultValue="2005" Name="startingYear" Type="Int32" />
    </SelectParameters>
</asp:ObjectDataSource>

