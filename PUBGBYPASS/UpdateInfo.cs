﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:2.0.50727.5483
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

// 
// This source code was auto-generated by xsd, Version=2.0.50727.3038.
// 
namespace PUBGBYPASSER {
    using System.Xml.Serialization;
    
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.3038")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType=true)]
    [System.Xml.Serialization.XmlRootAttribute(Namespace="", IsNullable=false)]
    public partial class UpdateChecker {
        
        private UpdateCheckerRelease releaseField;
        
        /// <remarks/>
        public UpdateCheckerRelease release {
            get {
                return this.releaseField;
            }
            set {
                this.releaseField = value;
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.3038")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType=true)]
    public partial class UpdateCheckerRelease {
        
        private string nameField;
        
        private byte majorField;
        
        private byte minorField;
        
        private byte revisionField;
        
        private byte buildField;
        
        private string infoField;
        
        private string infoUriField;
        
        private object downloadUriField;
        
        private ulong dateField;
        
        private string typeField;
        
        /// <remarks/>
        public string name {
            get {
                return this.nameField;
            }
            set {
                this.nameField = value;
            }
        }
        
        /// <remarks/>
        public byte major {
            get {
                return this.majorField;
            }
            set {
                this.majorField = value;
            }
        }
        
        /// <remarks/>
        public byte minor {
            get {
                return this.minorField;
            }
            set {
                this.minorField = value;
            }
        }
        
        /// <remarks/>
        public byte revision {
            get {
                return this.revisionField;
            }
            set {
                this.revisionField = value;
            }
        }
        
        /// <remarks/>
        public byte build {
            get {
                return this.buildField;
            }
            set {
                this.buildField = value;
            }
        }
        
        /// <remarks/>
        public string info {
            get {
                return this.infoField;
            }
            set {
                this.infoField = value;
            }
        }
        
        /// <remarks/>
        public string infoUri {
            get {
                return this.infoUriField;
            }
            set {
                this.infoUriField = value;
            }
        }
        
        /// <remarks/>
        public object downloadUri {
            get {
                return this.downloadUriField;
            }
            set {
                this.downloadUriField = value;
            }
        }
        
        /// <remarks/>
        public ulong date {
            get {
                return this.dateField;
            }
            set {
                this.dateField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string type {
            get {
                return this.typeField;
            }
            set {
                this.typeField = value;
            }
        }
    }
}
