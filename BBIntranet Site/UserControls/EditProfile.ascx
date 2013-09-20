<%@ Control Language="C#" AutoEventWireup="true" CodeFile="EditProfile.ascx.cs" Inherits="UserControls_EditProfile" %>
<table width="90%" border="0" align="center" cellpadding="1" style="font-size: .9em;">
    <tr>
        <td align="center" colspan="5">
            <asp:Label ID="lblResults" runat="server" Font-Bold="True" Font-Size="Large" ForeColor="DarkRed"></asp:Label></td>
    </tr>
    <tr>
        <td align="right" style="width: 100px">
        </td>
        <td align="left">
        </td>
        <td align="right" style="width: 100px">
        </td>
        <td align="left" colspan="2">
        </td>
    </tr>
    <tr>
        <td align="right" style="width: 100px"><asp:Label ID="Label1" runat="server" Text="First Name"></asp:Label></td>
        <td align="left"><asp:TextBox ID="txtGivenName" runat="server" Width="200px" /></td>
        <td align="right" style="width: 100px"><asp:Label ID="Label7" runat="server" Text="Last Name"></asp:Label></td>
        <td align="left" colspan="2"><asp:TextBox ID="txtSurname" runat="server" Width="200px" /></td>
    </tr>
    <tr>
        <td align="right" style="width: 100px"><asp:Label ID="Label2" runat="server" Text="Company Name" Width="97px"></asp:Label></td>
        <td align="left"><asp:TextBox ID="txtCompanyName" runat="server" Width="200px" /></td>
        <td align="right"colspan="3"></td>
    </tr>
    <tr>
        <td align="right" style="width: 100px"><asp:Label ID="Label3" runat="server" Text="Company Url" Width="83px"></asp:Label></td>
        <td align="left"><asp:TextBox ID="txtCompanyUrl" runat="server" Width="200px" /></td>
        <td align="right" style="width: 100px"><asp:Label ID="Label8" runat="server" Text="*E-mail"></asp:Label></td>
        <td align="left" colspan="2"><asp:TextBox ID="txtEmail" runat="server" Width="200px"></asp:TextBox>
            <asp:RequiredFieldValidator ID="EmailRequired" runat="server" ControlToValidate="txtEmail"
                ErrorMessage="E-mail is required." ToolTip="E-mail is required." ValidationGroup="CreateUserWizard1"
                Font-Bold="True">*</asp:RequiredFieldValidator></td>
    </tr>
    <tr>
        <td align="center" colspan="5">&nbsp;</td>
    </tr>
    <tr>
        <td align="right" style="width: 100px"><asp:Label ID="Label4" runat="server" Text="Street"></asp:Label></td>
        <td align="left" colspan="2"><asp:TextBox ID="txtStreet" runat="server" Width="90%" /></td>
        <td align="right" ><asp:Label ID="Label9" runat="server" Text="Postal Code/ZIP" Width="100px"></asp:Label></td>
        <td align="left"><asp:TextBox ID="txtPostalCode" runat="server" Width="80px" /></td>
    </tr>
    <tr>
        <td align="right" style="width: 100px"><asp:Label ID="Label5" runat="server" Text="City/Town"></asp:Label></td>
        <td align="left"><asp:TextBox ID="txtCity" runat="server" Width="200px" /></td>
        <td align="right" style="width: 100px"><asp:Label ID="Label10" runat="server" Text="Province/State" Width="86px"></asp:Label></td>
        <td align="left" colspan="2"><asp:TextBox ID="txtProvince" runat="server" Width="200px" /></td>
    </tr>
    <tr>
        <td align="center" colspan="5">&nbsp;</td>
    </tr>
    <tr>
        <td align="right" style="width: 100px"><asp:Label ID="Label6" runat="server" Text="Phone Number" Width="88px"></asp:Label></td>
        <td align="left"><asp:TextBox ID="txtPhone" runat="server" Width="200px" /></td>
        <td align="right" style="width: 100px"><asp:Label ID="Label11" runat="server" Text="Fax Number"></asp:Label></td>
        <td align="left" colspan="2"><asp:TextBox ID="txtFAX" runat="server" Width="200px" /></td>
    </tr>
    <tr>
        <td align="center" colspan="5">&nbsp;</td>
    </tr>
    <tr>
        <td align="right" style="width: 100px"></td>
        <td align="right"><asp:Button ID="btnSave" runat="server" OnClick="btnSave_Click" Text="Save" /></td>
        <td align="left" style="width: 100px"><asp:Button ID="btnReset" runat="server" OnClick="btnReset_Click" Text="Reset" /></td>
        <td align="left" colspan="2"></td>
    </tr>
</table>
