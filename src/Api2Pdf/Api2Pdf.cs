using Newtonsoft.Json;
using System;
using System.Collections.Generic;
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
        Api2PdfResult UrlToPdf(string url, bool? outputBinary = null);
        Task<Api2PdfResult> UrlToPdfAsync(string url, bool? outputBinary = null);
        Api2PdfResult MarkdownToPdf(ChromeMarkdownToPdfRequest request);
        Task<Api2PdfResult> MarkdownToPdfAsync(ChromeMarkdownToPdfRequest request);
        Api2PdfResult HtmlToImage(ChromeHtmlToImageRequest request);
        Task<Api2PdfResult> HtmlToImageAsync(ChromeHtmlToImageRequest request);
        Api2PdfResult UrlToImage(ChromeUrlToImageRequest request);
        Task<Api2PdfResult> UrlToImageAsync(ChromeUrlToImageRequest request);
        Api2PdfResult UrlToImage(string url, bool? outputBinary = null);
        Task<Api2PdfResult> UrlToImageAsync(string url, bool? outputBinary = null);
        Api2PdfResult MarkdownToImage(ChromeMarkdownToImageRequest request);
        Task<Api2PdfResult> MarkdownToImageAsync(ChromeMarkdownToImageRequest request);
    }

    public interface IWkhtml
    {
        Api2PdfResult HtmlToPdf(WkhtmlHtmlToPdfRequest request);
        Task<Api2PdfResult> HtmlToPdfAsync(WkhtmlHtmlToPdfRequest request);
        Api2PdfResult UrlToPdf(WkhtmlUrlToPdfRequest request);
        Task<Api2PdfResult> UrlToPdfAsync(WkhtmlUrlToPdfRequest request);
        Api2PdfResult UrlToPdf(string url, bool? outputBinary = null);
        Task<Api2PdfResult> UrlToPdfAsync(string url, bool? outputBinary = null);
    }

    public interface ILibreOffice
    {
        Api2PdfResult AnyToPdf(LibreFileConversionRequest request);
        Task<Api2PdfResult> AnyToPdfAsync(LibreFileConversionRequest request);
        Api2PdfResult Thumbnail(LibreFileConversionRequest request);
        Task<Api2PdfResult> ThumbnailAsync(LibreFileConversionRequest request);
        Api2PdfResult HtmlToDocx(LibreHtmlOrUrlConversionRequest request);
        Task<Api2PdfResult> HtmlToDocxAsync(LibreHtmlOrUrlConversionRequest request);
        Api2PdfResult HtmlToXlsx(LibreHtmlOrUrlConversionRequest request);
        Task<Api2PdfResult> HtmlToXlsxAsync(LibreHtmlOrUrlConversionRequest request);
    }

    public interface IPdfSharp
    {
        Api2PdfResult MergePdfs(PdfMergeRequest request);
        Task<Api2PdfResult> MergePdfsAsync(PdfMergeRequest request);
        Api2PdfResult SetPassword(PdfPasswordRequest request);
        Task<Api2PdfResult> SetPasswordAsync(PdfPasswordRequest request);
        Api2PdfResult ExtractPages(PdfExtractPagesRequest request);
        Task<Api2PdfResult> ExtractPagesAsync(PdfExtractPagesRequest request);
    }

    public interface IMarkitdown
    {
        Api2PdfResult ConvertToMarkdown(MarkitdownRequest request);
        Task<Api2PdfResult> ConvertToMarkdownAsync(MarkitdownRequest request);
    }

    public interface IOpenDataLoader
    {
        Api2PdfResult PdfToJson(OpenDataLoaderRequest request);
        Task<Api2PdfResult> PdfToJsonAsync(OpenDataLoaderRequest request);
        Api2PdfResult PdfToMarkdown(OpenDataLoaderRequest request);
        Task<Api2PdfResult> PdfToMarkdownAsync(OpenDataLoaderRequest request);
        Api2PdfResult PdfToHtml(OpenDataLoaderRequest request);
        Task<Api2PdfResult> PdfToHtmlAsync(OpenDataLoaderRequest request);
    }

    public interface IZip
    {
        Api2PdfResult GenerateZip(ZipRequest request);
        Task<Api2PdfResult> GenerateZipAsync(ZipRequest request);
    }

    public interface IZebra
    {
        Api2PdfResult GenerateBarcode(ZebraRequest request);
        Task<Api2PdfResult> GenerateBarcodeAsync(ZebraRequest request);
    }

    public interface IApi2PdfUtility
    {
        void Delete(string responseId);
        string Status();
        Task<string> StatusAsync();
        string Balance();
        Task<string> BalanceAsync();
    }

    public static class Api2PdfBaseUrls
    {
        public const string Default = "https://v2.api2pdf.com";
        public const string V2Xl = "https://v2-xl.api2pdf.com";
    }

    public class Api2Pdf
    {
        public IChrome Chrome { get; private set; }
        public IWkhtml Wkhtml { get; private set; }
        public ILibreOffice LibreOffice { get; private set; }
        public IPdfSharp PdfSharp { get; private set; }
        public IMarkitdown Markitdown { get; private set; }
        public IOpenDataLoader OpenDataLoader { get; private set; }
        public IZip Zip { get; private set; }
        public IZebra Zebra { get; private set; }
        public IApi2PdfUtility Utilities { get; private set; }

        public Api2Pdf(string apiKey)
        {
            Chrome = new Chrome(apiKey);
            Wkhtml = new Wkhtml(apiKey);
            LibreOffice = new LibreOffice(apiKey);
            PdfSharp = new PdfSharp(apiKey);
            Markitdown = new Markitdown(apiKey);
            OpenDataLoader = new OpenDataLoader(apiKey);
            Zip = new Zip(apiKey);
            Zebra = new Zebra(apiKey);
            Utilities = new Api2PdfUtility(apiKey);
        }

        public Api2Pdf(string apiKey, string baseUrl, Dictionary<string, string> httpHeaders = null)
        {
            Chrome = new Chrome(apiKey, baseUrl, httpHeaders);
            Wkhtml = new Wkhtml(apiKey, baseUrl, httpHeaders);
            LibreOffice = new LibreOffice(apiKey, baseUrl, httpHeaders);
            PdfSharp = new PdfSharp(apiKey, baseUrl, httpHeaders);
            Markitdown = new Markitdown(apiKey, baseUrl, httpHeaders);
            OpenDataLoader = new OpenDataLoader(apiKey, baseUrl, httpHeaders);
            Zip = new Zip(apiKey, baseUrl, httpHeaders);
            Zebra = new Zebra(apiKey, baseUrl, httpHeaders);
            Utilities = new Api2PdfUtility(apiKey, baseUrl, httpHeaders);
        }

        public Api2Pdf(string overrideBaseUrl, Dictionary<string, string> httpHeaders = null)
        {
            Chrome = new Chrome(overrideBaseUrl, httpHeaders);
            Wkhtml = new Wkhtml(overrideBaseUrl, httpHeaders);
            LibreOffice = new LibreOffice(overrideBaseUrl, httpHeaders);
            PdfSharp = new PdfSharp(overrideBaseUrl, httpHeaders);
            Markitdown = new Markitdown(overrideBaseUrl, httpHeaders);
            OpenDataLoader = new OpenDataLoader(overrideBaseUrl, httpHeaders);
            Zip = new Zip(overrideBaseUrl, httpHeaders);
            Zebra = new Zebra(overrideBaseUrl, httpHeaders);
            Utilities = new Api2PdfUtility(overrideBaseUrl, httpHeaders);
        }
    }

    public abstract class Api2PdfBase
    {
        protected string _baseUrl;
        protected Dictionary<string, string> _httpHeaders;
        protected readonly HttpClient _httpClient;

        public Api2PdfBase(string apiKey) : this(apiKey, Api2PdfBaseUrls.Default, null)
        {
        }

        public Api2PdfBase(string apiKey, string baseUrl, Dictionary<string, string> httpHeaders = null)
        {
            _httpClient = new HttpClient();
            _httpHeaders = BuildHeaders(apiKey, httpHeaders);
            _baseUrl = NormalizeBaseUrl(baseUrl);
        }

        public Api2PdfBase(string overrideBaseUrl, Dictionary<string, string> httpHeaders = null)
        {
            _httpClient = new HttpClient();
            _httpHeaders = httpHeaders ?? new Dictionary<string, string>();
            _baseUrl = NormalizeBaseUrl(overrideBaseUrl);
        }

        protected static string BuildServiceBaseUrl(string baseUrl, string servicePath)
        {
            return CombineUrl(NormalizeBaseUrl(baseUrl), servicePath);
        }

        protected Api2PdfResult SendApi2PdfRequest(HttpMethod method, string route, object body = null, IDictionary<string, string> queryParameters = null, bool? outputBinary = null)
        {
            return SendApi2PdfRequestAsync(method, route, body, queryParameters, outputBinary).GetAwaiter().GetResult();
        }

        protected async Task<Api2PdfResult> SendApi2PdfRequestAsync(HttpMethod method, string route, object body = null, IDictionary<string, string> queryParameters = null, bool? outputBinary = null)
        {
            using (var request = CreateRequestMessage(method, route, body, queryParameters, outputBinary))
            using (var response = await _httpClient.SendAsync(request).ConfigureAwait(false))
            {
                return await ParseApi2PdfResultAsync(response).ConfigureAwait(false);
            }
        }

        protected string SendStringRequest(HttpMethod method, string route, IDictionary<string, string> queryParameters = null)
        {
            return SendStringRequestAsync(method, route, queryParameters).GetAwaiter().GetResult();
        }

        protected async Task<string> SendStringRequestAsync(HttpMethod method, string route, IDictionary<string, string> queryParameters = null)
        {
            using (var request = CreateRequestMessage(method, route, null, queryParameters, null))
            using (var response = await _httpClient.SendAsync(request).ConfigureAwait(false))
            {
                return await response.Content.ReadAsStringAsync().ConfigureAwait(false);
            }
        }

        protected void SendDeleteRequest(string route)
        {
            SendDeleteRequestAsync(route).GetAwaiter().GetResult();
        }

        protected async Task SendDeleteRequestAsync(string route)
        {
            using (var request = CreateRequestMessage(HttpMethod.Delete, route, null, null, null))
            {
                await _httpClient.SendAsync(request).ConfigureAwait(false);
            }
        }

        private static Dictionary<string, string> BuildHeaders(string apiKey, Dictionary<string, string> httpHeaders)
        {
            var headers = httpHeaders != null
                ? new Dictionary<string, string>(httpHeaders)
                : new Dictionary<string, string>();

            headers["Authorization"] = apiKey;
            return headers;
        }

        private HttpRequestMessage CreateRequestMessage(HttpMethod method, string route, object body, IDictionary<string, string> queryParameters, bool? outputBinary)
        {
            var request = new HttpRequestMessage(method, BuildRequestUri(route, queryParameters, outputBinary));

            foreach (var header in _httpHeaders)
            {
                request.Headers.TryAddWithoutValidation(header.Key, header.Value);
            }

            if (body != null)
            {
                var payload = JsonConvert.SerializeObject(body);
                request.Content = new StringContent(payload, Encoding.UTF8, "application/json");
            }

            return request;
        }

        private Uri BuildRequestUri(string route, IDictionary<string, string> queryParameters, bool? outputBinary)
        {
            var baseUrl = string.IsNullOrWhiteSpace(route)
                ? _baseUrl
                : CombineUrl(_baseUrl, route);

            var queryParts = new List<string>();

            if (queryParameters != null)
            {
                foreach (var queryParameter in queryParameters)
                {
                    if (queryParameter.Value == null)
                    {
                        continue;
                    }

                    queryParts.Add($"{Uri.EscapeDataString(queryParameter.Key)}={Uri.EscapeDataString(queryParameter.Value)}");
                }
            }

            if (outputBinary.HasValue)
            {
                queryParts.Add($"outputBinary={(outputBinary.Value ? "true" : "false")}");
            }

            if (queryParts.Count == 0)
            {
                return new Uri(baseUrl);
            }

            return new Uri($"{baseUrl}?{string.Join("&", queryParts)}");
        }

        private async Task<Api2PdfResult> ParseApi2PdfResultAsync(HttpResponseMessage response)
        {
            var payload = await response.Content.ReadAsByteArrayAsync().ConfigureAwait(false);
            if (payload == null || payload.Length == 0)
            {
                return new Api2PdfResult
                {
                    Success = response.IsSuccessStatusCode,
                    Error = response.IsSuccessStatusCode ? null : response.ReasonPhrase
                };
            }

            if (LooksLikeJson(response, payload))
            {
                var text = Encoding.UTF8.GetString(payload);

                try
                {
                    var result = JsonConvert.DeserializeObject<Api2PdfResult>(text);
                    if (result != null)
                    {
                        return result;
                    }
                }
                catch
                {
                    if (!response.IsSuccessStatusCode)
                    {
                        return new Api2PdfResult
                        {
                            Success = false,
                            Error = text
                        };
                    }
                }
            }

            return new Api2PdfResult
            {
                MbOut = payload.Length / 1000m,
                ResultAsBytes = payload,
                Success = response.IsSuccessStatusCode,
                Error = response.IsSuccessStatusCode ? null : response.ReasonPhrase
            };
        }

        private static bool LooksLikeJson(HttpResponseMessage response, byte[] payload)
        {
            var mediaType = response.Content.Headers.ContentType?.MediaType;
            if (!string.IsNullOrWhiteSpace(mediaType) &&
                mediaType.IndexOf("json", StringComparison.OrdinalIgnoreCase) >= 0)
            {
                return true;
            }

            var text = Encoding.UTF8.GetString(payload).TrimStart();
            return text.StartsWith("{", StringComparison.Ordinal) || text.StartsWith("[", StringComparison.Ordinal);
        }

        private static string NormalizeBaseUrl(string baseUrl)
        {
            var normalized = string.IsNullOrWhiteSpace(baseUrl) ? Api2PdfBaseUrls.Default : baseUrl.Trim();
            return normalized.TrimEnd('/');
        }

        private static string CombineUrl(string baseUrl, string route)
        {
            if (string.IsNullOrWhiteSpace(route))
            {
                return baseUrl;
            }

            return $"{baseUrl.TrimEnd('/')}/{route.TrimStart('/')}";
        }
    }

    public class Chrome : Api2PdfBase, IChrome
    {
        public Chrome(string apiKey) : base(apiKey)
        {
            _baseUrl = BuildServiceBaseUrl(Api2PdfBaseUrls.Default, "/chrome");
        }

        public Chrome(string apiKey, string baseUrl, Dictionary<string, string> httpHeaders = null) : base(apiKey, baseUrl, httpHeaders)
        {
            _baseUrl = BuildServiceBaseUrl(baseUrl, "/chrome");
        }

        public Chrome(string overrideBaseUrl, Dictionary<string, string> httpHeaders = null) : base(overrideBaseUrl, httpHeaders)
        {
        }

        public Api2PdfResult HtmlToImage(ChromeHtmlToImageRequest request)
        {
            return SendApi2PdfRequest(HttpMethod.Post, "/image/html", request, outputBinary: request.OutputBinary);
        }

        public Task<Api2PdfResult> HtmlToImageAsync(ChromeHtmlToImageRequest request)
        {
            return SendApi2PdfRequestAsync(HttpMethod.Post, "/image/html", request, outputBinary: request.OutputBinary);
        }

        public Api2PdfResult HtmlToPdf(ChromeHtmlToPdfRequest request)
        {
            return SendApi2PdfRequest(HttpMethod.Post, "/pdf/html", request, outputBinary: request.OutputBinary);
        }

        public Task<Api2PdfResult> HtmlToPdfAsync(ChromeHtmlToPdfRequest request)
        {
            return SendApi2PdfRequestAsync(HttpMethod.Post, "/pdf/html", request, outputBinary: request.OutputBinary);
        }

        public Api2PdfResult UrlToPdf(ChromeUrlToPdfRequest request)
        {
            return SendApi2PdfRequest(HttpMethod.Post, "/pdf/url", request, outputBinary: request.OutputBinary);
        }

        public Task<Api2PdfResult> UrlToPdfAsync(ChromeUrlToPdfRequest request)
        {
            return SendApi2PdfRequestAsync(HttpMethod.Post, "/pdf/url", request, outputBinary: request.OutputBinary);
        }

        public Api2PdfResult UrlToPdf(string url, bool? outputBinary = null)
        {
            return SendApi2PdfRequest(HttpMethod.Get, "/pdf/url", queryParameters: new Dictionary<string, string>
            {
                ["url"] = url
            }, outputBinary: outputBinary);
        }

        public Task<Api2PdfResult> UrlToPdfAsync(string url, bool? outputBinary = null)
        {
            return SendApi2PdfRequestAsync(HttpMethod.Get, "/pdf/url", queryParameters: new Dictionary<string, string>
            {
                ["url"] = url
            }, outputBinary: outputBinary);
        }

        public Api2PdfResult MarkdownToPdf(ChromeMarkdownToPdfRequest request)
        {
            return SendApi2PdfRequest(HttpMethod.Post, "/pdf/markdown", request, outputBinary: request.OutputBinary);
        }

        public Task<Api2PdfResult> MarkdownToPdfAsync(ChromeMarkdownToPdfRequest request)
        {
            return SendApi2PdfRequestAsync(HttpMethod.Post, "/pdf/markdown", request, outputBinary: request.OutputBinary);
        }

        public Api2PdfResult UrlToImage(ChromeUrlToImageRequest request)
        {
            return SendApi2PdfRequest(HttpMethod.Post, "/image/url", request, outputBinary: request.OutputBinary);
        }

        public Task<Api2PdfResult> UrlToImageAsync(ChromeUrlToImageRequest request)
        {
            return SendApi2PdfRequestAsync(HttpMethod.Post, "/image/url", request, outputBinary: request.OutputBinary);
        }

        public Api2PdfResult UrlToImage(string url, bool? outputBinary = null)
        {
            return SendApi2PdfRequest(HttpMethod.Get, "/image/url", queryParameters: new Dictionary<string, string>
            {
                ["url"] = url
            }, outputBinary: outputBinary);
        }

        public Task<Api2PdfResult> UrlToImageAsync(string url, bool? outputBinary = null)
        {
            return SendApi2PdfRequestAsync(HttpMethod.Get, "/image/url", queryParameters: new Dictionary<string, string>
            {
                ["url"] = url
            }, outputBinary: outputBinary);
        }

        public Api2PdfResult MarkdownToImage(ChromeMarkdownToImageRequest request)
        {
            return SendApi2PdfRequest(HttpMethod.Post, "/image/markdown", request, outputBinary: request.OutputBinary);
        }

        public Task<Api2PdfResult> MarkdownToImageAsync(ChromeMarkdownToImageRequest request)
        {
            return SendApi2PdfRequestAsync(HttpMethod.Post, "/image/markdown", request, outputBinary: request.OutputBinary);
        }
    }

    public class Wkhtml : Api2PdfBase, IWkhtml
    {
        public Wkhtml(string apiKey) : base(apiKey)
        {
            _baseUrl = BuildServiceBaseUrl(Api2PdfBaseUrls.Default, "/wkhtml");
        }

        public Wkhtml(string apiKey, string baseUrl, Dictionary<string, string> httpHeaders = null) : base(apiKey, baseUrl, httpHeaders)
        {
            _baseUrl = BuildServiceBaseUrl(baseUrl, "/wkhtml");
        }

        public Wkhtml(string overrideBaseUrl, Dictionary<string, string> httpHeaders = null) : base(overrideBaseUrl, httpHeaders)
        {
        }

        public Api2PdfResult HtmlToPdf(WkhtmlHtmlToPdfRequest request)
        {
            return SendApi2PdfRequest(HttpMethod.Post, "/pdf/html", request, outputBinary: request.OutputBinary);
        }

        public Task<Api2PdfResult> HtmlToPdfAsync(WkhtmlHtmlToPdfRequest request)
        {
            return SendApi2PdfRequestAsync(HttpMethod.Post, "/pdf/html", request, outputBinary: request.OutputBinary);
        }

        public Api2PdfResult UrlToPdf(WkhtmlUrlToPdfRequest request)
        {
            return SendApi2PdfRequest(HttpMethod.Post, "/pdf/url", request, outputBinary: request.OutputBinary);
        }

        public Task<Api2PdfResult> UrlToPdfAsync(WkhtmlUrlToPdfRequest request)
        {
            return SendApi2PdfRequestAsync(HttpMethod.Post, "/pdf/url", request, outputBinary: request.OutputBinary);
        }

        public Api2PdfResult UrlToPdf(string url, bool? outputBinary = null)
        {
            return SendApi2PdfRequest(HttpMethod.Get, "/pdf/url", queryParameters: new Dictionary<string, string>
            {
                ["url"] = url
            }, outputBinary: outputBinary);
        }

        public Task<Api2PdfResult> UrlToPdfAsync(string url, bool? outputBinary = null)
        {
            return SendApi2PdfRequestAsync(HttpMethod.Get, "/pdf/url", queryParameters: new Dictionary<string, string>
            {
                ["url"] = url
            }, outputBinary: outputBinary);
        }
    }

    public class LibreOffice : Api2PdfBase, ILibreOffice
    {
        public LibreOffice(string apiKey) : base(apiKey)
        {
            _baseUrl = BuildServiceBaseUrl(Api2PdfBaseUrls.Default, "/libreoffice");
        }

        public LibreOffice(string apiKey, string baseUrl, Dictionary<string, string> httpHeaders = null) : base(apiKey, baseUrl, httpHeaders)
        {
            _baseUrl = BuildServiceBaseUrl(baseUrl, "/libreoffice");
        }

        public LibreOffice(string overrideBaseUrl, Dictionary<string, string> httpHeaders = null) : base(overrideBaseUrl, httpHeaders)
        {
        }

        public Api2PdfResult AnyToPdf(LibreFileConversionRequest request)
        {
            return SendApi2PdfRequest(HttpMethod.Post, "/any-to-pdf", request, outputBinary: request.OutputBinary);
        }

        public Task<Api2PdfResult> AnyToPdfAsync(LibreFileConversionRequest request)
        {
            return SendApi2PdfRequestAsync(HttpMethod.Post, "/any-to-pdf", request, outputBinary: request.OutputBinary);
        }

        public Api2PdfResult Thumbnail(LibreFileConversionRequest request)
        {
            return SendApi2PdfRequest(HttpMethod.Post, "/thumbnail", request, outputBinary: request.OutputBinary);
        }

        public Task<Api2PdfResult> ThumbnailAsync(LibreFileConversionRequest request)
        {
            return SendApi2PdfRequestAsync(HttpMethod.Post, "/thumbnail", request, outputBinary: request.OutputBinary);
        }

        public Api2PdfResult HtmlToDocx(LibreHtmlOrUrlConversionRequest request)
        {
            return SendApi2PdfRequest(HttpMethod.Post, "/html-to-docx", request, outputBinary: request.OutputBinary);
        }

        public Task<Api2PdfResult> HtmlToDocxAsync(LibreHtmlOrUrlConversionRequest request)
        {
            return SendApi2PdfRequestAsync(HttpMethod.Post, "/html-to-docx", request, outputBinary: request.OutputBinary);
        }

        public Api2PdfResult HtmlToXlsx(LibreHtmlOrUrlConversionRequest request)
        {
            return SendApi2PdfRequest(HttpMethod.Post, "/html-to-xlsx", request, outputBinary: request.OutputBinary);
        }

        public Task<Api2PdfResult> HtmlToXlsxAsync(LibreHtmlOrUrlConversionRequest request)
        {
            return SendApi2PdfRequestAsync(HttpMethod.Post, "/html-to-xlsx", request, outputBinary: request.OutputBinary);
        }
    }

    public class Markitdown : Api2PdfBase, IMarkitdown
    {
        public Markitdown(string apiKey) : base(apiKey)
        {
            _baseUrl = BuildServiceBaseUrl(Api2PdfBaseUrls.Default, "/markitdown");
        }

        public Markitdown(string apiKey, string baseUrl, Dictionary<string, string> httpHeaders = null) : base(apiKey, baseUrl, httpHeaders)
        {
            _baseUrl = BuildServiceBaseUrl(baseUrl, "/markitdown");
        }

        public Markitdown(string overrideBaseUrl, Dictionary<string, string> httpHeaders = null) : base(overrideBaseUrl, httpHeaders)
        {
        }

        public Api2PdfResult ConvertToMarkdown(MarkitdownRequest request)
        {
            return SendApi2PdfRequest(HttpMethod.Post, string.Empty, request, outputBinary: request.OutputBinary);
        }

        public Task<Api2PdfResult> ConvertToMarkdownAsync(MarkitdownRequest request)
        {
            return SendApi2PdfRequestAsync(HttpMethod.Post, string.Empty, request, outputBinary: request.OutputBinary);
        }
    }

    public class OpenDataLoader : Api2PdfBase, IOpenDataLoader
    {
        public OpenDataLoader(string apiKey) : base(apiKey)
        {
            _baseUrl = BuildServiceBaseUrl(Api2PdfBaseUrls.Default, "/opendataloader");
        }

        public OpenDataLoader(string apiKey, string baseUrl, Dictionary<string, string> httpHeaders = null) : base(apiKey, baseUrl, httpHeaders)
        {
            _baseUrl = BuildServiceBaseUrl(baseUrl, "/opendataloader");
        }

        public OpenDataLoader(string overrideBaseUrl, Dictionary<string, string> httpHeaders = null) : base(overrideBaseUrl, httpHeaders)
        {
        }

        public Api2PdfResult PdfToJson(OpenDataLoaderRequest request)
        {
            return SendApi2PdfRequest(HttpMethod.Post, "/json", request, outputBinary: request.OutputBinary);
        }

        public Task<Api2PdfResult> PdfToJsonAsync(OpenDataLoaderRequest request)
        {
            return SendApi2PdfRequestAsync(HttpMethod.Post, "/json", request, outputBinary: request.OutputBinary);
        }

        public Api2PdfResult PdfToMarkdown(OpenDataLoaderRequest request)
        {
            return SendApi2PdfRequest(HttpMethod.Post, "/markdown", request, outputBinary: request.OutputBinary);
        }

        public Task<Api2PdfResult> PdfToMarkdownAsync(OpenDataLoaderRequest request)
        {
            return SendApi2PdfRequestAsync(HttpMethod.Post, "/markdown", request, outputBinary: request.OutputBinary);
        }

        public Api2PdfResult PdfToHtml(OpenDataLoaderRequest request)
        {
            return SendApi2PdfRequest(HttpMethod.Post, "/html", request, outputBinary: request.OutputBinary);
        }

        public Task<Api2PdfResult> PdfToHtmlAsync(OpenDataLoaderRequest request)
        {
            return SendApi2PdfRequestAsync(HttpMethod.Post, "/html", request, outputBinary: request.OutputBinary);
        }
    }

    public class PdfSharp : Api2PdfBase, IPdfSharp
    {
        public PdfSharp(string apiKey) : base(apiKey)
        {
            _baseUrl = BuildServiceBaseUrl(Api2PdfBaseUrls.Default, "/pdfsharp");
        }

        public PdfSharp(string apiKey, string baseUrl, Dictionary<string, string> httpHeaders = null) : base(apiKey, baseUrl, httpHeaders)
        {
            _baseUrl = BuildServiceBaseUrl(baseUrl, "/pdfsharp");
        }

        public PdfSharp(string overrideBaseUrl, Dictionary<string, string> httpHeaders = null) : base(overrideBaseUrl, httpHeaders)
        {
        }

        public Api2PdfResult MergePdfs(PdfMergeRequest request)
        {
            return SendApi2PdfRequest(HttpMethod.Post, "/merge", request, outputBinary: request.OutputBinary);
        }

        public Task<Api2PdfResult> MergePdfsAsync(PdfMergeRequest request)
        {
            return SendApi2PdfRequestAsync(HttpMethod.Post, "/merge", request, outputBinary: request.OutputBinary);
        }

        public Api2PdfResult SetPassword(PdfPasswordRequest request)
        {
            return SendApi2PdfRequest(HttpMethod.Post, "/password", request, outputBinary: request.OutputBinary);
        }

        public Task<Api2PdfResult> SetPasswordAsync(PdfPasswordRequest request)
        {
            return SendApi2PdfRequestAsync(HttpMethod.Post, "/password", request, outputBinary: request.OutputBinary);
        }

        public Api2PdfResult ExtractPages(PdfExtractPagesRequest request)
        {
            return SendApi2PdfRequest(HttpMethod.Post, "/extract-pages", request, outputBinary: request.OutputBinary);
        }

        public Task<Api2PdfResult> ExtractPagesAsync(PdfExtractPagesRequest request)
        {
            return SendApi2PdfRequestAsync(HttpMethod.Post, "/extract-pages", request, outputBinary: request.OutputBinary);
        }
    }

    public class Zip : Api2PdfBase, IZip
    {
        public Zip(string apiKey) : base(apiKey)
        {
            _baseUrl = BuildServiceBaseUrl(Api2PdfBaseUrls.Default, "/zip");
        }

        public Zip(string apiKey, string baseUrl, Dictionary<string, string> httpHeaders = null) : base(apiKey, baseUrl, httpHeaders)
        {
            _baseUrl = BuildServiceBaseUrl(baseUrl, "/zip");
        }

        public Zip(string overrideBaseUrl, Dictionary<string, string> httpHeaders = null) : base(overrideBaseUrl, httpHeaders)
        {
        }

        public Api2PdfResult GenerateZip(ZipRequest request)
        {
            return SendApi2PdfRequest(HttpMethod.Post, string.Empty, request, outputBinary: request.OutputBinary);
        }

        public Task<Api2PdfResult> GenerateZipAsync(ZipRequest request)
        {
            return SendApi2PdfRequestAsync(HttpMethod.Post, string.Empty, request, outputBinary: request.OutputBinary);
        }
    }

    public class Zebra : Api2PdfBase, IZebra
    {
        public Zebra(string apiKey) : base(apiKey)
        {
            _baseUrl = BuildServiceBaseUrl(Api2PdfBaseUrls.Default, "/zebra");
        }

        public Zebra(string apiKey, string baseUrl, Dictionary<string, string> httpHeaders = null) : base(apiKey, baseUrl, httpHeaders)
        {
            _baseUrl = BuildServiceBaseUrl(baseUrl, "/zebra");
        }

        public Zebra(string overrideBaseUrl, Dictionary<string, string> httpHeaders = null) : base(overrideBaseUrl, httpHeaders)
        {
        }

        public Api2PdfResult GenerateBarcode(ZebraRequest request)
        {
            return SendApi2PdfRequest(HttpMethod.Get, string.Empty, queryParameters: BuildQueryParameters(request), outputBinary: request.OutputBinary);
        }

        public Task<Api2PdfResult> GenerateBarcodeAsync(ZebraRequest request)
        {
            return SendApi2PdfRequestAsync(HttpMethod.Get, string.Empty, queryParameters: BuildQueryParameters(request), outputBinary: request.OutputBinary);
        }

        private static IDictionary<string, string> BuildQueryParameters(ZebraRequest request)
        {
            return new Dictionary<string, string>
            {
                ["format"] = request.Format,
                ["value"] = request.Value,
                ["height"] = request.Height.ToString(),
                ["width"] = request.Width.ToString(),
                ["showlabel"] = request.ShowLabel ? "true" : "false"
            };
        }
    }

    public class Api2PdfUtility : Api2PdfBase, IApi2PdfUtility
    {
        public Api2PdfUtility(string apiKey) : base(apiKey)
        {
        }

        public Api2PdfUtility(string apiKey, string baseUrl, Dictionary<string, string> httpHeaders = null) : base(apiKey, baseUrl, httpHeaders)
        {
        }

        public Api2PdfUtility(string overrideBaseUrl, Dictionary<string, string> httpHeaders = null) : base(overrideBaseUrl, httpHeaders)
        {
        }

        public void Delete(string responseId)
        {
            SendDeleteRequest($"/file/{responseId}");
        }

        public string Status()
        {
            return SendStringRequest(HttpMethod.Get, "/status");
        }

        public Task<string> StatusAsync()
        {
            return SendStringRequestAsync(HttpMethod.Get, "/status");
        }

        public string Balance()
        {
            return SendStringRequest(HttpMethod.Get, "/balance");
        }

        public Task<string> BalanceAsync()
        {
            return SendStringRequestAsync(HttpMethod.Get, "/balance");
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
