using QLNH.GR.Desktop.BO;
using iTextSharp.text;
using iTextSharp.text.pdf;
using QLNH.GR.Desktop.BO.Entity;
using System.Windows;
using System.Globalization;
using System.Text;

namespace QLNH.GR.Desktop.Common
{
    public class Printer
    {
        public static void PrintReceipt(string storeName, string storeAddress, Order CurrentOrder, Promotion? PromotionAmount, string outputFilePath, iTextSharp.text.Rectangle pageSize, Invoice invoice)
        {
            List<OrderDetail> items = CurrentOrder?.ListOrderDetail;
            Document document = new Document(pageSize);
            try
            {
                // Register the code pages encoding provider
                Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
                PdfWriter writer = PdfWriter.GetInstance(document, new FileStream(outputFilePath, FileMode.Create));
                document.Open();
                BaseFont baseFont = BaseFont.CreateFont("C:\\Đồ án\\QLNH-Thesis\\ttf\\Tahoma Regular font.ttf", BaseFont.IDENTITY_H, BaseFont.EMBEDDED);
                Font font = new Font(baseFont, 12);
                // Add Store Information
                Paragraph storeNameParagraph = new Paragraph(storeName, new Font(baseFont, 18, Font.BOLD));
                storeNameParagraph.Alignment = Element.ALIGN_CENTER; // Set alignment to center
                document.Add(storeNameParagraph); // Add the paragraph to the document
                Paragraph addressParagraph = new Paragraph(storeAddress, font) { Alignment = Element.ALIGN_CENTER };
                document.Add(addressParagraph);
                document.Add(new Paragraph($"Nhân viên: {invoice.UserName}", font));
                document.Add(new Paragraph($"In lúc: {DateTime.Now.ToString(CultureInfo.GetCultureInfo("vi-VN"))}", font));
                document.Add(new Paragraph(" ")); // Add a blank line

                // Add Items
                PdfPTable table = new PdfPTable(3);

                Font boldFont = new Font(font.BaseFont, font.Size, Font.BOLD);
                table.AddCell(new PdfPCell(new Phrase("Tên món", boldFont)) { HorizontalAlignment = Element.ALIGN_CENTER });
                table.AddCell(new PdfPCell(new Phrase("Số lượng", boldFont)) { HorizontalAlignment = Element.ALIGN_CENTER });
                table.AddCell(new PdfPCell(new Phrase("Giá tiền", boldFont)) { HorizontalAlignment = Element.ALIGN_CENTER });


                //foreach (var item in CurrentOrder?.ListOrderDetail ?? new List<OrderDetail>())
                //{
                //    foreach (var detail in item.ListNormalDetailItem)
                //    {
                //        table.AddCell(new PdfPCell(new Phrase(detail.DishName, font)) { HorizontalAlignment = Element.ALIGN_LEFT });
                //        table.AddCell(new PdfPCell(new Phrase(item.Quantity.ToString(), font)) { HorizontalAlignment = Element.ALIGN_CENTER });
                //        table.AddCell(new PdfPCell(new Phrase(detail.Amount.GetValueOrDefault().ToString("C", CultureInfo.GetCultureInfo("vi-VN")), font)) { HorizontalAlignment = Element.ALIGN_RIGHT });
                //    }
                //}


                if (CurrentOrder?.ListOrderDetail != null)
                {
                    foreach (var item in CurrentOrder.ListOrderDetail)
                    {
                        if (item.ListNormalDetailItem != null)
                        {
                            foreach (var detail in item.ListNormalDetailItem)
                            {
                                table.AddCell(new PdfPCell(new Phrase(detail.DishName, font)) { HorizontalAlignment = Element.ALIGN_LEFT });
                                table.AddCell(new PdfPCell(new Phrase(item.Quantity.ToString(), font)) { HorizontalAlignment = Element.ALIGN_CENTER });
                                table.AddCell(new PdfPCell(new Phrase(detail.Amount.GetValueOrDefault().ToString("C", CultureInfo.GetCultureInfo("vi-VN")), font)) { HorizontalAlignment = Element.ALIGN_RIGHT });
                            }
                        }
                    }
                }


                document.Add(table);

                // Add Total Amount
                string totalAmountText = "Tổng tiền: ";
                string totalPromotionAmountText = "Khuyến mãi: ";
                string thankNote = "Cảm ơn đã dùng bữa tại nhà hàng!";
                document.Add(new Paragraph(" ")); // Add a blank line
                if (PromotionAmount.PromotionAmount != null)
                {
                    // Dòng khuyến mãi
                    // Create a table with 2 columns for the promotion amount
                    PdfPTable promotionAmountTable = new PdfPTable(2);
                    promotionAmountTable.WidthPercentage = 100; // Make the table span the width of the document

                    // Create the left cell for the text
                    PdfPCell promotionTextCell = new PdfPCell(new Phrase(totalPromotionAmountText, new Font(baseFont, 14, Font.BOLD))) { Border = PdfPCell.NO_BORDER, HorizontalAlignment = Element.ALIGN_LEFT };
                    promotionAmountTable.AddCell(promotionTextCell);

                    // Create the right cell for the amount
                    string formattedPromotionAmount = PromotionAmount.PromotionAmount.GetValueOrDefault().ToString("C", CultureInfo.GetCultureInfo("vi-VN"));
                    PdfPCell promotionAmountCell = new PdfPCell(new Phrase(formattedPromotionAmount, new Font(baseFont, 14, Font.BOLD))) { Border = PdfPCell.NO_BORDER, HorizontalAlignment = Element.ALIGN_RIGHT };
                    promotionAmountTable.AddCell(promotionAmountCell);

                    // Add the table to the document instead of the paragraph
                    document.Add(promotionAmountTable);

                }

                // Dòng tổng tiền
                //Paragraph totalAmountParagraph = new Paragraph(totalAmountText + CurrentOrder.Amount.GetValueOrDefault().ToString("C", CultureInfo.GetCultureInfo("vi-VN")), new Font(baseFont, 14, Font.BOLD));
                //totalAmountParagraph.Alignment = Element.ALIGN_RIGHT; // Set alignment to right
                //document.Add(totalAmountParagraph); // Add the paragraph to the document
                // Create a table with 2 columns
                PdfPTable totalAmountTable = new PdfPTable(2);
                totalAmountTable.WidthPercentage = 100; // Make the table span the width of the document

                // Create the left cell for the text
                PdfPCell textCell = new PdfPCell(new Phrase(totalAmountText, new Font(baseFont, 14, Font.BOLD))) { Border = PdfPCell.NO_BORDER, HorizontalAlignment = Element.ALIGN_LEFT };
                totalAmountTable.AddCell(textCell);

                // Create the right cell for the amount
                string formattedAmount = CurrentOrder.Amount.GetValueOrDefault().ToString("C", CultureInfo.GetCultureInfo("vi-VN"));
                PdfPCell amountCell = new PdfPCell(new Phrase(formattedAmount, new Font(baseFont, 14, Font.BOLD))) { Border = PdfPCell.NO_BORDER, HorizontalAlignment = Element.ALIGN_RIGHT };
                totalAmountTable.AddCell(amountCell);

                // Add the table to the document instead of the paragraph
                document.Add(totalAmountTable);

                document.Add(new Paragraph(" ")); // Add a blank line
                Font boldCenterFont = new Font(baseFont, 12, Font.BOLD);
                Paragraph dottedLine = new Paragraph("............................", boldCenterFont) { Alignment = Element.ALIGN_CENTER };
                document.Add(dottedLine);
                Paragraph thankNoteParagraph = new Paragraph(thankNote, new Font(baseFont, 12, Font.BOLD));
                thankNoteParagraph.Alignment = Element.ALIGN_CENTER; // Set alignment to center
                document.Add(thankNoteParagraph); // Add the paragraph to the document
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
                                Font font = FontFactory.GetFont("Times New Roman", BaseFont.IDENTITY_H, BaseFont.EMBEDDED, 12);
                                if (CurrentOrder.ListOrderDetail == null)
                                {
                                    return;
                                }
                                List<OrderDetail> items = CurrentOrder.ListOrderDetail.Where(item => item.OrderDetailStatus == EnumOrderDetailStatus.NotSentKitchen).ToList();
                                document.Open();

                            // Add Store Information
                            document.Add(new Paragraph("Gửi bếp", FontFactory.GetFont("Times New Roman", 18, Font.BOLD)));
                            document.Add(new Paragraph($"In tại: {DateTime.Now.ToString()}"));
                            document.Add(new Paragraph(" ")); // Add a blank line
                                // Add Store Information
                                document.Add(new Paragraph("Send kitchen tampt", FontFactory.GetFont("Times New Roman", 18, Font.BOLD)));
                                document.Add(new Paragraph($"Print at: {DateTime.Now.ToString()}"));
                                document.Add(new Paragraph(" ")); // Add a blank line

                            // Add Items
                            PdfPTable table = new PdfPTable(3);
                            PdfPCell cel11 = new PdfPCell(new iTextSharp.text.Phrase("Tên món")) { HorizontalAlignment = iTextSharp.text.Element.ALIGN_CENTER  };
                            table.AddCell(cel11);
                            PdfPCell cel12 = new PdfPCell(new iTextSharp.text.Phrase("Số lượng")) { HorizontalAlignment = iTextSharp.text.Element.ALIGN_CENTER};
                            table.AddCell(cel12);
                            PdfPCell cel13 = new PdfPCell(new iTextSharp.text.Phrase("Số tiền")) { HorizontalAlignment = iTextSharp.text.Element.ALIGN_CENTER };
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
