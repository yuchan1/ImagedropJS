using System;
using System.IO;
using System.Web;
using System.Web.Mvc;
using System.Web.Util;

namespace WebApplication.Common {

    /// <summary>
    /// ValidateJsonXssAttibute: ： Razorフォームを利用できない時の、AjaxのXSS対策用
    /// [ValidateJsonXssAttibute]属性
    /// </summary>
    public class ValidateJsonXssAttribute : ActionFilterAttribute {
        
        public override void OnActionExecuting(ActionExecutingContext filterContext) {
            var request = filterContext.HttpContext.Request;

            if (request != null && "application/json; charset=UTF-8".Equals(request.ContentType, StringComparison.OrdinalIgnoreCase)) {              
                if (request.ContentLength > 0 && request.Form.Count == 0) {

                    // InputStream has already been read once from "ProcessRequest"
                    if (request.InputStream.Position > 0) {
                        request.InputStream.Position = 0;
                    }
                    
                    using (var reader = new StreamReader(request.InputStream)) {
                        // Get posted JSON content
                        var postedContent = reader.ReadToEnd();
                        
                        // Invoke XSS validation
                        int failureIndex;
                        var isValid = RequestValidator.Current.InvokeIsValidRequestString(
                            HttpContext.Current, postedContent, RequestValidationSource.Form, "postedJson", out failureIndex
                        );

                        // Not valid, so throw request validation exception
                        if (!isValid) {
                            throw new HttpRequestValidationException("Potentially unsafe input detected");
                        }
                    }
                }
            }
        }
    }
}