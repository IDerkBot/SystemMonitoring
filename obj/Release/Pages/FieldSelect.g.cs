﻿#pragma checksum "..\..\..\Pages\FieldSelect.xaml" "{8829d00f-11b8-4213-878b-770e8597ac16}" "31D8411CE6390EE0A6E304FDCD83A80B0B519D3131E3A6297E6C2ABA076F4374"
//------------------------------------------------------------------------------
// <auto-generated>
//     Этот код создан программой.
//     Исполняемая версия:4.0.30319.42000
//
//     Изменения в этом файле могут привести к неправильной работе и будут потеряны в случае
//     повторной генерации кода.
// </auto-generated>
//------------------------------------------------------------------------------

using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Effects;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;
using System.Windows.Media.TextFormatting;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Shell;


namespace SystemMonitoring {
    
    
    /// <summary>
    /// FieldSelect
    /// </summary>
    public partial class FieldSelect : System.Windows.Controls.Page, System.Windows.Markup.IComponentConnector {
        
        
        #line 15 "..\..\..\Pages\FieldSelect.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.ComboBox CBDistrict;
        
        #line default
        #line hidden
        
        
        #line 21 "..\..\..\Pages\FieldSelect.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.GroupBox GB;
        
        #line default
        #line hidden
        
        
        #line 23 "..\..\..\Pages\FieldSelect.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.ComboBox CBField;
        
        #line default
        #line hidden
        
        
        #line 29 "..\..\..\Pages\FieldSelect.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button BtnNext;
        
        #line default
        #line hidden
        
        private bool _contentLoaded;
        
        /// <summary>
        /// InitializeComponent
        /// </summary>
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "4.0.0.0")]
        public void InitializeComponent() {
            if (_contentLoaded) {
                return;
            }
            _contentLoaded = true;
            System.Uri resourceLocater = new System.Uri("/SystemMonitoring;component/pages/fieldselect.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\..\Pages\FieldSelect.xaml"
            System.Windows.Application.LoadComponent(this, resourceLocater);
            
            #line default
            #line hidden
        }
        
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "4.0.0.0")]
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never)]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1033:InterfaceMethodsShouldBeCallableByChildTypes")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1800:DoNotCastUnnecessarily")]
        void System.Windows.Markup.IComponentConnector.Connect(int connectionId, object target) {
            switch (connectionId)
            {
            case 1:
            
            #line 9 "..\..\..\Pages\FieldSelect.xaml"
            ((SystemMonitoring.FieldSelect)(target)).Loaded += new System.Windows.RoutedEventHandler(this.Load);
            
            #line default
            #line hidden
            return;
            case 2:
            this.CBDistrict = ((System.Windows.Controls.ComboBox)(target));
            
            #line 15 "..\..\..\Pages\FieldSelect.xaml"
            this.CBDistrict.SelectionChanged += new System.Windows.Controls.SelectionChangedEventHandler(this.DistrictSelectChanged);
            
            #line default
            #line hidden
            return;
            case 3:
            
            #line 16 "..\..\..\Pages\FieldSelect.xaml"
            ((System.Windows.Controls.Button)(target)).Click += new System.Windows.RoutedEventHandler(this.AddDistrict_Click);
            
            #line default
            #line hidden
            return;
            case 4:
            this.GB = ((System.Windows.Controls.GroupBox)(target));
            return;
            case 5:
            this.CBField = ((System.Windows.Controls.ComboBox)(target));
            
            #line 23 "..\..\..\Pages\FieldSelect.xaml"
            this.CBField.SelectionChanged += new System.Windows.Controls.SelectionChangedEventHandler(this.FieldDistrictChanged);
            
            #line default
            #line hidden
            return;
            case 6:
            
            #line 24 "..\..\..\Pages\FieldSelect.xaml"
            ((System.Windows.Controls.Button)(target)).Click += new System.Windows.RoutedEventHandler(this.AddField_Click);
            
            #line default
            #line hidden
            return;
            case 7:
            this.BtnNext = ((System.Windows.Controls.Button)(target));
            
            #line 29 "..\..\..\Pages\FieldSelect.xaml"
            this.BtnNext.Click += new System.Windows.RoutedEventHandler(this.Next_Click);
            
            #line default
            #line hidden
            return;
            }
            this._contentLoaded = true;
        }
    }
}
