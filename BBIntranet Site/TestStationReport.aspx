<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeFile="TestStationReport.aspx.cs" Inherits="TestStationReport"
    Title="Test Station Report" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div class="genericContent">
        <div class="genericText">
            <div id="contactText" style="height:100%;">
				<img src="~/images/bts_head.jpg" alt="BeefBooster Bulls" title="BeefBooster Bulls" />
                <asp:Xml ID="xmlPretestReport" runat="server"  
                            TransformSource="PretestReport.xsl" 
                            DocumentSource="~/App_Data/XML/PretestReport.xml">
                </asp:Xml>
		    </div>
		</div>
    </div>
    <asp:Button ID="btnGenerateData" runat="server" Text="Re-generate Data" OnClick="RegenerateData" />
</asp:Content>
