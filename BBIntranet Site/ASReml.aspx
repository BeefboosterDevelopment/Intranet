<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="ASReml.aspx.cs" Inherits="ASReml" Title="Beefbooster Intranet Site - ASReml" %>

<%@ Register Src="UserControls/ASRemlExporter.ascx" TagName="ASRemlExporter"
    TagPrefix="uc1" %>
<%@ Register Src="UserControls/ASRemlImporter.ascx" TagName="ASRemlImporter"
    TagPrefix="uc2" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <table>
        <tr>
            <td>
                <uc1:ASRemlExporter runat="server" />
                
            </td>
        </tr>
        <tr>
            <td>
                <uc2:ASRemlImporter runat="server" />
            </td>
        </tr>
    </table>
 </asp:Content>

