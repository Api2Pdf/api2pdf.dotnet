using System;
using Api2PdfLibrary;

namespace ExampleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            var a2pClient = new Api2Pdf("YOUR-API-KEY"); //portal.api2pdf.com/register

            var result = a2pClient.HeadlessChrome.FromHtml("<p>Hello World</p>");
            Console.WriteLine(result.Pdf);
        }
    }
}
