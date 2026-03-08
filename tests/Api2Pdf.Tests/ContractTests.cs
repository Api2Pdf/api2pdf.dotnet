using System.Collections.Generic;
using System.Reflection;
using Newtonsoft.Json;

namespace Api2Pdf.Tests;

using Client = global::Api2Pdf.Api2Pdf;

public class PublicContractTests
{
    [Fact]
    public void Api2Pdf_client_exposes_expected_service_types()
    {
        var client = new Client("test-api-key");

        Assert.IsType<global::Api2Pdf.Chrome>(client.Chrome);
        Assert.IsType<global::Api2Pdf.Wkhtml>(client.Wkhtml);
        Assert.IsType<global::Api2Pdf.LibreOffice>(client.LibreOffice);
        Assert.IsType<global::Api2Pdf.PdfSharp>(client.PdfSharp);
        Assert.IsType<global::Api2Pdf.Api2PdfUtility>(client.Utilities);
    }

    [Fact]
    public void Public_contract_types_and_methods_remain_available()
    {
        var assembly = typeof(Client).Assembly;

        Assert.NotNull(assembly.GetType("Api2Pdf.Api2Pdf"));
        Assert.NotNull(assembly.GetType("Api2Pdf.Chrome"));
        Assert.NotNull(assembly.GetType("Api2Pdf.Wkhtml"));
        Assert.NotNull(assembly.GetType("Api2Pdf.LibreOffice"));
        Assert.NotNull(assembly.GetType("Api2Pdf.PdfSharp"));
        Assert.NotNull(assembly.GetType("Api2Pdf.Api2PdfResult"));

        AssertHasMethod<global::Api2Pdf.Chrome>("HtmlToPdf", typeof(global::Api2Pdf.ChromeHtmlToPdfRequest));
        AssertHasMethod<global::Api2Pdf.Chrome>("UrlToPdf", typeof(global::Api2Pdf.ChromeUrlToPdfRequest));
        AssertHasMethod<global::Api2Pdf.Chrome>("HtmlToImage", typeof(global::Api2Pdf.ChromeHtmlToImageRequest));
        AssertHasMethod<global::Api2Pdf.Chrome>("UrlToImage", typeof(global::Api2Pdf.ChromeUrlToImageRequest));
        AssertHasMethod<global::Api2Pdf.Wkhtml>("HtmlToPdf", typeof(global::Api2Pdf.WkhtmlHtmlToPdfRequest));
        AssertHasMethod<global::Api2Pdf.Wkhtml>("UrlToPdf", typeof(global::Api2Pdf.WkhtmlUrlToPdfRequest));
        AssertHasMethod<global::Api2Pdf.LibreOffice>("AnyToPdf", typeof(global::Api2Pdf.LibreFileConversionRequest));
        AssertHasMethod<global::Api2Pdf.LibreOffice>("Thumbnail", typeof(global::Api2Pdf.LibreFileConversionRequest));
        AssertHasMethod<global::Api2Pdf.LibreOffice>("PdfToHtml", typeof(global::Api2Pdf.LibreFileConversionRequest));
        AssertHasMethod<global::Api2Pdf.PdfSharp>("MergePdfs", typeof(global::Api2Pdf.PdfMergeRequest));
        AssertHasMethod<global::Api2Pdf.PdfSharp>("SetBookmarks", typeof(global::Api2Pdf.PdfBookmarkRequest));
        AssertHasMethod<global::Api2Pdf.PdfSharp>("SetPassword", typeof(global::Api2Pdf.PdfPasswordRequest));
        AssertHasMethod<global::Api2Pdf.PdfSharp>("ExtractPages", typeof(global::Api2Pdf.PdfExtractPagesRequest));
    }

    [Fact]
    public void Service_constructors_keep_expected_base_urls_and_headers()
    {
        var chrome = new InspectableChrome("test-api-key");
        var wkhtml = new InspectableWkhtml("test-api-key");
        var libre = new InspectableLibreOffice("test-api-key");
        var pdfSharp = new InspectablePdfSharp("test-api-key");

        Assert.Equal("https://v2.api2pdf.com/chrome", chrome.BaseUrl);
        Assert.Equal("https://v2.api2pdf.com/wkhtml", wkhtml.BaseUrl);
        Assert.Equal("https://v2.api2pdf.com/libreoffice", libre.BaseUrl);
        Assert.Equal("https://v2.api2pdf.com/pdfsharp", pdfSharp.BaseUrl);

        Assert.Equal("test-api-key", chrome.Headers["Authorization"]);
        Assert.Equal("test-api-key", wkhtml.Headers["Authorization"]);
        Assert.Equal("test-api-key", libre.Headers["Authorization"]);
        Assert.Equal("test-api-key", pdfSharp.Headers["Authorization"]);
    }

    [Fact]
    public void Override_base_url_constructor_preserves_supplied_values()
    {
        var headers = new Dictionary<string, string>
        {
            ["Authorization"] = "override-key",
            ["binary"] = "true"
        };

        var chrome = new InspectableChrome("https://example.test/chrome", headers);

        Assert.Equal("https://example.test/chrome", chrome.BaseUrl);
        Assert.Same(headers, chrome.Headers);
    }

    [Fact]
    public void Request_models_keep_default_values()
    {
        var chromeHtml = new global::Api2Pdf.ChromeHtmlToPdfRequest();
        var chromeUrl = new global::Api2Pdf.ChromeUrlToImageRequest();
        var wkhtmlHtml = new global::Api2Pdf.WkhtmlHtmlToPdfRequest();
        var wkhtmlUrl = new global::Api2Pdf.WkhtmlUrlToPdfRequest();
        var merge = new global::Api2Pdf.PdfMergeRequest();
        var extract = new global::Api2Pdf.PdfExtractPagesRequest();

        Assert.True(chromeHtml.Inline);
        Assert.False(chromeHtml.UseCustomStorage);
        Assert.NotNull(chromeHtml.Options);
        Assert.NotNull(chromeUrl.Options);
        Assert.NotNull(wkhtmlHtml.Options);
        Assert.NotNull(wkhtmlHtml.TocOptions);
        Assert.False(wkhtmlHtml.EnableToc);
        Assert.NotNull(wkhtmlUrl.Options);
        Assert.NotNull(wkhtmlUrl.TocOptions);
        Assert.False(wkhtmlUrl.EnableToc);
        Assert.NotNull(merge.ExtraHTTPHeaders);
        Assert.Equal(0, extract.Start);
        Assert.Equal(0, extract.End);
    }

    [Fact]
    public void Chrome_pdf_options_normalize_dimensions_and_preserve_defaults()
    {
        var options = new global::Api2Pdf.ChromeHtmlToPdfOptions();

        Assert.Equal("8.27in", options.Width);
        Assert.Equal("11.69in", options.Height);
        Assert.Equal(".4in", options.MarginTop);
        Assert.Equal(".4in", options.MarginBottom);
        Assert.Equal(".4in", options.MarginLeft);
        Assert.Equal(".4in", options.MarginRight);

        options.Width = "8.5";
        options.Height = "11";
        options.MarginTop = "1";
        options.MarginBottom = "2cm";

        Assert.Equal("8.5in", options.Width);
        Assert.Equal("11in", options.Height);
        Assert.Equal("1in", options.MarginTop);
        Assert.Equal("2cm", options.MarginBottom);

        options.Width = null;
        options.MarginLeft = null;

        Assert.Equal("8.27in", options.Width);
        Assert.Equal(".4in", options.MarginLeft);
    }

    [Fact]
    public void Request_models_serialize_expected_default_shape()
    {
        var request = new global::Api2Pdf.ChromeHtmlToPdfRequest
        {
            Html = "<p>Hello World</p>"
        };

        var json = JsonConvert.SerializeObject(request);

        Assert.Contains("\"Inline\":true", json);
        Assert.Contains("\"UseCustomStorage\":false", json);
        Assert.Contains("\"Html\":\"<p>Hello World</p>\"", json);
        Assert.Contains("\"Options\":", json);
        Assert.Contains("\"Delay\":0", json);
        Assert.Contains("\"UsePrintCss\":true", json);
    }

    [Fact]
    public void Api2Pdf_result_returns_cached_bytes_without_network()
    {
        var expected = new byte[] { 1, 2, 3 };
        var result = new global::Api2Pdf.Api2PdfResult
        {
            ResultAsBytes = expected
        };

        var actual = result.GetFileBytes();

        Assert.Same(expected, actual);
    }

    private static void AssertHasMethod<T>(string methodName, params Type[] parameterTypes)
    {
        var method = typeof(T).GetMethod(methodName, BindingFlags.Instance | BindingFlags.Public, null, parameterTypes, null);
        Assert.NotNull(method);
    }

    private sealed class InspectableChrome : global::Api2Pdf.Chrome
    {
        public InspectableChrome(string apiKey) : base(apiKey)
        {
        }

        public InspectableChrome(string overrideBaseUrl, Dictionary<string, string> headers) : base(overrideBaseUrl, headers)
        {
        }

        public string BaseUrl => _baseUrl;

        public Dictionary<string, string> Headers => _httpHeaders;
    }

    private sealed class InspectableWkhtml : global::Api2Pdf.Wkhtml
    {
        public InspectableWkhtml(string apiKey) : base(apiKey)
        {
        }

        public string BaseUrl => _baseUrl;

        public Dictionary<string, string> Headers => _httpHeaders;
    }

    private sealed class InspectableLibreOffice : global::Api2Pdf.LibreOffice
    {
        public InspectableLibreOffice(string apiKey) : base(apiKey)
        {
        }

        public string BaseUrl => _baseUrl;

        public Dictionary<string, string> Headers => _httpHeaders;
    }

    private sealed class InspectablePdfSharp : global::Api2Pdf.PdfSharp
    {
        public InspectablePdfSharp(string apiKey) : base(apiKey)
        {
        }

        public string BaseUrl => _baseUrl;

        public Dictionary<string, string> Headers => _httpHeaders;
    }
}