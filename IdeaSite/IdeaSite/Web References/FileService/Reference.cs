﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

// 
// This source code was auto-generated by Microsoft.VSDesigner, Version 4.0.30319.42000.
// 
#pragma warning disable 1591

namespace IdeaSite.FileService {
    using System;
    using System.Web.Services;
    using System.Diagnostics;
    using System.Web.Services.Protocols;
    using System.Xml.Serialization;
    using System.ComponentModel;
    
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.6.81.0")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Web.Services.WebServiceBindingAttribute(Name="FileServiceSoap", Namespace="http://tempuri.org/")]
    public partial class FileService : System.Web.Services.Protocols.SoapHttpClientProtocol {
        
        private System.Threading.SendOrPostCallback ScanByFileOperationCompleted;
        
        private System.Threading.SendOrPostCallback ScanByFileWithNameOperationCompleted;
        
        private System.Threading.SendOrPostCallback ScanByFileWithNameAndExtensionOperationCompleted;
        
        private System.Threading.SendOrPostCallback CheckFileByNameOperationCompleted;
        
        private System.Threading.SendOrPostCallback CheckFileByNameAndExtensionOperationCompleted;
        
        private bool useDefaultCredentialsSetExplicitly;
        
        /// <remarks/>
        public FileService() {
            this.Url = global::IdeaSite.Properties.Settings.Default.IdeaSite_FileService_FileService;
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
        public event ScanByFileCompletedEventHandler ScanByFileCompleted;
        
        /// <remarks/>
        public event ScanByFileWithNameCompletedEventHandler ScanByFileWithNameCompleted;
        
        /// <remarks/>
        public event ScanByFileWithNameAndExtensionCompletedEventHandler ScanByFileWithNameAndExtensionCompleted;
        
        /// <remarks/>
        public event CheckFileByNameCompletedEventHandler CheckFileByNameCompleted;
        
        /// <remarks/>
        public event CheckFileByNameAndExtensionCompletedEventHandler CheckFileByNameAndExtensionCompleted;
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://tempuri.org/ScanByFile", RequestNamespace="http://tempuri.org/", ResponseNamespace="http://tempuri.org/", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public bool ScanByFile([System.Xml.Serialization.XmlElementAttribute(DataType="base64Binary")] byte[] fsBytes) {
            object[] results = this.Invoke("ScanByFile", new object[] {
                        fsBytes});
            return ((bool)(results[0]));
        }
        
        /// <remarks/>
        public void ScanByFileAsync(byte[] fsBytes) {
            this.ScanByFileAsync(fsBytes, null);
        }
        
        /// <remarks/>
        public void ScanByFileAsync(byte[] fsBytes, object userState) {
            if ((this.ScanByFileOperationCompleted == null)) {
                this.ScanByFileOperationCompleted = new System.Threading.SendOrPostCallback(this.OnScanByFileOperationCompleted);
            }
            this.InvokeAsync("ScanByFile", new object[] {
                        fsBytes}, this.ScanByFileOperationCompleted, userState);
        }
        
        private void OnScanByFileOperationCompleted(object arg) {
            if ((this.ScanByFileCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.ScanByFileCompleted(this, new ScanByFileCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://tempuri.org/ScanByFileWithName", RequestNamespace="http://tempuri.org/", ResponseNamespace="http://tempuri.org/", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public bool ScanByFileWithName([System.Xml.Serialization.XmlElementAttribute(DataType="base64Binary")] byte[] fsBytes, string name) {
            object[] results = this.Invoke("ScanByFileWithName", new object[] {
                        fsBytes,
                        name});
            return ((bool)(results[0]));
        }
        
        /// <remarks/>
        public void ScanByFileWithNameAsync(byte[] fsBytes, string name) {
            this.ScanByFileWithNameAsync(fsBytes, name, null);
        }
        
        /// <remarks/>
        public void ScanByFileWithNameAsync(byte[] fsBytes, string name, object userState) {
            if ((this.ScanByFileWithNameOperationCompleted == null)) {
                this.ScanByFileWithNameOperationCompleted = new System.Threading.SendOrPostCallback(this.OnScanByFileWithNameOperationCompleted);
            }
            this.InvokeAsync("ScanByFileWithName", new object[] {
                        fsBytes,
                        name}, this.ScanByFileWithNameOperationCompleted, userState);
        }
        
        private void OnScanByFileWithNameOperationCompleted(object arg) {
            if ((this.ScanByFileWithNameCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.ScanByFileWithNameCompleted(this, new ScanByFileWithNameCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://tempuri.org/ScanByFileWithNameAndExtension", RequestNamespace="http://tempuri.org/", ResponseNamespace="http://tempuri.org/", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public bool ScanByFileWithNameAndExtension([System.Xml.Serialization.XmlElementAttribute(DataType="base64Binary")] byte[] fsBytes, string name, string extension) {
            object[] results = this.Invoke("ScanByFileWithNameAndExtension", new object[] {
                        fsBytes,
                        name,
                        extension});
            return ((bool)(results[0]));
        }
        
        /// <remarks/>
        public void ScanByFileWithNameAndExtensionAsync(byte[] fsBytes, string name, string extension) {
            this.ScanByFileWithNameAndExtensionAsync(fsBytes, name, extension, null);
        }
        
        /// <remarks/>
        public void ScanByFileWithNameAndExtensionAsync(byte[] fsBytes, string name, string extension, object userState) {
            if ((this.ScanByFileWithNameAndExtensionOperationCompleted == null)) {
                this.ScanByFileWithNameAndExtensionOperationCompleted = new System.Threading.SendOrPostCallback(this.OnScanByFileWithNameAndExtensionOperationCompleted);
            }
            this.InvokeAsync("ScanByFileWithNameAndExtension", new object[] {
                        fsBytes,
                        name,
                        extension}, this.ScanByFileWithNameAndExtensionOperationCompleted, userState);
        }
        
        private void OnScanByFileWithNameAndExtensionOperationCompleted(object arg) {
            if ((this.ScanByFileWithNameAndExtensionCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.ScanByFileWithNameAndExtensionCompleted(this, new ScanByFileWithNameAndExtensionCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://tempuri.org/CheckFileByName", RequestNamespace="http://tempuri.org/", ResponseNamespace="http://tempuri.org/", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public bool CheckFileByName(string name) {
            object[] results = this.Invoke("CheckFileByName", new object[] {
                        name});
            return ((bool)(results[0]));
        }
        
        /// <remarks/>
        public void CheckFileByNameAsync(string name) {
            this.CheckFileByNameAsync(name, null);
        }
        
        /// <remarks/>
        public void CheckFileByNameAsync(string name, object userState) {
            if ((this.CheckFileByNameOperationCompleted == null)) {
                this.CheckFileByNameOperationCompleted = new System.Threading.SendOrPostCallback(this.OnCheckFileByNameOperationCompleted);
            }
            this.InvokeAsync("CheckFileByName", new object[] {
                        name}, this.CheckFileByNameOperationCompleted, userState);
        }
        
        private void OnCheckFileByNameOperationCompleted(object arg) {
            if ((this.CheckFileByNameCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.CheckFileByNameCompleted(this, new CheckFileByNameCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://tempuri.org/CheckFileByNameAndExtension", RequestNamespace="http://tempuri.org/", ResponseNamespace="http://tempuri.org/", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public bool CheckFileByNameAndExtension(string name, string extension) {
            object[] results = this.Invoke("CheckFileByNameAndExtension", new object[] {
                        name,
                        extension});
            return ((bool)(results[0]));
        }
        
        /// <remarks/>
        public void CheckFileByNameAndExtensionAsync(string name, string extension) {
            this.CheckFileByNameAndExtensionAsync(name, extension, null);
        }
        
        /// <remarks/>
        public void CheckFileByNameAndExtensionAsync(string name, string extension, object userState) {
            if ((this.CheckFileByNameAndExtensionOperationCompleted == null)) {
                this.CheckFileByNameAndExtensionOperationCompleted = new System.Threading.SendOrPostCallback(this.OnCheckFileByNameAndExtensionOperationCompleted);
            }
            this.InvokeAsync("CheckFileByNameAndExtension", new object[] {
                        name,
                        extension}, this.CheckFileByNameAndExtensionOperationCompleted, userState);
        }
        
        private void OnCheckFileByNameAndExtensionOperationCompleted(object arg) {
            if ((this.CheckFileByNameAndExtensionCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.CheckFileByNameAndExtensionCompleted(this, new CheckFileByNameAndExtensionCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
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
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.6.81.0")]
    public delegate void ScanByFileCompletedEventHandler(object sender, ScanByFileCompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.6.81.0")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class ScanByFileCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        internal ScanByFileCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
                base(exception, cancelled, userState) {
            this.results = results;
        }
        
        /// <remarks/>
        public bool Result {
            get {
                this.RaiseExceptionIfNecessary();
                return ((bool)(this.results[0]));
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.6.81.0")]
    public delegate void ScanByFileWithNameCompletedEventHandler(object sender, ScanByFileWithNameCompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.6.81.0")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class ScanByFileWithNameCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        internal ScanByFileWithNameCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
                base(exception, cancelled, userState) {
            this.results = results;
        }
        
        /// <remarks/>
        public bool Result {
            get {
                this.RaiseExceptionIfNecessary();
                return ((bool)(this.results[0]));
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.6.81.0")]
    public delegate void ScanByFileWithNameAndExtensionCompletedEventHandler(object sender, ScanByFileWithNameAndExtensionCompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.6.81.0")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class ScanByFileWithNameAndExtensionCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        internal ScanByFileWithNameAndExtensionCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
                base(exception, cancelled, userState) {
            this.results = results;
        }
        
        /// <remarks/>
        public bool Result {
            get {
                this.RaiseExceptionIfNecessary();
                return ((bool)(this.results[0]));
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.6.81.0")]
    public delegate void CheckFileByNameCompletedEventHandler(object sender, CheckFileByNameCompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.6.81.0")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class CheckFileByNameCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        internal CheckFileByNameCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
                base(exception, cancelled, userState) {
            this.results = results;
        }
        
        /// <remarks/>
        public bool Result {
            get {
                this.RaiseExceptionIfNecessary();
                return ((bool)(this.results[0]));
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.6.81.0")]
    public delegate void CheckFileByNameAndExtensionCompletedEventHandler(object sender, CheckFileByNameAndExtensionCompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.6.81.0")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class CheckFileByNameAndExtensionCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        internal CheckFileByNameAndExtensionCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
                base(exception, cancelled, userState) {
            this.results = results;
        }
        
        /// <remarks/>
        public bool Result {
            get {
                this.RaiseExceptionIfNecessary();
                return ((bool)(this.results[0]));
            }
        }
    }
}

#pragma warning restore 1591