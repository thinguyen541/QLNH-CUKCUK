using QLNH.GR.Desktop.BO;
using iTextSharp.text;
using iTextSharp.text.pdf;
using QLNH.GR.Desktop.BO.Entity;
using System.Windows;

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
                PdfWriter writer = PdfWriter.GetInstance(document, new FileStream(outputFilePath, FileMode.Create));
                document.Open();
                // Step 4: Create a base font that supports Unicode
                Font font = FontFactory.GetFont("Arial", BaseFont.IDENTITY_H, BaseFont.EMBEDDED, 12);
                // Add Store Information
                document.Add(new Paragraph(storeName, FontFactory.GetFont("Arial", 18, Font.BOLD)));
                document.Add(new Paragraph(storeAddress));
                document.Add(new Paragraph($"Cashier: {invoice.UserName}"));
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

                foreach (var item in items)
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

                document.Add(table);

                // Add Total Amount
                document.Add(new Paragraph(" "));
                document.Add(new Paragraph("Total Amount: " + CurrentOrder.Amount.GetValueOrDefault().ToString("C"), FontFactory.GetFont("Arial", 14, Font.BOLD)));

                document.Add(new Paragraph("Thank you for dining with us!", FontFactory.GetFont("Arial", 12, Font.ITALIC)));
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

                                // Step 4: Create a base font that supports Unicode
                                Font font = FontFactory.GetFont("Arial", BaseFont.IDENTITY_H, BaseFont.EMBEDDED, 12);
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
