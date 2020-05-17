using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebApplication.Helpers {
    /// <summary>
    /// AjaxAntiForgeryTokenHelper:  Razorフォームを利用できない時の、AjaxのXSRF対策用
    /// AntiForgeryTokenコード取得
    /// </summary>
    public static class AjaxAntiForgeryTokenHelper {
        public static string AjaxAntiForgeryToken(this HtmlHelper helper) {
            string antiForgeryInputTag = helper.AntiForgeryToken().ToString();

            // Above gets the following: <input name="__RequestVerificationToken" type="hidden" value="PnQE7R0MIBBAzC7SqtVvwrJpGbRvPgzWHo5dSyoSaZoabRjf9pCyzjujYBU_qKDJmwIOiPRDwBV1TNVdXFVgzAvN9_l2yt9-nf4Owif0qIDz7WRAmydVPIm6_pmJAI--wvvFQO7g0VvoFArFtAR2v6Ch1wmXCZ89v0-lNOGZLZc1" />
            string removedStart = antiForgeryInputTag.Replace(@"<input name=""__RequestVerificationToken"" type=""hidden"" value=""", "");
            string tokenValue = removedStart.Replace(@""" />", "");
            
            if (antiForgeryInputTag == removedStart || removedStart == tokenValue) {
                throw new InvalidOperationException("Oops! The Html.AntiForgeryToken() method seems to return something I did not expect.");
            }
            
            return tokenValue;
        }
    }
}
