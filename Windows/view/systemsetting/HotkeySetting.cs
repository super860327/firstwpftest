using IDKin.IM.Data;
using IDKin.IM.Windows.Model;
using IDKin.IM.Windows.Properties;
using IDKin.IM.Windows.Util;
using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Markup;
namespace IDKin.IM.Windows.View.SystemSetting
{
	[GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
    public partial class HotkeySetting : Canvas//, IComponentConnector
	{
		private ISessionService sessionService = ServiceUtil.Instance.SessionService;
        //internal ListBoxItem recoverItem;
        //internal TextBlock tbkRecover;
        //internal System.Windows.Controls.TextBox tbxRecover;
        //internal TextBlock recoverCaption;
        //internal ListBoxItem cutItem;
        //internal TextBlock tbkCut;
        //internal System.Windows.Controls.TextBox tbxCut;
        //internal TextBlock cutCaption;
        //internal System.Windows.Controls.Button btnDefault;
        //private bool _contentLoaded;
		public HotkeySetting()
		{
			this.InitializeComponent();
			this.InitUI();
			this.AddEventListener();
		}
		private void InitUI()
		{
			this.tbxCut.Text = Settings.Default.CutScreenString;
			this.tbxRecover.Text = Settings.Default.PickupMsgString;
		}
		private void AddEventListener()
		{
			this.btnDefault.Click += new RoutedEventHandler(this.btnDefault_Click);
		}
		private void btnDefault_Click(object sender, RoutedEventArgs e)
		{
			if (System.Windows.MessageBox.Show("您确定要恢复热键到系统默认值？", "提示", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
			{
				Settings.Default.CutScreenString = "Ctrl + Alt + S";
				Settings.Default.PickupMsgString = "Ctrl + Alt + Z";
				Settings.Default.CutScreen = Keys.S;
				Settings.Default.PickupMsg = Keys.Z;
				Settings.Default.CutScreenType = 3;
				Settings.Default.CutScreenCAS = 1;
				DataModel.Instance.CutScreenKey = Settings.Default.CutScreen;
				DataModel.Instance.PickUpMsgKey = Settings.Default.PickupMsg;
				DataModel.Instance.CutScreenType = Settings.Default.CutScreenType;
				DataModel.Instance.CutScreenCAS = Settings.Default.CutScreenCAS;
				Settings.Default.Save();
				this.InitUI();
			}
		}
		private void TextBlock_MouseDown(object sender, MouseButtonEventArgs e)
		{
			if (e.Source == this.tbkRecover)
			{
				if (e.LeftButton == MouseButtonState.Pressed)
				{
					this.tbkRecover.Visibility = Visibility.Collapsed;
					this.tbxRecover.Visibility = Visibility.Visible;
					this.tbxRecover.Focus();
					this.recoverCaption.Text = "按键盘即可设置";
					DataModel.Instance.IsAllowPickup = false;
					DataModel.Instance.IsSetHotKey = true;
				}
			}
			if (e.Source == this.tbkCut)
			{
				if (e.LeftButton == MouseButtonState.Pressed)
				{
					this.tbkCut.Visibility = Visibility.Collapsed;
					this.tbxCut.Visibility = Visibility.Visible;
					this.tbxCut.Focus();
					this.cutCaption.Text = "按键盘即可设置";
					this.sessionService.IsAllowCut = false;
					DataModel.Instance.IsSetHotKey = true;
				}
			}
		}
		private void TextBox_LostFocus(object sender, RoutedEventArgs e)
		{
			if (e.Source == this.tbxRecover)
			{
				this.tbxRecover.Visibility = Visibility.Collapsed;
				this.tbkRecover.Visibility = Visibility.Visible;
				this.recoverCaption.Text = "";
				DataModel.Instance.IsAllowPickup = true;
			}
			if (e.Source == this.tbxCut)
			{
				this.tbxCut.Visibility = Visibility.Collapsed;
				this.tbkCut.Visibility = Visibility.Visible;
				this.cutCaption.Text = "";
				this.sessionService.IsAllowCut = true;
			}
		}
        //[System.Diagnostics.DebuggerNonUserCode]
        //public void InitializeComponent()
        //{
        //    if (!this._contentLoaded)
        //    {
        //        this._contentLoaded = true;
        //        Uri resourceLocater = new Uri("/IDKin.IM.Windows;component/view/systemsetting/hotkeysetting.xaml", UriKind.Relative);
        //        System.Windows.Application.LoadComponent(this, resourceLocater);
        //    }
        //}
        //[EditorBrowsable(EditorBrowsableState.Never), System.Diagnostics.DebuggerNonUserCode]
        //void IComponentConnector.Connect(int connectionId, object target)
        //{
        //    switch (connectionId)
        //    {
        //    case 1:
        //        this.recoverItem = (ListBoxItem)target;
        //        break;
        //    case 2:
        //        this.tbkRecover = (TextBlock)target;
        //        this.tbkRecover.MouseDown += new MouseButtonEventHandler(this.TextBlock_MouseDown);
        //        break;
        //    case 3:
        //        this.tbxRecover = (System.Windows.Controls.TextBox)target;
        //        this.tbxRecover.LostFocus += new RoutedEventHandler(this.TextBox_LostFocus);
        //        break;
        //    case 4:
        //        this.recoverCaption = (TextBlock)target;
        //        break;
        //    case 5:
        //        this.cutItem = (ListBoxItem)target;
        //        break;
        //    case 6:
        //        this.tbkCut = (TextBlock)target;
        //        this.tbkCut.MouseDown += new MouseButtonEventHandler(this.TextBlock_MouseDown);
        //        break;
        //    case 7:
        //        this.tbxCut = (System.Windows.Controls.TextBox)target;
        //        this.tbxCut.LostFocus += new RoutedEventHandler(this.TextBox_LostFocus);
        //        break;
        //    case 8:
        //        this.cutCaption = (TextBlock)target;
        //        break;
        //    case 9:
        //        this.btnDefault = (System.Windows.Controls.Button)target;
        //        break;
        //    default:
        //        this._contentLoaded = true;
        //        break;
        //    }
        //}
	}
}
