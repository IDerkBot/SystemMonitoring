﻿#pragma checksum "..\..\..\Pages\AddSensor.xaml" "{8829d00f-11b8-4213-878b-770e8597ac16}" "07BB8DE107D690C08E7E8A3D1C04131BF747A8D103A22DB550116CCAF71B738E"
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


namespace SystemMonitoring.AdminEditPages {
    
    
    /// <summary>
    /// AddSensor
    /// </summary>
    public partial class AddSensor : System.Windows.Controls.Page, System.Windows.Markup.IComponentConnector {
        
        
        #line 10 "..\..\..\Pages\AddSensor.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Grid Grid;
        
        #line default
        #line hidden
        
        
        #line 37 "..\..\..\Pages\AddSensor.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox ID;
        
        #line default
        #line hidden
        
        
        #line 38 "..\..\..\Pages\AddSensor.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox Humidity;
        
        #line default
        #line hidden
        
        
        #line 39 "..\..\..\Pages\AddSensor.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox Temperature;
        
        #line default
        #line hidden
        
        
        #line 40 "..\..\..\Pages\AddSensor.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox Phosphorus;
        
        #line default
        #line hidden
        
        
        #line 41 "..\..\..\Pages\AddSensor.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox Calium;
        
        #line default
        #line hidden
        
        
        #line 42 "..\..\..\Pages\AddSensor.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox Magniy;
        
        #line default
        #line hidden
        
        
        #line 43 "..\..\..\Pages\AddSensor.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox Calcium;
        
        #line default
        #line hidden
        
        
        #line 44 "..\..\..\Pages\AddSensor.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox Asot;
        
        #line default
        #line hidden
        
        
        #line 45 "..\..\..\Pages\AddSensor.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox Acidity;
        
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
            System.Uri resourceLocater = new System.Uri("/SystemMonitoring;component/pages/addsensor.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\..\Pages\AddSensor.xaml"
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
            this.Grid = ((System.Windows.Controls.Grid)(target));
            return;
            case 2:
            this.ID = ((System.Windows.Controls.TextBox)(target));
            
            #line 37 "..\..\..\Pages\AddSensor.xaml"
            this.ID.TextChanged += new System.Windows.Controls.TextChangedEventHandler(this.TB_TextChanged);
            
            #line default
            #line hidden
            return;
            case 3:
            this.Humidity = ((System.Windows.Controls.TextBox)(target));
            return;
            case 4:
            this.Temperature = ((System.Windows.Controls.TextBox)(target));
            return;
            case 5:
            this.Phosphorus = ((System.Windows.Controls.TextBox)(target));
            return;
            case 6:
            this.Calium = ((System.Windows.Controls.TextBox)(target));
            return;
            case 7:
            this.Magniy = ((System.Windows.Controls.TextBox)(target));
            return;
            case 8:
            this.Calcium = ((System.Windows.Controls.TextBox)(target));
            return;
            case 9:
            this.Asot = ((System.Windows.Controls.TextBox)(target));
            return;
            case 10:
            this.Acidity = ((System.Windows.Controls.TextBox)(target));
            return;
            case 11:
            
            #line 48 "..\..\..\Pages\AddSensor.xaml"
            ((System.Windows.Controls.Button)(target)).Click += new System.Windows.RoutedEventHandler(this.Back_Click);
            
            #line default
            #line hidden
            return;
            case 12:
            
            #line 49 "..\..\..\Pages\AddSensor.xaml"
            ((System.Windows.Controls.Button)(target)).Click += new System.Windows.RoutedEventHandler(this.Add_Click);
            
            #line default
            #line hidden
            return;
            }
            this._contentLoaded = true;
        }
    }
}

