﻿#pragma checksum "..\..\..\..\Windows\QuestionWindowUser.xaml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "A16E3D54E264E4FBC884C6FC7912F538D5F5DD46"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using ForumProj.Windows;
using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Controls.Ribbon;
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


namespace ForumProj.Windows {
    
    
    /// <summary>
    /// QuestionWindowUser
    /// </summary>
    public partial class QuestionWindowUser : System.Windows.Window, System.Windows.Markup.IComponentConnector {
        
        
        #line 57 "..\..\..\..\Windows\QuestionWindowUser.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBlock QuestionUsername;
        
        #line default
        #line hidden
        
        
        #line 70 "..\..\..\..\Windows\QuestionWindowUser.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBlock QuestionId;
        
        #line default
        #line hidden
        
        
        #line 76 "..\..\..\..\Windows\QuestionWindowUser.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBlock QuestionTopicBlock;
        
        #line default
        #line hidden
        
        
        #line 85 "..\..\..\..\Windows\QuestionWindowUser.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBlock QuestionDate;
        
        #line default
        #line hidden
        
        
        #line 97 "..\..\..\..\Windows\QuestionWindowUser.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBlock QuestionContent;
        
        #line default
        #line hidden
        
        
        #line 114 "..\..\..\..\Windows\QuestionWindowUser.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.StackPanel Answers;
        
        #line default
        #line hidden
        
        
        #line 120 "..\..\..\..\Windows\QuestionWindowUser.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBlock CategoryName;
        
        #line default
        #line hidden
        
        
        #line 147 "..\..\..\..\Windows\QuestionWindowUser.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox AnswerContentBox;
        
        #line default
        #line hidden
        
        
        #line 163 "..\..\..\..\Windows\QuestionWindowUser.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBlock ValidationInfo;
        
        #line default
        #line hidden
        
        private bool _contentLoaded;
        
        /// <summary>
        /// InitializeComponent
        /// </summary>
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "7.0.0.0")]
        public void InitializeComponent() {
            if (_contentLoaded) {
                return;
            }
            _contentLoaded = true;
            System.Uri resourceLocater = new System.Uri("/ForumProj;component/windows/questionwindowuser.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\..\..\Windows\QuestionWindowUser.xaml"
            System.Windows.Application.LoadComponent(this, resourceLocater);
            
            #line default
            #line hidden
        }
        
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "7.0.0.0")]
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never)]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1033:InterfaceMethodsShouldBeCallableByChildTypes")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1800:DoNotCastUnnecessarily")]
        void System.Windows.Markup.IComponentConnector.Connect(int connectionId, object target) {
            switch (connectionId)
            {
            case 1:
            this.QuestionUsername = ((System.Windows.Controls.TextBlock)(target));
            return;
            case 2:
            this.QuestionId = ((System.Windows.Controls.TextBlock)(target));
            return;
            case 3:
            this.QuestionTopicBlock = ((System.Windows.Controls.TextBlock)(target));
            return;
            case 4:
            this.QuestionDate = ((System.Windows.Controls.TextBlock)(target));
            return;
            case 5:
            this.QuestionContent = ((System.Windows.Controls.TextBlock)(target));
            return;
            case 6:
            this.Answers = ((System.Windows.Controls.StackPanel)(target));
            return;
            case 7:
            this.CategoryName = ((System.Windows.Controls.TextBlock)(target));
            return;
            case 8:
            this.AnswerContentBox = ((System.Windows.Controls.TextBox)(target));
            return;
            case 9:
            
            #line 156 "..\..\..\..\Windows\QuestionWindowUser.xaml"
            ((System.Windows.Controls.Button)(target)).Click += new System.Windows.RoutedEventHandler(this.AddAnswerButton);
            
            #line default
            #line hidden
            return;
            case 10:
            this.ValidationInfo = ((System.Windows.Controls.TextBlock)(target));
            return;
            }
            this._contentLoaded = true;
        }
    }
}

