<%@ Control Language="C#" AutoEventWireup="true" CodeFile="MainDBToWebDB.ascx.cs" Inherits="UserControls_MainDBToWebDB" %>
    <div>
        <asp:Panel ID="Panel1" runat="server" Height="146px" Width="626px">
            <table style="width: 100%">
                <tr>
                    <td>
                        Transfer Codes</td>
                    <td>
                        <asp:Button ID="btnUpdateCodes" runat="server" OnCommand="UpdateCodes"
                            Text="Transfer" /></td>
                    <td style="text-align: right">
                        Last Tranfer Date</td>
                    <td>
                        <asp:TextBox ID="txtUpdateCodesDate" runat="server">(not done)</asp:TextBox></td>
                </tr>
                <tr>
                    <td>
                        Transfer Invoice Data</td>
                    <td>
                        <asp:Button ID="btnUpdateInvoices" runat="server" OnCommand="UpdateInvoices"
                            Text="Transfer" /></td>
                    <td style="text-align: right">
                        Last Tranfer Date</td>
                    <td>
                        <asp:TextBox ID="txtUpdateInvoicessDate" runat="server">(not done)</asp:TextBox></td>
                </tr>
                <tr>
                    <td>
                    </td>
                    <td>
                    </td>
                    <td>
                    </td>
                    <td>
                    </td>
                </tr>
            </table>
        </asp:Panel>
    
    </div>
