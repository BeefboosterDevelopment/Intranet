<%@ Control Language="C#" AutoEventWireup="true" CodeFile="ASRemlExporter.ascx.cs" Inherits="ASRemlGeneratorControl" %>
    <table id="tblParameters" style="border-left-color: #66ccff; border-bottom-color: #66ccff; border-top-style: outset; border-top-color: #66ccff; border-right-style: outset; border-left-style: outset; border-right-color: #66ccff; border-bottom-style: outset; background-color: #ccffff;" >
        <tr>
            <td colspan="2">
                        <span style="font-weight: bold; font-family: Arial;">ASReml Exporter</span> - <span style="font-size: 0.8em; font-style: italic;"> Exports data from Beefboosters database
                    for submission to ASReml</span></td>
        </tr>
        <tr>
            <td>
                <asp:Button ID="btnGenerate" runat="server" Text="Generate" Width="77px" OnClick="btnGenerate_Click"   /></td>
            <td style="width: 428px">
                <asp:Label ID="lblZipFileName" runat="server" Width="411px" Font-Size="Smaller" ForeColor="Blue"></asp:Label>
                <asp:HiddenField ID="hdnZipFilePath" runat="server"></asp:HiddenField>
            </td>
        </tr>
        <tr>
            <td >
                <asp:Button ID="btnDownload" runat="server" Text="Download" Width="77px" Enabled="False" OnClick="btnDownload_Click" /></td>
            <td style="width: 428px">
                <asp:Button ID="btnReset" runat="server" Text="Reset" OnClick="btnReset_Click" /></td>

        </tr>

    </table>       