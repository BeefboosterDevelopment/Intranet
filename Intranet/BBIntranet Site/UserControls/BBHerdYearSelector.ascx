<%@ Control Language="C#" AutoEventWireup="true" CodeFile="BBHerdYearSelector.ascx.cs" Inherits="BBHerdYearSelector" %>
<%@ Register Src="BBYearSelector.ascx" TagName="BBYearSelector" TagPrefix="uc2" %>
<%@ Register Src="BBHerdSelector.ascx" TagName="BBHerdSelector" TagPrefix="uc1" %>
<div>
    <table style="width: 475px">
        <tr>
            <td>
                <uc1:BBHerdSelector ID="ucBBHerdSelector" runat="server" />
            </td>
            <td>
                <uc2:BBYearSelector ID="ucBBYearSelector" runat="server" />
            </td>
        </tr>
    </table>
</div>
