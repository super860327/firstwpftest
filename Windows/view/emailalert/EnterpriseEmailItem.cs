using IDKin.IM.Core;
using IDKin.IM.Windows.Model;
using IDKin.IM.Windows.Util;
using IDKin.IM.Windows.ViewModel;
using System;
using System.CodeDom.Compiler;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Markup;
namespace IDKin.IM.Windows.View.EmailAlert
{
	[GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
    public partial class EnterpriseEmailItem : TabItem//, IComponentConnector
	{
		private const int TIMESPAN = 1;
		private EMailModal modal;
		private EmailAccountItem item;
		private string strMsg = string.Empty;
		private string mailId;
        //internal ComboBox cbxEmailService;
        //internal TextBox txtID;
        //internal TextBox promptServer;
        //internal PasswordBox txtPwd;
        //internal TextBlock tbkMsg;
        //internal Button btnOpen;
        //internal ListBox lstMailID;
        //private bool _contentLoaded;
		public event System.EventHandler BeginStory;
		public event System.EventHandler StopStory;
		public EnterpriseEmailItem()
		{
			this.InitializeComponent();
			this.Initial();
			this.InitialEmail();
			this.txtID.KeyDown += new KeyEventHandler(this.txtID_KeyDown);
			this.txtPwd.KeyDown += new KeyEventHandler(this.txtPwd_KeyDown);
		}
		private void Initial()
		{
			System.Collections.Generic.List<EmailServerInfo> EmailServerInfos = new System.Collections.Generic.List<EmailServerInfo>
			{
				new EmailServerInfo
				{
					Text = "请选择邮件服务商"
				},
				new EmailServerInfo
				{
					URL = "http://webmail.idccenter.net",
					Server = "pop.idccenter.net",
					Text = "美橙互联"
				}
			};
			this.cbxEmailService.ItemsSource = EmailServerInfos;
			this.cbxEmailService.SelectedIndex = 0;
		}
		private void InitialEmail()
		{
			if (DataModel.Instance.EmailList != null && DataModel.Instance.EmailList.Count > 0)
			{
				for (int i = 0; i < DataModel.Instance.EmailList.Count; i++)
				{
					if (DataModel.Instance.EmailList[i].EMailType == EmailType.EnterpriseEmail)
					{
						EmailAccountItem item = new EmailAccountItem(EmailAccountItemType.type2);
						item.Tag = DataModel.Instance.EmailList[i].MailID;
						item.tbkAccount.Text = DataModel.Instance.EmailList[i].MailID;
						item.DataContext = DataModel.Instance.EmailList[i];
						item.ItemDelete += new System.EventHandler(this.item_ItemDelete);
						if (string.IsNullOrEmpty(DataModel.Instance.EmailList[i].PWD))
						{
							item.AccoutInvalid();
						}
						this.lstMailID.Items.Add(item);
					}
				}
			}
		}
		private void btnOpen_Click(object sender, RoutedEventArgs e)
		{
			this.tbkMsg.Text = string.Empty;
			this.mailId = string.Format("{0}@{1}", this.txtID.Text.Trim(), this.promptServer.Text.Trim());
			if (!this.IsExist(this.mailId))
			{
				this.tbkMsg.Text = string.Empty;
				this.GetMailInfo2(this.mailId);
			}
		}
		private void GetMailInfo2(string mailId)
		{
			try
			{
				this.InitialModel(mailId);
				if (this.BeginStory != null)
				{
					this.BeginStory(this, System.EventArgs.Empty);
				}
				Func<EMailModal, System.DateTime> testConnHandler = new Func<EMailModal, System.DateTime>(this.TestConnectAndReturnDateTime);
				testConnHandler.BeginInvoke(this.modal, new System.AsyncCallback(this.CallBackFunction), testConnHandler);
			}
			catch (System.Exception ex)
			{
				if (this.StopStory != null)
				{
					this.StopStory(this, System.EventArgs.Empty);
				}
				ServiceUtil.Instance.Logger.Error(ex.ToString());
			}
		}
		private void InitialModel(string mailId)
		{
			this.item = new EmailAccountItem(EmailAccountItemType.type2);
			this.item.tbkAccount.Text = mailId;
			this.item.Tag = mailId;
			this.modal = new EMailModal();
			this.modal.MailID = mailId;
			this.modal.Url = (this.cbxEmailService.SelectedItem as EmailServerInfo).URL;
			this.modal.PWD = this.txtPwd.Password;
			this.modal.EMailType = EmailType.EnterpriseEmail;
			this.modal.UID = ServiceUtil.Instance.SessionService.Uid;
			this.modal.Server = (this.cbxEmailService.SelectedItem as EmailServerInfo).Server;
			this.modal.Text = this.promptServer.Text.Trim();
			this.modal.Span = 1;
		}
		private System.DateTime TestConnectAndReturnDateTime(EMailModal modal)
		{
			POP3 pop3 = PopMailFactory.GetPopMail(modal);
			System.DateTime time = System.DateTime.MinValue;
			if (pop3 != null)
			{
				string str = pop3.TestConnect();
				this.strMsg = str;
				if (string.IsNullOrEmpty(str))
				{
					pop3.GetAll();
				}
				else
				{
                    //base.Dispatcher.BeginInvoke(delegate
                    //{
                    //    this.tbkMsg.Text = str;
                    //    this.btnOpen.IsEnabled = false;
                    //}, new object[0]);
				}
			}
			else
			{
                //base.Dispatcher.BeginInvoke(delegate
                //{
                //    this.tbkMsg.Text = "邮件服务地址出错";
                //    this.btnOpen.IsEnabled = false;
                //}, new object[0]);
			}
			return time;
		}
		private void CallBackFunction(System.IAsyncResult ar)
		{
			Func<EMailModal, System.DateTime> f = ar.AsyncState as Func<EMailModal, System.DateTime>;
			if (f != null)
			{
                //base.Dispatcher.BeginInvoke(delegate
                //{
                //    if (string.IsNullOrEmpty(this.strMsg))
                //    {
                //        System.DateTime time = f.EndInvoke(ar);
                //        EmailManagerViewModel viewModel = new EmailManagerViewModel();
                //        this.item.DataContext = this.modal;
                //        this.modal.LastUpdateTime = time;
                //        viewModel.AddEmail(ServiceUtil.Instance.SessionService.Uid, this.mailId, (int)this.modal.EMailType, this.modal.Server, this.modal.Url, time.ToString(), this.modal.Span.ToString());
                //        DataModel.Instance.EmailList.Add(this.modal);
                //        this.lstMailID.Items.Add(this.item);
                //        this.item.ItemDelete += new System.EventHandler(this.item_ItemDelete);
                //        this.txtID.Text = string.Empty;
                //        this.txtPwd.Password = string.Empty;
                //        Setting s = new Setting(this.modal);
                //        s.GetNewMailCountLoop();
                //    }
                //    else
                //    {
                //        this.tbkMsg.Text = this.strMsg;
                //    }
                //    if (this.StopStory != null)
                //    {
                //        this.StopStory(this, System.EventArgs.Empty);
                //    }
                //}, new object[0]);
			}
		}
		private void item_ItemDelete(object sender, System.EventArgs e)
		{
			if (this.lstMailID.SelectedItem != null)
			{
				this.Delete(this.lstMailID.SelectedItem as ListBoxItem);
			}
		}
		private void Delete(ListBoxItem item)
		{
			if (item != null)
			{
				EMailModal model = item.DataContext as EMailModal;
				if (model != null && DataModel.Instance.EmailList.Contains(model))
				{
					EmailManagerViewModel viewModel = new EmailManagerViewModel();
					viewModel.DeleteEmail(ServiceUtil.Instance.SessionService.Uid, model.MailID);
					if (DataModel.Instance.EmailList.IndexOf(model) >= 0)
					{
						DataModel.Instance.EmailList.Remove(model);
					}
					this.lstMailID.Items.Remove(item);
					(ServiceUtil.Instance.DataService.INWindow as INWindow).UpdateMailCount();
				}
			}
		}
		private void cboxServer_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			this.TextChanged();
		}
		private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
		{
			this.TextChanged();
		}
		private void txtPwd_PasswordChanged(object sender, RoutedEventArgs e)
		{
			this.TextChanged();
		}
		private void TextChanged()
		{
			this.tbkMsg.Text = string.Empty;
			if (string.IsNullOrWhiteSpace(this.txtID.Text) || string.IsNullOrWhiteSpace(this.txtPwd.Password) || this.cbxEmailService.SelectedItem == null || string.IsNullOrWhiteSpace(this.promptServer.Text) || this.cbxEmailService.SelectedIndex == 0)
			{
				this.btnOpen.IsEnabled = false;
			}
			else
			{
				this.btnOpen.IsEnabled = true;
			}
		}
		private bool IsExist(string mailID)
		{
			bool result;
			if (string.IsNullOrEmpty(mailID) || this.lstMailID.Items.Count == 0)
			{
				result = false;
			}
			else
			{
				foreach (ListBoxItem item in (System.Collections.IEnumerable)this.lstMailID.Items)
				{
					if (item.Tag.ToString() == mailID.Trim())
					{
						this.tbkMsg.Text = "已存在这个用户！";
						result = true;
						return result;
					}
				}
				result = false;
			}
			return result;
		}
		private void promptServer_TextChanged(object sender, TextChangedEventArgs e)
		{
			this.TextChanged();
		}
		private void cbxEmailService_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			this.TextChanged();
		}
		private void txtPwd_KeyDown(object sender, KeyEventArgs e)
		{
			if (this.btnOpen.IsEnabled && e.Key == Key.Return)
			{
				this.btnOpen.Focus();
				this.btnOpen_Click(sender, e);
			}
		}
		private void txtID_KeyDown(object sender, KeyEventArgs e)
		{
			if (e.Key == Key.Return)
			{
				this.txtPwd.Focus();
				this.txtPwd.SelectAll();
			}
		}
        //[System.Diagnostics.DebuggerNonUserCode]
        //public void InitializeComponent()
        //{
        //    if (!this._contentLoaded)
        //    {
        //        this._contentLoaded = true;
        //        Uri resourceLocater = new Uri("/IDKin.IM.Windows;component/view/emailalert/enterpriseemailitem.xaml", UriKind.Relative);
        //        Application.LoadComponent(this, resourceLocater);
        //    }
        //}
        //[EditorBrowsable(EditorBrowsableState.Never), System.Diagnostics.DebuggerNonUserCode]
        //void IComponentConnector.Connect(int connectionId, object target)
        //{
        //    switch (connectionId)
        //    {
        //    case 1:
        //        this.cbxEmailService = (ComboBox)target;
        //        this.cbxEmailService.SelectionChanged += new SelectionChangedEventHandler(this.cbxEmailService_SelectionChanged);
        //        break;
        //    case 2:
        //        this.txtID = (TextBox)target;
        //        this.txtID.TextChanged += new TextChangedEventHandler(this.TextBox_TextChanged);
        //        break;
        //    case 3:
        //        this.promptServer = (TextBox)target;
        //        this.promptServer.TextChanged += new TextChangedEventHandler(this.promptServer_TextChanged);
        //        break;
        //    case 4:
        //        this.txtPwd = (PasswordBox)target;
        //        this.txtPwd.PasswordChanged += new RoutedEventHandler(this.txtPwd_PasswordChanged);
        //        break;
        //    case 5:
        //        this.tbkMsg = (TextBlock)target;
        //        break;
        //    case 6:
        //        this.btnOpen = (Button)target;
        //        this.btnOpen.Click += new RoutedEventHandler(this.btnOpen_Click);
        //        break;
        //    case 7:
        //        this.lstMailID = (ListBox)target;
        //        break;
        //    default:
        //        this._contentLoaded = true;
        //        break;
        //    }
        //}
	}
}
