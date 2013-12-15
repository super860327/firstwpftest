using IDKin.IM.Communicate;
using IDKin.IM.Core;
using IDKin.IM.Data;
using IDKin.IM.Protocol.Commons;
using IDKin.IM.Protocol.DynamicWork;
using IDKin.IM.Windows.Command;
using IDKin.IM.Windows.Comps.OA.CurrentWork;
using IDKin.IM.Windows.Util;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Data;
namespace IDKin.IM.Windows.ViewModel
{
	public class AllBaseDynamicWorkObjViewModel : ViewModelBase
	{
		private IConnection connection = null;
		private ISessionService sessionService = null;
		private IDataService dataService = null;
		private int currentPage = 1;
		private int itemCountOfPage = 17;
		private bool getMoreDataCalled = false;
		private OAAllCurrentWork oAAllCurrentWork = null;
		private ObservableCollection<BaseDynamicWorkObjViewModel> baseDynamicWorkObjViewModels = null;
		private RelayCommand saveCommand;
		private Staff staff = null;
		private DynamicWorkModuleType moduleType = DynamicWorkModuleType.DOCUMENT_MANAGEMENT;
		public bool GetMoreDataCalled
		{
			get
			{
				return this.getMoreDataCalled;
			}
			set
			{
				this.getMoreDataCalled = value;
			}
		}
		public ObservableCollection<BaseDynamicWorkObjViewModel> BaseDynamicWorkObjViewModels
		{
			get
			{
				return this.baseDynamicWorkObjViewModels;
			}
		}
		public DynamicWorkModuleType ModuleType
		{
			get
			{
				return this.moduleType;
			}
		}
		public int CurrentPage
		{
			get
			{
				return this.currentPage;
			}
			set
			{
				this.currentPage = value;
			}
		}
		public RelayCommand GetMoreDataCommand
		{
			get
			{
				if (this.saveCommand == null)
				{
					this.saveCommand = new RelayCommand(delegate(object param)
					{
						this.GetMoreData();
					}, null);
				}
				return this.saveCommand;
			}
		}
		public AllBaseDynamicWorkObjViewModel(DynamicWorkModuleType moduleType, Staff staff, OAAllCurrentWork oAAllCurrentWork)
		{
			this.moduleType = moduleType;
			this.baseDynamicWorkObjViewModels = new ObservableCollection<BaseDynamicWorkObjViewModel>();
			this.currentPage = 1;
			this.InitService();
			if (staff == null)
			{
				throw new System.ArgumentNullException("staff");
			}
			if (oAAllCurrentWork == null)
			{
				throw new System.ArgumentNullException("oAAllCurrentWork");
			}
			this.oAAllCurrentWork = oAAllCurrentWork;
			this.staff = staff;
			this.dataService = ServiceUtil.Instance.DataService;
			this.connection.EventHandler.DynamicWorkDataGettedEvent += new DynamicWorkDataGettedHandler(this.OnDynamicWorkDataGetted);
			this.Group();
		}
		private void Group()
		{
			ICollectionView view = this.GetBaseDynamicWorkObjViewModelsView();
			if (view != null)
			{
				view.GroupDescriptions.Add(new PropertyGroupDescription("ActionYearMonthDay"));
			}
		}
		private ICollectionView GetBaseDynamicWorkObjViewModelsView()
		{
			return CollectionViewSource.GetDefaultView(this.baseDynamicWorkObjViewModels);
		}
		public void OnDynamicWorkDataGetted(DynamicWorkResponse res)
		{
			if (res.uid == this.staff.Uid)
			{
				if (res.moduleType == (int)this.moduleType)
				{
				}
			}
		}
		private DynamicWorkResponse GetValidTestData(DynamicWorkModuleType moduleType)
		{
			DynamicWorkResponse response = new DynamicWorkResponse();
			response.moduleType = (int)moduleType;
			DynamicWorkObjByTime bt = new DynamicWorkObjByTime();
			bt.actionYearMonthDay = System.DateTime.Now.ToString("yyyy MM dd");
			switch (moduleType)
			{
			case DynamicWorkModuleType.WORK_COOPERATION:
			{
				BaseDynamicWorkObj obj = new BaseDynamicWorkObj();
				obj.operationType = 1;
				obj.actionTime = System.DateTime.Now.ToString();
				obj.content = "%s给%s派发了一条工作任务 %s";
				obj.repalceStr.Add("发布人");
				obj.repalceStr.Add("执行人");
				obj.repalceStr.Add("任务名称");
				bt.baseDynamicWorkObj.Add(obj);
				obj = new BaseDynamicWorkObj();
				obj.operationType = 2;
				obj.actionTime = System.DateTime.Now.ToString();
				obj.content = "%s在工作任务%s中给%s递交了一份工作简报";
				obj.repalceStr.Add("执行人");
				obj.repalceStr.Add("任务名称");
				obj.repalceStr.Add("发布人");
				bt.baseDynamicWorkObj.Add(obj);
				obj = new BaseDynamicWorkObj();
				obj.operationType = 3;
				obj.actionTime = System.DateTime.Now.ToString();
				obj.content = "%s通过%s在工作任务%s中递交的工作简报";
				obj.repalceStr.Add("发布人");
				obj.repalceStr.Add("执行人");
				obj.repalceStr.Add("任务名称");
				bt.baseDynamicWorkObj.Add(obj);
				obj = new BaseDynamicWorkObj();
				obj.operationType = 4;
				obj.actionTime = System.DateTime.Now.ToString();
				obj.content = "%s未通过%s递交的工作简报%s";
				obj.repalceStr.Add("发布人");
				obj.repalceStr.Add("执行人");
				obj.repalceStr.Add("任务名称");
				bt.baseDynamicWorkObj.Add(obj);
				obj = new BaseDynamicWorkObj();
				obj.operationType = 5;
				obj.actionTime = System.DateTime.Now.ToString();
				obj.content = "%s提交了一个%s";
				obj.repalceStr.Add("申请人");
				obj.repalceStr.Add("请假申请");
				bt.baseDynamicWorkObj.Add(obj);
				obj = new BaseDynamicWorkObj();
				obj.operationType = 6;
				obj.actionTime = System.DateTime.Now.ToString();
				obj.content = "%s  通过   %s  的%s";
				obj.repalceStr.Add("审批人");
				obj.repalceStr.Add("申请人");
				obj.repalceStr.Add("请假申请");
				bt.baseDynamicWorkObj.Add(obj);
				obj = new BaseDynamicWorkObj();
				obj.operationType = 7;
				obj.actionTime = System.DateTime.Now.ToString();
				obj.content = "%s  未通过   %s  的%s";
				obj.repalceStr.Add("审批人");
				obj.repalceStr.Add("申请人");
				obj.repalceStr.Add("请假申请");
				bt.baseDynamicWorkObj.Add(obj);
				break;
			}
			case DynamicWorkModuleType.DOCUMENT_MANAGEMENT:
			{
				BaseDynamicWorkObj obj = new BaseDynamicWorkObj();
				obj.operationType = 1;
				obj.actionTime = System.DateTime.Now.ToString();
				obj.content = "%s  向   %s  授予了%s 文件 %s 的 %s 权限 %s";
				obj.repalceStr.Add("授权者");
				obj.repalceStr.Add("被授权者");
				obj.repalceStr.Add("文件数量");
				obj.repalceStr.Add("文件名");
				obj.repalceStr.Add("查看、下载");
				obj.repalceStr.Add("企业资料库授权");
				bt.baseDynamicWorkObj.Add(obj);
				obj = new BaseDynamicWorkObj();
				obj.operationType = 3;
				obj.actionTime = System.DateTime.Now.ToString();
				obj.content = "%s  向   %s  授予了目录%s   的 %s 权限 %s";
				obj.repalceStr.Add("授权者");
				obj.repalceStr.Add("被授权者");
				obj.repalceStr.Add("目录名");
				obj.repalceStr.Add("查看、下载");
				obj.repalceStr.Add("企业资料库授权");
				bt.baseDynamicWorkObj.Add(obj);
				obj = new BaseDynamicWorkObj();
				obj.operationType = 4;
				obj.actionTime = System.DateTime.Now.ToString();
				obj.content = "%s  在以下目录中上传了   %s  文件 %s%s";
				obj.repalceStr.Add("上传者");
				obj.repalceStr.Add("文件数量");
				obj.repalceStr.Add("文件名");
				obj.repalceStr.Add("目录名称");
				bt.baseDynamicWorkObj.Add(obj);
				obj = new BaseDynamicWorkObj();
				obj.operationType = 5;
				obj.actionTime = System.DateTime.Now.ToString();
				obj.content = "%s  编辑了目录    %s%s";
				obj.repalceStr.Add("操作者");
				obj.repalceStr.Add("目录名称");
				obj.repalceStr.Add("编辑后的目录名称");
				bt.baseDynamicWorkObj.Add(obj);
				obj = new BaseDynamicWorkObj();
				obj.operationType = 6;
				obj.actionTime = System.DateTime.Now.ToString();
				obj.content = "%s  在以下目录中编辑了    %s 文件 %s%s";
				obj.repalceStr.Add("操作者");
				obj.repalceStr.Add("文件数量");
				obj.repalceStr.Add("文件名");
				obj.repalceStr.Add("目录名称");
				bt.baseDynamicWorkObj.Add(obj);
				obj = new BaseDynamicWorkObj();
				obj.operationType = 7;
				obj.actionTime = System.DateTime.Now.ToString();
				obj.content = "%s  删除了目录    %s";
				obj.repalceStr.Add("操作者");
				obj.repalceStr.Add("目录名称");
				bt.baseDynamicWorkObj.Add(obj);
				obj = new BaseDynamicWorkObj();
				obj.operationType = 8;
				obj.actionTime = System.DateTime.Now.ToString();
				obj.content = "%s  在以下目录中删除了    %s 文件 %s%s";
				obj.repalceStr.Add("操作者");
				obj.repalceStr.Add("文件数量");
				obj.repalceStr.Add("文件名");
				obj.repalceStr.Add("目录名称");
				bt.baseDynamicWorkObj.Add(obj);
				obj = new BaseDynamicWorkObj();
				obj.operationType = 9;
				obj.actionTime = System.DateTime.Now.ToString();
				obj.content = "%s  从目录    %s 转移了 %s 文件至目录 %s ：  %s%s";
				obj.repalceStr.Add("操作者");
				obj.repalceStr.Add("目录名称");
				obj.repalceStr.Add("文件数量");
				obj.repalceStr.Add("目录名称");
				obj.repalceStr.Add("目录名、文件名");
				obj.repalceStr.Add("目标目录名称");
				bt.baseDynamicWorkObj.Add(obj);
				obj = new BaseDynamicWorkObj();
				obj.operationType = 10;
				obj.actionTime = System.DateTime.Now.ToString();
				obj.content = "%s  在以下目录中下载了    %s 文件 %s 、 %s%s";
				obj.repalceStr.Add("操作者");
				obj.repalceStr.Add("文件数量");
				obj.repalceStr.Add("目录名");
				obj.repalceStr.Add("文件名");
				obj.repalceStr.Add("目录名称");
				bt.baseDynamicWorkObj.Add(obj);
				obj = new BaseDynamicWorkObj();
				obj.operationType = 11;
				obj.actionTime = System.DateTime.Now.ToString();
				obj.content = "%s  在以下目录中添加了目录 %s%s";
				obj.repalceStr.Add("上传者");
				obj.repalceStr.Add("目录名");
				obj.repalceStr.Add("目录名称");
				bt.baseDynamicWorkObj.Add(obj);
				obj = new BaseDynamicWorkObj();
				obj.operationType = 12;
				obj.actionTime = System.DateTime.Now.ToString();
				obj.content = "%s  收藏了 %s文件 %s%s ";
				obj.repalceStr.Add("上传者");
				obj.repalceStr.Add("文件");
				obj.repalceStr.Add("目录名、文件名");
				obj.repalceStr.Add("收藏文件所在的目录名称");
				bt.baseDynamicWorkObj.Add(obj);
				break;
			}
			case DynamicWorkModuleType.INSIDE_DISCUSSION:
			{
				BaseDynamicWorkObj obj = new BaseDynamicWorkObj();
				obj.operationType = 1;
				obj.actionTime = System.DateTime.Now.ToString();
				obj.content = "%s 在 %s 版块中发表新主题 %s";
				obj.repalceStr.Add("发帖人");
				obj.repalceStr.Add("××");
				obj.repalceStr.Add("主题名称");
				bt.baseDynamicWorkObj.Add(obj);
				obj = new BaseDynamicWorkObj();
				obj.operationType = 2;
				obj.actionTime = System.DateTime.Now.ToString();
				obj.content = "%s 在 %s 版块中回复发帖人的主题 %s";
				obj.repalceStr.Add("回帖人");
				obj.repalceStr.Add("××");
				obj.repalceStr.Add("主题名称");
				bt.baseDynamicWorkObj.Add(obj);
				obj = new BaseDynamicWorkObj();
				obj.operationType = 3;
				obj.actionTime = System.DateTime.Now.ToString();
				obj.content = "%s  修改了主题 %s";
				obj.repalceStr.Add("发帖人");
				obj.repalceStr.Add("主题名称");
				bt.baseDynamicWorkObj.Add(obj);
				obj = new BaseDynamicWorkObj();
				obj.operationType = 4;
				obj.actionTime = System.DateTime.Now.ToString();
				obj.content = "%s  删除主题 %s";
				obj.repalceStr.Add("发帖人");
				obj.repalceStr.Add("主题名称");
				bt.baseDynamicWorkObj.Add(obj);
				obj = new BaseDynamicWorkObj();
				obj.operationType = 5;
				obj.actionTime = System.DateTime.Now.ToString();
				obj.content = "%s  移动主题 %s";
				obj.repalceStr.Add("发帖人");
				obj.repalceStr.Add("主题名称");
				bt.baseDynamicWorkObj.Add(obj);
				obj = new BaseDynamicWorkObj();
				obj.operationType = 6;
				obj.actionTime = System.DateTime.Now.ToString();
				obj.content = "%s  添加版块 %s";
				obj.repalceStr.Add("发帖人");
				obj.repalceStr.Add("版块名称");
				bt.baseDynamicWorkObj.Add(obj);
				obj = new BaseDynamicWorkObj();
				obj.operationType = 6;
				obj.actionTime = System.DateTime.Now.ToString();
				obj.content = "%s  修改了版块 %s";
				obj.repalceStr.Add("发帖人");
				obj.repalceStr.Add("版块名称");
				bt.baseDynamicWorkObj.Add(obj);
				obj = new BaseDynamicWorkObj();
				obj.operationType = 4;
				obj.actionTime = System.DateTime.Now.ToString();
				obj.content = "%s  删除版块 %s";
				obj.repalceStr.Add("发帖人");
				obj.repalceStr.Add("版块名称");
				bt.baseDynamicWorkObj.Add(obj);
				break;
			}
			case DynamicWorkModuleType.INSIDE_NOTICE:
			{
				BaseDynamicWorkObj obj = new BaseDynamicWorkObj();
				obj.operationType = 1;
				obj.actionTime = System.DateTime.Now.ToString();
				obj.content = "%s 发布了一条内部公告 %s";
				obj.repalceStr.Add("发布人");
				obj.repalceStr.Add("公告标题");
				bt.baseDynamicWorkObj.Add(obj);
				obj = new BaseDynamicWorkObj();
				obj.operationType = 2;
				obj.actionTime = System.DateTime.Now.ToString();
				obj.content = "%s 修改了内部公告 %s";
				obj.repalceStr.Add("发布人");
				obj.repalceStr.Add("公告标题");
				bt.baseDynamicWorkObj.Add(obj);
				obj = new BaseDynamicWorkObj();
				obj.operationType = 2;
				obj.actionTime = System.DateTime.Now.ToString();
				obj.content = "%s 删除了内部公告 %s";
				obj.repalceStr.Add("发布人");
				obj.repalceStr.Add("公告标题");
				bt.baseDynamicWorkObj.Add(obj);
				break;
			}
			case DynamicWorkModuleType.PROJECT_MANAGEMENT:
			{
				BaseDynamicWorkObj obj = new BaseDynamicWorkObj();
				obj.operationType = 1;
				obj.actionTime = System.DateTime.Now.ToString();
				obj.content = "%s 新建一条项目计划 %s";
				obj.repalceStr.Add("项目创建者");
				obj.repalceStr.Add("项目名称");
				bt.baseDynamicWorkObj.Add(obj);
				obj = new BaseDynamicWorkObj();
				obj.operationType = 2;
				obj.actionTime = System.DateTime.Now.ToString();
				obj.content = "%s 新建一条子项目计划 %s";
				obj.repalceStr.Add("项目创建者");
				obj.repalceStr.Add("项目名称");
				bt.baseDynamicWorkObj.Add(obj);
				obj = new BaseDynamicWorkObj();
				obj.operationType = 3;
				obj.actionTime = System.DateTime.Now.ToString();
				obj.content = "%s 确认项目 %s完成";
				obj.repalceStr.Add("项目管理者");
				obj.repalceStr.Add("项目名称");
				bt.baseDynamicWorkObj.Add(obj);
				obj = new BaseDynamicWorkObj();
				obj.operationType = 4;
				obj.actionTime = System.DateTime.Now.ToString();
				obj.content = "%s 给 %s派发了一条项目任务 %s";
				obj.repalceStr.Add("发布人");
				obj.repalceStr.Add("执行人");
				obj.repalceStr.Add("任务名称");
				bt.baseDynamicWorkObj.Add(obj);
				obj = new BaseDynamicWorkObj();
				obj.operationType = 5;
				obj.actionTime = System.DateTime.Now.ToString();
				obj.content = "%s 修改了项目任务 %s";
				obj.repalceStr.Add("修改人");
				obj.repalceStr.Add("任务名称");
				bt.baseDynamicWorkObj.Add(obj);
				obj = new BaseDynamicWorkObj();
				obj.operationType = 6;
				obj.actionTime = System.DateTime.Now.ToString();
				obj.content = "%s 在项目任务 %s中给 %s 递交了一份工作简报";
				obj.repalceStr.Add("执行人");
				obj.repalceStr.Add("任务名称");
				obj.repalceStr.Add("发布人");
				bt.baseDynamicWorkObj.Add(obj);
				obj = new BaseDynamicWorkObj();
				obj.operationType = 7;
				obj.actionTime = System.DateTime.Now.ToString();
				obj.content = "%s 通过执行人在项目任务 %s 中递交的工作简报";
				obj.repalceStr.Add("发布人");
				obj.repalceStr.Add("任务名称");
				bt.baseDynamicWorkObj.Add(obj);
				obj = new BaseDynamicWorkObj();
				obj.operationType = 8;
				obj.actionTime = System.DateTime.Now.ToString();
				obj.content = "%s 未通过执行人在项目任务 %s 中递交的工作简报";
				obj.repalceStr.Add("发布人");
				obj.repalceStr.Add("任务名称");
				bt.baseDynamicWorkObj.Add(obj);
				obj = new BaseDynamicWorkObj();
				obj.operationType = 9;
				obj.actionTime = System.DateTime.Now.ToString();
				obj.content = "%s 在项目任务 %s 中留言";
				obj.repalceStr.Add("留言人");
				obj.repalceStr.Add("任务名称");
				bt.baseDynamicWorkObj.Add(obj);
				obj = new BaseDynamicWorkObj();
				obj.operationType = 10;
				obj.actionTime = System.DateTime.Now.ToString();
				obj.content = "%s 在项目任务 %s 中修改了工作简报";
				obj.repalceStr.Add("执行人");
				obj.repalceStr.Add("任务名称");
				bt.baseDynamicWorkObj.Add(obj);
				obj = new BaseDynamicWorkObj();
				obj.operationType = 11;
				obj.actionTime = System.DateTime.Now.ToString();
				obj.content = "%s 暂停项目 %s";
				obj.repalceStr.Add("项目管理者");
				obj.repalceStr.Add("项目名称");
				bt.baseDynamicWorkObj.Add(obj);
				obj = new BaseDynamicWorkObj();
				obj.operationType = 12;
				obj.actionTime = System.DateTime.Now.ToString();
				obj.content = "%s 恢复项目 %s";
				obj.repalceStr.Add("项目管理者");
				obj.repalceStr.Add("项目名称");
				bt.baseDynamicWorkObj.Add(obj);
				obj = new BaseDynamicWorkObj();
				obj.operationType = 13;
				obj.actionTime = System.DateTime.Now.ToString();
				obj.content = "%s 中止项目 %s";
				obj.repalceStr.Add("项目管理者");
				obj.repalceStr.Add("项目名称");
				bt.baseDynamicWorkObj.Add(obj);
				obj = new BaseDynamicWorkObj();
				obj.operationType = 14;
				obj.actionTime = System.DateTime.Now.ToString();
				obj.content = "%s 启动项目 %s";
				obj.repalceStr.Add("项目管理者");
				obj.repalceStr.Add("项目名称");
				bt.baseDynamicWorkObj.Add(obj);
				obj = new BaseDynamicWorkObj();
				obj.operationType = 15;
				obj.actionTime = System.DateTime.Now.ToString();
				obj.content = "%s 删除项目 %s";
				obj.repalceStr.Add("项目管理者");
				obj.repalceStr.Add("项目名称");
				bt.baseDynamicWorkObj.Add(obj);
				obj = new BaseDynamicWorkObj();
				obj.operationType = 16;
				obj.actionTime = System.DateTime.Now.ToString();
				obj.content = "%s 暂停项目任务 %s";
				obj.repalceStr.Add("项目管理者");
				obj.repalceStr.Add("项目名称");
				bt.baseDynamicWorkObj.Add(obj);
				obj = new BaseDynamicWorkObj();
				obj.operationType = 17;
				obj.actionTime = System.DateTime.Now.ToString();
				obj.content = "%s 恢复项目任务 %s";
				obj.repalceStr.Add("项目管理者");
				obj.repalceStr.Add("项目名称");
				bt.baseDynamicWorkObj.Add(obj);
				obj = new BaseDynamicWorkObj();
				obj.operationType = 18;
				obj.actionTime = System.DateTime.Now.ToString();
				obj.content = "%s 中止项目任务 %s";
				obj.repalceStr.Add("项目管理者");
				obj.repalceStr.Add("项目名称");
				bt.baseDynamicWorkObj.Add(obj);
				obj = new BaseDynamicWorkObj();
				obj.operationType = 19;
				obj.actionTime = System.DateTime.Now.ToString();
				obj.content = "%s 启动项目任务 %s";
				obj.repalceStr.Add("项目管理者");
				obj.repalceStr.Add("项目名称");
				bt.baseDynamicWorkObj.Add(obj);
				obj = new BaseDynamicWorkObj();
				obj.operationType = 20;
				obj.actionTime = System.DateTime.Now.ToString();
				obj.content = "%s 删除项目任务 %s";
				obj.repalceStr.Add("项目管理者");
				obj.repalceStr.Add("项目名称");
				bt.baseDynamicWorkObj.Add(obj);
				obj = new BaseDynamicWorkObj();
				obj.operationType = 21;
				obj.actionTime = System.DateTime.Now.ToString();
				obj.content = "%s 在项目 %s ——客户往来中发表新主题%s";
				obj.repalceStr.Add("发布人");
				obj.repalceStr.Add("项目名称");
				obj.repalceStr.Add("主题名");
				bt.baseDynamicWorkObj.Add(obj);
				obj = new BaseDynamicWorkObj();
				obj.operationType = 22;
				obj.actionTime = System.DateTime.Now.ToString();
				obj.content = "%s 在项目 %s ——内部交流中发表新主题%s";
				obj.repalceStr.Add("发布人");
				obj.repalceStr.Add("项目名称");
				obj.repalceStr.Add("主题名");
				bt.baseDynamicWorkObj.Add(obj);
				obj = new BaseDynamicWorkObj();
				obj.operationType = 23;
				obj.actionTime = System.DateTime.Now.ToString();
				obj.content = "%s 在项目 %s 中对授权对象进行授权";
				obj.repalceStr.Add("项目管理者");
				obj.repalceStr.Add("项目名称");
				bt.baseDynamicWorkObj.Add(obj);
				obj = new BaseDynamicWorkObj();
				obj.operationType = 24;
				obj.actionTime = System.DateTime.Now.ToString();
				obj.content = "%s 在项目 %s 中取消授权对象的权限";
				obj.repalceStr.Add("项目管理者");
				obj.repalceStr.Add("项目名称");
				bt.baseDynamicWorkObj.Add(obj);
				obj = new BaseDynamicWorkObj();
				obj.operationType = 25;
				obj.actionTime = System.DateTime.Now.ToString();
				obj.content = "%s 在项目 %s 中修改授权对象的权限";
				obj.repalceStr.Add("项目管理者");
				obj.repalceStr.Add("项目名称");
				bt.baseDynamicWorkObj.Add(obj);
				break;
			}
			case DynamicWorkModuleType.SYSTEM_MANAGEMENT:
			{
				BaseDynamicWorkObj obj = new BaseDynamicWorkObj();
				obj.operationType = 1;
				obj.actionTime = System.DateTime.Now.ToString();
				obj.content = "%s 添加部门 %s";
				obj.repalceStr.Add("系统管理员");
				obj.repalceStr.Add("部门名称");
				bt.baseDynamicWorkObj.Add(obj);
				obj = new BaseDynamicWorkObj();
				obj.operationType = 2;
				obj.actionTime = System.DateTime.Now.ToString();
				obj.content = "%s 修改部门 %s";
				obj.repalceStr.Add("系统管理员");
				obj.repalceStr.Add("部门名称");
				bt.baseDynamicWorkObj.Add(obj);
				obj = new BaseDynamicWorkObj();
				obj.operationType = 3;
				obj.actionTime = System.DateTime.Now.ToString();
				obj.content = "%s 删除部门 %s";
				obj.repalceStr.Add("系统管理员");
				obj.repalceStr.Add("部门名称");
				bt.baseDynamicWorkObj.Add(obj);
				obj = new BaseDynamicWorkObj();
				obj.operationType = 4;
				obj.actionTime = System.DateTime.Now.ToString();
				obj.content = "%s 删除部门 %s";
				obj.repalceStr.Add("添加员工");
				obj.repalceStr.Add("员工姓名");
				bt.baseDynamicWorkObj.Add(obj);
				break;
			}
			case DynamicWorkModuleType.WORK_PLAN:
			{
				BaseDynamicWorkObj obj = new BaseDynamicWorkObj();
				obj.operationType = 1;
				obj.actionTime = System.DateTime.Now.ToString();
				obj.content = "%s 新建了一条工作计划 %s";
				obj.repalceStr.Add("计划创建者");
				obj.repalceStr.Add("计划名称");
				bt.baseDynamicWorkObj.Add(obj);
				obj = new BaseDynamicWorkObj();
				obj.operationType = 2;
				obj.actionTime = System.DateTime.Now.ToString();
				obj.content = "%s 修改了工作计划 %s";
				obj.repalceStr.Add("计划创建者");
				obj.repalceStr.Add("计划名称");
				bt.baseDynamicWorkObj.Add(obj);
				obj = new BaseDynamicWorkObj();
				obj.operationType = 3;
				obj.actionTime = System.DateTime.Now.ToString();
				obj.content = "%s 删除了工作计划 %s";
				obj.repalceStr.Add("计划创建者");
				obj.repalceStr.Add("计划名称");
				bt.baseDynamicWorkObj.Add(obj);
				obj = new BaseDynamicWorkObj();
				obj.operationType = 4;
				obj.actionTime = System.DateTime.Now.ToString();
				obj.content = "%s 共享了工作计划表 %s";
				obj.repalceStr.Add("计划创建者");
				obj.repalceStr.Add("‘创建者’的计划表");
				bt.baseDynamicWorkObj.Add(obj);
				obj = new BaseDynamicWorkObj();
				obj.operationType = 5;
				obj.actionTime = System.DateTime.Now.ToString();
				obj.content = "%s 对 %s 共享了工作计划 %s";
				obj.repalceStr.Add("计划创建者");
				obj.repalceStr.Add("共享者");
				obj.repalceStr.Add("计划名称");
				bt.baseDynamicWorkObj.Add(obj);
				break;
			}
			}
			response.dynamicWorkObjByTime.Add(bt);
			return response;
		}
		private void AddBaseDynamicWorkObj(System.Collections.Generic.List<BaseDynamicWorkObj> baseDynamicWorkObjs)
		{
			if (baseDynamicWorkObjs != null)
			{
				foreach (BaseDynamicWorkObj item in baseDynamicWorkObjs)
				{
					this.baseDynamicWorkObjViewModels.Add(new BaseDynamicWorkObjViewModel(item));
				}
			}
		}
		private void InitService()
		{
			this.connection = ServiceUtil.Instance.Connection;
			this.sessionService = ServiceUtil.Instance.SessionService;
		}
		public void GetMoreData()
		{
			this.GetMoreDataCalled = true;
			DynamicWorkRequest dwr = new DynamicWorkRequest();
			dwr.limit = this.itemCountOfPage;
			dwr.moduleType = (int)this.moduleType;
			dwr.page = this.currentPage;
			dwr.tiket = this.sessionService.Ticket;
			dwr.uid = this.staff.Uid;
			this.connection.Send(PacketType.OA_DYNAMIC_WORK, dwr);
		}
		protected override void OnDispose()
		{
			this.connection.EventHandler.DynamicWorkDataGettedEvent -= new DynamicWorkDataGettedHandler(this.OnDynamicWorkDataGetted);
		}
	}
}
