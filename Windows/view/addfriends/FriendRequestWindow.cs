using IDKin.IM.Core;
using IDKin.IM.Data;
using IDKin.IM.Protocol.Center;
using IDKin.IM.Theme.Helper;
using IDKin.IM.Windows.Comps;
using IDKin.IM.Windows.Util;
using IDKin.IM.Windows.ViewModel;
using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Markup;
namespace IDKin.IM.Windows.View.AddFriends
{
	[GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
    public partial class FriendRequestWindow : Window//, IComponentConnector
	{
		private ISessionService sessionService = ServiceUtil.Instance.SessionService;
		private IDataService dataService = ServiceUtil.Instance.DataService;
		private RosterViewModel viewModel = new RosterViewModel();
		private long uid;
		private long rosterId;
		private string rosterJid;
		private string message;
		private User user;
		private long categotyId;
        //internal Border OuterBorder;
        //internal Border InnerBorder;
        //internal Image imgIcon;
        //internal MinButton btnMin;
        //internal CloseButton btnClose;
        //internal TextBlock tbRequesterName;
        //internal TextBlock tbkId;
        //internal TextBlock tbkSex;
        //internal TextBlock tbkAge;
        //internal TextBlock tbkAddress;
        //internal TextBlock tbkPost;
        //internal Button btnViewInfo;
        //internal RadioButton radioButton1;
        //internal RadioButton radioButton2;
        //internal RadioButton radioButton3;
        //internal TextBox tbMessage;
        //internal Button btnAgree;
        //internal Button btnForget;
        //private bool _contentLoaded;
		public FriendRequestWindow(long uid, long rosterId, string rosterJid, string message, User user, long categotyId)
		{
			ThemeSwitcher.LoadSkin(ThemeEnum.Aero, this);
			this.InitializeComponent();
			this.uid = uid;
			this.rosterId = rosterId;
			this.rosterJid = rosterJid;
			this.message = message;
			this.user = user;
			this.categotyId = categotyId;
			this.InitUI();
			this.AddEventListener();
		}
		protected override void OnMouseLeftButtonDown(MouseButtonEventArgs e)
		{
			base.OnMouseLeftButtonDown(e);
			base.DragMove();
		}
		public void InitUI()
		{
			this.tbRequesterName.Text = this.user.name + "添加您为好友";
			this.tbkId.Text = this.uid.ToString() + "[" + this.user.name + "]";
			this.tbkAge.Text = this.user.age.ToString();
		}
		private void AddEventListener()
		{
			this.btnAgree.Click += new RoutedEventHandler(this.btnAgree_Click);
		}
		public void btnAgree_Click(object sender, RoutedEventArgs e)
		{
			int askType = 1;
			if (this.radioButton1.IsChecked == true)
			{
				askType = 2;
			}
			if (this.radioButton2.IsChecked == true)
			{
				askType = 1;
			}
			if (this.radioButton3.IsChecked == true)
			{
				askType = 3;
			}
			User user = new User();
			user.jid = this.sessionService.Jid;
			user.uid = this.sessionService.Uid;
			user.name = this.sessionService.Name;
			user.nickname = this.sessionService.NickName;
			user.signature = this.sessionService.Signature;
			user.status = (int)this.sessionService.Status;
			user.username = this.sessionService.UserName;
			this.viewModel.AddRosterRequestAsk(this.sessionService.Uid, this.uid, this.user.jid, this.tbMessage.Text, askType, 0L, 0L, user);
			INWindow inWindow = this.dataService.INWindow as INWindow;
			if (inWindow != null)
			{
				Roster roster = new Roster();
				roster.Jid = this.user.jid;
				roster.Uid = this.user.uid;
				roster.Name = this.user.name;
				roster.Nickname = this.user.nickname;
				inWindow.FriendsList.AddRoster(roster);
				this.dataService.AddRoster(roster);
			}
			base.Close();
		}
		private void btnMin_Click(object sender, RoutedEventArgs e)
		{
			base.WindowState = WindowState.Minimized;
		}
		private void btnClose_Click(object sender, RoutedEventArgs e)
		{
			base.Close();
		}
        //[System.Diagnostics.DebuggerNonUserCode]
        //public void InitializeComponent()
        //{
        //    if (!this._contentLoaded)
        //    {
        //        this._contentLoaded = true;
        //        Uri resourceLocater = new Uri("/IDKin.IM.Windows;component/view/addfriends/friendrequestwindow.xaml", UriKind.Relative);
        //        Application.LoadComponent(this, resourceLocater);
        //    }
        //}
        //[System.Diagnostics.DebuggerNonUserCode]
        //internal System.Delegate _CreateDelegate(System.Type delegateType, string handler)
        //{
        //    return System.Delegate.CreateDelegate(delegateType, this, handler);
        //}
        //[EditorBrowsable(EditorBrowsableState.Never), System.Diagnostics.DebuggerNonUserCode]
        //void IComponentConnector.Connect(int connectionId, object target)
        //{
        //    switch (connectionId)
        //    {
        //    case 1:
        //        this.OuterBorder = (Border)target;
        //        break;
        //    case 2:
        //        this.InnerBorder = (Border)target;
        //        break;
        //    case 3:
        //        this.imgIcon = (Image)target;
        //        break;
        //    case 4:
        //        this.btnMin = (MinButton)target;
        //        break;
        //    case 5:
        //        this.btnClose = (CloseButton)target;
        //        break;
        //    case 6:
        //        this.tbRequesterName = (TextBlock)target;
        //        break;
        //    case 7:
        //        this.tbkId = (TextBlock)target;
        //        break;
        //    case 8:
        //        this.tbkSex = (TextBlock)target;
        //        break;
        //    case 9:
        //        this.tbkAge = (TextBlock)target;
        //        break;
        //    case 10:
        //        this.tbkAddress = (TextBlock)target;
        //        break;
        //    case 11:
        //        this.tbkPost = (TextBlock)target;
        //        break;
        //    case 12:
        //        this.btnViewInfo = (Button)target;
        //        break;
        //    case 13:
        //        this.radioButton1 = (RadioButton)target;
        //        break;
        //    case 14:
        //        this.radioButton2 = (RadioButton)target;
        //        break;
        //    case 15:
        //        this.radioButton3 = (RadioButton)target;
        //        break;
        //    case 16:
        //        this.tbMessage = (TextBox)target;
        //        break;
        //    case 17:
        //        this.btnAgree = (Button)target;
        //        break;
        //    case 18:
        //        this.btnForget = (Button)target;
        //        break;
        //    default:
        //        this._contentLoaded = true;
        //        break;
        //    }
        //}
	}
}
