using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Services;
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
                if (dt1MES.Rows[0][i].ToString() != "") {
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
                if (dt1MES.Rows[0][i].ToString() != "") {
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

    }

    public class JES_REVICE_EQUIP_DATE {
        public string DATA_ID;
        public string SOURCE;
        public string ZH_TESTDATE;
        public string ZH_ANALYMANUF;
        public string ZH_ANALYNAME;
        public string ZH_TETYPE;
        public string ZH_TEMODEL;
        public string ZH_TEMDATE;
        public string ZH_VIN;
        public string ZH_TESTNO;
        public string ZH_DYNOMANUF;
        public string ZH_DYNOMODEL;
        public string ZH_TESTTYPE;
        public string ZH_APASS;
        public string ZH_OPASS;
        public string ZH_EPASS;
        public string ZH_RESULT;
        public string ZH_JCJLNO;
        public string ZH_JCXTNO;
        public string ZH_JCKSSJ;
        public string ZH_JCRJ;
        public string ZH_DPCGJNO;
        public string ZH_CGJXT;
        public string ZH_PFCSSJ;
        public string ZH_JYLX;
        public string ZH_YRFF;
        public string CS_RH;
        public string CS_ET;
        public string CS_AP;
        public string CS_COND;
        public string CS_HCND;
        public string CS_NOXND;
        public string CS_CO2ND;
        public string CS_YND;
        public string SDS_REAC;
        public string SDS_LEAC;
        public string SDS_LRCO;
        public string SDS_LLCO;
        public string SDS_LRHC;
        public string SDS_LLHC;
        public string SDS_HRCO;
        public string SDS_HLCO;
        public string SDS_HRHC;
        public string SDS_HLHC;
        public string SDS_JYWD;
        public string SDS_FDJZS;
        public string SDS_SDSFJCSJ;
        public string SDS_SDSFGKSJ;
        public string SDS_SSZMHCND;
        public string SDS_SSZMCOND;
        public string SDS_SSZMCO2ND;
        public string SDS_SSZMO2ND;
        public string SDS_SSZMGDS;
        public string WT_ARHC5025;
        public string WT_ALHC5025;
        public string WT_ARCO5025;
        public string WT_ALCO5025;
        public string WT_ARNOX5025;
        public string WT_ALNOX5025;
        public string WT_ARHC2540;
        public string WT_ALHC2540;
        public string WT_ARCO2540;
        public string WT_ALCO2540;
        public string WT_ARNOX2540;
        public string WT_ALNOX2540;
        public string WT_ZJHC5025;
        public string WT_ZJCO5025;
        public string WT_ZJNO5025;
        public string WT_ZGL5025;
        public string WT_FDJZS5025;
        public string WT_CS5025;
        public string WT_ZJHC2540;
        public string WT_ZJCO2540;
        public string WT_ZJNO2540;
        public string WT_ZGL2540;
        public string WT_FDJZS2540;
        public string WT_CS2540;
        public string WT_WTJCSJ;
        public string WT_WTGKSJ;
        public string WT_WTZMCS;
        public string WT_WTZMFDJZS;
        public string WT_WTZMFZ;
        public string WT_WTZMHCND;
        public string WT_WTZMCOND;
        public string WT_WTZMNOND;
        public string WT_WTZMCO2ND;
        public string WT_WTZMO2ND;
        public string WT_WTZMZ;
        public string WT_WTNOSDXS;
        public string WT_WTZMXSDF;
        public string WT_WTZMHCNDXZ;
        public string WT_WTZMCONDXZ;
        public string WT_WTZMNONDXZ;
        public string JY_VRHC;
        public string JY_VLHC;
        public string JY_VRCO;
        public string JY_VLCO;
        public string JY_VRNOX;
        public string JY_VLNOX;
        public string JY_JYCSSJ;
        public string JY_JYGL;
        public string JY_JYXSJL;
        public string JY_JYHCPF;
        public string JY_JYCOPF;
        public string JY_JYNOXPF;
        public string JY_JYPLCS;
        public string JY_JYGK;
        public string JY_JYZMCS;
        public string JY_JYZMZS;
        public string JY_JYZMZH;
        public string JY_JYZMHCND;
        public string JY_JYZMHCNDXZ;
        public string JY_JYZMCOND;
        public string JY_JYZMCONDXZ;
        public string JY_JYZMNOXND;
        public string JY_JYZMNOXNDXZ;
        public string JY_JYZMCO2ND;
        public string JY_JYZMO2ND;
        public string JY_JYXSO2ND;
        public string JY_JYXSLL;
        public string JY_JYXSXS;
        public string JY_JYNOSDXZ;
        public string JY_JYZMZ;
        public string ZY_RATEREV;
        public string ZY_REV;
        public string ZY_SMOKEK1;
        public string ZY_SMOKEK2;
        public string ZY_SMOKEK3;
        public string ZY_SMOKEAVG;
        public string ZY_SMOKEKLIMIT;
        public string ZY_ZYGXSZ;
        public string ZY_ZYJCSSJ;
        public string ZY_ZYGKSJ;
        public string ZY_ZYZS;
        public string ZY_YDJZZC;
        public string ZY_YDJMC;
        public string ZY_ZYCCRQ;
        public string ZY_ZYJDRQ;
        public string ZY_ZYJCJL;
        public string ZY_ZYBDJL;
        public string JZ_RATEREVUP;
        public string JZ_RATEREVDOWN;
        public string JZ_REV100;
        public string JZ_MAXPOWER;
        public string JZ_MAXPOWERLIMIT;
        public string JZ_SMOKE100;
        public string JZ_SMOKE80;
        public string JZ_SMOKELIMIT;
        public string JZ_NOX;
        public string JZ_NOXLIMIT;
        public string JZ_JSGXS100;
        public string JZ_JSGXS80;
        public string JZ_JSLBGL;
        public string JZ_JSFDJZS;
        public string JZ_JSJCSJ;
        public string JZ_JSGKSJ;
        public string JZ_JSZMCS;
        public string JZ_JSZMZS;
        public string JZ_JSZMZH;
        public string JZ_JSZMNJ;
        public string JZ_JSZMGXS;
        public string JZ_JSZMCO2ND;
        public string JZ_JSZMNOND;
        public string OBD_OTESTDATE;
        public string OBD_OBD;
        public string OBD_ODO;
        public string OBD_MODULEID;
        public string OBD_ECALID;
        public string OBD_ECVN;
        public string OBD_ACALID;
        public string OBD_ACVN;
        public string OBD_OCALLID;
        public string OBD_OCVN;
        public string HANDLE_STATUS;
        public string HANDLE_MESSAGE;
        public string RECORD_TIME;
        public string IS_DEL;
        public string DEL_TIME;
        public string UPDATE_TIME;
    }

    public class SAP_RETURN {
        public bool IsSuccess;
        public int Code;
        public string Message;
        public string MethodParameter;
    }

}
