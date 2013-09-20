<%@ Control Language="C#" AutoEventWireup="true" CodeFile="ASRemlImporter.ascx.cs" Inherits="ASRemlProcessorControl" %>
<script language="javascript" type="text/javascript">
// <!CDATA[


// ]]>
</script>

 
    <table id="Table1" style="border-top-style: outset; border-right-style: outset; border-left-style: outset; border-bottom-style: outset; width: 672px; height: 312px; border-left-color: #ccff66; border-bottom-color: #ccff66; border-top-color: #ccff66; border-right-color: #ccff66; background-color: #ccff99;" >
    <tr>
        <td colspan="2" align="left" >
            <span style="font-weight: bold; font-family: Arial;">ASReml Importer</span> - <em><span style="font-size: 0.8em; font-style: italic;">Imports EBVs and expected values into Beefboosters database</span></em></td>
    </tr>

    <tr>
        <td align="right" style="text-align: left; width: 179px; height: 24px;">
            <asp:Label ID="lblEBVDataFile" runat="server" Text="ASReml Data File" Font-Bold="True" Width="139px" ></asp:Label></td>
        <td style="width: 499px; height: 24px">
            <asp:FileUpload ID="uploader" runat="server" Width="546px" Height="27px"  />&nbsp;</td>
    </tr>
    <tr>
        <td align="center" colspan="2" style="text-align: left">           
            <br />
            <br />
            <asp:Button ID="btnUpload" runat="server" Text="Upload" Height="23px" OnClick="btnUpload_Click" Width="85px" /><asp:TextBox ID="txtUploadedFile" runat="server" ReadOnly="True" Width="531px"></asp:TextBox><br />
            <br />
            <div style="float:left;  height: 31px; width: 233px;">
                <asp:Button
                    ID="btnValidate" runat="server" Text="Validate" Height="23px"  Width="80px" OnClick="btnValidate_Click" />&nbsp; Strain
                <asp:TextBox ID="txtStrainCode" runat="server" Height="27px" ReadOnly="True" Width="78px" Font-Size="Large"></asp:TextBox></div>
            <br />
            <br />
            <br />
            <div id="DIV1" style="width: 235px; height: 31px" >
                <asp:Button ID="btnProcessFile" runat="server" Height="23px" Text="Import" Width="80px" OnClick="btnProcessFile_Click" />
                <asp:TextBox ID="txtResult" runat="server" Width="129px" Font-Size="Large" Height="22px" ReadOnly="True"></asp:TextBox></div>
            <br />

            <asp:Button ID="btnGetStatus" runat="server" OnClick="btnGetStatus_Click" Text="Show Import Status Message"
                Width="218px" Height="23px" /><br />
            <asp:TextBox ID="txtStatusMessage" runat="server" Height="113px" TextMode="MultiLine"
                Width="690px" BorderStyle="Inset" ReadOnly="True"></asp:TextBox></td>
    </tr>

</table>
