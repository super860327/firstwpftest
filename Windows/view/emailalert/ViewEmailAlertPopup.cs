using IDKin.IM.Core;
using IDKin.IM.Windows.Model;
using IDKin.IM.Windows.Util;
using IDKin.IM.Windows.ViewModel;
using Microsoft.CSharp.RuntimeBinder;
using mshtml;
using SHDocVw;
using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Markup;
namespace IDKin.IM.Windows.View.EmailAlert
{
    [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
    public partial class ViewEmailAlertPopup : Popup//, IComponentConnector
    {
        //[System.Runtime.CompilerServices.CompilerGenerated]
        //private static class <LoginToSite>o__SiteContainer16
        //{
        //    public static CallSite<Func<CallSite, object, HTMLDocument>> <>p__Site17;
        //    public static CallSite<Func<CallSite, object, bool>> <>p__Site18;
        //    public static CallSite<Func<CallSite, object, object, object>> <>p__Site19;
        //    public static CallSite<Func<CallSite, object, string>> <>p__Site1a;
        //    public static CallSite<Func<CallSite, object, bool>> <>p__Site1b;
        //    public static CallSite<Func<CallSite, object, object, object>> <>p__Site1c;
        //    public static CallSite<Func<CallSite, object, string>> <>p__Site1d;
        //    public static CallSite<Func<CallSite, object, bool>> <>p__Site1e;
        //    public static CallSite<Func<CallSite, object, object, object>> <>p__Site1f;
        //    public static CallSite<Func<CallSite, object, string>> <>p__Site20;
        //}
        //[System.Runtime.CompilerServices.CompilerGenerated]
        //private static class <Login>o__SiteContainer21
        //{
        //    public static CallSite<Func<CallSite, object, HTMLDocument>> <>p__Site22;
        //    public static CallSite<Func<CallSite, object, bool>> <>p__Site23;
        //    public static CallSite<Func<CallSite, object, object, object>> <>p__Site24;
        //    public static CallSite<Func<CallSite, object, string>> <>p__Site25;
        //    public static CallSite<Func<CallSite, object, bool>> <>p__Site26;
        //    public static CallSite<Func<CallSite, object, object, object>> <>p__Site27;
        //    public static CallSite<Func<CallSite, object, string>> <>p__Site28;
        //    public static CallSite<Func<CallSite, object, bool>> <>p__Site29;
        //    public static CallSite<Func<CallSite, object, object, object>> <>p__Site2a;
        //    public static CallSite<Func<CallSite, object, string>> <>p__Site2b;
        //    public static CallSite<Func<CallSite, object, string>> <>p__Site2c;
        //}
        //[System.Runtime.CompilerServices.CompilerGenerated]
        //private static class <LoginToIdc>o__SiteContainer2d
        //{
        //    public static CallSite<Func<CallSite, object, HTMLDocument>> <>p__Site2e;
        //    public static CallSite<Func<CallSite, object, bool>> <>p__Site2f;
        //    public static CallSite<Func<CallSite, object, object, object>> <>p__Site30;
        //    public static CallSite<Func<CallSite, object, string>> <>p__Site31;
        //    public static CallSite<Func<CallSite, object, bool>> <>p__Site32;
        //    public static CallSite<Func<CallSite, object, object, object>> <>p__Site33;
        //    public static CallSite<Func<CallSite, object, string>> <>p__Site34;
        //    public static CallSite<Func<CallSite, object, bool>> <>p__Site35;
        //    public static CallSite<Func<CallSite, object, object, object>> <>p__Site36;
        //    public static CallSite<Func<CallSite, object, string>> <>p__Site37;
        //    public static CallSite<Func<CallSite, object, bool>> <>p__Site38;
        //    public static CallSite<Func<CallSite, object, object, object>> <>p__Site39;
        //    public static CallSite<Func<CallSite, object, string>> <>p__Site3a;
        //}
        //[System.Runtime.CompilerServices.CompilerGenerated]
        //private static class <LoginToGmail>o__SiteContainer3b
        //{
        //    public static CallSite<Func<CallSite, object, HTMLDocument>> <>p__Site3c;
        //    public static CallSite<Func<CallSite, object, bool>> <>p__Site3d;
        //    public static CallSite<Func<CallSite, object, object, object>> <>p__Site3e;
        //    public static CallSite<Func<CallSite, object, string>> <>p__Site3f;
        //    public static CallSite<Func<CallSite, object, bool>> <>p__Site40;
        //    public static CallSite<Func<CallSite, object, object, object>> <>p__Site41;
        //    public static CallSite<Func<CallSite, object, string>> <>p__Site42;
        //    public static CallSite<Func<CallSite, object, bool>> <>p__Site43;
        //    public static CallSite<Func<CallSite, object, object, object>> <>p__Site44;
        //    public static CallSite<Func<CallSite, object, string>> <>p__Site45;
        //}
        private InternetExplorer internetExplorer;
        private EMailModal eMailModal;
        private object lockObj = new object();
        //internal ListBox emailAccountListBox;
        //private bool _contentLoaded;
        public ViewEmailAlertPopup()
        {

            this.InitializeComponent();
            if (DataModel.Instance.EmailList != null && DataModel.Instance.EmailList.Count > 0)
            {
                for (int i = 0; i < DataModel.Instance.EmailList.Count; i++)
                {
                    EmailAccountItem item = new EmailAccountItem(EmailAccountItemType.Type1);
                    item.tbkAccount.Text = DataModel.Instance.EmailList[i].MailID;
                    item.DataContext = DataModel.Instance.EmailList[i];
                    item.tbkCount.Text = string.Format("({0})", DataModel.Instance.EmailList[i].NewCount);
                    item.PreviewMouseDown += new MouseButtonEventHandler(this.item_PreviewMouseDown);
                    if (string.IsNullOrEmpty(DataModel.Instance.EmailList[i].PWD) || DataModel.Instance.EmailList[i].HasError)
                    {
                        item.AccoutInvalid();
                    }
                    else
                    {
                        item.AccoutValid();
                    }
                    this.emailAccountListBox.Items.Add(item);
                }
            }
        }
        private void item_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            try
            {
                EmailAccountItem item = sender as EmailAccountItem;
                if (item != null)
                {
                    base.Dispatcher.BeginInvoke(new System.Action<EmailAccountItem>(this.UpdateMailCount), new object[]
					{
						item
					});
                }
            }
            catch (System.Exception ex)
            {
                ServiceUtil.Instance.Logger.Error(ex.ToString());
            }
        }
        private void UpdateMailCount(EmailAccountItem item)
        {
            this.eMailModal = (item.DataContext as EMailModal);
            if (this.eMailModal != null)
            {
                System.Action<EmailAccountItem> a = new System.Action<EmailAccountItem>(this.OpenExplorer);
                a.BeginInvoke(item, delegate(System.IAsyncResult ar)
                {
                    a.EndInvoke(ar);
                    this.Setmail(this.eMailModal);
                }, null);
            }
        }
        private void updateEmailCount(EmailAccountItem item)
        {
            base.Dispatcher.BeginInvoke((Action)(() =>
            {
                item.tbkCount.Text = "(0)";
            }), new object[0]);
            this.eMailModal.NewCount = 0;
            (ServiceUtil.Instance.DataService.INWindow as INWindow).UpdateMailCount();
        }
        private void OpenExplorer(EmailAccountItem item)
        {
            this.updateEmailCount(item);
            this.OpenExplorer();
        }
        private void OpenExplorer()
        {
            //if (string.Compare(this.eMailModal.Server, "mail.sunupcg.cn", true) == 0)
            //{
            //    this.internetExplorer = (InternetExplorer)System.Activator.CreateInstance(System.Type.GetTypeFromCLSID(new System.Guid("0002DF01-0000-0000-C000-000000000046")));
            //    new ComAwareEventInfo(typeof(DWebBrowserEvents2_Event), "DocumentComplete").AddEventHandler(this.internetExplorer, new DWebBrowserEvents2_DocumentCompleteEventHandler(this, (System.UIntPtr)ldftn(internetExplorer_DocumentComplete)));
            //    object value = System.Reflection.Missing.Value;
            //    object value2 = System.Reflection.Missing.Value;
            //    object value3 = System.Reflection.Missing.Value;
            //    object value4 = System.Reflection.Missing.Value;
            //    this.internetExplorer.Navigate(this.eMailModal.Url, ref value, ref value2, ref value3, ref value4);
            //}
            //else
            //{
            //    if (string.Compare(this.eMailModal.Server, "pop.mail.yahoo.com", true) == 0)
            //    {
            //        if (!string.IsNullOrEmpty(this.eMailModal.Url) && this.eMailModal.Url.IndexOf("{0}") >= 0 && this.eMailModal.Url.IndexOf("{1}") >= 0)
            //        {
            //            try
            //            {
            //                Process.Start(string.Format(this.eMailModal.Url, this.eMailModal.MailID ?? string.Empty, this.eMailModal.PWD ?? string.Empty));
            //            }
            //            catch (System.Exception ex)
            //            {
            //                ServiceUtil.Instance.Logger.Error(ex.ToString());
            //            }
            //        }
            //    }
            //    else
            //    {
            //        if (string.Compare(this.eMailModal.Server, "pop.126.com", true) == 0)
            //        {
            //            if (!string.IsNullOrEmpty(this.eMailModal.Url) && this.eMailModal.Url.IndexOf("{0}") >= 0 && this.eMailModal.Url.IndexOf("{1}") >= 0)
            //            {
            //                try
            //                {
            //                    Process.Start(string.Format(this.eMailModal.Url, this.eMailModal.MailID ?? string.Empty, this.eMailModal.PWD ?? string.Empty));
            //                }
            //                catch (System.Exception ex)
            //                {
            //                    ServiceUtil.Instance.Logger.Error(ex.ToString());
            //                }
            //            }
            //        }
            //        else
            //        {
            //            if (string.Compare(this.eMailModal.Server, "pop.idccenter.net", true) == 0)
            //            {
            //                this.internetExplorer = (InternetExplorer)System.Activator.CreateInstance(System.Type.GetTypeFromCLSID(new System.Guid("0002DF01-0000-0000-C000-000000000046")));
            //                object value5 = System.Reflection.Missing.Value;
            //                object value6 = System.Reflection.Missing.Value;
            //                object value7 = System.Reflection.Missing.Value;
            //                object value8 = System.Reflection.Missing.Value;
            //                this.internetExplorer.Navigate(this.eMailModal.Url, ref value5, ref value6, ref value7, ref value8);
            //                new ComAwareEventInfo(typeof(DWebBrowserEvents2_Event), "DocumentComplete").AddEventHandler(this.internetExplorer, new DWebBrowserEvents2_DocumentCompleteEventHandler(this, (System.UIntPtr)ldftn(internetExplorer_DocumentComplete3)));
            //            }
            //            else
            //            {
            //                if (string.Compare(this.eMailModal.Server, "mail.idkin.net", true) == 0)
            //                {
            //                    this.internetExplorer = (InternetExplorer)System.Activator.CreateInstance(System.Type.GetTypeFromCLSID(new System.Guid("0002DF01-0000-0000-C000-000000000046")));
            //                    object value9 = System.Reflection.Missing.Value;
            //                    object value10 = System.Reflection.Missing.Value;
            //                    object value11 = System.Reflection.Missing.Value;
            //                    object value12 = System.Reflection.Missing.Value;
            //                    this.internetExplorer.Navigate("http://mail.idkin.net:32000/webmail/", ref value9, ref value10, ref value11, ref value12);
            //                    this.internetExplorer.Visible = true;
            //                }
            //                else
            //                {
            //                    if (string.Compare(this.eMailModal.Server, "pop.gmail.com", true) == 0)
            //                    {
            //                        this.internetExplorer = (InternetExplorer)System.Activator.CreateInstance(System.Type.GetTypeFromCLSID(new System.Guid("0002DF01-0000-0000-C000-000000000046")));
            //                        object value13 = System.Reflection.Missing.Value;
            //                        object value14 = System.Reflection.Missing.Value;
            //                        object value15 = System.Reflection.Missing.Value;
            //                        object value16 = System.Reflection.Missing.Value;
            //                        this.internetExplorer.Navigate(this.eMailModal.Url, ref value13, ref value14, ref value15, ref value16);
            //                        new ComAwareEventInfo(typeof(DWebBrowserEvents2_Event), "DocumentComplete").AddEventHandler(this.internetExplorer, new DWebBrowserEvents2_DocumentCompleteEventHandler(this, (System.UIntPtr)ldftn(internetExplorer_DocumentComplete4)));
            //                    }
            //                }
            //            }
            //        }
            //    }
            //}
        }
        private void internetExplorer_DocumentComplete(object pDisp, ref object URL)
        {
            //new ComAwareEventInfo(typeof(DWebBrowserEvents2_Event), "DocumentComplete").RemoveEventHandler(this.internetExplorer, new DWebBrowserEvents2_DocumentCompleteEventHandler(this, (System.UIntPtr)ldftn(internetExplorer_DocumentComplete)));
            //this.LoginToSite(this.internetExplorer);
            //this.internetExplorer.Visible = true;
        }
        private void internetExplorer_DocumentComplete2(object pDisp, ref object URL)
        {
            //new ComAwareEventInfo(typeof(DWebBrowserEvents2_Event), "DocumentComplete").RemoveEventHandler(this.internetExplorer, new DWebBrowserEvents2_DocumentCompleteEventHandler(this, (System.UIntPtr)ldftn(internetExplorer_DocumentComplete2)));
            //this.Login(this.internetExplorer);
            //this.internetExplorer.Visible = true;
        }
        private void internetExplorer_DocumentComplete3(object pDisp, ref object URL)
        {
            //new ComAwareEventInfo(typeof(DWebBrowserEvents2_Event), "DocumentComplete").RemoveEventHandler(this.internetExplorer, new DWebBrowserEvents2_DocumentCompleteEventHandler(this, (System.UIntPtr)ldftn(internetExplorer_DocumentComplete3)));
            //this.LoginToIdc(this.internetExplorer);
            //this.internetExplorer.Visible = true;
        }
        private void internetExplorer_DocumentComplete4(object pDisp, ref object URL)
        {
            //new ComAwareEventInfo(typeof(DWebBrowserEvents2_Event), "DocumentComplete").RemoveEventHandler(this.internetExplorer, new DWebBrowserEvents2_DocumentCompleteEventHandler(this, (System.UIntPtr)ldftn(internetExplorer_DocumentComplete4)));
            //this.LoginToGmail(this.internetExplorer);
            //this.internetExplorer.Visible = true;
        }
        private void LoginToSite(InternetExplorer internetExplorer)
        {
            //if (ViewEmailAlertPopup.<LoginToSite>o__SiteContainer16.<>p__Site17 == null)
            //{
            //    ViewEmailAlertPopup.<LoginToSite>o__SiteContainer16.<>p__Site17 = CallSite<Func<CallSite, object, HTMLDocument>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.Convert(CSharpBinderFlags.None, typeof(HTMLDocument), typeof(ViewEmailAlertPopup)));
            //}
            //HTMLDocument htmlDoc = ViewEmailAlertPopup.<LoginToSite>o__SiteContainer16.<>p__Site17.Target(ViewEmailAlertPopup.<LoginToSite>o__SiteContainer16.<>p__Site17, internetExplorer.Document);
            //IHTMLElementCollection iHTMLCol = null;
            //iHTMLCol = htmlDoc.getElementsByTagName("input");
            //foreach (IHTMLElement item in iHTMLCol)
            //{
            //    if (ViewEmailAlertPopup.<LoginToSite>o__SiteContainer16.<>p__Site18 == null)
            //    {
            //        ViewEmailAlertPopup.<LoginToSite>o__SiteContainer16.<>p__Site18 = CallSite<Func<CallSite, object, bool>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.UnaryOperation(CSharpBinderFlags.None, ExpressionType.IsTrue, typeof(ViewEmailAlertPopup), new CSharpArgumentInfo[]
            //        {
            //            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null)
            //        }));
            //    }
            //    Func<CallSite, object, bool> arg_126_0 = ViewEmailAlertPopup.<LoginToSite>o__SiteContainer16.<>p__Site18.Target;
            //    CallSite arg_126_1 = ViewEmailAlertPopup.<LoginToSite>o__SiteContainer16.<>p__Site18;
            //    if (ViewEmailAlertPopup.<LoginToSite>o__SiteContainer16.<>p__Site19 == null)
            //    {
            //        ViewEmailAlertPopup.<LoginToSite>o__SiteContainer16.<>p__Site19 = CallSite<Func<CallSite, object, object, object>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.BinaryOperation(CSharpBinderFlags.None, ExpressionType.NotEqual, typeof(ViewEmailAlertPopup), new CSharpArgumentInfo[]
            //        {
            //            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null),
            //            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant, null)
            //        }));
            //    }
            //    if (arg_126_0(arg_126_1, ViewEmailAlertPopup.<LoginToSite>o__SiteContainer16.<>p__Site19.Target(ViewEmailAlertPopup.<LoginToSite>o__SiteContainer16.<>p__Site19, item.getAttribute("name", 0), null)))
            //    {
            //        if (ViewEmailAlertPopup.<LoginToSite>o__SiteContainer16.<>p__Site1a == null)
            //        {
            //            ViewEmailAlertPopup.<LoginToSite>o__SiteContainer16.<>p__Site1a = CallSite<Func<CallSite, object, string>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.Convert(CSharpBinderFlags.None, typeof(string), typeof(ViewEmailAlertPopup)));
            //        }
            //        string str = ViewEmailAlertPopup.<LoginToSite>o__SiteContainer16.<>p__Site1a.Target(ViewEmailAlertPopup.<LoginToSite>o__SiteContainer16.<>p__Site1a, item.getAttribute("name", 0));
            //        if (str == "usernameshow")
            //        {
            //            item.setAttribute("value", this.eMailModal.MailID, 1);
            //            break;
            //        }
            //    }
            //}
            //foreach (IHTMLElement item in iHTMLCol)
            //{
            //    if (ViewEmailAlertPopup.<LoginToSite>o__SiteContainer16.<>p__Site1b == null)
            //    {
            //        ViewEmailAlertPopup.<LoginToSite>o__SiteContainer16.<>p__Site1b = CallSite<Func<CallSite, object, bool>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.UnaryOperation(CSharpBinderFlags.None, ExpressionType.IsTrue, typeof(ViewEmailAlertPopup), new CSharpArgumentInfo[]
            //        {
            //            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null)
            //        }));
            //    }
            //    Func<CallSite, object, bool> arg_2AF_0 = ViewEmailAlertPopup.<LoginToSite>o__SiteContainer16.<>p__Site1b.Target;
            //    CallSite arg_2AF_1 = ViewEmailAlertPopup.<LoginToSite>o__SiteContainer16.<>p__Site1b;
            //    if (ViewEmailAlertPopup.<LoginToSite>o__SiteContainer16.<>p__Site1c == null)
            //    {
            //        ViewEmailAlertPopup.<LoginToSite>o__SiteContainer16.<>p__Site1c = CallSite<Func<CallSite, object, object, object>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.BinaryOperation(CSharpBinderFlags.None, ExpressionType.NotEqual, typeof(ViewEmailAlertPopup), new CSharpArgumentInfo[]
            //        {
            //            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null),
            //            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant, null)
            //        }));
            //    }
            //    if (arg_2AF_0(arg_2AF_1, ViewEmailAlertPopup.<LoginToSite>o__SiteContainer16.<>p__Site1c.Target(ViewEmailAlertPopup.<LoginToSite>o__SiteContainer16.<>p__Site1c, item.getAttribute("name", 0), null)))
            //    {
            //        if (ViewEmailAlertPopup.<LoginToSite>o__SiteContainer16.<>p__Site1d == null)
            //        {
            //            ViewEmailAlertPopup.<LoginToSite>o__SiteContainer16.<>p__Site1d = CallSite<Func<CallSite, object, string>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.Convert(CSharpBinderFlags.None, typeof(string), typeof(ViewEmailAlertPopup)));
            //        }
            //        string str = ViewEmailAlertPopup.<LoginToSite>o__SiteContainer16.<>p__Site1d.Target(ViewEmailAlertPopup.<LoginToSite>o__SiteContainer16.<>p__Site1d, item.getAttribute("name", 0));
            //        if (str == "pwshow")
            //        {
            //            item.setAttribute("value", this.eMailModal.PWD, 1);
            //            break;
            //        }
            //    }
            //}
            //foreach (IHTMLElement item in iHTMLCol)
            //{
            //    if (ViewEmailAlertPopup.<LoginToSite>o__SiteContainer16.<>p__Site1e == null)
            //    {
            //        ViewEmailAlertPopup.<LoginToSite>o__SiteContainer16.<>p__Site1e = CallSite<Func<CallSite, object, bool>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.UnaryOperation(CSharpBinderFlags.None, ExpressionType.IsTrue, typeof(ViewEmailAlertPopup), new CSharpArgumentInfo[]
            //        {
            //            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null)
            //        }));
            //    }
            //    Func<CallSite, object, bool> arg_438_0 = ViewEmailAlertPopup.<LoginToSite>o__SiteContainer16.<>p__Site1e.Target;
            //    CallSite arg_438_1 = ViewEmailAlertPopup.<LoginToSite>o__SiteContainer16.<>p__Site1e;
            //    if (ViewEmailAlertPopup.<LoginToSite>o__SiteContainer16.<>p__Site1f == null)
            //    {
            //        ViewEmailAlertPopup.<LoginToSite>o__SiteContainer16.<>p__Site1f = CallSite<Func<CallSite, object, object, object>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.BinaryOperation(CSharpBinderFlags.None, ExpressionType.NotEqual, typeof(ViewEmailAlertPopup), new CSharpArgumentInfo[]
            //        {
            //            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null),
            //            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant, null)
            //        }));
            //    }
            //    if (arg_438_0(arg_438_1, ViewEmailAlertPopup.<LoginToSite>o__SiteContainer16.<>p__Site1f.Target(ViewEmailAlertPopup.<LoginToSite>o__SiteContainer16.<>p__Site1f, item.getAttribute("type", 0), null)))
            //    {
            //        if (ViewEmailAlertPopup.<LoginToSite>o__SiteContainer16.<>p__Site20 == null)
            //        {
            //            ViewEmailAlertPopup.<LoginToSite>o__SiteContainer16.<>p__Site20 = CallSite<Func<CallSite, object, string>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.Convert(CSharpBinderFlags.None, typeof(string), typeof(ViewEmailAlertPopup)));
            //        }
            //        string str = ViewEmailAlertPopup.<LoginToSite>o__SiteContainer16.<>p__Site20.Target(ViewEmailAlertPopup.<LoginToSite>o__SiteContainer16.<>p__Site20, item.getAttribute("type", 0));
            //        if (str == "submit")
            //        {
            //            item.click();
            //            break;
            //        }
            //    }
            //}
        }
        private void Login(InternetExplorer internetExplorer)
        {
            //HTMLDocument htmlDoc = null;
            //if (ViewEmailAlertPopup.<Login>o__SiteContainer21.<>p__Site22 == null)
            //{
            //    ViewEmailAlertPopup.<Login>o__SiteContainer21.<>p__Site22 = CallSite<Func<CallSite, object, HTMLDocument>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.Convert(CSharpBinderFlags.None, typeof(HTMLDocument), typeof(ViewEmailAlertPopup)));
            //}
            //htmlDoc = ViewEmailAlertPopup.<Login>o__SiteContainer21.<>p__Site22.Target(ViewEmailAlertPopup.<Login>o__SiteContainer21.<>p__Site22, internetExplorer.Document);
            //IHTMLElementCollection iHTMLCol = null;
            //iHTMLCol = htmlDoc.getElementsByTagName("input");
            //foreach (IHTMLElement item in iHTMLCol)
            //{
            //    if (ViewEmailAlertPopup.<Login>o__SiteContainer21.<>p__Site23 == null)
            //    {
            //        ViewEmailAlertPopup.<Login>o__SiteContainer21.<>p__Site23 = CallSite<Func<CallSite, object, bool>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.UnaryOperation(CSharpBinderFlags.None, ExpressionType.IsTrue, typeof(ViewEmailAlertPopup), new CSharpArgumentInfo[]
            //        {
            //            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null)
            //        }));
            //    }
            //    Func<CallSite, object, bool> arg_126_0 = ViewEmailAlertPopup.<Login>o__SiteContainer21.<>p__Site23.Target;
            //    CallSite arg_126_1 = ViewEmailAlertPopup.<Login>o__SiteContainer21.<>p__Site23;
            //    if (ViewEmailAlertPopup.<Login>o__SiteContainer21.<>p__Site24 == null)
            //    {
            //        ViewEmailAlertPopup.<Login>o__SiteContainer21.<>p__Site24 = CallSite<Func<CallSite, object, object, object>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.BinaryOperation(CSharpBinderFlags.None, ExpressionType.NotEqual, typeof(ViewEmailAlertPopup), new CSharpArgumentInfo[]
            //        {
            //            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null),
            //            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant, null)
            //        }));
            //    }
            //    if (arg_126_0(arg_126_1, ViewEmailAlertPopup.<Login>o__SiteContainer21.<>p__Site24.Target(ViewEmailAlertPopup.<Login>o__SiteContainer21.<>p__Site24, item.getAttribute("id", 0), null)))
            //    {
            //        if (ViewEmailAlertPopup.<Login>o__SiteContainer21.<>p__Site25 == null)
            //        {
            //            ViewEmailAlertPopup.<Login>o__SiteContainer21.<>p__Site25 = CallSite<Func<CallSite, object, string>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.Convert(CSharpBinderFlags.None, typeof(string), typeof(ViewEmailAlertPopup)));
            //        }
            //        string str = ViewEmailAlertPopup.<Login>o__SiteContainer21.<>p__Site25.Target(ViewEmailAlertPopup.<Login>o__SiteContainer21.<>p__Site25, item.getAttribute("id", 0));
            //        if (str == "emailinput")
            //        {
            //            item.setAttribute("value", this.eMailModal.MailID, 1);
            //            break;
            //        }
            //    }
            //}
            //foreach (IHTMLElement item in iHTMLCol)
            //{
            //    if (ViewEmailAlertPopup.<Login>o__SiteContainer21.<>p__Site26 == null)
            //    {
            //        ViewEmailAlertPopup.<Login>o__SiteContainer21.<>p__Site26 = CallSite<Func<CallSite, object, bool>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.UnaryOperation(CSharpBinderFlags.None, ExpressionType.IsTrue, typeof(ViewEmailAlertPopup), new CSharpArgumentInfo[]
            //        {
            //            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null)
            //        }));
            //    }
            //    Func<CallSite, object, bool> arg_2AF_0 = ViewEmailAlertPopup.<Login>o__SiteContainer21.<>p__Site26.Target;
            //    CallSite arg_2AF_1 = ViewEmailAlertPopup.<Login>o__SiteContainer21.<>p__Site26;
            //    if (ViewEmailAlertPopup.<Login>o__SiteContainer21.<>p__Site27 == null)
            //    {
            //        ViewEmailAlertPopup.<Login>o__SiteContainer21.<>p__Site27 = CallSite<Func<CallSite, object, object, object>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.BinaryOperation(CSharpBinderFlags.None, ExpressionType.NotEqual, typeof(ViewEmailAlertPopup), new CSharpArgumentInfo[]
            //        {
            //            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null),
            //            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant, null)
            //        }));
            //    }
            //    if (arg_2AF_0(arg_2AF_1, ViewEmailAlertPopup.<Login>o__SiteContainer21.<>p__Site27.Target(ViewEmailAlertPopup.<Login>o__SiteContainer21.<>p__Site27, item.getAttribute("id", 0), null)))
            //    {
            //        if (ViewEmailAlertPopup.<Login>o__SiteContainer21.<>p__Site28 == null)
            //        {
            //            ViewEmailAlertPopup.<Login>o__SiteContainer21.<>p__Site28 = CallSite<Func<CallSite, object, string>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.Convert(CSharpBinderFlags.None, typeof(string), typeof(ViewEmailAlertPopup)));
            //        }
            //        string str = ViewEmailAlertPopup.<Login>o__SiteContainer21.<>p__Site28.Target(ViewEmailAlertPopup.<Login>o__SiteContainer21.<>p__Site28, item.getAttribute("id", 0));
            //        if (str == "passinput")
            //        {
            //            item.setAttribute("value", this.eMailModal.PWD, 1);
            //            break;
            //        }
            //    }
            //}
            //iHTMLCol = htmlDoc.getElementsByTagName("button");
            //if (iHTMLCol != null && iHTMLCol.length > 0)
            //{
            //    foreach (IHTMLElement item in iHTMLCol)
            //    {
            //        if (ViewEmailAlertPopup.<Login>o__SiteContainer21.<>p__Site29 == null)
            //        {
            //            ViewEmailAlertPopup.<Login>o__SiteContainer21.<>p__Site29 = CallSite<Func<CallSite, object, bool>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.UnaryOperation(CSharpBinderFlags.None, ExpressionType.IsTrue, typeof(ViewEmailAlertPopup), new CSharpArgumentInfo[]
            //            {
            //                CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null)
            //            }));
            //        }
            //        Func<CallSite, object, bool> arg_460_0 = ViewEmailAlertPopup.<Login>o__SiteContainer21.<>p__Site29.Target;
            //        CallSite arg_460_1 = ViewEmailAlertPopup.<Login>o__SiteContainer21.<>p__Site29;
            //        if (ViewEmailAlertPopup.<Login>o__SiteContainer21.<>p__Site2a == null)
            //        {
            //            ViewEmailAlertPopup.<Login>o__SiteContainer21.<>p__Site2a = CallSite<Func<CallSite, object, object, object>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.BinaryOperation(CSharpBinderFlags.None, ExpressionType.NotEqual, typeof(ViewEmailAlertPopup), new CSharpArgumentInfo[]
            //            {
            //                CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null),
            //                CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant, null)
            //            }));
            //        }
            //        if (arg_460_0(arg_460_1, ViewEmailAlertPopup.<Login>o__SiteContainer21.<>p__Site2a.Target(ViewEmailAlertPopup.<Login>o__SiteContainer21.<>p__Site2a, item.getAttribute("type", 0), null)))
            //        {
            //            if (ViewEmailAlertPopup.<Login>o__SiteContainer21.<>p__Site2b == null)
            //            {
            //                ViewEmailAlertPopup.<Login>o__SiteContainer21.<>p__Site2b = CallSite<Func<CallSite, object, string>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.Convert(CSharpBinderFlags.None, typeof(string), typeof(ViewEmailAlertPopup)));
            //            }
            //            string str = ViewEmailAlertPopup.<Login>o__SiteContainer21.<>p__Site2b.Target(ViewEmailAlertPopup.<Login>o__SiteContainer21.<>p__Site2b, item.getAttribute("type", 0));
            //            if (ViewEmailAlertPopup.<Login>o__SiteContainer21.<>p__Site2c == null)
            //            {
            //                ViewEmailAlertPopup.<Login>o__SiteContainer21.<>p__Site2c = CallSite<Func<CallSite, object, string>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.Convert(CSharpBinderFlags.None, typeof(string), typeof(ViewEmailAlertPopup)));
            //            }
            //            string submit = ViewEmailAlertPopup.<Login>o__SiteContainer21.<>p__Site2c.Target(ViewEmailAlertPopup.<Login>o__SiteContainer21.<>p__Site2c, item.getAttribute("name", 0));
            //            if (submit == "submit" && str == "submit")
            //            {
            //                item.click();
            //                break;
            //            }
            //        }
            //    }
            //}
        }
        private void LoginToIdc(InternetExplorer internetExplorer)
		{
            //if (ViewEmailAlertPopup.<LoginToIdc>o__SiteContainer2d.<>p__Site2e == null)
            //{
            //    ViewEmailAlertPopup.<LoginToIdc>o__SiteContainer2d.<>p__Site2e = CallSite<Func<CallSite, object, HTMLDocument>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.Convert(CSharpBinderFlags.None, typeof(HTMLDocument), typeof(ViewEmailAlertPopup)));
            //}
            //HTMLDocument htmlDoc = ViewEmailAlertPopup.<LoginToIdc>o__SiteContainer2d.<>p__Site2e.Target(ViewEmailAlertPopup.<LoginToIdc>o__SiteContainer2d.<>p__Site2e, internetExplorer.Document);
            //IHTMLElementCollection iHTMLCol = null;
            //iHTMLCol = htmlDoc.getElementsByTagName("input");
            //foreach (IHTMLElement item in iHTMLCol)
            //{
            //    if (ViewEmailAlertPopup.<LoginToIdc>o__SiteContainer2d.<>p__Site2f == null)
            //    {
            //        ViewEmailAlertPopup.<LoginToIdc>o__SiteContainer2d.<>p__Site2f = CallSite<Func<CallSite, object, bool>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.UnaryOperation(CSharpBinderFlags.None, ExpressionType.IsTrue, typeof(ViewEmailAlertPopup), new CSharpArgumentInfo[]
            //        {
            //            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null)
            //        }));
            //    }
            //    Func<CallSite, object, bool> arg_126_0 = ViewEmailAlertPopup.<LoginToIdc>o__SiteContainer2d.<>p__Site2f.Target;
            //    CallSite arg_126_1 = ViewEmailAlertPopup.<LoginToIdc>o__SiteContainer2d.<>p__Site2f;
            //    if (ViewEmailAlertPopup.<LoginToIdc>o__SiteContainer2d.<>p__Site30 == null)
            //    {
            //        ViewEmailAlertPopup.<LoginToIdc>o__SiteContainer2d.<>p__Site30 = CallSite<Func<CallSite, object, object, object>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.BinaryOperation(CSharpBinderFlags.None, ExpressionType.NotEqual, typeof(ViewEmailAlertPopup), new CSharpArgumentInfo[]
            //        {
            //            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null),
            //            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant, null)
            //        }));
            //    }
            //    if (arg_126_0(arg_126_1, ViewEmailAlertPopup.<LoginToIdc>o__SiteContainer2d.<>p__Site30.Target(ViewEmailAlertPopup.<LoginToIdc>o__SiteContainer2d.<>p__Site30, item.getAttribute("name", 0), null)))
            //    {
            //        if (ViewEmailAlertPopup.<LoginToIdc>o__SiteContainer2d.<>p__Site31 == null)
            //        {
            //            ViewEmailAlertPopup.<LoginToIdc>o__SiteContainer2d.<>p__Site31 = CallSite<Func<CallSite, object, string>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.Convert(CSharpBinderFlags.None, typeof(string), typeof(ViewEmailAlertPopup)));
            //        }
            //        string str = ViewEmailAlertPopup.<LoginToIdc>o__SiteContainer2d.<>p__Site31.Target(ViewEmailAlertPopup.<LoginToIdc>o__SiteContainer2d.<>p__Site31, item.getAttribute("name", 0));
            //        if (str == "usr")
            //        {
            //            item.setAttribute("value", this.eMailModal.MailID.Substring(0, this.eMailModal.MailID.IndexOf('@')), 1);
            //            break;
            //        }
            //    }
            //}
            //foreach (IHTMLElement item in iHTMLCol)
            //{
            //    if (ViewEmailAlertPopup.<LoginToIdc>o__SiteContainer2d.<>p__Site32 == null)
            //    {
            //        ViewEmailAlertPopup.<LoginToIdc>o__SiteContainer2d.<>p__Site32 = CallSite<Func<CallSite, object, bool>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.UnaryOperation(CSharpBinderFlags.None, ExpressionType.IsTrue, typeof(ViewEmailAlertPopup), new CSharpArgumentInfo[]
            //        {
            //            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null)
            //        }));
            //    }
            //    Func<CallSite, object, bool> arg_2CA_0 = ViewEmailAlertPopup.<LoginToIdc>o__SiteContainer2d.<>p__Site32.Target;
            //    CallSite arg_2CA_1 = ViewEmailAlertPopup.<LoginToIdc>o__SiteContainer2d.<>p__Site32;
            //    if (ViewEmailAlertPopup.<LoginToIdc>o__SiteContainer2d.<>p__Site33 == null)
            //    {
            //        ViewEmailAlertPopup.<LoginToIdc>o__SiteContainer2d.<>p__Site33 = CallSite<Func<CallSite, object, object, object>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.BinaryOperation(CSharpBinderFlags.None, ExpressionType.NotEqual, typeof(ViewEmailAlertPopup), new CSharpArgumentInfo[]
            //        {
            //            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null),
            //            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant, null)
            //        }));
            //    }
            //    if (arg_2CA_0(arg_2CA_1, ViewEmailAlertPopup.<LoginToIdc>o__SiteContainer2d.<>p__Site33.Target(ViewEmailAlertPopup.<LoginToIdc>o__SiteContainer2d.<>p__Site33, item.getAttribute("name", 0), null)))
            //    {
            //        if (ViewEmailAlertPopup.<LoginToIdc>o__SiteContainer2d.<>p__Site34 == null)
            //        {
            //            ViewEmailAlertPopup.<LoginToIdc>o__SiteContainer2d.<>p__Site34 = CallSite<Func<CallSite, object, string>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.Convert(CSharpBinderFlags.None, typeof(string), typeof(ViewEmailAlertPopup)));
            //        }
            //        string str = ViewEmailAlertPopup.<LoginToIdc>o__SiteContainer2d.<>p__Site34.Target(ViewEmailAlertPopup.<LoginToIdc>o__SiteContainer2d.<>p__Site34, item.getAttribute("name", 0));
            //        if (str == "domain")
            //        {
            //            item.setAttribute("value", this.eMailModal.Text, 1);
            //            break;
            //        }
            //    }
            //}
            //foreach (IHTMLElement item in iHTMLCol)
            //{
            //    if (ViewEmailAlertPopup.<LoginToIdc>o__SiteContainer2d.<>p__Site35 == null)
            //    {
            //        ViewEmailAlertPopup.<LoginToIdc>o__SiteContainer2d.<>p__Site35 = CallSite<Func<CallSite, object, bool>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.UnaryOperation(CSharpBinderFlags.None, ExpressionType.IsTrue, typeof(ViewEmailAlertPopup), new CSharpArgumentInfo[]
            //        {
            //            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null)
            //        }));
            //    }
            //    Func<CallSite, object, bool> arg_453_0 = ViewEmailAlertPopup.<LoginToIdc>o__SiteContainer2d.<>p__Site35.Target;
            //    CallSite arg_453_1 = ViewEmailAlertPopup.<LoginToIdc>o__SiteContainer2d.<>p__Site35;
            //    if (ViewEmailAlertPopup.<LoginToIdc>o__SiteContainer2d.<>p__Site36 == null)
            //    {
            //        ViewEmailAlertPopup.<LoginToIdc>o__SiteContainer2d.<>p__Site36 = CallSite<Func<CallSite, object, object, object>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.BinaryOperation(CSharpBinderFlags.None, ExpressionType.NotEqual, typeof(ViewEmailAlertPopup), new CSharpArgumentInfo[]
            //        {
            //            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null),
            //            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant, null)
            //        }));
            //    }
            //    if (arg_453_0(arg_453_1, ViewEmailAlertPopup.<LoginToIdc>o__SiteContainer2d.<>p__Site36.Target(ViewEmailAlertPopup.<LoginToIdc>o__SiteContainer2d.<>p__Site36, item.getAttribute("name", 0), null)))
            //    {
            //        if (ViewEmailAlertPopup.<LoginToIdc>o__SiteContainer2d.<>p__Site37 == null)
            //        {
            //            ViewEmailAlertPopup.<LoginToIdc>o__SiteContainer2d.<>p__Site37 = CallSite<Func<CallSite, object, string>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.Convert(CSharpBinderFlags.None, typeof(string), typeof(ViewEmailAlertPopup)));
            //        }
            //        string str = ViewEmailAlertPopup.<LoginToIdc>o__SiteContainer2d.<>p__Site37.Target(ViewEmailAlertPopup.<LoginToIdc>o__SiteContainer2d.<>p__Site37, item.getAttribute("name", 0));
            //        if (str == "pass")
            //        {
            //            item.setAttribute("value", this.eMailModal.PWD, 1);
            //            break;
            //        }
            //    }
            //}
            //foreach (IHTMLElement item in iHTMLCol)
            //{
            //    if (ViewEmailAlertPopup.<LoginToIdc>o__SiteContainer2d.<>p__Site38 == null)
            //    {
            //        ViewEmailAlertPopup.<LoginToIdc>o__SiteContainer2d.<>p__Site38 = CallSite<Func<CallSite, object, bool>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.UnaryOperation(CSharpBinderFlags.None, ExpressionType.IsTrue, typeof(ViewEmailAlertPopup), new CSharpArgumentInfo[]
            //        {
            //            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null)
            //        }));
            //    }
            //    Func<CallSite, object, bool> arg_5DC_0 = ViewEmailAlertPopup.<LoginToIdc>o__SiteContainer2d.<>p__Site38.Target;
            //    CallSite arg_5DC_1 = ViewEmailAlertPopup.<LoginToIdc>o__SiteContainer2d.<>p__Site38;
            //    if (ViewEmailAlertPopup.<LoginToIdc>o__SiteContainer2d.<>p__Site39 == null)
            //    {
            //        ViewEmailAlertPopup.<LoginToIdc>o__SiteContainer2d.<>p__Site39 = CallSite<Func<CallSite, object, object, object>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.BinaryOperation(CSharpBinderFlags.None, ExpressionType.NotEqual, typeof(ViewEmailAlertPopup), new CSharpArgumentInfo[]
            //        {
            //            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null),
            //            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant, null)
            //        }));
            //    }
            //    if (arg_5DC_0(arg_5DC_1, ViewEmailAlertPopup.<LoginToIdc>o__SiteContainer2d.<>p__Site39.Target(ViewEmailAlertPopup.<LoginToIdc>o__SiteContainer2d.<>p__Site39, item.getAttribute("name", 0), null)))
            //    {
            //        if (ViewEmailAlertPopup.<LoginToIdc>o__SiteContainer2d.<>p__Site3a == null)
            //        {
            //            ViewEmailAlertPopup.<LoginToIdc>o__SiteContainer2d.<>p__Site3a = CallSite<Func<CallSite, object, string>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.Convert(CSharpBinderFlags.None, typeof(string), typeof(ViewEmailAlertPopup)));
            //        }
            //        string str = ViewEmailAlertPopup.<LoginToIdc>o__SiteContainer2d.<>p__Site3a.Target(ViewEmailAlertPopup.<LoginToIdc>o__SiteContainer2d.<>p__Site3a, item.getAttribute("name", 0));
            //        if (str == "input")
            //        {
            //            item.click();
            //            break;
            //        }
            //    }
            //}
		}
        private void LoginToGmail(InternetExplorer internetExplorer)
		{
            //if (ViewEmailAlertPopup.<LoginToGmail>o__SiteContainer3b.<>p__Site3c == null)
            //{
            //    ViewEmailAlertPopup.<LoginToGmail>o__SiteContainer3b.<>p__Site3c = CallSite<Func<CallSite, object, HTMLDocument>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.Convert(CSharpBinderFlags.None, typeof(HTMLDocument), typeof(ViewEmailAlertPopup)));
            //}
            //HTMLDocument htmlDoc = ViewEmailAlertPopup.<LoginToGmail>o__SiteContainer3b.<>p__Site3c.Target(ViewEmailAlertPopup.<LoginToGmail>o__SiteContainer3b.<>p__Site3c, internetExplorer.Document);
            //IHTMLElementCollection iHTMLCol = null;
            //iHTMLCol = htmlDoc.getElementsByTagName("input");
            //foreach (IHTMLElement item in iHTMLCol)
            //{
            //    if (ViewEmailAlertPopup.<LoginToGmail>o__SiteContainer3b.<>p__Site3d == null)
            //    {
            //        ViewEmailAlertPopup.<LoginToGmail>o__SiteContainer3b.<>p__Site3d = CallSite<Func<CallSite, object, bool>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.UnaryOperation(CSharpBinderFlags.None, ExpressionType.IsTrue, typeof(ViewEmailAlertPopup), new CSharpArgumentInfo[]
            //        {
            //            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null)
            //        }));
            //    }
            //    Func<CallSite, object, bool> arg_126_0 = ViewEmailAlertPopup.<LoginToGmail>o__SiteContainer3b.<>p__Site3d.Target;
            //    CallSite arg_126_1 = ViewEmailAlertPopup.<LoginToGmail>o__SiteContainer3b.<>p__Site3d;
            //    if (ViewEmailAlertPopup.<LoginToGmail>o__SiteContainer3b.<>p__Site3e == null)
            //    {
            //        ViewEmailAlertPopup.<LoginToGmail>o__SiteContainer3b.<>p__Site3e = CallSite<Func<CallSite, object, object, object>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.BinaryOperation(CSharpBinderFlags.None, ExpressionType.NotEqual, typeof(ViewEmailAlertPopup), new CSharpArgumentInfo[]
            //        {
            //            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null),
            //            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant, null)
            //        }));
            //    }
            //    if (arg_126_0(arg_126_1, ViewEmailAlertPopup.<LoginToGmail>o__SiteContainer3b.<>p__Site3e.Target(ViewEmailAlertPopup.<LoginToGmail>o__SiteContainer3b.<>p__Site3e, item.getAttribute("id", 0), null)))
            //    {
            //        if (ViewEmailAlertPopup.<LoginToGmail>o__SiteContainer3b.<>p__Site3f == null)
            //        {
            //            ViewEmailAlertPopup.<LoginToGmail>o__SiteContainer3b.<>p__Site3f = CallSite<Func<CallSite, object, string>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.Convert(CSharpBinderFlags.None, typeof(string), typeof(ViewEmailAlertPopup)));
            //        }
            //        string str = ViewEmailAlertPopup.<LoginToGmail>o__SiteContainer3b.<>p__Site3f.Target(ViewEmailAlertPopup.<LoginToGmail>o__SiteContainer3b.<>p__Site3f, item.getAttribute("id", 0));
            //        if (str == "Email")
            //        {
            //            item.setAttribute("value", this.eMailModal.MailID, 1);
            //            break;
            //        }
            //    }
            //}
            //foreach (IHTMLElement item in iHTMLCol)
            //{
            //    if (ViewEmailAlertPopup.<LoginToGmail>o__SiteContainer3b.<>p__Site40 == null)
            //    {
            //        ViewEmailAlertPopup.<LoginToGmail>o__SiteContainer3b.<>p__Site40 = CallSite<Func<CallSite, object, bool>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.UnaryOperation(CSharpBinderFlags.None, ExpressionType.IsTrue, typeof(ViewEmailAlertPopup), new CSharpArgumentInfo[]
            //        {
            //            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null)
            //        }));
            //    }
            //    Func<CallSite, object, bool> arg_2AF_0 = ViewEmailAlertPopup.<LoginToGmail>o__SiteContainer3b.<>p__Site40.Target;
            //    CallSite arg_2AF_1 = ViewEmailAlertPopup.<LoginToGmail>o__SiteContainer3b.<>p__Site40;
            //    if (ViewEmailAlertPopup.<LoginToGmail>o__SiteContainer3b.<>p__Site41 == null)
            //    {
            //        ViewEmailAlertPopup.<LoginToGmail>o__SiteContainer3b.<>p__Site41 = CallSite<Func<CallSite, object, object, object>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.BinaryOperation(CSharpBinderFlags.None, ExpressionType.NotEqual, typeof(ViewEmailAlertPopup), new CSharpArgumentInfo[]
            //        {
            //            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null),
            //            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant, null)
            //        }));
            //    }
            //    if (arg_2AF_0(arg_2AF_1, ViewEmailAlertPopup.<LoginToGmail>o__SiteContainer3b.<>p__Site41.Target(ViewEmailAlertPopup.<LoginToGmail>o__SiteContainer3b.<>p__Site41, item.getAttribute("id", 0), null)))
            //    {
            //        if (ViewEmailAlertPopup.<LoginToGmail>o__SiteContainer3b.<>p__Site42 == null)
            //        {
            //            ViewEmailAlertPopup.<LoginToGmail>o__SiteContainer3b.<>p__Site42 = CallSite<Func<CallSite, object, string>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.Convert(CSharpBinderFlags.None, typeof(string), typeof(ViewEmailAlertPopup)));
            //        }
            //        string str = ViewEmailAlertPopup.<LoginToGmail>o__SiteContainer3b.<>p__Site42.Target(ViewEmailAlertPopup.<LoginToGmail>o__SiteContainer3b.<>p__Site42, item.getAttribute("id", 0));
            //        if (str == "Passwd")
            //        {
            //            item.setAttribute("value", this.eMailModal.PWD, 1);
            //            break;
            //        }
            //    }
            //}
            //foreach (IHTMLElement item in iHTMLCol)
            //{
            //    if (ViewEmailAlertPopup.<LoginToGmail>o__SiteContainer3b.<>p__Site43 == null)
            //    {
            //        ViewEmailAlertPopup.<LoginToGmail>o__SiteContainer3b.<>p__Site43 = CallSite<Func<CallSite, object, bool>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.UnaryOperation(CSharpBinderFlags.None, ExpressionType.IsTrue, typeof(ViewEmailAlertPopup), new CSharpArgumentInfo[]
            //        {
            //            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null)
            //        }));
            //    }
            //    Func<CallSite, object, bool> arg_438_0 = ViewEmailAlertPopup.<LoginToGmail>o__SiteContainer3b.<>p__Site43.Target;
            //    CallSite arg_438_1 = ViewEmailAlertPopup.<LoginToGmail>o__SiteContainer3b.<>p__Site43;
            //    if (ViewEmailAlertPopup.<LoginToGmail>o__SiteContainer3b.<>p__Site44 == null)
            //    {
            //        ViewEmailAlertPopup.<LoginToGmail>o__SiteContainer3b.<>p__Site44 = CallSite<Func<CallSite, object, object, object>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.BinaryOperation(CSharpBinderFlags.None, ExpressionType.NotEqual, typeof(ViewEmailAlertPopup), new CSharpArgumentInfo[]
            //        {
            //            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null),
            //            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant, null)
            //        }));
            //    }
            //    if (arg_438_0(arg_438_1, ViewEmailAlertPopup.<LoginToGmail>o__SiteContainer3b.<>p__Site44.Target(ViewEmailAlertPopup.<LoginToGmail>o__SiteContainer3b.<>p__Site44, item.getAttribute("id", 0), null)))
            //    {
            //        if (ViewEmailAlertPopup.<LoginToGmail>o__SiteContainer3b.<>p__Site45 == null)
            //        {
            //            ViewEmailAlertPopup.<LoginToGmail>o__SiteContainer3b.<>p__Site45 = CallSite<Func<CallSite, object, string>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.Convert(CSharpBinderFlags.None, typeof(string), typeof(ViewEmailAlertPopup)));
            //        }
            //        string str = ViewEmailAlertPopup.<LoginToGmail>o__SiteContainer3b.<>p__Site45.Target(ViewEmailAlertPopup.<LoginToGmail>o__SiteContainer3b.<>p__Site45, item.getAttribute("id", 0));
            //        if (str == "signIn")
            //        {
            //            item.click();
            //            break;
            //        }
            //    }
            //}
		}
        private void Setmail(EMailModal eMailModal)
        {
            POP3 pop3 = PopMailFactory.GetPopMail(eMailModal);
            pop3.GetAll();
            EmailManagerViewModel viewModel = new EmailManagerViewModel();
            viewModel.UpdateEmail(ServiceUtil.Instance.SessionService.Uid, eMailModal.MailID, (int)eMailModal.EMailType, eMailModal.Span.ToString(), "");
        }
        private void ListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (base.IsOpen)
            {
                base.IsOpen = false;
            }
        }
        private void SettingAccountHandler(object sender, RoutedEventArgs e)
        {
            if (base.IsOpen)
            {
                base.IsOpen = false;
            }
            SettingEmailAccountWindow setting = SettingEmailAccountWindow.GetSettingEmailAccountWindow;
            if (setting.IsLoaded)
            {
                setting.Activate();
            }
            else
            {
                setting.Show();
            }
        }
        //[System.Diagnostics.DebuggerNonUserCode]
        //public void InitializeComponent()
        //{
        //    if (!this._contentLoaded)
        //    {
        //        this._contentLoaded = true;
        //        Uri resourceLocater = new Uri("/IDKin.IM.Windows;component/view/emailalert/viewemailalertpopup.xaml", UriKind.Relative);
        //        Application.LoadComponent(this, resourceLocater);
        //    }
        //}
        //[EditorBrowsable(EditorBrowsableState.Never), System.Diagnostics.DebuggerNonUserCode]
        //void IComponentConnector.Connect(int connectionId, object target)
        //{
        //    switch (connectionId)
        //    {
        //        case 1:
        //            this.emailAccountListBox = (ListBox)target;
        //            this.emailAccountListBox.SelectionChanged += new SelectionChangedEventHandler(this.ListBox_SelectionChanged);
        //            break;
        //        case 2:
        //            ((Button)target).Click += new RoutedEventHandler(this.SettingAccountHandler);
        //            break;
        //        default:
        //            this._contentLoaded = true;
        //            break;
        //    }
        //}
    }
}
