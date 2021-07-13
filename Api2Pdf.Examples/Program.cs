using System;
using System.Collections.Generic;

namespace Api2Pdf.Examples
{
    class Program
    {
        public const string API_KEY = "";
        static void Main(string[] args)
        {
            if (string.IsNullOrEmpty(API_KEY))
            {
                Console.WriteLine("Set API Key variable before running script");
                return;
            }

            Console.WriteLine("Starting up...");

            var a2pClient = new Api2Pdf(API_KEY);

            //test data
            string sampleHtml = "<p>Hello World</p>";
            string sampleWebsite = "https://www.api2pdf.com";
            string sampleDocxFile = "https://www.api2pdf.com/wp-content/themes/api2pdf/assets/samples/sample-word-doc.docx";
            string sampleHtmlFile = "http://www.api2pdf.com/wp-content/uploads/2021/01/sampleHtml.html";
            string sampleHtmlToXlsxFile = "http://www.api2pdf.com/wp-content/uploads/2021/01/sampleTables.html";
            string samplePdf = "http://www.api2pdf.com/wp-content/uploads/2021/01/1a082b03-2bd6-4703-989d-0443a88e3b0f-4.pdf";

            Api2PdfResult result;

            //Chrome - HTML to PDF
            result = a2pClient.Chrome.HtmlToPdf(new ChromeHtmlToPdfRequest
            {
                Html = sampleHtml
            });

            Console.WriteLine($"Chrome HTML to PDF: {result.FileUrl}");

            //Chrome - HTML to PDF
            result = a2pClient.Chrome.HtmlToPdf(new ChromeHtmlToPdfRequest
            {
                Html = sampleHtml,
                FileName = "test.pdf",
                Inline = false,
                Options = new ChromeHtmlToPdfOptions
                {
                    Delay = 3000,
                    HeaderTemplate = "<div class=\"page-footer\" style=\"width:100%; text-align:right; font-size:12px;\">Page <span class=\"pageNumber\"></span> of <span class=\"totalPages\"></span></div>",
                    FooterTemplate = "<div class=\"page-footer\" style=\"width:100%; text-align:right; font-size:12px;\">Page <span class=\"pageNumber\"></span> of <span class=\"totalPages\"></span></div>",
                    DisplayHeaderFooter = true,
                    Landscape = true
                }
            });

            Console.WriteLine($"Chrome HTML to PDF, additional options: {result.FileUrl}");

            //Chrome - URL to PDF
            result = a2pClient.Chrome.UrlToPdf(new ChromeUrlToPdfRequest
            {
                Url = sampleWebsite
            });

            Console.WriteLine($"Chrome URL to PDF: {result.FileUrl}");

            //Chrome - HTML to Image
            result = a2pClient.Chrome.HtmlToImage(new ChromeHtmlToImageRequest
            {
                Html = sampleHtml
            });

            Console.WriteLine($"Chrome HTML to Image: {result.FileUrl}");

            //Chrome - URL to Image
            result = a2pClient.Chrome.UrlToImage(new ChromeUrlToImageRequest
            {
                Url = "https://www.api2pdf.com"
            });

            Console.WriteLine($"Chrome Url to Image: {result.FileUrl}");

            //Wkhtml

            //Wkhtml - HTML to PDF
            result = a2pClient.Wkhtml.HtmlToPdf(new WkhtmlHtmlToPdfRequest
            {
                Html = sampleHtml
            });

            Console.WriteLine($"Wkhtml HTML to PDF: {result.FileUrl}");

            //Wkhtml - HTML to PDF
            result = a2pClient.Wkhtml.HtmlToPdf(new WkhtmlHtmlToPdfRequest
            {
                Html = sampleHtml,
                FileName = "sample.pdf",
                Inline = false,
                EnableToc = true,
                Options = new System.Collections.Generic.Dictionary<string, string>()
                {
                    { "orientation", "landscape" }
                }             
            });

            Console.WriteLine($"Wkhtml HTML to PDF, additional options and Table of Contents enabled: {result.FileUrl}");

            //Wkhtml - URL to PDF
            result = a2pClient.Wkhtml.UrlToPdf(new WkhtmlUrlToPdfRequest
            {
                Url = sampleWebsite
            });

            Console.WriteLine($"Wkhtml URL to PDF: {result.FileUrl}");

            //libreoffice

            //LibreOffice - Any to PDF
            result = a2pClient.LibreOffice.AnyToPdf(new LibreFileConversionRequest
            {
                Url = sampleDocxFile
            });

            Console.WriteLine($"LibreOffice Docx to PDF: {result.FileUrl}");

            //LibreOffice - Html to Docx
            result = a2pClient.LibreOffice.HtmlToDocx(new LibreFileConversionRequest
            {
                Url = sampleHtmlFile
            });

            Console.WriteLine($"LibreOffice Html to Docx: {result.FileUrl}");

            //LibreOffice - Html to Xlsx
            result = a2pClient.LibreOffice.HtmlToXlsx(new LibreFileConversionRequest
            {
                Url = sampleHtmlToXlsxFile
            });

            Console.WriteLine($"LibreOffice Html to Xlsx: {result.FileUrl}");


            //LibreOffice - PDF to HTML
            result = a2pClient.LibreOffice.PdfToHtml(new LibreFileConversionRequest
            {
                Url = samplePdf
            });

            Console.WriteLine($"LibreOffice PDF to HTML: {result.FileUrl}");

            //PdfSharp

            //PdfSharp - Merge PDFs together
            result = a2pClient.PdfSharp.MergePdfs(new PdfMergeRequest
            {
                Urls = new List<string>()
                {
                    samplePdf, samplePdf, samplePdf
                }
            });

            Console.WriteLine($"Merge PDFs: {result.FileUrl}");

            //PdfSharp - add bookmarks
            result = a2pClient.PdfSharp.SetBookmarks(new PdfBookmarkRequest
            {
                Url = result.FileUrl,
                Bookmarks = new List<PdfBookmark>()
                {
                    new PdfBookmark
                    {
                        Page = 0,
                        Title = "Introduction"
                    },
                    new PdfBookmark
                    {
                        Page = 1,
                        Title = "Second Page"
                    }
                }
            });

            Console.WriteLine($"PdfSharp - set bookmarks: {result.FileUrl}");

            //PdfSharp - add password
            result = a2pClient.PdfSharp.SetPassword(new PdfPasswordRequest
            {
                Url = samplePdf,
                UserPassword = "hello"
            });

            Console.WriteLine($"PdfSharp - set password: {result.FileUrl}");

            //Delete PDF
            result = a2pClient.Chrome.HtmlToPdf(new ChromeHtmlToPdfRequest
            {
                Html = sampleHtml
            });
            a2pClient.Utilities.Delete(result.ResponseId);

            Console.WriteLine($"Delete PDF: this url should break: {result.FileUrl}");
        }
    }
}
