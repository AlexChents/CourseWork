using CourseChentsov.Helpers;
using CourseChentsov.Models;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CourseChentsov.Controllers
{
    [Authorize]
    public class PrintController : Controller
    {
        private CourseContext db = new CourseContext();

        public FileResult PrintDeclarations(string arrayDeclaration)
        {
            var id = arrayDeclaration.Split(',');
            List<int> idInt = new List<int>();
            for (int i = 0; i < id.Length; i++)
            {
                idInt.Add(Convert.ToInt32(id[i]));
            }

            for (int i = 0; i < idInt.Count(); i++)
            {
                int index = idInt[i];
                Package package = db.Packages.FirstOrDefault(p => p.Id == index);
                package.IsPrint = true;
                db.SaveChanges();
            }

            iTextSharp.text.Document document = new iTextSharp.text.Document(PageSize.B7, 5, 5, 5, 5);
            var pdfWriter = PdfWriter.GetInstance(document, new FileStream(Server.MapPath("~/Files/package.pdf"), FileMode.Create));
            document.Open();
            //Определение шрифта необходимо для сохранения кириллического текста
            //Иначе мы не увидим кириллический текст
            //Если мы работаем только с англоязычными текстами, то шрифт можно не указывать
            BaseFont baseFont = BaseFont.CreateFont("C:\\Windows\\Fonts\\arial.ttf", BaseFont.IDENTITY_H, BaseFont.NOT_EMBEDDED);
            iTextSharp.text.Font font = new iTextSharp.text.Font(baseFont, iTextSharp.text.Font.DEFAULTSIZE, iTextSharp.text.Font.NORMAL);

            //Обход по всем таблицам датасета (хотя в данном случае мы можем опустить
            //Так как в нашей бд только одна таблица)

            for (int i = 0; i < idInt.Count; i++)
            {
                var declaration = db.Packages.Find(idInt[i]);
                //Создаем объект таблицы и передаем в нее число столбцов таблицы из нашего датасета
                for (int j = 1; j <= declaration.CountSeats; j++)
                {
                    PdfPTable table = new PdfPTable(10);

                    //Добавим в таблицу общий заголовок
                    PdfPCell cell = new PdfPCell(new Phrase("№" + declaration.NumberDelivery, new Font(baseFont, 20, Font.BOLDITALIC)))
                    {
                        Colspan = 10,
                        HorizontalAlignment = 1,
                        //Убираем границу первой ячейки, чтобы балы как заголовок
                        BorderWidthBottom = 2,
                        BorderWidthLeft = 2,
                        BorderWidthRight = 2,
                        BorderWidthTop = 2,
                        Padding = 5
                    };
                    table.AddCell(cell);

                    cell = new PdfPCell(new Phrase(declaration.DepartmentRecipient.City.Region.RegionName, font))
                    {
                        Colspan = 10,
                        //Убираем границу первой ячейки, чтобы балы как заголовок
                        BorderWidthLeft = 2,
                        BorderWidthRight = 2,
                        Padding = 5
                    };
                    table.AddCell(cell);

                    cell = new PdfPCell(new Phrase(declaration.DepartmentRecipient.City.Name, font))
                    {
                        Colspan = 10,
                        //Убираем границу первой ячейки, чтобы балы как заголовок
                        BorderWidthBottom = 1,
                        BorderWidthLeft = 2,
                        BorderWidthRight = 2,
                        Padding = 5
                    };
                    table.AddCell(cell);


                    cell = new PdfPCell(new Phrase("ОТ: " + declaration.DateSend.ToShortDateString(), font))
                    {
                        Colspan = 5,
                        HorizontalAlignment = 1,
                        //Убираем границу первой ячейки, чтобы балы как заголовок
                        BorderWidthBottom = 1,
                        BorderWidthLeft = 2,
                        BorderWidthRight = 1,
                        BorderWidthTop = 1,
                        Padding = 5
                    };

                    table.AddCell(cell);
                    cell = new PdfPCell(new Phrase("КОМУ: ", font))
                    {
                        Colspan = 5,
                        HorizontalAlignment = 1,
                        //Убираем границу первой ячейки, чтобы балы как заголовок
                        BorderWidthBottom = 1,
                        BorderWidthLeft = 1,
                        BorderWidthRight = 2,
                        BorderWidthTop = 1,
                        Padding = 5
                    };

                    table.AddCell(cell);
                    cell = new PdfPCell(new Phrase(declaration.User.SenderName + " " + declaration.User.LastName + " " + declaration.User.FirstName + " " +
                        declaration.User.ThirdName + " " + declaration.User.PhoneNumber + " " +
                        db.Cities.Where(c => c.Id == declaration.DepartmentSendId).Select(c => c.Name).FirstOrDefault() +
                        ", Отделение №" + declaration.DepartmentSendId, font))
                    {
                        Colspan = 5,
                        HorizontalAlignment = 0,
                        //Убираем границу первой ячейки, чтобы балы как заголовок
                        BorderWidthBottom = 1,
                        BorderWidthLeft = 2,
                        BorderWidthRight = 1,
                        BorderWidthTop = 1,
                        Padding = 5
                    };

                    table.AddCell(cell);
                    cell = new PdfPCell(new Phrase(declaration.Recipient.RecipientName + " " + declaration.Recipient.LastName + " " +
                        declaration.Recipient.FirstName + " " + declaration.Recipient.ThirdName + " " +
                        declaration.Recipient.PhoneNumber + " " + declaration.DepartmentRecipient.City.Name + ", Отделение №" + declaration.DepartmentRecipientId, font))
                    {
                        Colspan = 5,
                        HorizontalAlignment = 0,
                        //Убираем границу первой ячейки, чтобы балы как заголовок
                        BorderWidthBottom = 1,
                        BorderWidthLeft = 1,
                        BorderWidthRight = 2,
                        BorderWidthTop = 1,
                        Padding = 5
                    };

                    table.AddCell(cell);
                    cell = new PdfPCell(new Phrase("Стоимость доставки: " + declaration.DeliveryCost + " грн.", font))
                    {
                        Colspan = 7,
                        HorizontalAlignment = 0,
                        //Убираем границу первой ячейки, чтобы балы как заголовок
                        BorderWidthBottom = 1,
                        BorderWidthLeft = 2,
                        BorderWidthRight = 1,
                        BorderWidthTop = 1,
                        Padding = 5
                    };

                    table.AddCell(cell);
                    cell = new PdfPCell(new Phrase("Вес: " + declaration.Weight + " кг.", font))
                    {
                        Colspan = 3,
                        HorizontalAlignment = 0,
                        //Убираем границу первой ячейки, чтобы балы как заголовок
                        BorderWidthBottom = 1,
                        BorderWidthLeft = 1,
                        BorderWidthRight = 2,
                        BorderWidthTop = 1,
                        Padding = 5
                    };

                    table.AddCell(cell);
                    cell = new PdfPCell(new Phrase("Место: " + j, font))
                    {
                        Colspan = 5,
                        HorizontalAlignment = 1,
                        //Убираем границу первой ячейки, чтобы балы как заголовок
                        BorderWidthBottom = 1,
                        BorderWidthLeft = 2,
                        BorderWidthRight = 1,
                        BorderWidthTop = 1,
                        Padding = 5
                    };

                    table.AddCell(cell);
                    cell = new PdfPCell(new Phrase("ИЗ: " + declaration.CountSeats, font))
                    {
                        Colspan = 5,
                        HorizontalAlignment = 1,
                        //Убираем границу первой ячейки, чтобы балы как заголовок
                        BorderWidthBottom = 1,
                        BorderWidthLeft = 1,
                        BorderWidthRight = 2,
                        BorderWidthTop = 1,
                        Padding = 5
                    };

                    table.AddCell(cell);
                    cell = new PdfPCell(new Phrase("Комментарий: " + declaration.Comment, font))
                    {
                        Colspan = 10,
                        HorizontalAlignment = 1,
                        //Убираем границу первой ячейки, чтобы балы как заголовок
                        BorderWidthBottom = 2,
                        BorderWidthLeft = 2,
                        BorderWidthRight = 2,
                        BorderWidthTop = 1,
                        Padding = 5
                    };

                    table.AddCell(cell);

                    document.Add(table);

                    var cb = new PdfContentByte(pdfWriter);
                    BarcodeEAN barcode;
                    if (j > 0 && j < 10)
                        barcode = new BarcodeEAN { CodeType = Barcode.EAN13, Code = declaration.NumberDelivery.ToString() + "0" + j.ToString() };
                    else
                        barcode = new BarcodeEAN { CodeType = Barcode.EAN13, Code = declaration.NumberDelivery.ToString() + j.ToString() };

                    iTextSharp.text.Image img = barcode.CreateImageWithBarcode(cb, BaseColor.BLACK, BaseColor.BLACK);

                    // System.Drawing
                    System.Drawing.Image image = barcode.CreateDrawingImage(System.Drawing.Color.Black, System.Drawing.Color.Black);
                    img.SetAbsolutePosition(80, 20);

                    document.Add(img);

                    document.NewPage();
                }
            }
            document.Close();



            ViewBag.ArrayDeclaration = idInt;
            // Путь к файлу
            string file_path = Server.MapPath("~/Files/package.pdf");
            // Тип файла - content-type
            string file_type = "application/pdf";
            // Имя файла - необязательно
            string file_name = "package.pdf";
            return File(file_path, file_type, file_name);
        }

        public FileResult PrintRegister(int id)
        {
            iTextSharp.text.Document document = new iTextSharp.text.Document(PageSize.A4.Rotate(), 10, 10, 10, 10);

            DispatchRegister dispatchRegister = db.DispatchRegisters.Find(id);
            dispatchRegister.IsPrint = true;
            db.SaveChanges();

            var pdfWriter = PdfWriter.GetInstance(document, new FileStream(Server.MapPath("~/Files/register.pdf"), FileMode.Create));
            document.Open();
            //Определение шрифта необходимо для сохранения кириллического текста
            //Иначе мы не увидим кириллический текст
            //Если мы работаем только с англоязычными текстами, то шрифт можно не указывать
            BaseFont baseFont = BaseFont.CreateFont("C:\\Windows\\Fonts\\arial.ttf", BaseFont.IDENTITY_H, BaseFont.NOT_EMBEDDED);
            iTextSharp.text.Font font = new iTextSharp.text.Font(baseFont, iTextSharp.text.Font.DEFAULTSIZE, iTextSharp.text.Font.NORMAL);


            PdfPTable table = new PdfPTable(14);
            PdfPCell cell = new PdfPCell(new Phrase("Реестр №" + id + " от " + db.DispatchRegisters.FirstOrDefault(dr => dr.Id == id).DateCreate.ToShortDateString(), new Font(baseFont, 25, Font.BOLDITALIC)))
            {
                Colspan = 14,
                HorizontalAlignment = 1,
            };
            table.AddCell(cell);

            cell = new PdfPCell(new Phrase("отправитель: " + db.DispatchRegisters.FirstOrDefault(d => d.Id == id).User.SenderName, new Font(baseFont, 12, Font.BOLDITALIC)))
            {
                Colspan = 6,
                HorizontalAlignment = 1,
                //Убираем границу первой ячейки, чтобы балы как заголовок
                Padding = 5
            };
            table.AddCell(cell);

            cell = new PdfPCell(new Phrase("Получатель: отделение №" + db.Packages.FirstOrDefault(d => d.DispatchRegisterId == id).DepartmentSendId + ", г. " + 
                db.Packages.FirstOrDefault(d => d.DispatchRegisterId == id).DepartmentSend.City.Name + ", " +
                db.Packages.FirstOrDefault(d => d.DispatchRegisterId == id).DepartmentSend.City.Region.RegionName, new Font(baseFont, 12, Font.BOLDITALIC)))
            {
                Colspan = 8,
                HorizontalAlignment = 1,
                //Убираем границу первой ячейки, чтобы балы как заголовок
                BorderWidthBottom = 1,
                BorderWidthLeft = 1,
                BorderWidthRight = 1,
                BorderWidthTop = 1,
                Padding = 5
            };
            table.AddCell(cell);

  
            cell = new PdfPCell(new Phrase("№ п/п", new Font(baseFont, 9, Font.BOLDITALIC)))
            {
                Colspan = 1,
                HorizontalAlignment = 1,
                //Убираем границу первой ячейки, чтобы балы как заголовок
                BorderWidthBottom = 1,
                BorderWidthLeft = 1,
                BorderWidthRight = 1,
                BorderWidthTop = 1,
                Padding = 5
            };
            table.AddCell(cell);

            cell = new PdfPCell(new Phrase("№ декларации", new Font(baseFont, 9, Font.BOLDITALIC)))
            {
                Colspan = 2,
                HorizontalAlignment = 1,
                //Убираем границу первой ячейки, чтобы балы как заголовок
                BorderWidthBottom = 1,
                BorderWidthLeft = 1,
                BorderWidthRight = 1,
                BorderWidthTop = 1,
                Padding = 5
            };
            table.AddCell(cell);

            cell = new PdfPCell(new Phrase("Отправитель", new Font(baseFont, 9, Font.BOLDITALIC)))
            {
                Colspan = 2,
                HorizontalAlignment = 1,
                //Убираем границу первой ячейки, чтобы балы как заголовок
                BorderWidthBottom = 1,
                BorderWidthLeft = 1,
                BorderWidthRight = 1,
                BorderWidthTop = 1,
                Padding = 5
            };
            table.AddCell(cell);

            cell = new PdfPCell(new Phrase("Получатель", new Font(baseFont, 9, Font.BOLDITALIC)))
            {
                Colspan = 2,
                HorizontalAlignment = 1,
                //Убираем границу первой ячейки, чтобы балы как заголовок
                BorderWidthBottom = 1,
                BorderWidthLeft = 1,
                BorderWidthRight = 1,
                BorderWidthTop = 1,
                Padding = 5
            };
            table.AddCell(cell);

            cell = new PdfPCell(new Phrase("Адрес получателя", new Font(baseFont, 9, Font.BOLDITALIC)))
            {
                Colspan = 2,
                HorizontalAlignment = 1,
                //Убираем границу первой ячейки, чтобы балы как заголовок
                BorderWidthBottom = 1,
                BorderWidthLeft = 1,
                BorderWidthRight = 1,
                BorderWidthTop = 1,
                Padding = 5
            };
            table.AddCell(cell);

            cell = new PdfPCell(new Phrase("Количество мест", new Font(baseFont, 9, Font.BOLDITALIC)))
            {
                Colspan = 1,
                HorizontalAlignment = 1,
                //Убираем границу первой ячейки, чтобы балы как заголовок
                BorderWidthBottom = 1,
                BorderWidthLeft = 1,
                BorderWidthRight = 1,
                BorderWidthTop = 1,
                Padding = 5
            };
            table.AddCell(cell);

            cell = new PdfPCell(new Phrase("Вес, кг.", new Font(baseFont, 9, Font.BOLDITALIC)))
            {
                Colspan = 1,
                HorizontalAlignment = 1,
                //Убираем границу первой ячейки, чтобы балы как заголовок
                BorderWidthBottom = 1,
                BorderWidthLeft = 1,
                BorderWidthRight = 1,
                BorderWidthTop = 1,
                Padding = 5
            };
            table.AddCell(cell);

            cell = new PdfPCell(new Phrase("Стоимость доставки", new Font(baseFont, 9, Font.BOLDITALIC)))
            {
                Colspan = 1,
                HorizontalAlignment = 1,
                //Убираем границу первой ячейки, чтобы балы как заголовок
                BorderWidthBottom = 1,
                BorderWidthLeft = 1,
                BorderWidthRight = 1,
                BorderWidthTop = 1,
                Padding = 5
            };
            table.AddCell(cell);

            cell = new PdfPCell(new Phrase("Примечание", new Font(baseFont, 9, Font.BOLDITALIC)))
            {
                Colspan = 2,
                HorizontalAlignment = 1,
                //Убираем границу первой ячейки, чтобы балы как заголовок
                BorderWidthBottom = 1,
                BorderWidthLeft = 1,
                BorderWidthRight = 1,
                BorderWidthTop = 1,
                Padding = 5
            };
            table.AddCell(cell);

            int countDeclarations = db.Packages.Count(p => p.DispatchRegisterId == id);
            var packages = db.Packages.Where(p => p.DispatchRegisterId == id).Select(p => p).ToList();
            int counter = 0;
            int countSeats = 0;
            double costDelivery = 0;
            double weight = 0;
            foreach (var item in packages)
            {
                counter++;
                cell = new PdfPCell(new Phrase(counter.ToString(), new Font(baseFont, 8, Font.NORMAL)))
                {
                    Colspan = 1,
                    HorizontalAlignment = 1,
                    BorderWidthBottom = 1,
                    BorderWidthLeft = 1,
                    BorderWidthRight = 1,
                    BorderWidthTop = 1,
                    Padding = 5
                };
                table.AddCell(cell);

                cell = new PdfPCell(new Phrase(item.NumberDelivery.ToString(), new Font(baseFont, 8, Font.NORMAL)))
                {
                    Colspan = 2,
                    HorizontalAlignment = 1,
                    BorderWidthBottom = 1,
                    BorderWidthLeft = 1,
                    BorderWidthRight = 1,
                    BorderWidthTop = 1,
                    Padding = 5
                };
                table.AddCell(cell);

                cell = new PdfPCell(new Phrase(item.User.SenderName + " " + item.User.LastName + " " + item.User.FirstName + " " + item.User.ThirdName + " " + item.User.PhoneNumber, new Font(baseFont, 8, Font.NORMAL)))
                {
                    Colspan = 2,
                    HorizontalAlignment = 1,
                    BorderWidthBottom = 1,
                    BorderWidthLeft = 1,
                    BorderWidthRight = 1,
                    BorderWidthTop = 1,
                    Padding = 5
                };
                table.AddCell(cell);
                PdfPCell cell1 = new PdfPCell(new Phrase(item.Recipient.RecipientName + " " + item.Recipient.LastName + " " + item.Recipient.FirstName + " " + item.Recipient.ThirdName + " " + item.Recipient.PhoneNumber, new Font(baseFont, 8, Font.NORMAL)))
                {
                    Colspan = 2,
                    HorizontalAlignment = 1,
                    BorderWidthBottom = 1,
                    BorderWidthLeft = 1,
                    BorderWidthRight = 1,
                    BorderWidthTop = 1,
                    Padding = 5
                };
                table.AddCell(cell1);
                
                cell1 = new PdfPCell(new Phrase("Отделение №" + item.DepartmentRecipientId + ", г. " + item.DepartmentRecipient.City.Name, new Font(baseFont, 8, Font.NORMAL)))
                {
                    Colspan = 2,
                    HorizontalAlignment = 1,
                    BorderWidthBottom = 1,
                    BorderWidthLeft = 1,
                    BorderWidthRight = 1,
                    BorderWidthTop = 1,
                    Padding = 5
                };
                table.AddCell(cell1);

                cell = new PdfPCell(new Phrase(item.CountSeats.ToString(), new Font(baseFont, 8, Font.NORMAL)))
                {
                    Colspan = 1,
                    HorizontalAlignment = 1,
                    BorderWidthBottom = 1,
                    BorderWidthLeft = 1,
                    BorderWidthRight = 1,
                    BorderWidthTop = 1,
                    Padding = 5
                };
                table.AddCell(cell);

                cell = new PdfPCell(new Phrase(item.Weight.ToString(), new Font(baseFont, 8, Font.NORMAL)))
                {
                    Colspan = 1,
                    HorizontalAlignment = 1,
                    BorderWidthBottom = 1,
                    BorderWidthLeft = 1,
                    BorderWidthRight = 1,
                    BorderWidthTop = 1,
                    Padding = 5
                };
                table.AddCell(cell);

                cell = new PdfPCell(new Phrase(item.DeliveryCost.ToString(), new Font(baseFont, 8, Font.NORMAL)))
                {
                    Colspan = 1,
                    HorizontalAlignment = 1,
                    BorderWidthBottom = 1,
                    BorderWidthLeft = 1,
                    BorderWidthRight = 1,
                    BorderWidthTop = 1,
                    Padding = 5
                };
                table.AddCell(cell);

                cell = new PdfPCell(new Phrase(item.Comment, new Font(baseFont, 8, Font.NORMAL)))
                {
                    Colspan = 2,
                    HorizontalAlignment = 1,
                    BorderWidthBottom = 1,
                    BorderWidthLeft = 1,
                    BorderWidthRight = 1,
                    BorderWidthTop = 1,
                    Padding = 5
                };
                table.AddCell(cell);
                
                countSeats += item.CountSeats;
                costDelivery += item.DeliveryCost;
                weight += item.Weight;
            }
            
            cell = new PdfPCell(new Phrase("ИТОГО", new Font(baseFont, 9, Font.BOLDITALIC)))
            {
                Colspan = 9,
                BorderWidthBottom = 1,
                BorderWidthLeft = 1,
                BorderWidthRight = 1,
                BorderWidthTop = 1,
                Padding = 5
            };
            table.AddCell(cell);

            cell = new PdfPCell(new Phrase(countSeats.ToString(), new Font(baseFont, 9, Font.BOLDITALIC)))
            {
                Colspan = 1,
                HorizontalAlignment = 1,
                BorderWidthBottom = 1,
                BorderWidthLeft = 1,
                BorderWidthRight = 1,
                BorderWidthTop = 1,
                Padding = 5
            };
            table.AddCell(cell);

            cell = new PdfPCell(new Phrase(weight.ToString(), new Font(baseFont, 9, Font.BOLDITALIC)))
            {
                Colspan = 1,
                HorizontalAlignment = 1,
                BorderWidthBottom = 1,
                BorderWidthLeft = 1,
                BorderWidthRight = 1,
                BorderWidthTop = 1,
                Padding = 5
            };
            table.AddCell(cell);

            cell = new PdfPCell(new Phrase(costDelivery.ToString(), new Font(baseFont, 9, Font.BOLDITALIC)))
            {
                Colspan = 1,
                HorizontalAlignment = 1,
                BorderWidthBottom = 1,
                BorderWidthLeft = 1,
                BorderWidthRight = 1,
                BorderWidthTop = 1,
                Padding = 5
            };
            table.AddCell(cell);

            cell = new PdfPCell(new Phrase("", new Font(baseFont, 9, Font.BOLDITALIC)))
            {
                Colspan = 2,
                HorizontalAlignment = 1,
                BorderWidthBottom = 1,
                BorderWidthLeft = 1,
                BorderWidthRight = 1,
                BorderWidthTop = 1,
                Padding = 5
            };
            table.AddCell(cell);


            cell = new PdfPCell(new Phrase("\n\n\n", new Font(baseFont, 10, Font.BOLDITALIC)))
            {
                Colspan = 14,
            };
            table.AddCell(cell);

            cell = new PdfPCell(new Phrase("Отгрузил________________________________________М.П.", new Font(baseFont, 9, Font.BOLDITALIC)))
            {
                Colspan = 7,
            };
            table.AddCell(cell);

            cell = new PdfPCell(new Phrase("Получил________________________________________М.П.", new Font(baseFont, 9, Font.BOLDITALIC)))
            {
                Colspan = 7,
            };
            table.AddCell(cell);
            document.Add(table);

            document.Close();


            string file_path = Server.MapPath("~/Files/register.pdf");
            // Тип файла - content-type
            string file_type = "application/pdf";
            // Имя файла - необязательно
            string file_name = "register.pdf";
            return File(file_path, file_type, file_name);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
