using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Api2PdfLibrary;

namespace Examples.ConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            var apiKey = "REPLACE WITH YOUR KEY";
            var api2Pdf = new Api2Pdf(apiKey);
            Console.WriteLine(api2Pdf.WkHtmlToPdf.FromHtml("hello world", true).Pdf);
            Console.ReadLine();
        }
    }
}
