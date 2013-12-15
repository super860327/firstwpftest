using IDKin.IM.Core;
using IDKin.IM.Data;
using IDKin.IM.Log;
using IDKin.IM.Windows.Comps;
using System;
using System.Collections.Generic;
using System.Text;
namespace IDKin.IM.Windows.Util
{
	public class SearchUtil
	{
		private IDataService dataService = ServiceUtil.Instance.DataService;
		private static SearchUtil instance = null;
		private System.Text.StringBuilder staffIndex = new System.Text.StringBuilder();
		private ILogger logger = ServiceUtil.Instance.Logger;
		public static SearchUtil Instance
		{
			get
			{
				if (SearchUtil.instance == null)
				{
					SearchUtil.instance = new SearchUtil();
				}
				return SearchUtil.instance;
			}
		}
		public void AddIndex(string userName, string name, uint uid)
		{
			this.staffIndex.Append(userName);
			this.staffIndex.Append("+");
			this.staffIndex.Append(name);
			this.staffIndex.Append("+");
			this.staffIndex.Append(uid);
			this.staffIndex.Append(",");
		}
		public System.Collections.Generic.List<Staff> SearchStaff(string value)
		{
			System.Collections.Generic.List<Staff> result;
			try
			{
				if (!string.IsNullOrEmpty(value))
				{
					result = this.IndexOfStaff(value);
					return result;
				}
			}
			catch (System.Exception e)
			{
				this.logger.Error(e.ToString());
			}
			result = null;
			return result;
		}
		private System.Collections.Generic.List<Staff> IndexOfStaff(string value)
		{
			System.Collections.Generic.List<Staff> result;
			try
			{
				System.Collections.Generic.List<Staff> staffList = new System.Collections.Generic.List<Staff>();
				System.Collections.Generic.ICollection<Staff> staffs = this.dataService.GetStaffList();
				if (staffs != null)
				{
					foreach (Staff staff in staffs)
					{
						if (staff.UserName.IndexOf(value) != -1 || staff.Name.IndexOf(value) != -1 || staff.Uid.ToString().IndexOf(value) != -1)
						{
							staffList.Add(staff);
						}
					}
				}
				result = staffList;
				return result;
			}
			catch (System.Exception e)
			{
				this.logger.Error(e.ToString());
			}
			result = null;
			return result;
		}
		public System.Collections.Generic.List<SearchNodeStaff> SearchStaffNode(string value)
		{
			System.Collections.Generic.List<SearchNodeStaff> result;
			try
			{
				if (!string.IsNullOrEmpty(value))
				{
					System.Collections.Generic.List<Staff> staffList = this.IndexOfStaff(value);
					System.Collections.Generic.List<SearchNodeStaff> nodes = null;
					if (staffList != null && staffList.Count > 0)
					{
						nodes = new System.Collections.Generic.List<SearchNodeStaff>();
						foreach (Staff staff in staffList)
						{
							SearchNodeStaff node = new SearchNodeStaff(staff);
							if (node != null)
							{
								nodes.Add(node);
							}
						}
					}
					result = nodes;
					return result;
				}
			}
			catch (System.Exception e)
			{
				this.logger.Error(e.ToString());
			}
			result = null;
			return result;
		}
	}
}
