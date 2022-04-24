using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CourseChentsov.Models.ViewModel
{
    public class HtmlResult : ActionResult
    {
        private string htmlCode;
        public HtmlResult(string html)
        {
            htmlCode = html;
        }
        public override void ExecuteResult(ControllerContext context)
        {
            string fullHtmlCode = "<div class='modal-content'>";
            fullHtmlCode += "<div class='modal-header'>";
            fullHtmlCode += "<button class='close' data-dismiss='modal' area-hidden='true'>X</button><h4>Сообщение</h4></div>";
            fullHtmlCode += "<div class='modal-body'>" + htmlCode + "</div>";
            fullHtmlCode += "</div>";
            context.HttpContext.Response.Write(fullHtmlCode);
        }
    }
}