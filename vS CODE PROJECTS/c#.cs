using System;
using System.IO;
using System.Drawing;
using System.Drawing.Imaging;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;
using iText.Layout.Properties;
using iText.IO.Image;
using QRCoder;

class CertificateGenerator
{
    static void Main()
    {
        string name = "Muhammad Abdullah";
        string url = "https://example.com/certificate/muhammad-abdullah";  // Your unique link here
        string outputPath = "Certificate_MuhammadAbdullah.pdf";

        // Generate QR Code
        Bitmap qrCodeImage = GenerateQrCode(url);

        // Convert Bitmap to iText ImageData
        ImageData qrCodeData;
        using (var ms = new MemoryStream())
        {
            qrCodeImage.Save(ms, ImageFormat.Png);
            qrCodeData = ImageDataFactory.Create(ms.ToArray());
        }

        // Create PDF
        using (PdfWriter writer = new PdfWriter(outputPath))
        {
            using (PdfDocument pdf = new PdfDocument(writer))
            {
                Document document = new Document(pdf);

                // Add Title
                Paragraph title = new Paragraph("Certificate of Achievement")
                    .SetTextAlignment(TextAlignment.CENTER)
                    .SetFontSize(24)
                    .SetBold()
                    .SetMarginBottom(20);
                document.Add(title);

                // Add Subtitle or Description
                Paragraph subtitle = new Paragraph("This certificate is proudly presented to")
                    .SetTextAlignment(TextAlignment.CENTER)
                    .SetFontSize(16)
                    .SetItalic()
                    .SetMarginBottom(20);
                document.Add(subtitle);

                // Add Name
                Paragraph nameParagraph = new Paragraph(name)
                    .SetTextAlignment(TextAlignment.CENTER)
                    .SetFontSize(32)
                    .SetBold()
                    .SetFontColor(iText.Kernel.Colors.ColorConstants.BLUE)
                    .SetMarginBottom(30);
                document.Add(nameParagraph);

                // Add some descriptive text
                Paragraph desc = new Paragraph("In recognition of your outstanding performance and dedication.")
                    .SetTextAlignment(TextAlignment.CENTER)
                    .SetFontSize(14)
                    .SetMarginBottom(50);
                document.Add(desc);

                // Add QR code image centered
                Image qrImage = new Image(qrCodeData).SetWidth(120).SetHorizontalAlignment(HorizontalAlignment.CENTER);
                document.Add(qrImage);

                // Add clickable link below QR code
                Link link = new Link(url, new iText.Kernel.Pdf.Action.PdfAction(url));
                Paragraph linkParagraph = new Paragraph(link.SetUnderline())
                    .SetTextAlignment(TextAlignment.CENTER)
                    .SetFontSize(12)
                    .SetMarginTop(10);
                document.Add(linkParagraph);

                // Add footer or signature area if needed
                Paragraph footer = new Paragraph("Date: " + DateTime.Now.ToString("MMMM dd, yyyy"))
                    .SetTextAlignment(TextAlignment.RIGHT)
                    .SetFontSize(10)
                    .SetMarginTop(60);
                document.Add(footer);

                document.Close();
            }
        }

        Console.WriteLine("Certificate generated: " + outputPath);
    }

    static Bitmap GenerateQrCode(string url)
    {
        using (QRCodeGenerator qrGenerator = new QRCodeGenerator())
        {
            QRCodeData qrCodeData = qrGenerator.CreateQrCode(url, QRCodeGenerator.ECCLevel.Q);
            using (QRCode qrCode = new QRCode(qrCodeData))
            {
                return qrCode.GetGraphic(20); // 20 is pixel per module, adjust for size
            }
        }
    }
}
