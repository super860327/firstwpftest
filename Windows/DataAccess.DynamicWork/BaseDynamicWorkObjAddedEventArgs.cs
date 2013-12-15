using IDKin.IM.Protocol.DynamicWork;
using System;
namespace IDKin.IM.Windows.DataAccess.DynamicWork
{
	public class BaseDynamicWorkObjAddedEventArgs : System.EventArgs
	{
		public BaseDynamicWorkObj NewBaseDynamicWorkObj
		{
			get;
			private set;
		}
		public BaseDynamicWorkObjAddedEventArgs(BaseDynamicWorkObj baseDynamicWorkObj)
		{
			this.NewBaseDynamicWorkObj = baseDynamicWorkObj;
		}
	}
}
