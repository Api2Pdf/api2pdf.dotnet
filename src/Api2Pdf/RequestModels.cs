using Newtonsoft.Json;
using System.Collections.Generic;

namespace Api2Pdf
{
    #region Common
    public abstract class ConversionRequest
    {
        public string FileName { get; set; }
        public bool Inline { get; set; } = true;
        public bool UseCustomStorage { get; set; } = false;
        public CustomStorageOptions Storage { get; set; }

        [JsonIgnore]
        public bool? OutputBinary { get; set; }
    }

    public class CustomStorageOptions
    {
        public string Method { get; set; } = "PUT";
        public string Url { get; set; }
        public Dictionary<string, string> ExtraHTTPHeaders { get; set; }
    }

    public abstract class HtmlRequest : ConversionRequest
    {
        public string Html { get; set; }
    }

    public abstract class MarkdownRequest : ConversionRequest
    {
        public string Markdown { get; set; }
    }

    public abstract class UrlRequest : ConversionRequest
    {
        public string Url { get; set; }
        public Dictionary<string, string> ExtraHTTPHeaders { get; set; }
    }

    public abstract class HtmlOrUrlRequest : UrlRequest
    {
        public string Html { get; set; }
    }

    public class ViewPortOptions
    {
        public int Width { get; set; } = 1920;
        public int Height { get; set; } = 1080;
        public bool IsMobile { get; set; } = false;
        public decimal DeviceScaleFactor { get; set; } = 1.0m;
        public bool IsLandscape { get; set; } = false;
        public bool HasTouch { get; set; } = false;
    }

    #endregion

    #region Chrome
    public class ChromeUrlToPdfRequest : UrlRequest
    {
        public ChromeUrlToPdfOptions Options { get; set; } = new ChromeUrlToPdfOptions();
    }

    public class ChromeHtmlToPdfRequest : HtmlRequest
    {
        public ChromeHtmlToPdfOptions Options { get; set; } = new ChromeHtmlToPdfOptions();
    }

    public class ChromeMarkdownToPdfRequest : MarkdownRequest
    {
        public ChromeHtmlToPdfOptions Options { get; set; } = new ChromeHtmlToPdfOptions();
    }

    public class ChromeUrlToImageRequest : UrlRequest
    {
        public ChromeUrlToImageOptions Options { get; set; } = new ChromeUrlToImageOptions();
    }

    public class ChromeHtmlToImageRequest : HtmlRequest
    {
        public ChromeHtmlToImageOptions Options { get; set; } = new ChromeHtmlToImageOptions();
    }

    public class ChromeMarkdownToImageRequest : MarkdownRequest
    {
        public ChromeHtmlToImageOptions Options { get; set; } = new ChromeHtmlToImageOptions();
    }


    public abstract class ChromeImageOptions
    {
        public int Delay { get; set; } = 0;

        public bool FullPage { get; set; } = true;

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public ViewPortOptions ViewPortOptions { get; set; }
    }

    public class ChromeUrlToImageOptions : ChromeImageOptions
    {
        public string PuppeteerWaitForMethod { get; set; } = "WaitForNavigation";

        public string PuppeteerWaitForValue { get; set; } = "Load";
    }

    public class ChromeHtmlToImageOptions : ChromeImageOptions
    {
        public string PuppeteerWaitForMethod { get; set; } = "WaitForExpression";

        public string PuppeteerWaitForValue { get; set; } = "window.IsPageLoaded";
    }

    public abstract class ChromePdfOptions
    {
        public int Delay { get; set; } = 0; //increase this in milliseconds to give chrome more time to render your page
        public bool UsePrintCss { get; set; } = true;
        public bool Landscape { get; set; } = false;
        public bool DisplayHeaderFooter { get; set; } = false;
        public bool PrintBackground { get; set; } = true;
        public bool OmitBackground { get; set; } = false;
        public decimal Scale { get; set; } = 1.0m;
        public bool PreferCSSPageSize { get; set; } = false;
        public bool Tagged { get; set; } = true;
        public bool Outline { get; set; } = false;

        private string _width = "8.27in";
        public string Width
        {
            get
            {
                return _width;
            }
            set
            {
                if (string.IsNullOrEmpty(value))
                {
                    _width = "8.27in";
                }
                else
                {
                    _width = FixDimensionSuffix(value);
                }
            }
        }

        private string _height = "11.69in";
        public string Height
        {
            get
            {
                return _height;
            }
            set
            {
                if (string.IsNullOrEmpty(value))
                {
                    _height = "11.69in";
                }
                else
                {
                    _height = FixDimensionSuffix(value);
                }
            }
        }

        private string _marginTop = ".4in";
        private string _marginBottom = ".4in";
        private string _marginLeft = ".4in";
        private string _marginRight = ".4in";

        public string MarginTop
        {
            get
            {
                return _marginTop;
            }
            set
            {
                if (string.IsNullOrEmpty(value))
                {
                    _marginTop = ".4in";
                }
                else
                {
                    _marginTop = FixDimensionSuffix(value);
                }
            }
        }

        public string MarginBottom
        {
            get
            {
                return _marginBottom;
            }
            set
            {
                if (string.IsNullOrEmpty(value))
                {
                    _marginBottom = ".4in";
                }
                else
                {
                    _marginBottom = FixDimensionSuffix(value);
                }
            }
        }

        public string MarginLeft
        {
            get
            {
                return _marginLeft;
            }
            set
            {
                if (string.IsNullOrEmpty(value))
                {
                    _marginLeft = ".4in";
                }
                else
                {
                    _marginLeft = FixDimensionSuffix(value);
                }
            }
        }

        public string MarginRight
        {
            get
            {
                return _marginRight;
            }
            set
            {
                if (string.IsNullOrEmpty(value))
                {
                    _marginRight = ".4in";
                }
                else
                {
                    _marginRight = FixDimensionSuffix(value);
                }
            }
        }

        public string PageRanges { get; set; } = "";
        public string HeaderTemplate { get; set; } = "<span></span>";
        public string FooterTemplate { get; set; } = "<span></span>";
        private string FixDimensionSuffix(string val)
        {
            if (!val.ToLower().EndsWith("px") &&
                !val.ToLower().EndsWith("in") &&
                !val.ToLower().EndsWith("%") &&
                !val.ToLower().EndsWith("cm") &&
                !val.ToLower().EndsWith("mm"))
            {
                return val + "in";
            }
            return val;
        }
    }
    public class ChromeUrlToPdfOptions : ChromePdfOptions
    {
        public string PuppeteerWaitForMethod { get; set; } = "WaitForNavigation";

        public string PuppeteerWaitForValue { get; set; } = "Load";
    }

    public class ChromeHtmlToPdfOptions : ChromePdfOptions
    {
        public string PuppeteerWaitForMethod { get; set; } = "WaitForExpression";

        public string PuppeteerWaitForValue { get; set; } = "window.IsPageLoaded";
    }
    #endregion

    #region Wkhtml

    public class WkhtmlUrlToPdfRequest : UrlRequest
    {
        public Dictionary<string, string> Options { get; set; } = new Dictionary<string, string>();

        public bool EnableToc { get; set; } = false;

        public Dictionary<string, string> TocOptions { get; set; } = new Dictionary<string, string>();
    }

    public class WkhtmlHtmlToPdfRequest : HtmlRequest
    {
        public Dictionary<string, string> Options { get; set; } = new Dictionary<string, string>();

        public bool EnableToc { get; set; } = false;

        public Dictionary<string, string> TocOptions { get; set; } = new Dictionary<string, string>();
    }
    #endregion

    #region LibreOffice

    public class LibreFileConversionRequest : UrlRequest
    {
    }

    public class LibreHtmlOrUrlConversionRequest : HtmlOrUrlRequest
    {
    }

    #endregion

    #region Markitdown

    public class MarkitdownRequest : UrlRequest
    {
    }

    #endregion

    #region OpenDataLoader

    public class OpenDataLoaderRequest : UrlRequest
    {
    }

    #endregion

    #region PdfSharp

    public class PdfMergeRequest : ConversionRequest
    {
        public IEnumerable<string> Urls { get; set; }
        public Dictionary<string, string> ExtraHTTPHeaders { get; set; } = new Dictionary<string, string>();
    }

    public class PdfPasswordRequest : UrlRequest
    {
        public string UserPassword { get; set; }
        public string OwnerPassword { get; set; }
    }

    public class PdfExtractPagesRequest : UrlRequest
    {
        public int Start { get; set; } = 0;
        public int End { get; set; } = 0;
    }

    #endregion

    #region Zip

    public class ZipRequest : ConversionRequest
    {
        public Dictionary<string, string> ExtraHTTPHeaders { get; set; } = new Dictionary<string, string>();
        public IEnumerable<ZipFileInfo> Files { get; set; } = new List<ZipFileInfo>();
    }

    public class ZipFileInfo
    {
        public string Url { get; set; }
        public string FileName { get; set; }
    }

    #endregion

    #region Zebra

    public class ZebraRequest
    {
        public string Format { get; set; }
        public string Value { get; set; }
        public int Height { get; set; } = 0;
        public int Width { get; set; } = 0;
        public bool ShowLabel { get; set; } = false;

        [JsonIgnore]
        public bool? OutputBinary { get; set; }
    }

    #endregion
}
