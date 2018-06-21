# api2pdf.dotnet
.NET bindings for [Api2Pdf REST API](https://www.api2pdf.com/documentation) 

Api2Pdf.com is a REST API for instantly generating PDF documents from HTML, URLs, Microsoft Office Documents (Word, Excel, PPT), and images. The API also supports merge / concatenation of two or more PDFs. Api2Pdf is a wrapper for popular libraries such as **wkhtmltopdf**, **Headless Chrome**, and **LibreOffice**.

- [Installation](#installation)
- [Resources](#resources)
- [Authorization](#authorization)
- [Usage](#usage)
- [FAQ](#faq)


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
1. Create an account and login at [portal.api2pdf.com](https://portal.api2pdf.com)
2. Add a balance to your account (no monthly commitment, sign up with as little as $1)
3. Create an application and grab your API Key
    
## <a name="#usage"></a>Usage

### Initialize the Client

All usage starts by initializing the client by passing your API key as a parameter to the constructor.

    var a2pClient = Api2Pdf("YOUR-API-KEY");

Once you initialize the client, you can make calls like so:

    var apiResponse = a2pClient.HeadlessChrome.FromHtml('<p>Hello, World</p>');
    Console.WriteLine(apiResponse.Pdf);
    Console.ReadLine();
    
### Result Object

An `Api2PdfResponse` object is returned from every API call. If a call is unsuccessful then `success` will show False and the `error` will provide the reason for failure. Additional attributes include the total data usage in, out, and the cost for the API call, typically very small fractions of a penny.

    {
	    'pdf': 'https://link-to-pdf-only-available-for-24-hours',
	    'mbIn': 0.08421039581298828,
	    'mbOut': 0.08830547332763672,
	    'cost': 0.00017251586914062501,
	    'success': True,
	    'error': None,
	    'responseId': '6e46637a-650d-46d5-af0b-3d7831baccbb'
    }
    
### <a name="wkhtmltopdf"></a> wkhtmltopdf

**Convert HTML to PDF**

    var apiResponse = a2pClient.WkHtmlToPdf.FromHtml("<p>Hello, World</p>");
    
**Convert HTML to PDF (load PDF in browser window and specify a file name)**

    var apiResponse = a2pClient.WkHtmlToPdf.FromHtml("<p>Hello, World</p>", inline=true, outputFileName="test.pdf");
    
**Convert HTML to PDF (use keyword arguments for advanced wkhtmltopdf settings)**
[View full list of wkhtmltopdf options available.](https://www.api2pdf.com/documentation/advanced-options-wkhtmltopdf/)

    options = {
        'orientation': 'landscape',
        'pageSize': 'A4'
    }
    var apiResponse = a2pClient.WkHtmlToPdf.FromHtml("<p>Hello, World</p>", **options);

**Convert URL to PDF**

    var apiResponse = a2pClient.WkHtmlToPdf.FromUrl("http://www.api2pdf.com");
    
**Convert URL to PDF (load PDF in browser window and specify a file name)**

    var apiResponse = a2pClient.WkHtmlToPdf.FromUrl("http://www.api2pdf.com", inline=true, outputFileName="test.pdf");
    
**Convert URL to PDF (use keyword arguments for advanced wkhtmltopdf settings)**
[View full list of wkhtmltopdf options available.](https://www.api2pdf.com/documentation/advanced-options-wkhtmltopdf/)

    options = {
        'orientation': 'landscape',
        'pageSize': 'A4'
    }
    var apiResponse = a2pClient.WkHtmlToPdf.FromUrl("http://www.api2pdf.com", **options);


---

## <a name="chrome"></a>Headless Chrome

**Convert HTML to PDF**

    var apiResponse = a2pClient.HeadlessChrome.FromHtml("<p>Hello, World</p>");
    
**Convert HTML to PDF (load PDF in browser window and specify a file name)**

    var apiResponse = a2pClient.HeadlessChrome.FromHtml("<p>Hello, World</p>", inline=true, outputFileName="test.pdf");
    
**Convert HTML to PDF (use keyword arguments for advanced Headless Chrome settings)**
[View full list of Headless Chrome options available.](https://www.api2pdf.com/documentation/advanced-options-headless-chrome/)

    options = {
        'orientation': 'landscape',
        'pageSize': 'A4'
    }
    var apiResponse = a2pClient.HeadlessChrome.FromHtml("<p>Hello, World</p>", **options);

**Convert URL to PDF**

    var apiResponse = a2pClient.HeadlessChrome.FromUrl("http://www.api2pdf.com");
    
**Convert URL to PDF (load PDF in browser window and specify a file name)**

    var apiResponse = a2pClient.HeadlessChrome.FromUrl("http://www.api2pdf.com", inline=true, outputFileName="test.pdf");
    
**Convert URL to PDF (use keyword arguments for advanced Headless Chrome settings)**
[View full list of Headless Chrome options available.](https://www.api2pdf.com/documentation/advanced-options-headless-chrome/)

    options = {
        'orientation': 'landscape',
        'pageSize': 'A4'
    }
    var apiResponse = a2pClient.HeadlessChrome.FromUrl("http://www.api2pdf.com", **options);
    
---

## <a name="libreoffice"></a>LibreOffice

LibreOffice supports the conversion to PDF from the following file formats:

- doc, docx, xls, xlsx, ppt, pptx, gif, jpg, png, bmp, rtf, txt, html

You must provide a url to the file. Our engine will consume the file at that URL and convert it to the PDF.

**Convert Microsoft Office Document or Image to PDF**

    var apiResponse = a2pClient.LibreOffice.Convert("https://www.api2pdf.com/wp-content/themes/api2pdf/assets/samples/sample-word-doc.docx");
    
**Convert Microsoft Office Document or Image to PDF (load PDF in browser window and specify a file name)**

    var apiResponse = a2pClient.LibreOffice.Convert("https://www.api2pdf.com/wp-content/themes/api2pdf/assets/samples/sample-word-doc.docx", inline=true, outputFileName="test.pdf");
    
---
    
## <a name="merge"></a>Merge / Concatenate Two or More PDFs

To use the merge endpoint, supply a list of urls to existing PDFs. The engine will consume all of the PDFs and merge them into a single PDF, in the order in which they were provided in the list.

**Merge PDFs from list of URLs to existing PDFs**

    var links_to_pdfs = new List<string>() {"https://LINK-TO-PDF", "https://LINK-TO-PDF"};
    var apiResponse = a2pClient.Merge(links_to_pdfs);

**Merge PDFs from list of URLs to existing PDFs (load PDF in browser window and specify a file name)**

    var links_to_pdfs = new List<string>() {"https://LINK-TO-PDF", "https://LINK-TO-PDF"};
    var apiResponse = a2pClient.Merge(links_to_pdfs, inline=true, outputFileName="test.pdf");
    
---
    
## <a name="helpers"></a>Helper Methods

**void Api2PdfResponse.SavePdf(string localPath)**

On any `Api2PdfResponse` that succesfully generated a pdf, you can use the handy `SavePdf(string localPdf)` method to download the pdf to a local cache.

    var a2pClient = Api2Pdf("YOUR-API-KEY");
    var links_to_pdfs = new List<string>() {"https://LINK-TO-PDF", "https://LINK-TO-PDF"};
    var apiResponse = a2pClient.Merge(links_to_pdfs, inline=true, outputFileName="test.pdf");
    apiResponse.SavePdf("path-to-local-folder");

**byte[] Api2PdfResponse.GetPdfBytes()**

You can use `GetPdfBytes()` method to download the pdf to a byte array.

    var a2pClient = Api2Pdf("YOUR-API-KEY");
    var links_to_pdfs = new List<string>() {"https://LINK-TO-PDF", "https://LINK-TO-PDF"};
    var apiResponse = a2pClient.Merge(links_to_pdfs, inline=true, outputFileName="test.pdf");
    apiResponse.GetPdfBytes();
    
---
## <a name="faq"></a>FAQ

#### How do you bill?
$1 will be deducted from your balance every month as long as you maintain an active account. This charge begins 30 days after your first sign up for the service. In addition, we charge $0.001 per megabyte (data in + data out). We require customers to maintain a positive balance on their account to use the service. You can turn off auto-recharge at any time and let your funds run out if you no longer wish to use the service. See our [pricing calculator](https://www.api2pdf.com/pricing/).

#### Do you offer free accounts?
The average customer spents about $2/month on our product. We do not have free accounts as this time. Feel free to check out alternatives and competitors.

#### Cancellation and refunds
We do not have any long term contracts. You can leave us at anytime with no further commitments. As our minimum cost is $1.00, we do not provide refunds.

#### Are there any limits?
Api2Pdf does not set any specific limits on PDF file size, however our system does have processing power limitations. Each PDF request is provided 3 GB of RAM to work with and 90 seconds to generate the PDF. We offer WKHTMLTOPDF, Headless Chrome, and LibreOffice to do conversions. Our platform will have the same limits as those underlying components. If the underlying component fails to convert to PDF, it will also fail via our service. Some examples are:

- Password protected PDFs
- Encrypted PDFs
- HTML that references erroneous content
- Protected Office Documents

#### How long are PDFs stored on Api2Pdf.com?
After generating a PDF via the API, you are provided with a link to the file. This link will hold the PDF for 24 hours. If you wish to keep your PDF long term, download the file to your local cache.
