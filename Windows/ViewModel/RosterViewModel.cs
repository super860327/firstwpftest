using IDKin.IM.Communicate;
using IDKin.IM.Data;
using IDKin.IM.Protocol.Account;
using IDKin.IM.Protocol.Center;
using IDKin.IM.Protocol.Commons;
using IDKin.IM.Windows.Comps;
using IDKin.IM.Windows.Model;
using IDKin.IM.Windows.Util;
using System;
using System.Collections.Generic;
using System.Windows.Media;
namespace IDKin.IM.Windows.ViewModel
{
	public class RosterViewModel
	{
		private ISessionService sessionService = ServiceUtil.Instance.SessionService;
		private IDataService dataService = ServiceUtil.Instance.DataService;
		private IConnection connection = ServiceUtil.Instance.Connection;
		private WindowModel windowModel = WindowModel.Instance;
		public RosterViewModel()
		{
			this.connection.EventHandler.SearchRosterEvent = new SearchRosterHandler(this.SearchRosterEvent);
		}
		public void AddRosterRequestAsk(long uid, long ruid, string rjid, string message, int type, long categoryId, long rosterCategoryId, IDKin.IM.Protocol.Center.User user)
		{
			RosterAddResponse response = new RosterAddResponse();
			response.category_id = categoryId;
			response.message = message;
			response.rjid = rjid;
			response.roster_category_id = rosterCategoryId;
			response.ruid = ruid;
			response.type = type;
			response.uid = uid;
			response.user = user;
			this.connection.Send(PacketType.ROSTER_ADD_ASK, response);
		}
		public void AddRosterRequest(long uid, long rosterId, string rosterJid, string message, IDKin.IM.Protocol.Center.User user, long categotyId = 0L)
		{
			RosterAddRequest request = new RosterAddRequest();
			request.category_id = categotyId;
			request.message = message;
			request.rjid = rosterJid;
			request.ruid = rosterId;
			request.uid = uid;
			request.user = user;
			this.connection.Send(PacketType.ROSTER_ADD, request);
		}
		public void SearchRosterByUid(long rosterId, string fromJid)
		{
			SearchRequest request = new SearchRequest();
			request.uid = rosterId;
			request.from_jid = fromJid;
			request.type = 0;
			this.connection.Send(PacketType.ACCOUNT_SEARCH, request);
		}
		private void SearchRosterEvent(SearchResponse response)
		{
			if (response != null && response.users != null)
			{
				this.AddUsersHandle(response.users);
			}
		}
		private void AddUsersHandle(System.Collections.Generic.List<IDKin.IM.Protocol.Account.User> users)
		{
			if (users != null && users.Count > 0)
			{
				bool isOdd = true;
				foreach (IDKin.IM.Protocol.Account.User user in users)
				{
					SearchResultItem item = new SearchResultItem();
					item.InitItemData(user);
					this.windowModel.ResultWindow.resultList.Items.Add(item);
					isOdd = this.SetResultItemBG(item, isOdd);
				}
			}
		}
		private bool SetResultItemBG(SearchResultItem item, bool isOdd)
		{
			if (isOdd)
			{
				item.canvas.Background = new SolidColorBrush(Color.FromRgb(255, 255, 255));
			}
			else
			{
				item.canvas.Background = new SolidColorBrush(Color.FromRgb(238, 253, 255));
			}
			return !isOdd;
		}
	}
}
