using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.Services.Protocols;
using Newtonsoft.Json;

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

        [WebMethod(Description = "青云谱工厂上传SAP方法")]
        public string WriteDataToJES(DataTable dt1MES, DataTable dt2MES, out string strMsg) {
            strMsg = "";
            for (int i = 0; i < dt1MES.Columns.Count; i++) {
                if (dt1MES.Rows[0][i].ToString().Length > 0) {
                    strMsg += dt1MES.Rows[0][i].ToString() + " | ";
                }
            }
            strMsg = strMsg.Substring(0, strMsg.Length - 3) + " [[[ ";
            for (int i = 0; i < dt2MES.Rows.Count; i++) {
                for (int j = 0; j < dt2MES.Columns.Count; j++) {
                    strMsg += dt2MES.Rows[i][j].ToString() + " | ";
                }
                strMsg += " /// ";
            }
            strMsg = strMsg.Substring(0, strMsg.Length - 8) + " ]]]";
            SAP_RETURN ret = new SAP_RETURN {
                IsSuccess = true,
                Code = 200,
                Message = "OK",
                MethodParameter = ""
            };
            return JsonConvert.SerializeObject(ret);
        }

        [WebMethod(Description = "小蓝工厂上传MES方法")]
        public string WriteDataToMes(DataTable dt1MES, DataTable dt2MES, out string strMsg) {
            strMsg = "";
            for (int i = 0; i < dt1MES.Columns.Count; i++) {
                if (dt1MES.Rows[0][i].ToString().Length > 0) {
                    strMsg += dt1MES.Rows[0][i].ToString() + " | ";
                }
            }
            strMsg = strMsg.Substring(0, strMsg.Length - 3) + " [[[ ";
            for (int i = 0; i < dt2MES.Rows.Count; i++) {
                for (int j = 0; j < dt2MES.Columns.Count; j++) {
                    strMsg += dt2MES.Rows[i][j].ToString() + " | ";
                }
                strMsg += " /// ";
            }
            strMsg = strMsg.Substring(0, strMsg.Length - 8) + " ]]]";
            return "OK";
        }

        [WebMethod(Description = "SAP通过VIN号获取OBD协议的方法")]
        public Response GetProtocol(Request request) {
            Response response = new Response {
                MESSAGE = "1"
            };
            if (request.ZVIN != null) {
                switch (request.ZVIN[request.ZVIN.Length - 1]) {
                case '0':
                    response.ZODBWZ = "SAE J1939";
                    break;
                case '1':
                    response.ZODBWZ = "SAE 1850 PWM";
                    break;
                case '2':
                    response.ZODBWZ = "SAE 1850 VPW";
                    break;
                case '3':
                    response.ZODBWZ = "ISO 9141";
                    break;
                case '4':
                    response.ZODBWZ = "ISO 14230";
                    break;
                case '5':
                    response.ZODBWZ = "ISO 14230";
                    break;
                case '6':
                    response.ZODBWZ = "ISO 15031";
                    break;
                case '7':
                    response.ZODBWZ = "ISO 27145";
                    break;
                case '8':
                    response.ZODBWZ = "ISO 15765-4";
                    break;
                case '9':
                    response.ZODBWZ = "ISO 15765-4";
                    break;
                default:
                    response.ZODBWZ = "ISO 15765-4";
                    break;
                }
            }
            return response;
        }

    }

    public class SAP_RETURN {
        public bool IsSuccess;
        public int Code;
        public string Message;
        public string MethodParameter;
    }

    public class Request {
        public string ZVIN;
    }

    public partial class Response {
        public string MESSAGE;
        public string ZCSFS;
        public string WERKS;
        public string ZBZL;
        public string ZJ;
        public string ZFDJEDGL;
        public string ZDZZD;
        public string ZFDJEDZS;
        public string PL;
        public string ZQGS;
        public string VEHICLEMODEL;
        public string RLZL;
        public string BSXXS;
        public string QDFS;
        public string QDZW;
        public string ZZL;
        public string PQGSL;
        public string ZPFJD;
        public string ZODBWZ;
    }
}
