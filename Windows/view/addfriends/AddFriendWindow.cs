using IDKin.IM.Data;
using IDKin.IM.Protocol.Center;
using IDKin.IM.Theme.Helper;
using IDKin.IM.Windows.BindingVO;
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
using System.Windows.Media.Imaging;
namespace IDKin.IM.Windows.View.AddFriends
{
	[GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
    public partial class AddFriendWindow : Window//, IComponentConnector
	{
		private ISessionService sessionService = ServiceUtil.Instance.SessionService;
		private IDataService dataService = ServiceUtil.Instance.DataService;
		private RosterViewModel viewModel = new RosterViewModel();
		private string username;
		private long userId;
		private int sex;
		private int age;
		private string address;
		private string post;
		private string jid;
		private BitmapImage head;
		public AddFriendWindow()
		{
			ThemeSwitcher.LoadSkin(ThemeEnum.Aero, this);
			this.InitializeComponent();
			this.InitUI();
			this.AddEventListener();
		}
		public void InitData(string username, long uid, int sex, int age, string address, string post, string jid, BitmapImage head = null)
		{
			try
			{
				this.userId = uid;
				this.username = username;
				this.sex = sex;
				this.age = age;
				this.address = address;
				this.post = post;
				this.head = head;
				this.jid = jid;
				this.tbkId.Text = username;
				switch (sex)
				{
				case 0:
					this.tbkSex.Text = "女";
					break;
				case 1:
					this.tbkSex.Text = "男";
					break;
				}
				this.tbkAge.Text = age.ToString();
				this.tbkAddress.Text = address;
				this.tbkPost.Text = post;
			}
			catch (System.Exception)
			{
			}
		}
		protected override void OnMouseLeftButtonDown(MouseButtonEventArgs e)
		{
			base.OnMouseLeftButtonDown(e);
			base.DragMove();
		}
		private void InitUI()
		{
		}
		private void AddEventListener()
		{
			this.btnSure.Click += new RoutedEventHandler(this.btnSure_Click);
			this.btnCancel.Click += new RoutedEventHandler(this.btnClose_Click);
		}
		private void btnSure_Click(object sender, RoutedEventArgs e)
		{
			try
			{
				User user = new User();
				user.jid = this.sessionService.Jid;
				user.uid = this.sessionService.Uid;
				user.name = this.sessionService.Name;
				user.nickname = this.sessionService.NickName;
				user.signature = this.sessionService.Signature;
				user.status = (int)this.sessionService.Status;
				user.username = this.sessionService.UserName;
				this.viewModel.AddRosterRequest(this.sessionService.Uid, this.userId, this.jid, this.tbMessage.Text, user, 0L);
				RequestResultWindow result = new RequestResultWindow();
				result.resultVO = new RequestResultVO();
				result.resultVO.Reason = Visibility.Collapsed;
				result.resultVO.IsSure = Visibility.Collapsed;
				result.DataContext = result.resultVO;
				result.ShowDialog();
			}
			catch (System.Exception ex)
			{
				System.Console.WriteLine(ex.ToString());
			}
		}
		private void btnMin_Click(object sender, RoutedEventArgs e)
		{
			base.WindowState = WindowState.Minimized;
		}
		private void btnClose_Click(object sender, RoutedEventArgs e)
		{
			base.Close();
		}
	}
}
