using IDKin.IM.Protocol.DynamicWork;
using System;
using System.Collections.Generic;
namespace IDKin.IM.Windows.ViewModel
{
	public class BaseDynamicWorkObjViewModel : ViewModelBase
	{
		private readonly BaseDynamicWorkObj baseDynamicWorkObj;
		private System.Collections.Generic.List<string> unReplaceStrs = null;
		private string[] splitStrs = new string[]
		{
			"%s"
		};
		private string actionYearMonthDay = string.Empty;
		private string actionHourMinute = string.Empty;
		public BaseDynamicWorkObj BaseDynamicWorkObj
		{
			get
			{
				return this.baseDynamicWorkObj;
			}
		}
		public System.Collections.Generic.List<string> UnReplaceStrs
		{
			get
			{
				return this.unReplaceStrs;
			}
		}
		public string ActionTime
		{
			get
			{
				return this.baseDynamicWorkObj.actionTime;
			}
			set
			{
				this.baseDynamicWorkObj.actionTime = value;
			}
		}
		public string FromName
		{
			get
			{
				return this.baseDynamicWorkObj.fromName;
			}
			set
			{
				this.baseDynamicWorkObj.fromName = value;
			}
		}
		public string ToName
		{
			get
			{
				return this.baseDynamicWorkObj.toName;
			}
			set
			{
				this.baseDynamicWorkObj.toName = value;
			}
		}
		public System.Collections.Generic.List<string> RepalceStrs
		{
			get
			{
				return this.baseDynamicWorkObj.repalceStr;
			}
		}
		public string Title
		{
			get
			{
				return this.baseDynamicWorkObj.title;
			}
			set
			{
				this.baseDynamicWorkObj.title = value;
			}
		}
		public string Url
		{
			get
			{
				return this.baseDynamicWorkObj.url;
			}
			set
			{
				this.baseDynamicWorkObj.url = value;
			}
		}
		public string ActionYearMonthDay
		{
			get
			{
				return this.actionYearMonthDay;
			}
		}
		public string ActionHourMinute
		{
			get
			{
				return this.actionHourMinute;
			}
		}
		public BaseDynamicWorkObjViewModel(BaseDynamicWorkObj baseDynamicWorkObj)
		{
			if (baseDynamicWorkObj == null)
			{
				throw new System.ArgumentNullException("dynamicWorkModel");
			}
			this.baseDynamicWorkObj = baseDynamicWorkObj;
			System.DateTime dt;
			if (System.DateTime.TryParse(baseDynamicWorkObj.actionTime, out dt))
			{
				this.actionYearMonthDay = dt.ToString("yyyy MM dd");
				this.actionHourMinute = dt.ToString("HH: ss");
				this.unReplaceStrs = new System.Collections.Generic.List<string>(baseDynamicWorkObj.content.Split(this.splitStrs, System.StringSplitOptions.RemoveEmptyEntries));
				return;
			}
			throw new System.InvalidCastException(baseDynamicWorkObj.actionTime);
		}
	}
}
