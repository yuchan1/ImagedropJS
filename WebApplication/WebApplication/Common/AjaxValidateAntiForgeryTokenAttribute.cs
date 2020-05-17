using System.Net;
using System.Web.Helpers;
using System.Web.Mvc;

namespace WebApplication.Common {

    /// <summary>
    /// AjaxValidateAntiForgeryTokenAttrubute： Razorフォームを利用できない時の、AjaxのXSRF対策用
    /// [AjaxValidateAntiForgeryToken]属性
    /// </summary>
    public class AjaxValidateAntiForgeryTokenAttribute : ActionFilterAttribute, IAuthorizationFilter {

        public void OnAuthorization(AuthorizationContext filterContext) {
            var request = filterContext.HttpContext.Request;
            if (request != null) {
                //  Ajax POSTs and normal form posts have to be treated differently when it comes
                //  to validating the AntiForgeryToken
                if (request.HttpMethod == WebRequestMethods.Http.Post || request.HttpMethod == WebRequestMethods.Http.Get) {
                    
                    var antiForgeryCookie = request.Cookies[AntiForgeryConfig.CookieName];
                    var cookieValue = antiForgeryCookie != null ? antiForgeryCookie.Value : null;

                    AntiForgery.Validate(cookieValue, request.Headers["__RequestVerificationToken"]);
                } else {
                    new ValidateAntiForgeryTokenAttribute().OnAuthorization(filterContext);
                }
            }
        }
    }
}
