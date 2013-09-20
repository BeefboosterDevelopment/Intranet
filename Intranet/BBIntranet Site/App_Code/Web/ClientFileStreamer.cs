using System;
using System.IO;
using System.Web;

/// <summary>
/// Summary description for ClientFileStreamer
/// </summary>
public class ClientFileStreamer
{
    readonly HttpContext m_context;

    public ClientFileStreamer(HttpContext context)
    {
        m_context = context;
    }

    public void SendFileToClient(byte[] blob, string fullFilePath)
    {
        SendFileToClient(blob, fullFilePath, true);
    }

    public void SendFileToClient(byte[] blob, string fullFilePath, bool asAttachment)
    {
        if (blob == null)
            throw new ApplicationException("Can not view document. " + fullFilePath + " does not have an associated binary.");

        FileInfo fi = new FileInfo(fullFilePath);
        string displayName = fi.Name;

        MemoryStream memStream = null;

        try
        {
            // create the stream
            memStream = new MemoryStream(blob);

            m_context.Response.ClearHeaders();
            m_context.Response.ClearContent();
            m_context.Response.BufferOutput = true;
            if (fi.Extension.ToLower() == ".pdf")
                m_context.Response.ContentType = "application/pdf";
            else
                m_context.Response.ContentType = "application/octet-stream";


            if (asAttachment)
            {
                // prompt
                //m_context.Response.AddHeader("Content-Disposition", "attachment; filename=" + fileName);
                m_context.Response.AddHeader("Content-Disposition", "attachment; filename=" + displayName);
            }
            else
            {
                //  don't prompt
                m_context.Response.AddHeader("content-disposition", "inline; filename=" + displayName);
            }
            m_context.Response.OutputStream.Write(memStream.ToArray(), 0, memStream.ToArray().Length);
            m_context.Response.Flush();
            m_context.Response.Close();
        }
        catch (Exception ex)
        {
            m_context.Response.Write("Error : " + ex.Message);
        }
        finally
        {
            if (memStream != null)
                memStream.Close();
        }
    }

}