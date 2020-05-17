using System;
using System.Web;
using log4net;

namespace WebApplication.Common {
    public class WriteLog {
        private readonly ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly string user = "";

        public WriteLog() {
            log4net.Config.XmlConfigurator.Configure();

            // クライアントIPアドレス取得、開発用サーバーでは空文字になる↓
            user = System.Web.HttpContext.Current.Request.UserHostAddress;
        }

        public void Write() {
            logger.Info(HttpContext.Current.Request.Url.AbsoluteUri + " " + user + " access");
        }

        public void Write(string message) {
            logger.Error(HttpContext.Current.Request.Url.AbsoluteUri + " " + user + " " + "error message: " + message);
        }
    }
}
