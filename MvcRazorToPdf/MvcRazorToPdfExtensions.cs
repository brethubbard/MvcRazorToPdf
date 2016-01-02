using System;
using System.Web.Mvc;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.Collections.Generic;

namespace MvcRazorToPdf
{
    public static class MvcRazorToPdfExtensions
    {
        public static byte[] GeneratePdf(this ControllerContext context, object model=null, string viewName=null,
            Action<PdfWriter, Document> configureSettings=null)
        {
            return new MvcRazorToPdf().GeneratePdfOutput(context, model, viewName, configureSettings);
        }
        public static byte[] GeneratePdf(this ControllerContext context, IEnumerable<Tuple<string, object>> viewsAndModels,
            Action<PdfWriter, Document> configureSettings = null)
        {
            return new MvcRazorToPdf().GeneratePdfOutput(context, viewsAndModels, configureSettings);
        }
    }
}