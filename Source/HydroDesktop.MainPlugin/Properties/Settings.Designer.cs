﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.18034
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace HydroDesktop.Main.Properties {
    
    
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.VisualStudio.Editors.SettingsDesigner.SettingsSingleFileGenerator", "11.0.0.0")]
    internal sealed partial class Settings : global::System.Configuration.ApplicationSettingsBase {
        
        private static Settings defaultInstance = ((Settings)(global::System.Configuration.ApplicationSettingsBase.Synchronized(new Settings())));
        
        public static Settings Default {
            get {
                return defaultInstance;
            }
        }
        
        [global::System.Configuration.ApplicationScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("HydroDesktop_Quick_Start_Guide_1.5.pdf")]
        public string QuickStartGuideName {
            get {
                return ((string)(this["QuickStartGuideName"]));
            }
        }
        
        [global::System.Configuration.ApplicationScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("hydrodesktop_sample_projects")]
        public string SampleProjectsDirectory {
            get {
                return ((string)(this["SampleProjectsDirectory"]));
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("True")]
        public bool IsWelcomeScreenDisplayed {
            get {
                return ((bool)(this["IsWelcomeScreenDisplayed"]));
            }
            set {
                this["IsWelcomeScreenDisplayed"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("http://userguide.hydrodesktop.org")]
        public string remoteHelpUri {
            get {
                return ((string)(this["remoteHelpUri"]));
            }
            set {
                this["remoteHelpUri"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("HydroDesktop User Guide.pdf")]
        public string localHelpUri {
            get {
                return ((string)(this["localHelpUri"]));
            }
            set {
                this["localHelpUri"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("http://quickstart.hydrodesktop.org")]
        public string quickStartUri {
            get {
                return ((string)(this["quickStartUri"]));
            }
            set {
                this["quickStartUri"] = value;
            }
        }
    }
}
