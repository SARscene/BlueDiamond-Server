//------------------------------------------------------------------------------
// <autogenerated>
//     This code was generated by the XsdToClasses tool.
//     Tool Version:    1.2.23.0
//     Runtime Version: 4.0.30319.42000
//     Generated on:    10/23/2015 3:17:06 PM
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </autogenerated>
//------------------------------------------------------------------------------
// Workaround for bug http://lab.msdn.microsoft.com/productfeedback/viewfeedback.aspx?feedbackid=d457a36e-0ce8-4368-9a27-92762860d8e1
#pragma warning disable 1591
namespace BlueDiamond.Desktop {
    
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.6.79.0")]
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://BlueToque.ca/BlueDiamond.Desktop/Configuration")]
    [System.Xml.Serialization.XmlRootAttribute("Configuration", Namespace="http://BlueToque.ca/BlueDiamond.Desktop/Configuration", IsNullable=false)]
    public partial class ConfigurationType : System.ComponentModel.INotifyPropertyChanged {
        
        /// <summary />
        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;
        
        /// <summary />
        protected void RaisePropertyChanged(string propertyName) {
            System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
            if ((propertyChanged != null)) {
                propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }
        
        /// <summary />
        private int portField;
        
        /// <remarks/>
        public int Port {
            get {
                return this.portField;
            }
            set {
                this.portField = value;
                this.RaisePropertyChanged("Port");
            }
        }
        
        /// <summary />
        private int pathField;
        
        /// <remarks/>
        public int Path {
            get {
                return this.pathField;
            }
            set {
                this.pathField = value;
                this.RaisePropertyChanged("Path");
            }
        }
        
        /// <summary />
        private System.Xml.XmlElement anyField;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlAnyElementAttribute()]
        public System.Xml.XmlElement Any {
            get {
                return this.anyField;
            }
            set {
                this.anyField = value;
                this.RaisePropertyChanged("Any");
            }
        }
    }
}
#pragma warning restore 1591