# api2pdf.dotnet

.NET bindings for the [Api2Pdf REST API](https://v2.api2pdf.com).

Api2Pdf.com is a REST API for generating PDFs, images, and Office document conversions from HTML, URLs, and existing files. This client wraps the Api2Pdf endpoints for Chrome, wkhtmltopdf, LibreOffice, and PdfSharp-backed operations.

## Installation

```bash
dotnet add package Api2Pdf
```

```powershell
Install-Package Api2Pdf -Version 2.1.1
```

## Getting Started

Create an account at [portal.api2pdf.com](https://portal.api2pdf.com/register) to get your API key.

```csharp
using Api2Pdf;

var client = new Api2Pdf("YOUR-API-KEY");

var request = new ChromeHtmlToPdfRequest
{
    Html = "<p>Hello World</p>"
};

var result = client.Chrome.HtmlToPdf(request);
Console.WriteLine(result.FileUrl);
```

Every API call returns an `Api2PdfResult`.

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

Common request options:

- `Inline`: `true` to open in the browser, `false` to trigger download behavior.
- `FileName`: optionally set the output filename, such as `sample.pdf`.
- `UseCustomStorage` and `Storage`: optionally upload output to your own storage target.

## Supported Resources

- `Wkhtml`
- `Chrome`
- `LibreOffice`
- `PdfSharp`
- `Utilities`

## Wkhtmltopdf

Convert HTML to PDF:

```csharp
var request = new WkhtmlHtmlToPdfRequest
{
    Html = "<p>Hello World</p>"
};

var result = client.Wkhtml.HtmlToPdf(request);
```

Convert HTML to PDF with advanced wkhtmltopdf settings:

```csharp
var request = new WkhtmlHtmlToPdfRequest
{
    Html = "<p>Hello World</p>",
    FileName = "sample.pdf",
    Inline = false,
    Options = new Dictionary<string, string>
    {
        ["orientation"] = "landscape",
        ["pageSize"] = "Letter"
    }
};

var result = client.Wkhtml.HtmlToPdf(request);
```

Convert URL to PDF:

```csharp
var request = new WkhtmlUrlToPdfRequest
{
    Url = "https://www.api2pdf.com"
};

var result = client.Wkhtml.UrlToPdf(request);
```

More wkhtmltopdf options are documented [here](https://www.api2pdf.com/documentation/advanced-options-wkhtmltopdf/).

## Headless Chrome

Convert HTML to PDF:

```csharp
var request = new ChromeHtmlToPdfRequest
{
    Html = "<p>Hello World</p>"
};

var result = client.Chrome.HtmlToPdf(request);
```

Convert URL to PDF with advanced Chrome settings:

```csharp
var request = new ChromeUrlToPdfRequest
{
    Url = "https://www.api2pdf.com",
    FileName = "sample.pdf",
    Inline = false,
    Options = new ChromeUrlToPdfOptions
    {
        Delay = 3000,
        DisplayHeaderFooter = true,
        Landscape = true,
        HeaderTemplate = "<div style=\"font-size:12px;\">Header Content Here</div>"
    }
};

var result = client.Chrome.UrlToPdf(request);
```

Convert HTML or URL to image:

```csharp
var htmlImage = client.Chrome.HtmlToImage(new ChromeHtmlToImageRequest
{
    Html = "<p>Hello World</p>"
});

var urlImage = client.Chrome.UrlToImage(new ChromeUrlToImageRequest
{
    Url = "https://www.api2pdf.com"
});
```

More Chrome options are documented [here](https://www.api2pdf.com/documentation/advanced-options-headless-chrome/).

## LibreOffice

LibreOffice endpoints convert Office documents, images, HTML, email files, and PDFs. These endpoints consume a file from a URL you provide.

Convert a file to PDF:

```csharp
var request = new LibreFileConversionRequest
{
    Url = "https://www.api2pdf.com/wp-content/themes/api2pdf/assets/samples/sample-word-doc.docx"
};

var result = client.LibreOffice.AnyToPdf(request);
```

Generate a thumbnail or preview image:

```csharp
var request = new LibreFileConversionRequest
{
    Url = "https://www.api2pdf.com/wp-content/themes/api2pdf/assets/samples/sample-word-doc.docx"
};

var result = client.LibreOffice.Thumbnail(request);
```

Convert HTML to DOCX or XLSX:

```csharp
var docx = client.LibreOffice.HtmlToDocx(new LibreFileConversionRequest
{
    Url = "https://www.api2pdf.com/wp-content/uploads/2021/01/sampleHtml.html"
});

var xlsx = client.LibreOffice.HtmlToXlsx(new LibreFileConversionRequest
{
    Url = "https://www.api2pdf.com/wp-content/uploads/2021/01/sampleTables.html"
});
```

Convert PDF to HTML:

```csharp
var result = client.LibreOffice.PdfToHtml(new LibreFileConversionRequest
{
    Url = "https://www.api2pdf.com/wp-content/uploads/2021/01/1a082b03-2bd6-4703-989d-0443a88e3b0f-4.pdf"
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

Add bookmarks:

```csharp
var result = client.PdfSharp.SetBookmarks(new PdfBookmarkRequest
{
    Url = "https://LINK-TO-PDF",
    Bookmarks = new List<PdfBookmark>
    {
        new PdfBookmark { Page = 0, Title = "Introduction" },
        new PdfBookmark { Page = 1, Title = "Second Page" }
    }
});
```

Add a password:

```csharp
var result = client.PdfSharp.SetPassword(new PdfPasswordRequest
{
    Url = "https://LINK-TO-PDF",
    UserPassword = "password"
});
```

Extract pages:

```csharp
var result = client.PdfSharp.ExtractPages(new PdfExtractPagesRequest
{
    Url = "https://LINK-TO-PDF",
    Start = 0,
    End = 2
});
```

More details on page extraction are [here](https://www.api2pdf.com/extract-pages-out-of-a-pdf-with-rest-api/).

## Helper Methods

Delete a generated file before the normal 24-hour retention window:

```csharp
var request = new ChromeHtmlToPdfRequest
{
    Html = "<p>Hello World</p>"
};

var result = client.Chrome.HtmlToPdf(request);
client.Utilities.Delete(result.ResponseId);
```

Save a file locally:

```csharp
var result = client.Chrome.HtmlToPdf(new ChromeHtmlToPdfRequest
{
    Html = "<p>Hello World</p>"
});

result.SaveFile("path-to-local-file.pdf");
```

Download file bytes:

```csharp
var result = client.Chrome.HtmlToPdf(new ChromeHtmlToPdfRequest
{
    Html = "<p>Hello World</p>"
});

byte[] bytes = result.GetFileBytes();
```

## Development

The repo now uses a conventional layout:

- `src/Api2Pdf` for the shipping library
- `tests/Api2Pdf.Tests` for contract and regression tests

## Resources

- [Api2Pdf documentation](https://www.api2pdf.com/documentation/v2)
- [Api2Pdf FAQ](https://www.api2pdf.com/faq)

