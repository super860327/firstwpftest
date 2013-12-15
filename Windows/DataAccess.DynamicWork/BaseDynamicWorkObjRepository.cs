using IDKin.IM.Protocol.DynamicWork;
using System;
using System.Collections.ObjectModel;
namespace IDKin.IM.Windows.DataAccess.DynamicWork
{
	public class BaseDynamicWorkObjRepository
	{
		private readonly ObservableCollection<BaseDynamicWorkObj> baseDynamicWorkObjs;
		public event System.EventHandler<BaseDynamicWorkObjAddedEventArgs> BaseDynamicWorkObjAdded;
		public BaseDynamicWorkObjRepository()
		{
			this.baseDynamicWorkObjs = new ObservableCollection<BaseDynamicWorkObj>();
		}
		public void AddCustomer(BaseDynamicWorkObj baseDynamicWorkObj)
		{
			if (baseDynamicWorkObj == null)
			{
				throw new System.ArgumentNullException("dynamicWorkModel");
			}
			if (!this.baseDynamicWorkObjs.Contains(baseDynamicWorkObj))
			{
				this.baseDynamicWorkObjs.Add(baseDynamicWorkObj);
				if (this.BaseDynamicWorkObjAdded != null)
				{
					this.BaseDynamicWorkObjAdded(this, new BaseDynamicWorkObjAddedEventArgs(baseDynamicWorkObj));
				}
			}
		}
		public bool ContainsCustomer(BaseDynamicWorkObj baseDynamicWorkObj)
		{
			if (baseDynamicWorkObj == null)
			{
				throw new System.ArgumentNullException("baseDynamicWorkObj");
			}
			return this.baseDynamicWorkObjs.Contains(baseDynamicWorkObj);
		}
		public ObservableCollection<BaseDynamicWorkObj> GetDynamicWorkModels()
		{
			return new ObservableCollection<BaseDynamicWorkObj>(this.baseDynamicWorkObjs);
		}
	}
}
