using System;
using System.Collections.Generic;
using System.Text;

namespace Api2PdfLibrary.Models
{
    public abstract class PdfRequestBase
    {
        public bool InlinePdf { get; set; }
        public string FileName { get; set; }
    }

    public class WkHtmlToPdfUrlRequest : PdfRequestBase
    {
        public string Url { get; set; }

        public Dictionary<string, string> Options { get; set; }
    }

    public class WkHtmlToPdfHtmlRequest : PdfRequestBase
    {
        public string Html { get; set; }
        public Dictionary<string, string> Options { get; set; }
    }

    public class ChromeUrlRequest : PdfRequestBase
    {
        public string Url { get; set; }
        public Dictionary<string, string> Options { get; set; }
    }

    public class ChromeHtmlRequest : PdfRequestBase
    {
        public string Html { get; set; }
        public Dictionary<string, string> Options { get; set; }
    }

    public class LibreOfficeConvertRequest : PdfRequestBase
    {
        public string Url { get; set; }
    }

    public class MergeRequest : PdfRequestBase
    {
        public string[] Urls { get; set; }
    } 
}
