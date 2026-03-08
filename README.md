# api2pdf.dotnet

.NET bindings for the [Api2Pdf REST API](https://v2.api2pdf.com).

Api2Pdf.com is a powerful REST API for document generation, file conversion, and automated content extraction in .NET applications. It supports HTML to PDF, URL to PDF, HTML to image, URL to image, Microsoft Office document conversion, email and image file conversion, PDF page extraction, PDF password protection, file zipping, barcode and QR code generation, markdown conversion, structured PDF data extraction, and image previews or thumbnails for PDF, office, and email files. Api2Pdf is built on proven engines and libraries including wkhtmltopdf, Headless Chrome, PdfSharp, LibreOffice, and related tools to provide reliable PDF generation, document processing, and file transformation workflows through a single API.

The package targets `netstandard2.0` and wraps the current Api2Pdf service groups for Chrome, wkhtmltopdf, LibreOffice, Markitdown, OpenDataLoader, PdfSharp, Zip, Zebra, and utility endpoints.

## Installation

```bash
dotnet add package Api2Pdf
```

```powershell
Install-Package Api2Pdf -Version 2.1.1
```

## Quick Start

Create an account at [portal.api2pdf.com](https://portal.api2pdf.com/register) to get your API key.

```csharp
using Api2Pdf;

var client = new Api2Pdf("YOUR-API-KEY");

var result = client.Chrome.HtmlToPdf(new ChromeHtmlToPdfRequest
{
    Html = "<html><body><h1>Hello, world!</h1></body></html>"
});

if (result.Success)
{
    Console.WriteLine(result.FileUrl);
}
else
{
    Console.WriteLine(result.Error);
}
```

Async usage works the same way:

```csharp
using Api2Pdf;

var client = new Api2Pdf("YOUR-API-KEY");

var result = await client.Chrome.UrlToPdfAsync(new ChromeUrlToPdfRequest
{
    Url = "https://www.api2pdf.com"
});
```

## Custom Domains

The default constructor uses `https://v2.api2pdf.com`.

If you want to route requests to a different Api2Pdf domain, use the `apiKey + baseUrl` constructor:

```csharp
using Api2Pdf;

var client = new Api2Pdf("YOUR-API-KEY", "https://your-custom-domain.api2pdf.com");
```

The package also exposes a constant for the XL cluster:

```csharp
using Api2Pdf;

var client = new Api2Pdf("YOUR-API-KEY", Api2PdfBaseUrls.V2Xl);
```

`v2-xl.api2pdf.com` provides much larger compute resources and is intended for heavier workloads, with additional cost compared to the default cluster.

## Understanding Responses

Most API methods return an `Api2PdfResult`.

```json
{
  "fileUrl": "https://link-to-file-available-for-24-hours",
  "mbOut": 0.08830547332763672,
  "cost": 0.00017251586914062501,
  "seconds": 2,
  "success": true,
  "error": null,
  "responseId": "6e46637a-650d-46d5-af0b-3d7831baccbb"
}
```

Important members:

- `Success`: whether the request succeeded.
- `Error`: error text when the request fails.
- `FileUrl`: URL of the generated file when the API returns standard JSON.
- `ResponseId`: identifier used by `Utilities.Delete(...)`.
- `GetFileBytes()`: returns bytes already in memory for binary responses, or downloads from `FileUrl` when needed.

## Common Request Features

Most request models support these properties:

- `FileName`: set the output file name.
- `Inline`: `true` to open in the browser, `false` to trigger download behavior.
- `UseCustomStorage` and `Storage`: send the output directly to your own storage target.
- `OutputBinary`: when the endpoint supports it, request binary content instead of the standard JSON payload.
- `ExtraHTTPHeaders`: forward custom headers when Api2Pdf fetches a source URL.

Example custom storage configuration:

```csharp
var request = new ChromeHtmlToPdfRequest
{
    Html = "<p>Hello World</p>",
    UseCustomStorage = true,
    Storage = new CustomStorageOptions
    {
        Method = "PUT",
        Url = "https://your-presigned-upload-url"
    }
};
```

## Chrome

### HTML or URL to PDF

```csharp
var htmlPdf = client.Chrome.HtmlToPdf(new ChromeHtmlToPdfRequest
{
    Html = "<p>Hello World</p>",
    Options = new ChromeHtmlToPdfOptions
    {
        Delay = 3000,
        DisplayHeaderFooter = true,
        HeaderTemplate = "<div style=\"font-size:12px;\">Header</div>",
        FooterTemplate = "<div style=\"font-size:12px;\">Footer</div>",
        Landscape = true,
        PreferCSSPageSize = true
    }
});

var urlPdf = client.Chrome.UrlToPdf(new ChromeUrlToPdfRequest
{
    Url = "https://www.api2pdf.com",
    ExtraHTTPHeaders = new Dictionary<string, string>
    {
        ["Authorization"] = "Bearer token-for-the-source-site"
    },
    Options = new ChromeUrlToPdfOptions
    {
        PuppeteerWaitForMethod = "WaitForNavigation",
        PuppeteerWaitForValue = "Load"
    }
});
```

For simple GET-style URL conversion, you can also call:

```csharp
var result = client.Chrome.UrlToPdf("https://www.api2pdf.com");
```

### Markdown to PDF

```csharp
var result = client.Chrome.MarkdownToPdf(new ChromeMarkdownToPdfRequest
{
    Markdown = "# Invoice\n\nThis PDF was generated from markdown."
});
```

### HTML, URL, or Markdown to image

```csharp
var htmlImage = client.Chrome.HtmlToImage(new ChromeHtmlToImageRequest
{
    Html = "<p>Hello image</p>",
    Options = new ChromeHtmlToImageOptions
    {
        FullPage = true,
        ViewPortOptions = new ViewPortOptions
        {
            Width = 1440,
            Height = 900
        }
    }
});

var urlImage = client.Chrome.UrlToImage("https://www.api2pdf.com");

var markdownImage = client.Chrome.MarkdownToImage(new ChromeMarkdownToImageRequest
{
    Markdown = "# Screenshot\n\nGenerated from markdown."
});
```

## Wkhtmltopdf

```csharp
var htmlPdf = client.Wkhtml.HtmlToPdf(new WkhtmlHtmlToPdfRequest
{
    Html = "<p>Hello World</p>",
    EnableToc = true,
    Options = new Dictionary<string, string>
    {
        ["orientation"] = "landscape",
        ["pageSize"] = "Letter"
    },
    TocOptions = new Dictionary<string, string>
    {
        ["disableDottedLines"] = "true"
    }
});

var urlPdf = client.Wkhtml.UrlToPdf(new WkhtmlUrlToPdfRequest
{
    Url = "https://www.api2pdf.com"
});
```

For the simple GET route:

```csharp
var result = client.Wkhtml.UrlToPdf("https://www.api2pdf.com");
```

## LibreOffice

Use LibreOffice endpoints for file and Office conversions.

Convert a file URL to PDF:

```csharp
var result = client.LibreOffice.AnyToPdf(new LibreFileConversionRequest
{
    Url = "https://www.api2pdf.com/wp-content/themes/api2pdf/assets/samples/sample-word-doc.docx"
});
```

Generate a thumbnail:

```csharp
var result = client.LibreOffice.Thumbnail(new LibreFileConversionRequest
{
    Url = "https://www.api2pdf.com/wp-content/themes/api2pdf/assets/samples/sample-word-doc.docx"
});
```

Convert HTML or a URL to DOCX or XLSX:

```csharp
var docx = client.LibreOffice.HtmlToDocx(new LibreHtmlOrUrlConversionRequest
{
    Html = "<html><body><h1>Hello Word</h1></body></html>"
});

var xlsx = client.LibreOffice.HtmlToXlsx(new LibreHtmlOrUrlConversionRequest
{
    Url = "https://www.api2pdf.com/wp-content/uploads/2021/01/sampleTables.html"
});
```

## Markitdown

Convert a file URL to markdown:

```csharp
var result = client.Markitdown.ConvertToMarkdown(new MarkitdownRequest
{
    Url = "https://example.com/sample.docx"
});
```

## OpenDataLoader

Extract structured content from a PDF URL:

```csharp
var json = client.OpenDataLoader.PdfToJson(new OpenDataLoaderRequest
{
    Url = "https://example.com/sample.pdf"
});

var markdown = client.OpenDataLoader.PdfToMarkdown(new OpenDataLoaderRequest
{
    Url = "https://example.com/sample.pdf"
});

var html = client.OpenDataLoader.PdfToHtml(new OpenDataLoaderRequest
{
    Url = "https://example.com/sample.pdf"
});
```

## PdfSharp

Merge PDFs:

```csharp
var result = client.PdfSharp.MergePdfs(new PdfMergeRequest
{
    Urls = new List<string>
    {
        "https://LINK-TO-PDF-1",
        "https://LINK-TO-PDF-2"
    }
});
```

Set a password:

```csharp
var result = client.PdfSharp.SetPassword(new PdfPasswordRequest
{
    Url = "https://LINK-TO-PDF",
    UserPassword = "password",
    OwnerPassword = "owner-password"
});
```

Extract a page range:

```csharp
var result = client.PdfSharp.ExtractPages(new PdfExtractPagesRequest
{
    Url = "https://LINK-TO-PDF",
    Start = 0,
    End = 2
});
```

## Zip

Create a zip from multiple files:

```csharp
var result = client.Zip.GenerateZip(new ZipRequest
{
    Files = new List<ZipFileInfo>
    {
        new ZipFileInfo
        {
            Url = "https://example.com/report.pdf",
            FileName = "docs/report.pdf"
        },
        new ZipFileInfo
        {
            Url = "https://example.com/image.png",
            FileName = "images/image.png"
        }
    },
    OutputBinary = true
});

byte[] zipBytes = result.GetFileBytes();
```

## Zebra

Generate a barcode or QR code:

```csharp
var result = client.Zebra.GenerateBarcode(new ZebraRequest
{
    Format = "QR_CODE",
    Value = "https://www.api2pdf.com",
    Width = 300,
    Height = 300,
    ShowLabel = false
});
```

## Utilities

Delete a generated file:

```csharp
var result = client.Chrome.HtmlToPdf(new ChromeHtmlToPdfRequest
{
    Html = "<p>Hello World</p>"
});

client.Utilities.Delete(result.ResponseId);
```

Check status or remaining balance:

```csharp
string status = client.Utilities.Status();
string balance = client.Utilities.Balance();
```

## Working With Files

Save to disk:

```csharp
var result = client.Chrome.HtmlToPdf(new ChromeHtmlToPdfRequest
{
    Html = "<p>Hello World</p>"
});

result.SaveFile("path-to-local-file.pdf");
```

Get bytes directly:

```csharp
var result = client.Chrome.HtmlToPdf(new ChromeHtmlToPdfRequest
{
    Html = "<p>Hello World</p>",
    OutputBinary = true
});

byte[] bytes = result.GetFileBytes();
```

## Development

The repo uses a conventional layout:

- `src/Api2Pdf` for the shipping library
- `tests/Api2Pdf.Tests` for contract and regression tests

## Resources

- [Api2Pdf documentation](https://v2.api2pdf.com)
- [Api2Pdf FAQ](https://www.api2pdf.com/faq)
