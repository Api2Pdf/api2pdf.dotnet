# api2pdf.dotnet
.NET bindings for [Api2Pdf REST API](https://www.api2pdf.com/documentation/v2) 

Api2Pdf.com is a powerful REST API for instantly generating PDF and Office documents from HTML, URLs, Microsoft Office Documents (Word, Excel, PPT), Email files, and images. You can generate image preview or thumbnail of a PDF, office document, or email file. The API also supports merge / concatenation of two or more PDFs, setting passwords on PDFs, and adding bookmarks to PDFs. Api2Pdf is a wrapper for popular libraries such as **wkhtmltopdf**, **Headless Chrome**, **PdfSharp**, and **LibreOffice**.

- [Installation](#installation)
- [Resources](#resources)
- [Authorization](#authorization)
- [Usage](#usage)
- [FAQ](https://www.api2pdf.com/faq)


## <a name="installation"></a>Add a dependency

### nuget

Run the nuget command for installing the client `Install-Package Api2Pdf`

## <a name="resources"></a>Resources

Resources this API supports:

- [wkhtmltopdf](#wkhtmltopdf)
- [Headless Chrome](#chrome)
- [LibreOffice](#libreoffice)
- [Merge / Concatenate PDFs](#merge)
- [Helper Methods](#helpers)

## <a name="authorization"></a>Authorization

### Acquire API Key

Create an account at [portal.api2pdf.com](https://portal.api2pdf.com/register) to get your API key.
    
## <a name="#usage"></a>Usage

### Initialize the Client

All usage starts by initializing the client by passing your API key as a parameter to the constructor.

    var a2pClient = new Api2Pdf("YOUR-API-KEY");

Once you initialize the client, you can make calls like so:

    var request = new ChromeHtmlToPdfRequest
    {
        Html = "<p>Hello World</p>"
    };
    var apiResponse = a2pClient.Chrome.HtmlToPdf(request);
    Console.WriteLine(apiResponse.FileUrl);
    Console.ReadLine();

For each endpoint there are common options you can specify too:

- *Inline* - set to True if you would like the PDF to be opened up in a browser window. Set to false if you want the browser to "download" the file.
- *FileName* - specify your own file name such as "sample.pdf". Inline option above should be set to false if you specify your own file name. If no file name is provided, a random file name will be generated for you.
    
### Result Object

An `Api2PdfResult` object is returned from every API call. If a call is unsuccessful then `success` will show False and the `error` will provide the reason for failure. Additional attributes include the bandwidth out, seconds of compute, and the cost for the API call, typically very small fractions of a penny.

    {
	    'fileUrl': 'https://link-to-file-available-for-24-hours',
	    'mbOut': 0.08830547332763672,
	    'cost': 0.00017251586914062501,
        'seconds': 2,
	    'success': True,
	    'error': None,
	    'responseId': '6e46637a-650d-46d5-af0b-3d7831baccbb'
    }

---
    
## <a name="wkhtmltopdf"></a> wkhtmltopdf

**Convert HTML to PDF**

    var request = new WkhtmlHtmlToPdfRequest
    {
        Html = "<p>Hello World</p>"
    };
    var apiResponse = a2pClient.Wkhtml.HtmlToPdf(request);
    
**Convert HTML to PDF (use dictionary for advanced wkhtmltopdf settings)**
[View full list of wkhtmltopdf options available.](https://www.api2pdf.com/documentation/advanced-options-wkhtmltopdf/)

    var options = new Dictionary<string, string>();
    options.Add("orientation", "landscape");
    options.Add("pageSize", "Letter");

    var request = new WkhtmlHtmlToPdfRequest
    {
        Html = "<p>Hello World</p>",
        FileName = "sample.pdf",
        Inline = true
        Options = options
    }
    var apiResponse = a2pClient.Wkhtml.HtmlToPdf(request);

**Convert URL to PDF**

    var request = new WkhtmlUrlToPdfRequest
    {
        Url = "https://www.api2pdf.com"
    };
    var apiResponse = a2pClient.WkHtml.UrlToPdf(request);
    
**Convert URL to PDF (use dictionary for advanced wkhtmltopdf settings)**
[View full list of wkhtmltopdf options available.](https://www.api2pdf.com/documentation/advanced-options-wkhtmltopdf/)

    var options = new Dictionary<string, string>();
    options.Add("orientation", "landscape");
    options.Add("pageSize", "Letter");

    var request = new WkhtmlUrlToPdfRequest
    {
        Url = "https://wwww.api2pdf.com",
        FileName = "sample.pdf",
        Inline = true
        Options = options
    }
    var apiResponse = a2pClient.WkHtml.UrlToPdf(request);


---

## <a name="chrome"></a>Headless Chrome

**Convert HTML to PDF**

    var request = new ChromeHtmlToPdfRequest
    {
        Html = "<p>Hello World</p>"
    };
    var apiResponse = a2pClient.Chrome.HtmlToPdf(request);
    
**Convert HTML to PDF (use dictionary for advanced Headless Chrome settings)**
[View full list of Headless Chrome options available.](https://www.api2pdf.com/documentation/advanced-options-headless-chrome/)

    var options = new ChromeHtmlToPdfOptions
    {
        Delay = 3000,
        HeaderTemplate = "<div style=\"font-size:12px;\">Header Content Here</div>",
        DisplayHeaderFooter = true,
        Landscape = true
    }
    var request = new ChromeHtmlToPdfRequest
    {
        Html = "<p>Hello World</p>",
        FileName = "sample.pdf",
        Inline = true
        Options = options
    };
    var apiResponse = a2pClient.Chrome.HtmlToPdf(request);

**Convert URL to PDF**

    var request = new ChromeUrlToPdfRequest
    {
        Url = "https://www.api2pdf.com"
    };
    var apiResponse = a2pClient.Chrome.UrlToPdf(request);
    
**Convert URL to PDF (use dictionary for advanced Headless Chrome settings)**
[View full list of Headless Chrome options available.](https://www.api2pdf.com/documentation/advanced-options-headless-chrome/)

    var options = new ChromeUrlToPdfOptions
    {
        Delay = 3000,
        HeaderTemplate = "<div style=\"font-size:12px;\">Header Content Here</div>",
        DisplayHeaderFooter = true,
        Landscape = true
    }
    var request = new ChromeUrlToPdfRequest
    {
        Url = "https://www.api2pdf.com",
        FileName = "sample.pdf",
        Inline = true
        Options = options
    };
    var apiResponse = a2pClient.Chrome.UrlToPdf(request);
    
---

## <a name="libreoffice"></a>LibreOffice

Convert any office file to PDF, image file to PDF, email file to PDF, HTML to Word, HTML to Excel, and PDF to HTML. Any file that can be reasonably opened by LibreOffice should be convertible. Additionally, we have an endpoint for generating a *thumbnail* of the first page of your PDF or Office Document. This is great for generating an image preview of your files to users.

You must provide a url to the file. Our engine will consume the file at that URL and convert it to the PDF.

**Convert Microsoft Office Document or Image to PDF**

    var request = new LibreFileConversionRequest
    {
        Url = "https://www.api2pdf.com/wp-content/themes/api2pdf/assets/samples/sample-word-doc.docx"
    }
    var apiResponse = a2pClient.LibreOffice.AnyToPdf(request);

**Thumbnail or Image Preview of a PDF or Office Document or Email file**

    var request = new LibreFileConversionRequest
    {
        Url = "https://www.api2pdf.com/wp-content/themes/api2pdf/assets/samples/sample-word-doc.docx"
    }
    var apiResponse = a2pClient.LibreOffice.Thumbnail(request);

**Convert HTML to Microsoft Word or Docx**

    var request = new LibreFileConversionRequest
    {
        Url = "http://www.api2pdf.com/wp-content/uploads/2021/01/sampleHtml.html"
    }
    var apiResponse = a2pClient.LibreOffice.HtmlToDocx(request);

**Convert HTML to Microsoft Excel or Xlsx**

    var request = new LibreFileConversionRequest
    {
        Url = "http://www.api2pdf.com/wp-content/uploads/2021/01/sampleTables.html"
    }
    var apiResponse = a2pClient.LibreOffice.HtmlToXlsx(request);

**Convert PDF to HTML**

    var request = new LibreFileConversionRequest
    {
        Url = "http://www.api2pdf.com/wp-content/uploads/2021/01/1a082b03-2bd6-4703-989d-0443a88e3b0f-4.pdf"
    }
    var apiResponse = a2pClient.LibreOffice.PdfToHtml(request);
    
---
    
## <a name="merge"></a>PdfSharp

**Merge PDFs from list of URLs to existing PDFs**

To use the merge endpoint, supply a list of urls to existing PDFs. The engine will consume all of the PDFs and merge them into a single PDF, in the order in which they were provided in the list.

    var pdfsToMerge = new List<string>() {"https://LINK-TO-PDF", "https://LINK-TO-PDF"};
    var request = new PdfMergeRequest
    {
        Urls = pdfsToMerge
    };
    var apiResponse = a2pClient.PdfSharp.MergePdfs(request);

**Add PDF bookmarks to an existing PDF**

    var bookmarks = new List<PdfBookmark>()
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
    var request = new PdfBookmarkRequest
    {
        Url = "https://LINK-TO-PDF,
        Bookmarks = bookmarks
    };
    var apiResponse = a2pClient.PdfSharp.SetBookmarks(request);

**Add password to existing PDF**

    var request = new PdfPasswordRequest
    {
        Url = "https://LINK-TO-PDF",
        UserPassword = "password"
    };
    var apiResponse = a2pClient.PdfSharp.SetPassword(request);
    
---
    
## <a name="helpers"></a>Helper Methods

**Delete a file conversion on command from API2PDF servers**

By default, Api2Pdf will delete your generated file 24 hours after it has been generated. For those with high security needs, you may want to delete your file on command. You can do so by making an DELETE api call with the `responseId` attribute that was returned on the original JSON payload.

    var a2pClient = Api2Pdf("YOUR-API-KEY");
    var request = new ChromeHtmlToPdfRequest
    {
        Html = "<p>Hello World</p>"
    };
    var apiResponse = a2pClient.Chrome.HtmlToPdf(request);
    
    //delete file
    a2pClient.Utilities.Delete(apiResponse.ResponseId);

**void Api2PdfResult.SavePdf(string localPath)**

On any `Api2PdfResult` that succesfully generated a file, you can use the handy `SaveFile(string localPath)` method to download the file to a local cache.

    var a2pClient = Api2Pdf("YOUR-API-KEY");
    var request = new ChromeHtmlToPdfRequest
    {
        Html = "<p>Hello World</p>"
    };
    var apiResponse = a2pClient.Chrome.HtmlToPdf(request);
    apiResponse.SaveFile("path-to-local-folder");

**byte[] Api2PdfResult.GetFileBytes()**

You can use `GetFileBytes()` method to download the file to a byte array.

    var a2pClient = Api2Pdf("YOUR-API-KEY");
    var request = new ChromeHtmlToPdfRequest
    {
        Html = "<p>Hello World</p>"
    };
    var apiResponse = a2pClient.Chrome.HtmlToPdf(request);
    var resultAsBytes = apiResponse.GetFileBytes();
    

