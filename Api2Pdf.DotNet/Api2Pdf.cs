using System;
using System.Collections.Generic;
using System.Net.Http;
using Api2PdfLibrary.Models;
using Api2PdfLibrary.Extensions;
using System.Linq;
using System.Threading.Tasks;
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
        
        private static readonly string API_MERGE_URL = $"{API_BASE_URL}/merge";
        private static readonly string API_DELETE_PDF_URL_BASE = $"{API_BASE_URL}/pdf/";

        

        public class Api2PdfResponse
        {
            public string Pdf { get; set; }
            public double MbIn { get; set; }
            public double MbOut { get; set; }
            public double Cost { get; set; }
            public bool Success { get; set; }
            public string Error { get; set; }
            public string ResponseId { get; set; }

            public void SavePdf(string localPath)
            {
                var wc = new System.Net.WebClient();
                wc.DownloadFile(Pdf, localPath);
            }
            
            public Task SavePdfAsync(string localPath)
            {
                var wc = new System.Net.WebClient();
                var uri = new Uri(Pdf);
                return wc.DownloadFileTaskAsync(uri, localPath);
            }

            public byte[] GetPdfBytes()
            {
                var wc = new System.Net.WebClient();
                return wc.DownloadData(Pdf);
            }
            
            public Task<byte[]> GetPdfBytesAsync()
            {
                var wc = new System.Net.WebClient();
                return wc.DownloadDataTaskAsync(Pdf);
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


            return _httpClient.PostPdfRequest<Api2PdfResponse>(API_MERGE_URL, mergeRequest);
        }

        public Task<Api2PdfResponse> MergeAsync(IEnumerable<string> pdfUrls, bool inline = false, string outputFileName = null)
        {
            var mergeRequest = new MergeRequest
            {
                Urls = pdfUrls.ToArray(),
                FileName = outputFileName,
                InlinePdf = inline
            };


            return _httpClient.PostPdfRequestAsync<Api2PdfResponse>(API_MERGE_URL, mergeRequest);
        }
        
        public Api2PdfResponse Delete(string responseId)
        {
            return _httpClient.DeletePdfRequest<Api2PdfResponse>($"{API_DELETE_PDF_URL_BASE}{responseId}");
        }
        
        public Task<Api2PdfResponse> DeleteAsync(string responseId)
        {
            return _httpClient.DeletePdfRequestAsync<Api2PdfResponse>($"{API_DELETE_PDF_URL_BASE}{responseId}");
        }
    }

    public class LibreOfficeHandler
    {
        private string _apiKey;

        private static readonly string API_LIBRE_CONVERT_URL = $"{API_BASE_URL}/libreoffice/convert";
        
        public LibreOfficeHandler(string apiKey)
        {
            _apiKey = apiKey;
        }

        public Api2PdfResponse Convert(string url, bool inline = false, string outputFileName = null)
        {
            var libreRequest = CreateLibreOfficeConvertRequest(url, inline, outputFileName);

            return _httpClient.PostPdfRequest<Api2PdfResponse>(API_LIBRE_CONVERT_URL, libreRequest);
        }

        private static LibreOfficeConvertRequest CreateLibreOfficeConvertRequest(string url, bool inline, string outputFileName)
        {
            var libreRequest = new LibreOfficeConvertRequest
            {
                FileName = outputFileName,
                InlinePdf = inline,
                Url = url
            };
            return libreRequest;
        }

        public Task<Api2PdfResponse> ConvertAsync(string url, bool inline = false, string outputFileName = null)
        {
            var libreRequest = CreateLibreOfficeConvertRequest(url, inline, outputFileName);

            return _httpClient.PostPdfRequestAsync<Api2PdfResponse>(API_LIBRE_CONVERT_URL, libreRequest);
        }
    }

    public class WkHtmlToPdfHandler
    {
        private string _apiKey;
        private static readonly string API_WKHTML_HTML_URL = $"{API_BASE_URL}/wkhtmltopdf/html";
        private static readonly string API_WKHTML_URL_URL = $"{API_BASE_URL}/wkhtmltopdf/url";
        
        public WkHtmlToPdfHandler(string apiKey)
        {
            _apiKey = apiKey;
        }

        public Api2PdfResponse FromHtml(string html, bool inline = false, string outputFileName = null, Dictionary<string, string> options = null)
        {
            var wkRequest = CreateWkHtmlToPdfHtmlRequest(html, inline, outputFileName, options);

            return _httpClient.PostPdfRequest<Api2PdfResponse>(API_WKHTML_HTML_URL, wkRequest);
        }

        private static WkHtmlToPdfHtmlRequest CreateWkHtmlToPdfHtmlRequest(string html, bool inline, string outputFileName,
            Dictionary<string, string> options)
        {
            var wkRequest = new WkHtmlToPdfHtmlRequest
            {
                FileName = outputFileName,
                InlinePdf = inline,
                Html = html,
                Options = new Dictionary<string, string>()
            };

            if (options != null)
            {
                foreach (var o in options)
                    wkRequest.Options[o.Key] = o.Value;
            }

            return wkRequest;
        }

        public Task<Api2PdfResponse> FromHtmlAsync(string html, bool inline = false, string outputFileName = null, Dictionary<string, string> options = null)
        {
            var wkRequest = CreateWkHtmlToPdfHtmlRequest(html, inline, outputFileName, options);

            return _httpClient.PostPdfRequestAsync<Api2PdfResponse>(API_WKHTML_HTML_URL, wkRequest);
        }

        public Api2PdfResponse FromUrl(string url, bool inline = false, string outputFileName = null, Dictionary<string, string> options = null)
        {
            var wkRequest = CreateWkHtmlToPdfUrlRequest(url, inline, outputFileName, options);

            return _httpClient.PostPdfRequest<Api2PdfResponse>(API_WKHTML_URL_URL, wkRequest);
        }

        private static WkHtmlToPdfUrlRequest CreateWkHtmlToPdfUrlRequest(string url, bool inline, string outputFileName,
            Dictionary<string, string> options)
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

            return wkRequest;
        }
        
        public Task<Api2PdfResponse> FromUrlAsync(string url, bool inline = false, string outputFileName = null, Dictionary<string, string> options = null)
        {
            var wkRequest = CreateWkHtmlToPdfUrlRequest(url, inline, outputFileName, options);

            return _httpClient.PostPdfRequestAsync<Api2PdfResponse>(API_WKHTML_URL_URL, wkRequest);
        }
    }

    public class HeadlessChromeHandler
    {
        private string _apiKey;
        private static readonly string API_CHROME_HTML_URL = $"{API_BASE_URL}/chrome/html";
        private static readonly string API_CHROME_URL_URL = $"{API_BASE_URL}/chrome/url";

        public HeadlessChromeHandler(string apiKey)
        {
            _apiKey = apiKey;
        }

        public Api2PdfResponse FromHtml(string html, bool inline = false, string outputFileName = null, Dictionary<string, string> options = null)
        {
            var chromeRequest = CreateChromeHtmlRequest(html, inline, outputFileName, options);

            return _httpClient.PostPdfRequest<Api2PdfResponse>($"{API_BASE_URL}/chrome/html", chromeRequest);
        }

        private static ChromeHtmlRequest CreateChromeHtmlRequest(string html, bool inline, string outputFileName,
            Dictionary<string, string> options)
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

            return chromeRequest;
        }
        
        public Task<Api2PdfResponse> FromHtmlAsync(string html, bool inline = false, string outputFileName = null, Dictionary<string, string> options = null)
        {
            var chromeRequest = CreateChromeHtmlRequest(html, inline, outputFileName, options);

            return _httpClient.PostPdfRequestAsync<Api2PdfResponse>(API_CHROME_HTML_URL, chromeRequest);
        }

        public Api2PdfResponse FromUrl(string url, bool inline = false, string outputFileName = null, Dictionary<string, string> options = null)
        {
            var chromeRequest = CreateChromeUrlRequest(url, inline, outputFileName, options);

            return _httpClient.PostPdfRequest<Api2PdfResponse>(API_CHROME_URL_URL, chromeRequest);
        }

        private static ChromeUrlRequest CreateChromeUrlRequest(string url, bool inline, string outputFileName,
            Dictionary<string, string> options)
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

            return chromeRequest;
        }
        
        public Task<Api2PdfResponse> FromUrlAsync(string url, bool inline = false, string outputFileName = null, Dictionary<string, string> options = null)
        {
            var chromeRequest = CreateChromeUrlRequest(url, inline, outputFileName, options);

            return _httpClient.PostPdfRequestAsync<Api2PdfResponse>(API_CHROME_URL_URL, chromeRequest);
        }

    }   
}