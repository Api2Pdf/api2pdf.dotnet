using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Api2Pdf
{
    public interface IChrome
    {
        Api2PdfResult HtmlToPdf(ChromeHtmlToPdfRequest request);
        Task<Api2PdfResult> HtmlToPdfAsync(ChromeHtmlToPdfRequest request);
        Api2PdfResult UrlToPdf(ChromeUrlToPdfRequest request);
        Task<Api2PdfResult> UrlToPdfAsync(ChromeUrlToPdfRequest request);
        Api2PdfResult HtmlToImage(ChromeHtmlToImageRequest request);
        Task<Api2PdfResult> HtmlToImageAsync(ChromeHtmlToImageRequest request);
        Api2PdfResult UrlToImage(ChromeUrlToImageRequest request);
        Task<Api2PdfResult> UrlToImageAsync(ChromeUrlToImageRequest request);
    }

    public interface IWkhtml
    {
        Api2PdfResult HtmlToPdf(WkhtmlHtmlToPdfRequest request);
        Task<Api2PdfResult> HtmlToPdfAsync(WkhtmlHtmlToPdfRequest request);
        Api2PdfResult UrlToPdf(WkhtmlUrlToPdfRequest request);
        Task<Api2PdfResult> UrlToPdfAsync(WkhtmlUrlToPdfRequest request);
    }

    public interface ILibreOffice
    {
        Api2PdfResult AnyToPdf(LibreFileConversionRequest request);
        Task<Api2PdfResult> AnyToPdfAsync(LibreFileConversionRequest request);
        Api2PdfResult PdfToHtml(LibreFileConversionRequest request);
        Task<Api2PdfResult> PdfToHtmlAsync(LibreFileConversionRequest request);
        Api2PdfResult HtmlToDocx(LibreFileConversionRequest request);
        Task<Api2PdfResult> HtmlToDocxAsync(LibreFileConversionRequest request);
        Api2PdfResult HtmlToXlsx(LibreFileConversionRequest request);
        Task<Api2PdfResult> HtmlToXlsxAsync(LibreFileConversionRequest request);
    }

    public interface IPdfSharp
    {
        Api2PdfResult MergePdfs(PdfMergeRequest request);
        Task<Api2PdfResult> MergePdfsAsync(PdfMergeRequest request);
        Api2PdfResult SetBookmarks(PdfBookmarkRequest request);
        Task<Api2PdfResult> SetBookmarksAsync(PdfBookmarkRequest request);
        Api2PdfResult SetPassword(PdfPasswordRequest request);
        Task<Api2PdfResult> SetPasswordAsync(PdfPasswordRequest request);

    }

    public interface IApi2PdfUtility
    {
        void Delete(string responseId);
    }

    public class Api2Pdf
    {
        public IChrome Chrome { get; private set; }
        public IWkhtml Wkhtml { get; private set; }
        public ILibreOffice LibreOffice { get; private set; }
        public IPdfSharp PdfSharp { get; private set; }
        public IApi2PdfUtility Utilities { get; private set; }

        public Api2Pdf(string apiKey)
        {
            Chrome = new Chrome(apiKey);
            Wkhtml = new Wkhtml(apiKey);
            LibreOffice = new LibreOffice(apiKey);
            PdfSharp = new PdfSharp(apiKey);
            Utilities = new Api2PdfUtility(apiKey);
        }

        public Api2Pdf(string overrideBaseUrl, Dictionary<string, string> httpHeaders = null)
        {
            Chrome = new Chrome(overrideBaseUrl, httpHeaders);
            Wkhtml = new Wkhtml(overrideBaseUrl, httpHeaders);
            LibreOffice = new LibreOffice(overrideBaseUrl, httpHeaders);
            PdfSharp = new PdfSharp(overrideBaseUrl, httpHeaders);
            Utilities = new Api2PdfUtility(overrideBaseUrl, httpHeaders);
        }
    }

    public abstract class Api2PdfBase
    {
        protected const string API2PDF_BASE_URL = "https://v2.api2pdf.com";
        protected string _baseUrl;

        protected Dictionary<string, string> _httpHeaders;

        protected static HttpClient _httpClient;

        public Api2PdfBase(string apiKey)
        {
            _httpHeaders = new Dictionary<string, string>()
            {
                { "Authorization", apiKey }
            };
            _baseUrl = API2PDF_BASE_URL;
        }

        public Api2PdfBase(string overrideBaseUrl, Dictionary<string, string> httpHeaders = null)
        {
            _httpHeaders = httpHeaders;
            _baseUrl = overrideBaseUrl;
        }

        protected T MakeRequest<T>(string route, object obj)
        {
            if (_httpClient == null)
            {
                _httpClient = new HttpClient();
                if (_httpHeaders != null)
                {
                    foreach (var header in _httpHeaders)
                    {
                        _httpClient.DefaultRequestHeaders.Add(header.Key, header.Value);
                    }
                }
            }

            var content = new StringContent(JsonConvert.SerializeObject(obj));
            return JsonConvert.DeserializeObject<T>(_httpClient.PostAsync(_baseUrl + route, content).Result.Content.ReadAsStringAsync().Result);
        }

        protected async Task<Api2PdfResult> MakeRequestAsync(string route, object obj)
        {
            if (_httpClient == null)
            {
                _httpClient = new HttpClient();
                if (_httpHeaders != null)
                {
                    foreach (var header in _httpHeaders)
                    {
                        _httpClient.DefaultRequestHeaders.Add(header.Key, header.Value);
                    }
                }
            }

            var content = new StringContent(JsonConvert.SerializeObject(obj));
            var response = await _httpClient.PostAsync(_baseUrl + route, content);
            if (response.StatusCode == HttpStatusCode.OK && _httpHeaders.ContainsKey("binary") && _httpHeaders["binary"] == "true")
            {
                var resultAsBytes = await response.Content.ReadAsByteArrayAsync();
                return new Api2PdfResult
                {
                    MbOut = resultAsBytes.Length / 1000,
                    ResultAsBytes = resultAsBytes,
                    Success = true
                };
            }     
            else
            {
                var resultAsJson = await response.Content.ReadAsStringAsync();
                var resultObj = JsonConvert.DeserializeObject<Api2PdfResult>(resultAsJson);
                return resultObj;
            }
        }

        protected Api2PdfResult MakeRequest(string route, object obj)
        {
            if (_httpClient == null)
            {
                _httpClient = new HttpClient();
                if (_httpHeaders != null)
                {
                    foreach (var header in _httpHeaders)
                    {
                        _httpClient.DefaultRequestHeaders.Add(header.Key, header.Value);
                    }
                }
            }

            var content = new StringContent(JsonConvert.SerializeObject(obj));
            var response = _httpClient.PostAsync(_baseUrl + route, content).Result;
            if (response.StatusCode == HttpStatusCode.OK && _httpHeaders.ContainsKey("binary") && _httpHeaders["binary"] == "true")
            {
                var resultAsBytes = response.Content.ReadAsByteArrayAsync().Result;
                return new Api2PdfResult
                {
                    MbOut = resultAsBytes.Length / 1000,
                    ResultAsBytes = resultAsBytes,
                    Success = true
                };
            }
            else
            {
                var resultAsJson = response.Content.ReadAsStringAsync().Result;
                var resultObj = JsonConvert.DeserializeObject<Api2PdfResult>(resultAsJson);
                return resultObj;
            }
        }
    }

    public class Chrome : Api2PdfBase, IChrome
    {
        public Chrome(string apiKey) : base(apiKey)
        {
            _baseUrl = API2PDF_BASE_URL + "/chrome";
        }

        public Chrome(string overrideBaseUrl, Dictionary<string, string> httpHeaders = null) : base(overrideBaseUrl, httpHeaders)
        {
        }

        public Api2PdfResult HtmlToImage(ChromeHtmlToImageRequest request)
        {
            string route = "/image/html";
            var response = MakeRequest(route, request);
            return response;
        }

        public async Task<Api2PdfResult> HtmlToImageAsync(ChromeHtmlToImageRequest request)
        {
            string route = "/image/html";
            var response = await MakeRequestAsync(route, request);
            return response;
        }

        public Api2PdfResult HtmlToPdf(ChromeHtmlToPdfRequest request)
        {
            string route = "/pdf/html";
            var response = MakeRequest(route, request);
            return response;
        }

        public async Task<Api2PdfResult> HtmlToPdfAsync(ChromeHtmlToPdfRequest request)
        {
            string route = "/pdf/html";
            var response = await MakeRequestAsync(route, request);
            return response;
        }

        public Api2PdfResult UrlToImage(ChromeUrlToImageRequest request)
        {
            string route = "/image/url";
            var response = MakeRequest(route, request);
            return response;
        }

        public async Task<Api2PdfResult> UrlToImageAsync(ChromeUrlToImageRequest request)
        {
            string route = "/image/url";
            var response = await MakeRequestAsync(route, request);
            return response; ;
        }

        public Api2PdfResult UrlToPdf(ChromeUrlToPdfRequest request)
        {
            string route = "/pdf/url";
            var response = MakeRequest(route, request);
            return response;
        }

        public async Task<Api2PdfResult> UrlToPdfAsync(ChromeUrlToPdfRequest request)
        {
            string route = "/pdf/url";
            var response = await MakeRequestAsync(route, request);
            return response;
        }
    }

    public class Wkhtml : Api2PdfBase, IWkhtml
    {
        public Wkhtml(string apiKey) : base(apiKey)
        {
            _baseUrl = API2PDF_BASE_URL + "/wkhtml";
        }

        public Wkhtml(string overrideBaseUrl, Dictionary<string, string> httpHeaders = null) : base(overrideBaseUrl, httpHeaders)
        {
        }

        public Api2PdfResult HtmlToPdf(WkhtmlHtmlToPdfRequest request)
        {
            string route = "/pdf/html";
            var response = MakeRequest(route, request);
            return response;
        }

        public async Task<Api2PdfResult> HtmlToPdfAsync(WkhtmlHtmlToPdfRequest request)
        {
            string route = "/pdf/html";
            var response = await MakeRequestAsync(route, request);
            return response;
        }

        public Api2PdfResult UrlToPdf(WkhtmlUrlToPdfRequest request)
        {
            string route = "/pdf/url";
            var response = MakeRequest(route, request);
            return response;
        }

        public async Task<Api2PdfResult> UrlToPdfAsync(WkhtmlUrlToPdfRequest request)
        {
            string route = "/pdf/url";
            var response = await MakeRequestAsync(route, request);
            return response;
        }
    }

    public class LibreOffice : Api2PdfBase, ILibreOffice
    {
        public LibreOffice(string apiKey) : base(apiKey)
        {
            _baseUrl = API2PDF_BASE_URL + "/libreoffice";
        }

        public LibreOffice(string overrideBaseUrl, Dictionary<string, string> httpHeaders = null) : base(overrideBaseUrl, httpHeaders)
        {
        }

        public Api2PdfResult AnyToPdf(LibreFileConversionRequest request)
        {
            string route = "/any-to-pdf";
            var response = MakeRequest(route, request);
            return response;
        }

        public async Task<Api2PdfResult> AnyToPdfAsync(LibreFileConversionRequest request)
        {
            string route = "/any-to-pdf";
            var response = await MakeRequestAsync(route, request);
            return response;
        }

        public Api2PdfResult Thumbnail(LibreFileConversionRequest request)
        {
            string route = "/thumbnail";
            var response = MakeRequest(route, request);
            return response;
        }

        public async Task<Api2PdfResult> ThumbnailAsync(LibreFileConversionRequest request)
        {
            string route = "/thumbnail";
            var response = await MakeRequestAsync(route, request);
            return response;
        }

        public Api2PdfResult HtmlToDocx(LibreFileConversionRequest request)
        {
            string route = "/html-to-docx";
            var response = MakeRequest(route, request);
            return response;
        }

        public async Task<Api2PdfResult> HtmlToDocxAsync(LibreFileConversionRequest request)
        {
            string route = "/html-to-docx";
            var response = await MakeRequestAsync(route, request);
            return response;
        }

        public Api2PdfResult HtmlToXlsx(LibreFileConversionRequest request)
        {
            string route = "/html-to-xlsx";
            var response = MakeRequest(route, request);
            return response;
        }

        public async Task<Api2PdfResult> HtmlToXlsxAsync(LibreFileConversionRequest request)
        {
            string route = "/html-to-xlsx";
            var response = await MakeRequestAsync(route, request);
            return response;
        }

        public Api2PdfResult PdfToHtml(LibreFileConversionRequest request)
        {
            string route = "/pdf-to-html";
            var response = MakeRequest(route, request);
            return response;
        }

        public async Task<Api2PdfResult> PdfToHtmlAsync(LibreFileConversionRequest request)
        {
            string route = "/pdf-to-html";
            var response = await MakeRequestAsync(route, request);
            return response;
        }
    }

    public class PdfSharp : Api2PdfBase, IPdfSharp
    {
        public PdfSharp(string apiKey) : base(apiKey)
        {
            _baseUrl = API2PDF_BASE_URL + "/pdfsharp";
        }

        public PdfSharp(string overrideBaseUrl, Dictionary<string, string> httpHeaders = null) : base(overrideBaseUrl, httpHeaders)
        {
        }

        public Api2PdfResult MergePdfs(PdfMergeRequest request)
        {
            string route = "/merge";
            var response = MakeRequest(route, request);
            return response;
        }

        public async Task<Api2PdfResult> MergePdfsAsync(PdfMergeRequest request)
        {
            string route = "/merge";
            var response = await MakeRequestAsync(route, request);
            return response;
        }

        public Api2PdfResult SetBookmarks(PdfBookmarkRequest request)
        {
            string route = "/bookmarks";
            var response = MakeRequest(route, request);
            return response;
        }

        public async Task<Api2PdfResult> SetBookmarksAsync(PdfBookmarkRequest request)
        {
            string route = "/bookmarks";
            var response = await MakeRequestAsync(route, request);
            return response;
        }

        public Api2PdfResult SetPassword(PdfPasswordRequest request)
        {
            string route = "/password";
            var response = MakeRequest(route, request);
            return response;
        }

        public async Task<Api2PdfResult> SetPasswordAsync(PdfPasswordRequest request)
        {
            string route = "/password";
            var response = await MakeRequestAsync(route, request);
            return response;
        }
    }

    public class Api2PdfUtility : Api2PdfBase, IApi2PdfUtility
    {
        public Api2PdfUtility(string apiKey) : base(apiKey)
        {
        }

        public Api2PdfUtility(string overrideBaseUrl, Dictionary<string, string> httpHeaders = null) : base(overrideBaseUrl, httpHeaders)
        {
        }

        public void Delete(string responseId)
        {
            if (_httpClient == null)
            {
                _httpClient = new HttpClient();
                if (_httpHeaders != null)
                {
                    foreach (var header in _httpHeaders)
                    {
                        _httpClient.DefaultRequestHeaders.Add(header.Key, header.Value);
                    }
                }
            }

            _httpClient.DeleteAsync(_baseUrl + $"/file/{responseId}");
        }
    }

    public class Api2PdfResult
    {
        public string FileUrl { get; set; }
        public decimal MbOut { get; set; }
        public decimal Cost { get; set; }
        public bool Success { get; set; }
        public string ResponseId { get; set; }
        public decimal Seconds { get; set; }
        public string Error { get; set; }
        
        [JsonIgnore]
        public byte[] ResultAsBytes { private get; set; }

        public void SaveFile(string localPath)
        {
            var wc = new System.Net.WebClient();
            wc.DownloadFile(FileUrl, localPath);
        }

        public byte[] GetFileBytes()
        {
            if (ResultAsBytes != null)
                return ResultAsBytes;
            var wc = new System.Net.WebClient();
            var bytes = wc.DownloadData(FileUrl);
            ResultAsBytes = bytes;
            return ResultAsBytes;
        }
    }
}
