﻿#pragma checksum "E:\zhangling\code\Bicycle0\Bicycle\Sign_in.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "59BF1807E704EC2D79EF180F06AD73D0"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Bicycle
{
    partial class Sign_in : 
        global::Windows.UI.Xaml.Controls.Page, 
        global::Windows.UI.Xaml.Markup.IComponentConnector,
        global::Windows.UI.Xaml.Markup.IComponentConnector2
    {
        /// <summary>
        /// Connect()
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Windows.UI.Xaml.Build.Tasks"," 14.0.0.0")]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        public void Connect(int connectionId, object target)
        {
            switch(connectionId)
            {
            case 1:
                {
                    this.home = (global::Windows.UI.Xaml.Controls.AppBarButton)(target);
                    #line 11 "..\..\..\Sign_in.xaml"
                    ((global::Windows.UI.Xaml.Controls.AppBarButton)this.home).Click += this.home_button;
                    #line default
                }
                break;
            case 2:
                {
                    this._account = (global::Windows.UI.Xaml.Controls.TextBox)(target);
                }
                break;
            case 3:
                {
                    this.password = (global::Windows.UI.Xaml.Controls.PasswordBox)(target);
                }
                break;
            case 4:
                {
                    this.SignIn = (global::Windows.UI.Xaml.Controls.Button)(target);
                    #line 23 "..\..\..\Sign_in.xaml"
                    ((global::Windows.UI.Xaml.Controls.Button)this.SignIn).Click += this.SignIn_button;
                    #line default
                }
                break;
            case 5:
                {
                    this.SignUp = (global::Windows.UI.Xaml.Controls.Button)(target);
                    #line 24 "..\..\..\Sign_in.xaml"
                    ((global::Windows.UI.Xaml.Controls.Button)this.SignUp).Click += this.SignUp_button;
                    #line default
                }
                break;
            default:
                break;
            }
            this._contentLoaded = true;
        }

        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Windows.UI.Xaml.Build.Tasks"," 14.0.0.0")]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        public global::Windows.UI.Xaml.Markup.IComponentConnector GetBindingConnector(int connectionId, object target)
        {
            global::Windows.UI.Xaml.Markup.IComponentConnector returnValue = null;
            return returnValue;
        }
    }
}

