﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace KSPCompiler.Resources {
    using System;
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "4.0.0.0")]
    [System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    public class CompilerMessageResources {
        
        private static System.Resources.ResourceManager resourceMan;
        
        private static System.Globalization.CultureInfo resourceCulture;
        
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal CompilerMessageResources() {
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        public static System.Resources.ResourceManager ResourceManager {
            get {
                if (object.Equals(null, resourceMan)) {
                    System.Resources.ResourceManager temp = new System.Resources.ResourceManager("KSPCompiler.Resources.CompilerMessageResources", typeof(CompilerMessageResources).Assembly);
                    resourceMan = temp;
                }
                return resourceMan;
            }
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        public static System.Globalization.CultureInfo Culture {
            get {
                return resourceCulture;
            }
            set {
                resourceCulture = value;
            }
        }
        
        public static string syntax_error {
            get {
                return ResourceManager.GetString("syntax.error", resourceCulture);
            }
        }
        
        public static string synax_error_detail {
            get {
                return ResourceManager.GetString("synax.error.detail", resourceCulture);
            }
        }
        
        public static string symbol_error_declare_variable_outside {
            get {
                return ResourceManager.GetString("symbol.error.declare.variable.outside", resourceCulture);
            }
        }
        
        public static string symbol_error_declare_variable_reserved {
            get {
                return ResourceManager.GetString("symbol.error.declare.variable.reserved", resourceCulture);
            }
        }
        
        public static string symbol_error_declare_variable_ni_reserved {
            get {
                return ResourceManager.GetString("symbol.error.declare.variable.ni_reserved", resourceCulture);
            }
        }
        
        public static string symbol_error_declare_variable_already {
            get {
                return ResourceManager.GetString("symbol.error.declare.variable.already", resourceCulture);
            }
        }
        
        public static string symbol_error_declare_variable_unkown {
            get {
                return ResourceManager.GetString("symbol.error.declare.variable.unkown", resourceCulture);
            }
        }
        
        public static string symbol_waring_declare_oninit {
            get {
                return ResourceManager.GetString("symbol.waring.declare.oninit", resourceCulture);
            }
        }
        
        public static string symbol_warning_declare_callback_unkown {
            get {
                return ResourceManager.GetString("symbol.warning.declare.callback.unkown", resourceCulture);
            }
        }
        
        public static string symbol_error_declare_callback_already {
            get {
                return ResourceManager.GetString("symbol.error.declare.callback.already", resourceCulture);
            }
        }
    }
}
