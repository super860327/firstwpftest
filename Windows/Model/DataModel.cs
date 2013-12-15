using IDKin.IM.Core;
using IDKin.IM.Data;
using IDKin.IM.Log;
using IDKin.IM.Windows.Model.RecentLink;
using IDKin.IM.Windows.Util;
using IDKin.IM.Windows.View;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Forms;
namespace IDKin.IM.Windows.Model
{
	public class DataModel
	{
		private static DataModel instance = null;
		private ILogger logger = ServiceUtil.Instance.Logger;
		private System.Collections.Generic.IDictionary<DataService.WindowType, Window> window = null;
		private System.Collections.Generic.IDictionary<string, System.Collections.Generic.List<IDKin.IM.Core.Message>> messageCache = new System.Collections.Generic.Dictionary<string, System.Collections.Generic.List<IDKin.IM.Core.Message>>();
		private System.Collections.Generic.List<string> messageSort = new System.Collections.Generic.List<string>();
		private System.Collections.Generic.List<string> messageBoxSort = new System.Collections.Generic.List<string>();
		private System.Collections.Generic.List<RecentLinkInfo> recentLinkInfoList = new System.Collections.Generic.List<RecentLinkInfo>();
		private System.Collections.Generic.IDictionary<string, CustomGroup> customeGroupName = new System.Collections.Generic.Dictionary<string, CustomGroup>();
		private System.Collections.Generic.List<EMailModal> emailList = new System.Collections.Generic.List<EMailModal>();
		private bool isEnable = false;
		private string direction = "left";
		private Keys pickUpMsgKey;
		private int pickUpMsgType = 3;
		private int pickUpMsgCAS = 1;
		private Keys cutScreenKey;
		private int cutScreenType = 3;
		private int cutScreenCAS = 1;
		private bool isSetHotKey = false;
		private bool isAllowPickup = true;
		public System.Collections.Generic.List<RecentLinkInfo> RecentLinkInfoList
		{
			get
			{
				return this.recentLinkInfoList;
			}
		}
		public System.Collections.Generic.List<string> MessageBoxSort
		{
			get
			{
				return this.messageBoxSort;
			}
		}
		public System.Collections.Generic.IDictionary<string, CustomGroup> CustomeGroupName
		{
			get
			{
				return this.customeGroupName;
			}
		}
		public System.Collections.Generic.List<EMailModal> EmailList
		{
			get
			{
				return this.emailList;
			}
		}
		public static DataModel Instance
		{
			get
			{
				if (DataModel.instance == null)
				{
					DataModel.instance = new DataModel();
				}
				return DataModel.instance;
			}
		}
		public System.Collections.Generic.IDictionary<string, System.Collections.Generic.List<IDKin.IM.Core.Message>> MessageCache
		{
			get
			{
				return this.messageCache;
			}
		}
		public bool IsEnable
		{
			get
			{
				return this.isEnable;
			}
			set
			{
				this.isEnable = value;
			}
		}
		public string Direction
		{
			get
			{
				return this.direction;
			}
			set
			{
				this.direction = value;
			}
		}
		public Keys PickUpMsgKey
		{
			get
			{
				return this.pickUpMsgKey;
			}
			set
			{
				this.pickUpMsgKey = value;
			}
		}
		public int PickUpMsgType
		{
			get
			{
				return this.pickUpMsgType;
			}
			set
			{
				this.pickUpMsgType = value;
			}
		}
		public int PickUpMsgCAS
		{
			get
			{
				return this.pickUpMsgCAS;
			}
			set
			{
				this.pickUpMsgCAS = value;
			}
		}
		public Keys CutScreenKey
		{
			get
			{
				return this.cutScreenKey;
			}
			set
			{
				this.cutScreenKey = value;
			}
		}
		public int CutScreenType
		{
			get
			{
				return this.cutScreenType;
			}
			set
			{
				this.cutScreenType = value;
			}
		}
		public int CutScreenCAS
		{
			get
			{
				return this.cutScreenCAS;
			}
			set
			{
				this.cutScreenCAS = value;
			}
		}
		public bool IsSetHotKey
		{
			get
			{
				return this.isSetHotKey;
			}
			set
			{
				this.isSetHotKey = value;
			}
		}
		public bool IsAllowPickup
		{
			get
			{
				return this.isAllowPickup;
			}
			set
			{
				this.isAllowPickup = value;
			}
		}
		private void AddMessageBoxSort(string key)
		{
			if (!string.IsNullOrEmpty(key))
			{
				if (this.messageBoxSort.IndexOf(key) == -1)
				{
					this.messageBoxSort.Insert(0, key);
				}
			}
		}
		private void RemoveMessageBoxSort(string key)
		{
			if (!string.IsNullOrEmpty(key))
			{
				if (this.messageBoxSort.IndexOf(key) != -1)
				{
					this.messageBoxSort.Remove(key);
				}
			}
		}
		private void AddMessageSort(string key)
		{
			if (!string.IsNullOrEmpty(key))
			{
				if (this.messageSort.IndexOf(key) != -1)
				{
					this.messageSort.Remove(key);
				}
				this.messageSort.Add(key);
			}
		}
		private void RemoveMessageSort(string key)
		{
			if (!string.IsNullOrEmpty(key))
			{
				if (this.messageSort.IndexOf(key) != -1)
				{
					this.messageSort.Remove(key);
				}
			}
		}
		private string GetLastMessageKey()
		{
			string result;
			if (this.messageSort.Count > 0)
			{
				result = this.messageSort[this.messageSort.Count - 1];
			}
			else
			{
				result = string.Empty;
			}
			return result;
		}
		public System.Collections.Generic.List<IDKin.IM.Core.Message> GetLastMessage()
		{
			string key = this.GetLastMessageKey();
			System.Collections.Generic.List<IDKin.IM.Core.Message> result;
			if (!string.IsNullOrEmpty(key))
			{
				if (this.messageCache.ContainsKey(key))
				{
					result = this.messageCache[key];
					return result;
				}
			}
			result = null;
			return result;
		}
		public void AddMessage(long id, MessageActorType type, IDKin.IM.Core.Message message)
		{
			string key = type.ToString() + id;
			if (!this.messageCache.ContainsKey(key))
			{
				System.Collections.Generic.List<IDKin.IM.Core.Message> list = new System.Collections.Generic.List<IDKin.IM.Core.Message>();
				this.messageCache.Add(key, list);
			}
			System.Collections.Generic.List<IDKin.IM.Core.Message> messageList = this.messageCache[key];
			messageList.Add(message);
			this.AddMessageSort(key);
			this.AddMessageBoxSort(key);
		}
		public void AddCooperationStatffMessage(long id, string projectid, MessageActorType type, IDKin.IM.Core.Message message)
		{
			string key = type.ToString() + CooperationStaff.GetUidAndProjectid(id, projectid);
			if (!this.messageCache.ContainsKey(key))
			{
				System.Collections.Generic.List<IDKin.IM.Core.Message> list = new System.Collections.Generic.List<IDKin.IM.Core.Message>();
				this.messageCache.Add(key, list);
			}
			System.Collections.Generic.List<IDKin.IM.Core.Message> messageList = this.messageCache[key];
			messageList.Add(message);
			this.AddMessageSort(key);
			this.AddMessageBoxSort(key);
		}
		public void RemoveMessage(long id, MessageActorType type)
		{
			if (this.messageCache.ContainsKey(type.ToString() + id))
			{
				this.messageCache.Remove(type.ToString() + id);
			}
			this.RemoveMessageSort(type.ToString() + id);
			this.RemoveMessageBoxSort(type.ToString() + id);
		}
		public void RemoveCooperationStatffMessage(long id, string projectid, MessageActorType type)
		{
			string key = type.ToString() + CooperationStaff.GetUidAndProjectid(id, projectid);
			if (this.messageCache.ContainsKey(key))
			{
				this.messageCache.Remove(key);
			}
			this.RemoveMessageSort(key);
			this.RemoveMessageBoxSort(key);
		}
		public bool HasMessage()
		{
			return this.messageCache.Count > 0;
		}
		public System.Collections.Generic.List<IDKin.IM.Core.Message> GetMessage(string key)
		{
			System.Collections.Generic.List<IDKin.IM.Core.Message> result;
			if (!string.IsNullOrEmpty(key) && this.messageCache.ContainsKey(key))
			{
				result = this.messageCache[key];
			}
			else
			{
				result = null;
			}
			return result;
		}
		public System.Collections.Generic.List<IDKin.IM.Core.Message> GetCooperationStaffMessage(long id, string projectid, MessageActorType type)
		{
			string key = type.ToString() + CooperationStaff.GetUidAndProjectid(id, projectid);
			System.Collections.Generic.List<IDKin.IM.Core.Message> result;
			if (this.messageCache.ContainsKey(key))
			{
				result = this.messageCache[key];
			}
			else
			{
				result = null;
			}
			return result;
		}
		public System.Collections.Generic.List<IDKin.IM.Core.Message> GetMessage(long id, MessageActorType type)
		{
			System.Collections.Generic.List<IDKin.IM.Core.Message> result;
			if (this.messageCache.ContainsKey(type.ToString() + id))
			{
				result = this.messageCache[type.ToString() + id];
			}
			else
			{
				result = null;
			}
			return result;
		}
		public void RemoveMessageBox()
		{
			if (this.window != null && this.window.ContainsKey(DataService.WindowType.MessageBox))
			{
				this.window.Remove(DataService.WindowType.MessageBox);
			}
		}
		public MessageBoxWindow GetMessageBox()
		{
			MessageBoxWindow result;
			if (this.window != null && this.window.ContainsKey(DataService.WindowType.MessageBox))
			{
				result = (MessageBoxWindow)this.window[DataService.WindowType.MessageBox];
			}
			else
			{
				result = null;
			}
			return result;
		}
		public void SetMessageBox(MessageBoxWindow window)
		{
			if (this.window == null)
			{
				this.window = new System.Collections.Generic.Dictionary<DataService.WindowType, Window>();
			}
			if (!this.window.ContainsKey(DataService.WindowType.MessageBox))
			{
				this.window.Add(DataService.WindowType.MessageBox, window);
			}
		}
		public System.Collections.Generic.List<IDKin.IM.Core.Message> GetMessagesByOrder()
		{
			System.Collections.Generic.List<IDKin.IM.Core.Message> result;
			if (this.messageCache.Count == 0)
			{
				result = null;
			}
			else
			{
				using (System.Collections.Generic.IEnumerator<System.Collections.Generic.List<IDKin.IM.Core.Message>> enumerator = this.messageCache.Values.GetEnumerator())
				{
					if (enumerator.MoveNext())
					{
						System.Collections.Generic.List<IDKin.IM.Core.Message> list = enumerator.Current;
						result = list;
						return result;
					}
				}
				result = null;
			}
			return result;
		}
		public void ClearAll()
		{
			try
			{
				if (this.window != null && this.messageCache != null)
				{
					this.isEnable = false;
				}
				if (this.recentLinkInfoList != null)
				{
					this.recentLinkInfoList.Clear();
				}
				if (this.CustomeGroupName != null && this.CustomeGroupName.Count > 0)
				{
					this.CustomeGroupName.Clear();
				}
			}
			catch (System.Exception ex)
			{
				this.logger.Error(ex.ToString());
			}
		}
	}
}
