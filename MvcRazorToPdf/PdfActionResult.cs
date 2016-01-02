using System;
using System.Web.Mvc;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.Collections.Generic;

namespace MvcRazorToPdf
{
    public class PdfActionResult : ActionResult
    {
        public PdfActionResult(string viewName, object model)
        {
            ViewName = viewName;
            Model = model;
        }

        public PdfActionResult(object model)
        {
            Model = model;
        }

        public PdfActionResult(object model, Action<PdfWriter, Document> configureSettings)
        {
            if (configureSettings == null) 
                throw new ArgumentNullException("configureSettings");
            Model = model;
            ConfigureSettings = configureSettings;
        }

        public PdfActionResult(string viewName, object model, Action<PdfWriter, Document> configureSettings)
        {
            if (configureSettings == null) 
                throw new ArgumentNullException("configureSettings");
            ViewName = viewName;
            Model = model;
            ConfigureSettings = configureSettings;
        }
        /// <summary>
        /// Action result where an IEnumberable of tuples of views and objects are used to generate 
        /// a single PDF document for all the views and models provided
        /// </summary>
        /// <param name="viewsAndModels">An IEnumerable of Tuples containing the name of the view and the object the view is expecting.</param>
        /// <param name="configureSettings">Action to call for configuring the settings before writing the PDF document.</param>
        public PdfActionResult(IEnumerable<Tuple<string, object>> viewsAndModels, Action<PdfWriter, Document> configureSettings = null)
        {
            ConfigureSettings = configureSettings;
            ViewsAndModels = viewsAndModels;
        }

        public string ViewName { get; set; }
        public object Model { get; set; }
        public Action<PdfWriter, Document> ConfigureSettings { get; set; }
        public IEnumerable<Tuple<string, object>> ViewsAndModels { get; set; }

        public string FileDownloadName { get; set; }

        public override void ExecuteResult(ControllerContext context)
        {
            IView viewEngineResult;
            ViewContext viewContext;

            if (ViewName == null)
            {
                ViewName = context.RouteData.GetRequiredString("action");
            }

            context.Controller.ViewData.Model = Model;


            if (context.HttpContext.Request.QueryString["html"] != null &&
                context.HttpContext.Request.QueryString["html"].ToLower().Equals("true") &&
                ViewsAndModels != null)
            {
                RenderHtmlOutput(context);
            }
            else
            {
                if (!String.IsNullOrEmpty(FileDownloadName))
                {
                    context.HttpContext.Response.AddHeader("content-disposition",
                        "attachment; filename=" + FileDownloadName);
                }
                if(ViewsAndModels == null)
                {
                    new FileContentResult(context.GeneratePdf(Model, ViewName, ConfigureSettings), "application/pdf")
                    .ExecuteResult(context);
                }
                else
                {
                    new FileContentResult(context.GeneratePdf(ViewsAndModels, ConfigureSettings), "application/pdf")
                    .ExecuteResult(context);
                }
            }
        }

        private void RenderHtmlOutput(ControllerContext context)
        {
            IView viewEngineResult = ViewEngines.Engines.FindView(context, ViewName, null).View;
            var viewContext = new ViewContext(context, viewEngineResult, context.Controller.ViewData,
                context.Controller.TempData, context.HttpContext.Response.Output);
            viewEngineResult.Render(viewContext, context.HttpContext.Response.Output);
        }
    }
}