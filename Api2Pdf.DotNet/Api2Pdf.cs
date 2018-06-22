using System;
using System.Collections.Generic;
using System.Net.Http;
using Api2PdfLibrary.Models;
using Api2PdfLibrary.Extensions;
using System.Linq;
using static Api2PdfLibrary.Api2Pdf;

namespace Api2PdfLibrary
{
    public class Api2Pdf
    {
        public const string API_BASE_URL = "https://v2018.api2pdf.com/";
        public static HttpClient _httpClient;

        private string _apiKey;        
        public LibreOfficeHandler LibreOffice;
        public HeadlessChromeHandler HeadlessChrome;
        public WkHtmlToPdfHandler WkHtmlToPdf;

        

        public class Api2PdfResponse
        {
            public string Pdf { get; set; }
            public double MbIn { get; set; }
            public double MbOut { get; set; }
            public double Cost { get; set; }
            public bool Success { get; set; }
            public string Error { get; set; }

            public void SavePdf(string localPath)
            {
                var wc = new System.Net.WebClient();
                wc.DownloadFile(Pdf, localPath);
            }

            public byte[] GetPdfBytes()
            {
                var wc = new System.Net.WebClient();
                return wc.DownloadData(Pdf);
            }
        }

        public Api2Pdf(string apiKey, string tag = null)
        {
            _apiKey = apiKey;
            LibreOffice = new LibreOfficeHandler(apiKey);
            HeadlessChrome = new HeadlessChromeHandler(apiKey);
            WkHtmlToPdf = new WkHtmlToPdfHandler(apiKey);

            if (_httpClient == null)
            {
                _httpClient = new HttpClient();
                _httpClient.DefaultRequestHeaders.Add("Authorization", apiKey);

                if(!string.IsNullOrWhiteSpace(tag))
                    _httpClient.DefaultRequestHeaders.Add("Tag", tag);
            }
        }

        public Api2PdfResponse Merge(IEnumerable<string> pdfUrls, bool inline = false, string outputFileName = null)
        {
            var mergeRequest = new MergeRequest
            {
                Urls = pdfUrls.ToArray(),
                FileName = outputFileName,
                InlinePdf = inline
            };


            return _httpClient.PostPdfRequest<Api2PdfResponse>($"{API_BASE_URL}/merge", mergeRequest);
        }
    }

    public class LibreOfficeHandler
    {
        private string _apiKey;
        public LibreOfficeHandler(string apiKey)
        {
            _apiKey = apiKey;
        }

        public Api2PdfResponse Convert(string url, bool inline = false, string outputFileName = null)
        {
            var libreRequest = new LibreOfficeConvertRequest
            {
                FileName = outputFileName,
                InlinePdf = inline,
                Url = url
            };

            return _httpClient.PostPdfRequest<Api2PdfResponse>($"{API_BASE_URL}/libreoffice/convert", libreRequest);
        }
    }

    public class WkHtmlToPdfHandler
    {
        private string _apiKey;
        public WkHtmlToPdfHandler(string apiKey)
        {
            _apiKey = apiKey;
        }

        public Api2PdfResponse FromHtml(string html, bool inline = false, string outputFileName = null, Dictionary<string, string> options = null)
        {
            var wkRequest = new WkHtmlToPdfHtmlRequest
            {
                FileName = outputFileName,
                InlinePdf = inline,
                Html = html,
                Options = new Dictionary<string, string>()
            };

            if(options != null)
            {
                foreach (var o in options)
                    wkRequest.Options[o.Key] = o.Value;
            }

            return _httpClient.PostPdfRequest<Api2PdfResponse>($"{API_BASE_URL}/wkhtmltopdf/html", wkRequest);
        }

        public Api2PdfResponse FromUrl(string url, bool inline = false, string outputFileName = null, Dictionary<string, string> options = null)
        {
            var wkRequest = new WkHtmlToPdfUrlRequest
            {
                FileName = outputFileName,
                InlinePdf = inline,
                Url = url,
                Options = new Dictionary<string, string>()
            };

            if (options != null)
            {
                foreach (var o in options)
                    wkRequest.Options[o.Key] = o.Value;
            }

            return _httpClient.PostPdfRequest<Api2PdfResponse>($"{API_BASE_URL}/wkhtmltopdf/url", wkRequest);
        }
    }

    public class HeadlessChromeHandler
    {
        private string _apiKey;
        public HeadlessChromeHandler(string apiKey)
        {
            _apiKey = apiKey;
        }

        public Api2PdfResponse FromHtml(string html, bool inline = false, string outputFileName = null, Dictionary<string, string> options = null)
        {
            var chromeRequest = new ChromeHtmlRequest
            {
                FileName = outputFileName,
                InlinePdf = inline,
                Html = html,
                Options = new Dictionary<string, string>()
            };

            if (options != null)
            {
                foreach (var o in options)
                    chromeRequest.Options[o.Key] = o.Value;
            }

            return _httpClient.PostPdfRequest<Api2PdfResponse>($"{API_BASE_URL}/chrome/html", chromeRequest);
        }

        public Api2PdfResponse FromUrl(string url, bool inline = false, string outputFileName = null, Dictionary<string, string> options = null)
        {
            var chromeRequest = new ChromeUrlRequest
            {
                FileName = outputFileName,
                InlinePdf = inline,
                Url = url,
                Options = new Dictionary<string, string>()
            };

            if (options != null)
            {
                foreach (var o in options)
                    chromeRequest.Options[o.Key] = o.Value;
            }

            return _httpClient.PostPdfRequest<Api2PdfResponse>($"{API_BASE_URL}/chrome/url", chromeRequest);
        }
    }   
}