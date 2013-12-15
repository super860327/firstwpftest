using IDKin.IM.Data;
using IDKin.IM.Protocol.Account;
using IDKin.IM.Windows.Util;
using IDKin.IM.Windows.View.AddFriends;
using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;
using System.Windows.Media.Imaging;
namespace IDKin.IM.Windows.Comps
{
	[GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
    public partial class SearchResultItem : ListViewItem//, IComponentConnector
	{
		private IDataService dataService = ServiceUtil.Instance.DataService;
		private ISessionService sessionService = ServiceUtil.Instance.SessionService;
		private string username;
		private long userId;
		private int sex = 0;
		private int age;
		private string address = string.Empty;
		private string post = string.Empty;
		private BitmapImage head = null;
		private string jid;
		//internal Canvas canvas;
		//internal Image imgHead;
		//internal TextBlock tbkId;
		//internal TextBlock tbkSex;
		//internal TextBlock tbkAge;
		//internal TextBlock tbkAddress;
		//internal TextBlock tbkPost;
		//internal Button btnAddFriend;
		//internal Button btnViewInfo;
        ////private bool _contentLoaded;
		public SearchResultItem()
		{
			this.InitializeComponent();
			this.InitUI();
			this.AddEventListener();
		}
		private void InitUI()
		{
		}
		public void InitItemData(User user)
		{
			if (user != null)
			{
				this.username = user.username;
				this.userId = user.uid;
				this.jid = user.jid;
				this.age = user.age;
				this.imgHead.Source = this.head;
				this.tbkId.Text = string.Concat(new object[]
				{
					this.username,
					"[ ",
					this.userId,
					" ]"
				});
				switch (this.sex)
				{
				case 0:
					this.tbkSex.Text = "女";
					break;
				case 1:
					this.tbkSex.Text = "男";
					break;
				}
				this.tbkAge.Text = this.age.ToString();
				this.tbkAddress.Text = this.address;
				this.tbkPost.Text = this.post;
			}
		}
		private void AddEventListener()
		{
			this.btnAddFriend.Click += new RoutedEventHandler(this.btnAddFriend_Click);
		}
		private void btnAddFriend_Click(object sender, RoutedEventArgs e)
		{
			AddFriendWindow addFriendWindow = new AddFriendWindow();
			addFriendWindow.InitData(this.username, this.userId, this.sex, this.age, this.address, this.post, this.jid, this.head);
			addFriendWindow.Show();
		}
        //[System.Diagnostics.DebuggerNonUserCode]
        //public void InitializeComponent()
        //{
        //    if (!this._contentLoaded)
        //    {
        //        this._contentLoaded = true;
        //        Uri resourceLocater = new Uri("/IDKin.IM.Windows;component/comps/searchrosterresultitem.xaml", UriKind.Relative);
        //        Application.LoadComponent(this, resourceLocater);
        //    }
        //}
        //[EditorBrowsable(EditorBrowsableState.Never), System.Diagnostics.DebuggerNonUserCode]
        //void IComponentConnector.Connect(int connectionId, object target)
        //{
        //    switch (connectionId)
        //    {
        //    case 1:
        //        this.canvas = (Canvas)target;
        //        break;
        //    case 2:
        //        this.imgHead = (Image)target;
        //        break;
        //    case 3:
        //        this.tbkId = (TextBlock)target;
        //        break;
        //    case 4:
        //        this.tbkSex = (TextBlock)target;
        //        break;
        //    case 5:
        //        this.tbkAge = (TextBlock)target;
        //        break;
        //    case 6:
        //        this.tbkAddress = (TextBlock)target;
        //        break;
        //    case 7:
        //        this.tbkPost = (TextBlock)target;
        //        break;
        //    case 8:
        //        this.btnAddFriend = (Button)target;
        //        break;
        //    case 9:
        //        this.btnViewInfo = (Button)target;
        //        break;
        //    default:
        //        this._contentLoaded = true;
        //        break;
        //    }
        //}
	}
}
