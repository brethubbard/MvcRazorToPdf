using System.Collections.Generic;
using SysIO = System.IO;
using System.Web.Mvc;
using iTextSharp.text;
using MvcRazorToPdf;
using MvcRazorToPdfExample.Models;
using System;

namespace MvcRazorToPdfExample.Controllers
{
    public class PdfController : Controller
    {
        public ActionResult Index()
        {
            var model = new PdfExample
            {
                Heading = "Heading",
                Items = new List<BasketItem>
                {
                    new BasketItem
                    {
                        Id = 1,
                        Description = "Item 1",
                        Price = 1.99m
                    },
                    new BasketItem
                    {
                        Id = 2,
                        Description = "Item 2",
                        Price = 2.99m
                    }
                }
            };

            return new PdfActionResult(model);
        }

        public ActionResult SaveToAppData()
        {
            var model = new PdfExample
            {
                Heading = "Heading",
                Items = new List<BasketItem>
                {
                    new BasketItem
                    {
                        Id = 1,
                        Description = "Item 1",
                        Price = 1.99m
                    },
                    new BasketItem
                    {
                        Id = 2,
                        Description = "Item 2",
                        Price = 2.99m
                    }
                }
            };

            byte[] pdfOutput = ControllerContext.GeneratePdf(model, "IndexWithAccessToDocumentAndWriter");
            string fullPath = Server.MapPath("~/App_Data/FreshlyMade.pdf");

            if (SysIO.File.Exists(fullPath))
            {
                SysIO.File.Delete(fullPath);
            }
            SysIO.File.WriteAllBytes(fullPath, pdfOutput);

            return View("SaveToAppData");
        }

        public ActionResult IndexWithAccessToDocumentAndWriter()
        {
            var model = new PdfExample
            {
                Heading = "Heading",
                Items = new List<BasketItem>
                {
                    new BasketItem
                    {
                        Id = 1,
                        Description = "Item 1",
                        Price = 1.99m
                    },
                    new BasketItem
                    {
                        Id = 2,
                        Description = "Item 2",
                        Price = 2.99m
                    }
                }
            };
            
            return new PdfActionResult(model, (writer, document) =>
            {
                document.SetPageSize(new Rectangle(500f, 500f, 90));
                document.NewPage();
            });
        }

        public ActionResult IndexWithAccessToDocumentAndWriterDownloadFile()
        {
            var model = new PdfExample
            {
                Heading = "Heading",
                Items = new List<BasketItem>
                {
                    new BasketItem
                    {
                        Id = 1,
                        Description = "Item 1",
                        Price = 1.99m
                    },
                    new BasketItem
                    {
                        Id = 2,
                        Description = "Item 2",
                        Price = 2.99m
                    }
                }
            };

            return new PdfActionResult(model, (writer, document) =>
            {
                document.SetPageSize(new Rectangle(500f, 500f, 90));
                document.NewPage();
            })
            {
                FileDownloadName = "ElanWasHere.pdf"
            };
        }

        public ActionResult MultipleViewsAndModels()
        {
            var viewsAndModels = new List<Tuple<string, object>>();
            var model1 = new PdfExample
            {
                Heading = "John's Items",
                Items = new List<BasketItem>
                {
                    new BasketItem
                    {
                        Id = 1,
                        Description = "Item 1",
                        Price = 1.99m
                    },
                    new BasketItem
                    {
                        Id = 2,
                        Description = "Item 2",
                        Price = 2.99m
                    }
                }
            };
            var model2 = new PdfExample
            {
                Heading = "Joe's Items",
                Items = new List<BasketItem>
                {
                    new BasketItem
                    {
                        Id = 1,
                        Description = "Item 3",
                        Price = 3.99m
                    },
                    new BasketItem
                    {
                        Id = 2,
                        Description = "Item 4",
                        Price = 4.99m
                    }
                }
            };
            var view = "IndexWithAccessToDocumentAndWriter";
            viewsAndModels.Add(new Tuple<string, object>(view, model1));
            viewsAndModels.Add(new Tuple<string, object>(view, model2));
            return new PdfActionResult(viewsAndModels, (writer, document) =>
            {
                document.SetPageSize(new Rectangle(500f, 500f, 90));
                document.NewPage();
            });
        }
    }
}