using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Services;

namespace WebServiceDemo {
    /// <summary>
    /// WebServiceDemo 的摘要说明
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // 若要允许使用 ASP.NET AJAX 从脚本中调用此 Web 服务，请取消注释以下行。 
    // [System.Web.Script.Services.ScriptService]
    public class WebServiceDemo : System.Web.Services.WebService {

        [WebMethod]
        public string HelloWorld() {
            return "Hello World";
        }

        [WebMethod(Description = "上传MES方法")]
        public string WriteDataToMes(DataTable dt1MES, DataTable dt2MES, out string strMsg) {
            strMsg = "";
            for (int i = 0; i < dt1MES.Columns.Count; i++) {
                strMsg += dt1MES.Rows[0][i].ToString() + " | ";
            }
            strMsg = strMsg.Substring(0, strMsg.Length - 3) + " / ";
            for (int i = 0; i < dt2MES.Rows.Count; i++) {
                for (int j = 0; j < dt2MES.Columns.Count; j++) {
                    strMsg += dt2MES.Rows[i][j].ToString() + " | ";
                }
            }
            strMsg = strMsg.Substring(0, strMsg.Length - 3);
            return "OK";
        }

    }
}
