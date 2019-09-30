﻿//------------------------------------------------------------------------------
// <auto-generated>
//     此代码由工具生成。
//     运行时版本:4.0.30319.42000
//
//     对此文件的更改可能会导致不正确的行为，并且如果
//     重新生成代码，这些更改将会丢失。
// </auto-generated>
//------------------------------------------------------------------------------

// 
// 此源代码是由 Microsoft.VSDesigner 4.0.30319.42000 版自动生成。
// 
#pragma warning disable 1591

namespace SH_OBD.MES1 {
    using System;
    using System.Web.Services;
    using System.Diagnostics;
    using System.Web.Services.Protocols;
    using System.Xml.Serialization;
    using System.ComponentModel;
    using System.Data;
    
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.8.3752.0")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Web.Services.WebServiceBindingAttribute(Name="WebServiceDemoSoap", Namespace="http://tempuri.org/")]
    public partial class WebServiceDemo : System.Web.Services.Protocols.SoapHttpClientProtocol {
        
        private System.Threading.SendOrPostCallback ReceiveDeviceDataOperationCompleted;
        
        private System.Threading.SendOrPostCallback WriteDataToMesOperationCompleted;
        
        private bool useDefaultCredentialsSetExplicitly;
        
        /// <remarks/>
        public WebServiceDemo() {
            this.Url = global::SH_OBD.Properties.Settings.Default.SH_OBD_MES1_WebServiceDemo;
            if ((this.IsLocalFileSystemWebService(this.Url) == true)) {
                this.UseDefaultCredentials = true;
                this.useDefaultCredentialsSetExplicitly = false;
            }
            else {
                this.useDefaultCredentialsSetExplicitly = true;
            }
        }
        
        public new string Url {
            get {
                return base.Url;
            }
            set {
                if ((((this.IsLocalFileSystemWebService(base.Url) == true) 
                            && (this.useDefaultCredentialsSetExplicitly == false)) 
                            && (this.IsLocalFileSystemWebService(value) == false))) {
                    base.UseDefaultCredentials = false;
                }
                base.Url = value;
            }
        }
        
        public new bool UseDefaultCredentials {
            get {
                return base.UseDefaultCredentials;
            }
            set {
                base.UseDefaultCredentials = value;
                this.useDefaultCredentialsSetExplicitly = true;
            }
        }
        
        /// <remarks/>
        public event ReceiveDeviceDataCompletedEventHandler ReceiveDeviceDataCompleted;
        
        /// <remarks/>
        public event WriteDataToMesCompletedEventHandler WriteDataToMesCompleted;
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://tempuri.org/ReceiveDeviceData", RequestNamespace="http://tempuri.org/", ResponseNamespace="http://tempuri.org/", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public string ReceiveDeviceData(string strIN) {
            object[] results = this.Invoke("ReceiveDeviceData", new object[] {
                        strIN});
            return ((string)(results[0]));
        }
        
        /// <remarks/>
        public void ReceiveDeviceDataAsync(string strIN) {
            this.ReceiveDeviceDataAsync(strIN, null);
        }
        
        /// <remarks/>
        public void ReceiveDeviceDataAsync(string strIN, object userState) {
            if ((this.ReceiveDeviceDataOperationCompleted == null)) {
                this.ReceiveDeviceDataOperationCompleted = new System.Threading.SendOrPostCallback(this.OnReceiveDeviceDataOperationCompleted);
            }
            this.InvokeAsync("ReceiveDeviceData", new object[] {
                        strIN}, this.ReceiveDeviceDataOperationCompleted, userState);
        }
        
        private void OnReceiveDeviceDataOperationCompleted(object arg) {
            if ((this.ReceiveDeviceDataCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.ReceiveDeviceDataCompleted(this, new ReceiveDeviceDataCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://tempuri.org/WriteDataToMes", RequestNamespace="http://tempuri.org/", ResponseNamespace="http://tempuri.org/", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public string WriteDataToMes(System.Data.DataTable dt1MES, System.Data.DataTable dt2MES, out string strMsg) {
            object[] results = this.Invoke("WriteDataToMes", new object[] {
                        dt1MES,
                        dt2MES});
            strMsg = ((string)(results[1]));
            return ((string)(results[0]));
        }
        
        /// <remarks/>
        public void WriteDataToMesAsync(System.Data.DataTable dt1MES, System.Data.DataTable dt2MES) {
            this.WriteDataToMesAsync(dt1MES, dt2MES, null);
        }
        
        /// <remarks/>
        public void WriteDataToMesAsync(System.Data.DataTable dt1MES, System.Data.DataTable dt2MES, object userState) {
            if ((this.WriteDataToMesOperationCompleted == null)) {
                this.WriteDataToMesOperationCompleted = new System.Threading.SendOrPostCallback(this.OnWriteDataToMesOperationCompleted);
            }
            this.InvokeAsync("WriteDataToMes", new object[] {
                        dt1MES,
                        dt2MES}, this.WriteDataToMesOperationCompleted, userState);
        }
        
        private void OnWriteDataToMesOperationCompleted(object arg) {
            if ((this.WriteDataToMesCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.WriteDataToMesCompleted(this, new WriteDataToMesCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }
        
        /// <remarks/>
        public new void CancelAsync(object userState) {
            base.CancelAsync(userState);
        }
        
        private bool IsLocalFileSystemWebService(string url) {
            if (((url == null) 
                        || (url == string.Empty))) {
                return false;
            }
            System.Uri wsUri = new System.Uri(url);
            if (((wsUri.Port >= 1024) 
                        && (string.Compare(wsUri.Host, "localHost", System.StringComparison.OrdinalIgnoreCase) == 0))) {
                return true;
            }
            return false;
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.8.3752.0")]
    public delegate void ReceiveDeviceDataCompletedEventHandler(object sender, ReceiveDeviceDataCompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.8.3752.0")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class ReceiveDeviceDataCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        internal ReceiveDeviceDataCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
                base(exception, cancelled, userState) {
            this.results = results;
        }
        
        /// <remarks/>
        public string Result {
            get {
                this.RaiseExceptionIfNecessary();
                return ((string)(this.results[0]));
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.8.3752.0")]
    public delegate void WriteDataToMesCompletedEventHandler(object sender, WriteDataToMesCompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.8.3752.0")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class WriteDataToMesCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        internal WriteDataToMesCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
                base(exception, cancelled, userState) {
            this.results = results;
        }
        
        /// <remarks/>
        public string Result {
            get {
                this.RaiseExceptionIfNecessary();
                return ((string)(this.results[0]));
            }
        }
        
        /// <remarks/>
        public string strMsg {
            get {
                this.RaiseExceptionIfNecessary();
                return ((string)(this.results[1]));
            }
        }
    }
}

#pragma warning restore 1591