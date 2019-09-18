using System;
using System.CodeDom;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using System.Web.Services.Description;
using System.Xml.Serialization;

namespace SH_OBD {
    public class WSHelper {
        /// <summary>
        /// 输出的dll文件名称
        /// </summary>
        private static string m_OutputDllFilename;

        /// <summary>
        /// WebService代理类名称
        /// </summary>
        private static string m_ProxyClassName;

        /// <summary>
        /// WebService代理类实例
        /// </summary>
        private static object m_ObjInvoke;

        /// <summary>
        /// WebService方法名
        /// </summary>
        private static string[] m_Methods;

        /// <summary>
        /// 接口方法字典
        /// </summary>
        private static Dictionary<string, MethodInfo> m_MethodDic = new Dictionary<string, MethodInfo>();

        /// <summary>
        /// 创建WebService，生成客户端代理程序集文件
        /// </summary>
        /// <param name="error">错误信息</param>
        /// <returns>返回：true或false</returns>
        public static bool CreateWebService(DBandMES dbandMES, out string error) {
            try {
                error = string.Empty;
                m_OutputDllFilename = dbandMES.WebServiceName + ".dll";
                m_ProxyClassName = dbandMES.WebServiceName;
                m_Methods = dbandMES.GetMethodArray();
                string webServiceUrl = dbandMES.WebServiceAddress + dbandMES.WebServiceName + ".asmx";
                webServiceUrl += "?WSDL";

                // 如果程序集已存在，直接使用
                if (File.Exists(Path.Combine(Environment.CurrentDirectory, m_OutputDllFilename))) {
                    BuildMethods(dbandMES.GetMethodArray());
                    return true;
                }

                //使用 WebClient 下载 WSDL 信息。
                WebClient web = new WebClient();
                Stream stream = web.OpenRead(webServiceUrl);

                //创建和格式化 WSDL 文档。
                if (stream != null) {
                    // 格式化WSDL
                    ServiceDescription description = ServiceDescription.Read(stream);

                    // 创建客户端代理类。
                    ServiceDescriptionImporter importer = new ServiceDescriptionImporter {
                        ProtocolName = "Soap",
                        Style = ServiceDescriptionImportStyle.Client,
                        CodeGenerationOptions =
                            CodeGenerationOptions.GenerateProperties | CodeGenerationOptions.GenerateNewAsync
                    };

                    // 添加 WSDL 文档。
                    importer.AddServiceDescription(description, null, null);

                    //使用 CodeDom 编译客户端代理类。
                    CodeNamespace nmspace = new CodeNamespace();
                    CodeCompileUnit unit = new CodeCompileUnit();
                    unit.Namespaces.Add(nmspace);

                    ServiceDescriptionImportWarnings warning = importer.Import(nmspace, unit);
                    CodeDomProvider provider = CodeDomProvider.CreateProvider("CSharp");

                    CompilerParameters parameter = new CompilerParameters {
                        GenerateExecutable = false,
                        // 指定输出dll文件名。
                        OutputAssembly = m_OutputDllFilename
                    };

                    parameter.ReferencedAssemblies.Add("System.dll");
                    parameter.ReferencedAssemblies.Add("System.XML.dll");
                    parameter.ReferencedAssemblies.Add("System.Web.Services.dll");
                    parameter.ReferencedAssemblies.Add("System.Data.dll");

                    // 编译输出程序集
                    CompilerResults result = provider.CompileAssemblyFromDom(parameter, unit);

                    // 使用 Reflection 调用 WebService。
                    if (!result.Errors.HasErrors) {
                        BuildMethods(dbandMES.GetMethodArray());
                        return true;
                    } else {
                        error = "反射生成dll文件时异常";
                    }
                    stream.Close();
                    stream.Dispose();
                } else {
                    error = "打开WebServiceUrl失败";
                }
            } catch (Exception ex) {
                error = ex.Message;
            }
            return false;
        }

        /// <summary>
        /// 反射构建Methods
        /// </summary>
        private static void BuildMethods(string[] methods) {
            Assembly asm = Assembly.LoadFrom(m_OutputDllFilename);
            //var types = asm.GetTypes();
            Type asmType = asm.GetType(m_ProxyClassName);
            m_ObjInvoke = Activator.CreateInstance(asmType);

            //var methods = asmType.GetMethods();
            foreach (string method in methods) {
                MethodInfo methodInfo = asmType.GetMethod(method);
                if (methodInfo != null) {
                    m_MethodDic.Add(method, methodInfo);
                }
            }
        }

        /// <summary>
        /// 获取请求响应
        /// </summary>
        /// <param name="method">方法</param>
        /// <param name="para">参数</param>
        /// <returns>返回：string</returns>
        public static string GetResponseString(string method, params object[] para) {
            string result = null;
            if (m_MethodDic.ContainsKey(method)) {
                var temp = m_MethodDic[method].Invoke(m_ObjInvoke, para);
                if (temp != null) {
                    result = temp.ToString();
                }
            }
            return result;
        }

        public static string GetResponseOutString(string method, out string strMsg, params object[] paraIn) {
            strMsg = "";
            string result = null;
            object[] para = new object[paraIn.Length + 1];
            for (int i = 0; i < paraIn.Length; i++) {
                para[i] = paraIn[i];
            }
            para[paraIn.Length] = strMsg;
            if (m_MethodDic.ContainsKey(method)) {
                var temp = m_MethodDic[method].Invoke(m_ObjInvoke, para);
                if (temp != null) {
                    result = temp.ToString();
                }
                strMsg = para[paraIn.Length].ToString();
            }
            return result;
        }

        public static string GetMethodName(int index) {
            if (m_Methods != null && m_Methods.Length > index) {
                return m_Methods[index];
            } else {
                return "";
            }
        }

    }

    /// <summary>
    /// 请求信息帮助
    /// </summary>
    public partial class HttpHelper {
        private static HttpHelper m_Helper;
        /// <summary>
        /// 单例
        /// </summary>
        public static HttpHelper Helper {
            get { return m_Helper ?? (m_Helper = new HttpHelper()); }
        }

        /// <summary>
        /// 获取请求的数据
        /// </summary>
        /// <param name="strUrl">请求地址</param>
        /// <param name="requestMode">请求方式</param>
        /// <param name="parameters">参数</param>
        /// <param name="requestCoding">请求编码</param>
        /// <param name="responseCoding">响应编码</param>
        /// <param name="timeout">请求超时时间（毫秒）</param>
        /// <returns>返回：请求成功响应信息，失败返回null</returns>
        public string GetResponseString(string strUrl, ERequestMode requestMode, Dictionary<string, string> parameters, Encoding requestCoding, Encoding responseCoding, out string error, int timeout = 300) {
            error = "";
            string url = VerifyUrl(strUrl);
            HttpWebRequest webRequest = (HttpWebRequest)WebRequest.Create(new Uri(url));

            HttpWebResponse webResponse = null;
            switch (requestMode) {
            case ERequestMode.Get:
                webResponse = GetRequest(webRequest, out error, timeout);
                break;
            case ERequestMode.Post:
                webResponse = PostRequest(webRequest, parameters, requestCoding, out error, timeout);
                break;
            }

            if (webResponse != null && webResponse.StatusCode == HttpStatusCode.OK) {
                using (Stream newStream = webResponse.GetResponseStream()) {
                    if (newStream != null)
                        using (StreamReader reader = new StreamReader(newStream, responseCoding)) {
                            string result = reader.ReadToEnd();
                            return result;
                        }
                }
            }
            return null;
        }


        /// <summary>
        /// get 请求指定地址返回响应数据
        /// </summary>
        /// <param name="webRequest">请求</param>
        /// <param name="timeout">请求超时时间（毫秒）</param>
        /// <returns>返回：响应信息</returns>
        private HttpWebResponse GetRequest(HttpWebRequest webRequest, out string error, int timeout) {
            try {
                webRequest.Accept = "text/html, application/xhtml+xml, application/json, text/javascript, */*; q=0.01";
                webRequest.Headers.Add("Accept-Language", "zh-cn,en-US,en;q=0.5");
                webRequest.Headers.Add("Cache-Control", "no-cache");
                webRequest.UserAgent = "DefaultUserAgent";
                webRequest.Timeout = timeout;
                webRequest.Method = "GET";

                // 接收返回信息
                HttpWebResponse webResponse = (HttpWebResponse)webRequest.GetResponse();
                error = "";
                return webResponse;
            } catch (Exception ex) {
                error = ex.Message;
                return null;
            }
        }


        /// <summary>
        /// post 请求指定地址返回响应数据
        /// </summary>
        /// <param name="webRequest">请求</param>
        /// <param name="parameters">传入参数</param>
        /// <param name="timeout">请求超时时间（毫秒）</param>
        /// <param name="requestCoding">请求编码</param>
        /// <returns>返回：响应信息</returns>
        private HttpWebResponse PostRequest(HttpWebRequest webRequest, Dictionary<string, string> parameters, Encoding requestCoding, out string error, int timeout) {
            try {
                // 拼接参数
                string postStr = string.Empty;
                if (parameters != null) {
                    parameters.All(o => {
                        if (string.IsNullOrEmpty(postStr))
                            postStr = string.Format("{0}={1}", o.Key, o.Value);
                        else
                            postStr += string.Format("&{0}={1}", o.Key, o.Value);

                        return true;
                    });
                }

                byte[] byteArray = requestCoding.GetBytes(postStr);
                webRequest.Accept = "text/html, application/xhtml+xml, application/json, text/javascript, */*; q=0.01";
                webRequest.Headers.Add("Accept-Language", "zh-cn,en-US,en;q=0.5");
                webRequest.Headers.Add("Cache-Control", "no-cache");
                webRequest.UserAgent = "DefaultUserAgent";
                webRequest.Timeout = timeout;
                webRequest.ContentType = "application/x-www-form-urlencoded";
                webRequest.ContentLength = byteArray.Length;
                webRequest.Method = "POST";

                // 将参数写入流
                using (Stream newStream = webRequest.GetRequestStream()) {
                    newStream.Write(byteArray, 0, byteArray.Length);
                    newStream.Close();
                }

                // 接收返回信息
                HttpWebResponse webResponse = (HttpWebResponse)webRequest.GetResponse();
                error = "";
                return webResponse;
            } catch (Exception ex) {
                error = ex.Message;
                return null;
            }
        }

        /// <summary>
        /// 验证URL
        /// </summary>
        /// <param name="url">待验证 URL</param>
        /// <returns></returns>
        private string VerifyUrl(string url) {
            if (string.IsNullOrEmpty(url))
                throw new Exception("URL 地址不可以为空！");

            if (url.StartsWith("http://", StringComparison.CurrentCultureIgnoreCase))
                return url;

            return string.Format("http://{0}", url);
        }

        public enum ERequestMode {
            Get,
            Post
        }

    }
}
