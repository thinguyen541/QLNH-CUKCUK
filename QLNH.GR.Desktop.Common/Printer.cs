using QLNH.GR.Desktop.BO;
using iTextSharp.text;
using iTextSharp.text.pdf;
using QLNH.GR.Desktop.BO.Entity;
using System.Windows;
using System.Text;
using static iTextSharp.text.pdf.AcroFields;
using System.util;

namespace QLNH.GR.Desktop.Common
{
    public class Printer
    {
        public static void PrintReceipt(string storeName, string storeAddress, Order CurrentOrder, string outputFilePath, iTextSharp.text.Rectangle pageSize, Invoice invoice)
        {
            List<OrderDetail> items = CurrentOrder?.ListOrderDetail;
            Document document = new Document(pageSize);

            try
            {

                // Register the code pages encoding provider
                Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
                PdfWriter writer = PdfWriter.GetInstance(document, new FileStream(outputFilePath, FileMode.Create));
                document.Open();
                BaseFont baseFont = BaseFont.CreateFont("E:\\Documents\\git_local\\QLNH.GR.Desktop\\ttf\\Tahoma Regular font.ttf", BaseFont.IDENTITY_H, BaseFont.EMBEDDED);
                Font font = new Font(baseFont, 12);
                Font fontBold12 = new Font(baseFont, 12, Font.BOLD);
                Font fontBold14 = new Font(baseFont, 14, Font.BOLD);
                Font fontBold13 = new Font(baseFont, 13, Font.BOLD);
                // Add Store Information
                document.Add(new Paragraph(storeName, fontBold14));
                document.Add(new Paragraph(storeAddress));
                document.Add(new Paragraph($"Cashier: {invoice.UserName}", font));
                document.Add(new Paragraph($"Print at: {DateTime.Now.ToString()}"));
                document.Add(new Paragraph(" ")); // Add a blank line

                // Add Items
                PdfPTable table = new PdfPTable(3);
                PdfPCell cel11 = new PdfPCell(new iTextSharp.text.Phrase("Item name")) { HorizontalAlignment = iTextSharp.text.Element.ALIGN_CENTER };
                table.AddCell(cel11);
                PdfPCell cel12 = new PdfPCell(new iTextSharp.text.Phrase("Quantity")) { HorizontalAlignment = iTextSharp.text.Element.ALIGN_CENTER, };
                table.AddCell(cel12);
                PdfPCell cel13 = new PdfPCell(new iTextSharp.text.Phrase("Total price")) { HorizontalAlignment = iTextSharp.text.Element.ALIGN_CENTER };
                table.AddCell(cel13);
                decimal totalItem = 0;
                decimal subtotal = 0;
                foreach (var item in items)
                {
                    foreach (var detail in item.ListNormalDetailItem)
                    {
                        totalItem += item.Quantity.GetValueOrDefault();
                        subtotal += detail.Amount.GetValueOrDefault();
                        PdfPCell itemnameCell = new PdfPCell(new Phrase(detail.DishName, font)) { HorizontalAlignment = Element.ALIGN_LEFT, };
                        table.AddCell(itemnameCell);
                        PdfPCell quantityCell = new PdfPCell(new Phrase(item.Quantity.ToString(), font)) { HorizontalAlignment = Element.ALIGN_CENTER, };
                        table.AddCell(quantityCell);
                        PdfPCell amountCell = new PdfPCell(new Phrase(detail.Amount.GetValueOrDefault().ToString("C"), font)) { HorizontalAlignment = Element.ALIGN_RIGHT };
                        table.AddCell(amountCell);
                    }
                    foreach (var detail in item.ListModifierDetailItem)
                    {
                        subtotal += detail.Amount.GetValueOrDefault();
                        PdfPCell nameCell = new PdfPCell(new Phrase(detail.DishName, font)) { HorizontalAlignment = Element.ALIGN_CENTER, Indent = 12 };
                        table.AddCell(nameCell);
                        PdfPCell quantityCell = new PdfPCell(new Phrase(item.Quantity.ToString(), font)) { HorizontalAlignment = Element.ALIGN_CENTER };
                        table.AddCell(quantityCell);
                        PdfPCell amountCell = new PdfPCell(new Phrase(detail.Amount.GetValueOrDefault().ToString("C"), font)) { HorizontalAlignment = Element.ALIGN_RIGHT };
                        table.AddCell(amountCell);
                    }

                }

                
                PdfPCell TotolCell = new PdfPCell(new Phrase("Total", fontBold12)) { HorizontalAlignment = Element.ALIGN_CENTER,  };
                table.AddCell(TotolCell);
                PdfPCell totalQuantityCell = new PdfPCell(new Phrase(totalItem.ToString(), fontBold12)) { HorizontalAlignment = Element.ALIGN_CENTER };
                table.AddCell(totalQuantityCell);
                PdfPCell SubtotalCell = new PdfPCell(new Phrase(subtotal.ToString("C"), fontBold12)) { HorizontalAlignment = Element.ALIGN_RIGHT };
                table.AddCell(SubtotalCell);
                document.Add(table);

            
                
                if (invoice.PromotionAmount.GetValueOrDefault() != 0)
                {
                    var total = invoice.PromotionAmount.GetValueOrDefault();
                    if (total < 0)
                    {
                        total = 0 - total;
                    }
                    document.Add(new Paragraph("Discount: " + invoice.PromotionName + $" (-{total.ToString("C")})",
                        fontBold13));

                }
                if (invoice.Tipamount!= 0)
                {
                    document.Add(new Paragraph("Tip amount: " + invoice.Tipamount.ToString("C"),
                    fontBold13));
                }
                document.Add(new Paragraph("Total amount: " + CurrentOrder.Amount.GetValueOrDefault().ToString("C"), fontBold13));

                document.Add(new Paragraph("Thank you for dining with us!", fontBold13));
            }
            catch (DocumentException docEx)
            {
                Console.WriteLine(docEx.Message);
            }
            catch (IOException ioEx)
            {
                Console.WriteLine(ioEx.Message);
            }
            finally
            {
                document.Close();
            }

            Console.WriteLine("Receipt PDF created successfully!");
        }

        public static void PrintSendKitchen(Order CurrentOrder, string outputFilePath)
        {
            try
            {
                using (FileStream fs = new FileStream(outputFilePath, FileMode.Create, FileAccess.Write, FileShare.None))
                {
                    // Create a new PDF document
                    float width = 5 * 72; // 5 inches
                    float height = 7 * 72; // 7 inches


                    iTextSharp.text.Rectangle customPageSize = new iTextSharp.text.Rectangle(width, height);
                    using (Document document = new Document(customPageSize))
                    {
                        // Bind the document to the stream
                        using (PdfWriter writer = PdfWriter.GetInstance(document, fs))
                        {

                            try
                            {


                                BaseFont baseFont = BaseFont.CreateFont("E:\\Documents\\git_local\\QLNH.GR.Desktop\\ttf\\Tahoma Regular font.ttf", BaseFont.IDENTITY_H, BaseFont.EMBEDDED);
                                Font font = new Font(baseFont, 12);
                                if (CurrentOrder.ListOrderDetail == null)
                                {
                                    return;
                                }
                                List<OrderDetail> items = CurrentOrder.ListOrderDetail.Where(item => item.OrderDetailStatus == EnumOrderDetailStatus.NotSentKitchen).ToList();
                                document.Open();

                                // Add Store Information
                                document.Add(new Paragraph("Send kitchen tampt", FontFactory.GetFont("Arial", 18, Font.BOLD)));
                                document.Add(new Paragraph($"Print at: {DateTime.Now.ToString()}"));
                                document.Add(new Paragraph(" ")); // Add a blank line

                                // Add Items
                                PdfPTable table = new PdfPTable(3);
                                PdfPCell cel11 = new PdfPCell(new iTextSharp.text.Phrase("Item name")) { HorizontalAlignment = iTextSharp.text.Element.ALIGN_CENTER };
                                table.AddCell(cel11);
                                PdfPCell cel12 = new PdfPCell(new iTextSharp.text.Phrase("Quantity")) { HorizontalAlignment = iTextSharp.text.Element.ALIGN_CENTER };
                                table.AddCell(cel12);
                                PdfPCell cel13 = new PdfPCell(new iTextSharp.text.Phrase("Total price")) { HorizontalAlignment = iTextSharp.text.Element.ALIGN_CENTER };
                                table.AddCell(cel13);

                                foreach (var item in items)
                                {
                                    if (item.ListNormalDetailItem != null)
                                    {
                                        foreach (var detail in item.ListNormalDetailItem)
                                        {

                                            PdfPCell itemnameCell = new PdfPCell(new Phrase(detail.DishName, font)) { HorizontalAlignment = Element.ALIGN_LEFT, };
                                            table.AddCell(itemnameCell);
                                            PdfPCell quantityCell = new PdfPCell(new Phrase(item.Quantity.ToString(), font)) { HorizontalAlignment = Element.ALIGN_CENTER, };
                                            table.AddCell(quantityCell);
                                            PdfPCell amountCell = new PdfPCell(new Phrase(detail.Amount.GetValueOrDefault().ToString("C"), font)) { HorizontalAlignment = Element.ALIGN_RIGHT };
                                            table.AddCell(amountCell);
                                        }
                                        foreach (var detail in item.ListModifierDetailItem)
                                        {
                                            PdfPCell nameCell = new PdfPCell(new Phrase(detail.DishName, font)) { HorizontalAlignment = Element.ALIGN_CENTER, Indent = 12 };
                                            table.AddCell(nameCell);
                                            PdfPCell quantityCell = new PdfPCell(new Phrase(item.Quantity.ToString(), font)) { HorizontalAlignment = Element.ALIGN_CENTER };
                                            table.AddCell(quantityCell);
                                            PdfPCell amountCell = new PdfPCell(new Phrase(detail.Amount.GetValueOrDefault().ToString("C"), font)) { HorizontalAlignment = Element.ALIGN_RIGHT };
                                            table.AddCell(amountCell);
                                        }
                                    }
                                }

                                document.Add(table);


                            }
                            catch (DocumentException docEx)
                            {
                                Console.WriteLine(docEx.Message);
                            }
                            catch (IOException ioEx)
                            {
                                Console.WriteLine(ioEx.Message);
                            }
                            finally
                            {
                                document.Close();
                            }

                        }
                    }
                }
            }
            catch (Exception ioEx)
            {
                Console.WriteLine(ioEx.Message);
            }
        }



    }

}
