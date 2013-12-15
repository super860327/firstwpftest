using IDKin.IM.Protocol.DynamicWork;
using IDKin.IM.Windows.Util;
using IDKin.IM.Windows.ViewModel;
using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Markup;
namespace IDKin.IM.Windows.Comps.OA.CurrentWork
{
	[GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
	public partial class OAAllCurrentWork : UserControl//, IComponentConnector
	{
		private AllBaseDynamicWorkObjViewModel allBaseDynamicWorkObjViewModel = null;
		//internal TableRowGroup RowGroup;
		//private bool _contentLoaded;
		public AllBaseDynamicWorkObjViewModel AllBaseDynamicWorkObjViewModel
		{
			get
			{
				return this.allBaseDynamicWorkObjViewModel;
			}
			set
			{
				this.allBaseDynamicWorkObjViewModel = value;
			}
		}
		public OAAllCurrentWork()
		{
			this.InitializeComponent();
		}
		public void AddDynamicWorkResponse(DynamicWorkResponse response)
		{
			if (response != null)
			{
				if (response.dynamicWorkObjByTime != null)
				{
					DynamicWorkModuleType moduleType;
					if (!System.Enum.TryParse<DynamicWorkModuleType>(response.moduleType.ToString(), out moduleType))
					{
						ServiceUtil.Instance.Logger.Error("DynamicWorkModuleType 转换异常");
					}
					else
					{
						foreach (DynamicWorkObjByTime item in response.dynamicWorkObjByTime)
						{
							this.RowGroup.Rows.Add(this.CreateOAAllCurrentWorkDateRow(item.actionYearMonthDay));
							foreach (BaseDynamicWorkObj itemBaseDynamicWorkObj in item.baseDynamicWorkObj)
							{
								this.CreateOAAllCurrentWorkItem(moduleType, itemBaseDynamicWorkObj);
							}
						}
					}
				}
			}
		}
		private void CreateOAAllCurrentWorkItem(DynamicWorkModuleType dynamicWorkModuleType, BaseDynamicWorkObj baseDynamicWorkObj)
		{
			BaseDynamicWorkObjViewModel viewModel = new BaseDynamicWorkObjViewModel(baseDynamicWorkObj);
			switch (dynamicWorkModuleType)
			{
			case DynamicWorkModuleType.WORK_COOPERATION:
				this.WorkCooperationTypeHandler(viewModel);
				break;
			case DynamicWorkModuleType.DOCUMENT_MANAGEMENT:
				this.DocumentManagementTypeHandler(viewModel);
				break;
			case DynamicWorkModuleType.INSIDE_DISCUSSION:
				this.InsideDiscussionTypeHandler(viewModel);
				break;
			case DynamicWorkModuleType.INSIDE_NOTICE:
				this.AddDynamicWorkItemFour(viewModel, DynamicWorkItemFourStyle.LastHyperLink);
				break;
			case DynamicWorkModuleType.PROJECT_MANAGEMENT:
				this.ProjectManagementTypeHandler(viewModel);
				break;
			case DynamicWorkModuleType.SYSTEM_MANAGEMENT:
				this.AddDynamicWorkItemFour(viewModel, DynamicWorkItemFourStyle.None);
				break;
			case DynamicWorkModuleType.WORK_PLAN:
				this.WorkPlanTypeHandler(viewModel);
				break;
			}
		}
		private void InsideDiscussionTypeHandler(BaseDynamicWorkObjViewModel viewModel)
		{
			InsideDiscussionType type;
			if (!System.Enum.TryParse<InsideDiscussionType>(viewModel.BaseDynamicWorkObj.operationType.ToString(), out type))
			{
				ServiceUtil.Instance.Logger.Error("DocumentManagementType 转换异常");
			}
			else
			{
				switch (type)
				{
				case InsideDiscussionType.CreateTopic:
					this.AddDynamicWorkItemSix(viewModel, DynamicWorkItemSixStyle.FourLastHyperLink);
					break;
				case InsideDiscussionType.ReturnTopic:
					this.AddDynamicWorkItemSix(viewModel, DynamicWorkItemSixStyle.FourLastHyperLink);
					break;
				case InsideDiscussionType.ModifyTopic:
					this.AddDynamicWorkItemFour(viewModel, DynamicWorkItemFourStyle.LastHyperLink);
					break;
				case InsideDiscussionType.DeleteTopic:
					this.AddDynamicWorkItemFour(viewModel, DynamicWorkItemFourStyle.LastHyperLink);
					break;
				case InsideDiscussionType.MoveTopic:
					this.AddDynamicWorkItemFour(viewModel, DynamicWorkItemFourStyle.LastHyperLink);
					break;
				case InsideDiscussionType.CreateBlock:
					this.AddDynamicWorkItemFour(viewModel, DynamicWorkItemFourStyle.LastHyperLink);
					break;
				case InsideDiscussionType.ModifyBlock:
					this.AddDynamicWorkItemFour(viewModel, DynamicWorkItemFourStyle.LastHyperLink);
					break;
				case InsideDiscussionType.DeleteBlock:
					this.AddDynamicWorkItemFour(viewModel, DynamicWorkItemFourStyle.LastHyperLink);
					break;
				}
			}
		}
		private void DocumentManagementTypeHandler(BaseDynamicWorkObjViewModel viewModel)
		{
			DocumentManagementType type;
			if (!System.Enum.TryParse<DocumentManagementType>(viewModel.BaseDynamicWorkObj.operationType.ToString(), out type))
			{
				ServiceUtil.Instance.Logger.Error("DocumentManagementType 转换异常");
			}
			else
			{
				switch (type)
				{
				case DocumentManagementType.AuthorizeFile:
					this.AddDynamicWorkItemTwelve(viewModel, DynamicWorkItemTwelveStyle.LastHyperlink);
					break;
				case DocumentManagementType.AuthorizeDirectory:
					this.AddDynamicWorkItemTen(viewModel, DynamicWorkItemTenStyle.LastHyperlink);
					break;
				case DocumentManagementType.UploadFile:
					this.AddDynamicWorkItemSevenTwo(viewModel, DynamicWorkItemSevenStyle.LastHyperlink);
					break;
				case DocumentManagementType.EditDirectory:
					this.AddDynamicWorkItemFiveTwo(viewModel, DynamicWorkItemFiveStyle.LastHyperLinkSpectial);
					break;
				case DocumentManagementType.EditFile:
					this.AddDynamicWorkItemSevenTwo(viewModel, DynamicWorkItemSevenStyle.LastHyperlink);
					break;
				case DocumentManagementType.DeleteDirectory:
					this.AddDynamicWorkItemFour(viewModel, DynamicWorkItemFourStyle.None);
					break;
				case DocumentManagementType.DeleteFile:
					this.AddDynamicWorkItemSevenTwo(viewModel, DynamicWorkItemSevenStyle.LastHyperlink);
					break;
				case DocumentManagementType.MoveFile:
					this.AddDynamicWorkItemTen(viewModel, DynamicWorkItemTenStyle.LastHyperlink);
					break;
				case DocumentManagementType.DownloadFile:
					this.AddDynamicWorkItemNine(viewModel, DynamicWorkItemNineStyle.LastHyperlink);
					break;
				case DocumentManagementType.CreateDirectory:
					this.AddDynamicWorkItemFiveTwo(viewModel, DynamicWorkItemFiveStyle.LastHyperLinkSpectial);
					break;
				case DocumentManagementType.Favour:
					this.AddDynamicWorkItemSevenTwo(viewModel, DynamicWorkItemSevenStyle.LastHyperlink);
					break;
				}
			}
		}
		private void AddDynamicWorkItemNine(BaseDynamicWorkObjViewModel viewModel, DynamicWorkItemNineStyle style)
		{
			if (viewModel.RepalceStrs.Count == 5 && viewModel.UnReplaceStrs.Count == 3)
			{
				DynamicWorkItemNine row = new DynamicWorkItemNine(viewModel.ActionHourMinute, viewModel.RepalceStrs[0], viewModel.UnReplaceStrs[0], viewModel.RepalceStrs[1], viewModel.UnReplaceStrs[1], viewModel.RepalceStrs[2], viewModel.UnReplaceStrs[2], viewModel.RepalceStrs[3], viewModel.RepalceStrs[4], viewModel.Url);
				row.UseStyle(style);
				this.RowGroup.Rows.Add(row);
			}
			else
			{
				viewModel.UnReplaceStrs.Clear();
				viewModel.UnReplaceStrs.Add("在以下目录中下载了");
				viewModel.UnReplaceStrs.Add("文件");
				viewModel.UnReplaceStrs.Add("、");
				viewModel.RepalceStrs.Clear();
				viewModel.RepalceStrs.Add("操作者");
				viewModel.RepalceStrs.Add("文件数量");
				viewModel.RepalceStrs.Add("目录名");
				viewModel.RepalceStrs.Add("文件名");
				viewModel.RepalceStrs.Add("目录名称");
				DynamicWorkItemNine row = new DynamicWorkItemNine(viewModel.ActionHourMinute, viewModel.RepalceStrs[0], viewModel.UnReplaceStrs[0], viewModel.RepalceStrs[1], viewModel.UnReplaceStrs[1], viewModel.RepalceStrs[2], viewModel.UnReplaceStrs[2], viewModel.RepalceStrs[3], viewModel.UnReplaceStrs[3], viewModel.Url);
				row.UseStyle(style);
				this.RowGroup.Rows.Add(row);
			}
		}
		private void AddDynamicWorkItemTen(BaseDynamicWorkObjViewModel viewModel, DynamicWorkItemTenStyle style)
		{
			if (viewModel.RepalceStrs.Count == 5 && viewModel.UnReplaceStrs.Count == 4)
			{
				DynamicWorkItemTen row = new DynamicWorkItemTen(viewModel.ActionHourMinute, viewModel.RepalceStrs[0], viewModel.UnReplaceStrs[0], viewModel.RepalceStrs[1], viewModel.UnReplaceStrs[1], viewModel.RepalceStrs[2], viewModel.UnReplaceStrs[2], viewModel.RepalceStrs[3], viewModel.UnReplaceStrs[3], viewModel.RepalceStrs[4], viewModel.Url);
				row.UseStyle(style);
				this.RowGroup.Rows.Add(row);
			}
			else
			{
				viewModel.UnReplaceStrs.Clear();
				viewModel.UnReplaceStrs.Add("向");
				viewModel.UnReplaceStrs.Add("授予了目录");
				viewModel.UnReplaceStrs.Add("的");
				viewModel.UnReplaceStrs.Add("权限");
				viewModel.RepalceStrs.Clear();
				viewModel.RepalceStrs.Add("授权者");
				viewModel.RepalceStrs.Add("被授权者");
				viewModel.RepalceStrs.Add("目录名");
				viewModel.RepalceStrs.Add("查看、下载");
				viewModel.RepalceStrs.Add("企业资料库授权");
				DynamicWorkItemTen row = new DynamicWorkItemTen(viewModel.ActionHourMinute, viewModel.RepalceStrs[0], viewModel.UnReplaceStrs[0], viewModel.RepalceStrs[1], viewModel.UnReplaceStrs[1], viewModel.RepalceStrs[2], viewModel.UnReplaceStrs[2], viewModel.RepalceStrs[3], viewModel.UnReplaceStrs[3], viewModel.RepalceStrs[4], viewModel.Url);
				row.UseStyle(style);
				this.RowGroup.Rows.Add(row);
			}
		}
		private void AddDynamicWorkItemTwelve(BaseDynamicWorkObjViewModel viewModel, DynamicWorkItemTwelveStyle style)
		{
			if (viewModel.RepalceStrs.Count == 6 && viewModel.UnReplaceStrs.Count == 5)
			{
				DynamicWorkItemTwelve row = new DynamicWorkItemTwelve(viewModel.ActionHourMinute, viewModel.RepalceStrs[0], viewModel.UnReplaceStrs[0], viewModel.RepalceStrs[1], viewModel.UnReplaceStrs[1], viewModel.RepalceStrs[2], viewModel.UnReplaceStrs[2], viewModel.RepalceStrs[3], viewModel.UnReplaceStrs[3], viewModel.RepalceStrs[4], viewModel.UnReplaceStrs[4], viewModel.RepalceStrs[5], viewModel.Url);
				row.UseStyle(style);
				this.RowGroup.Rows.Add(row);
			}
			else
			{
				viewModel.UnReplaceStrs.Clear();
				viewModel.UnReplaceStrs.Add("向");
				viewModel.UnReplaceStrs.Add("授予了");
				viewModel.UnReplaceStrs.Add("文件");
				viewModel.UnReplaceStrs.Add("的");
				viewModel.UnReplaceStrs.Add("权限");
				viewModel.RepalceStrs.Clear();
				viewModel.RepalceStrs.Add("授权者");
				viewModel.RepalceStrs.Add("被授权者");
				viewModel.RepalceStrs.Add("文件数量");
				viewModel.RepalceStrs.Add("文件名");
				viewModel.RepalceStrs.Add("查看、下载");
				viewModel.RepalceStrs.Add("企业资料库授权");
				DynamicWorkItemTwelve row = new DynamicWorkItemTwelve(viewModel.ActionHourMinute, viewModel.RepalceStrs[0], viewModel.UnReplaceStrs[0], viewModel.RepalceStrs[1], viewModel.UnReplaceStrs[1], viewModel.RepalceStrs[2], viewModel.UnReplaceStrs[2], viewModel.RepalceStrs[3], viewModel.UnReplaceStrs[3], viewModel.RepalceStrs[4], viewModel.UnReplaceStrs[4], viewModel.RepalceStrs[5], viewModel.Url);
				row.UseStyle(style);
				this.RowGroup.Rows.Add(row);
			}
		}
		private void WorkPlanTypeHandler(BaseDynamicWorkObjViewModel viewModel)
		{
			WorkPlanType type;
			if (!System.Enum.TryParse<WorkPlanType>(viewModel.BaseDynamicWorkObj.operationType.ToString(), out type))
			{
				ServiceUtil.Instance.Logger.Error("WorkPlanType 转换异常");
			}
			else
			{
				switch (type)
				{
				case WorkPlanType.CreateWorkPlan:
					this.AddDynamicWorkItemFour(viewModel, DynamicWorkItemFourStyle.LastHyperLink);
					break;
				case WorkPlanType.ModifyWorkPlan:
					this.AddDynamicWorkItemFour(viewModel, DynamicWorkItemFourStyle.LastHyperLink);
					break;
				case WorkPlanType.DeleteWorkPlan:
					this.AddDynamicWorkItemFour(viewModel, DynamicWorkItemFourStyle.LastHyperLink);
					break;
				case WorkPlanType.ShareWorkPlanList:
					this.AddDynamicWorkItemFour(viewModel, DynamicWorkItemFourStyle.LastHyperLink);
					break;
				case WorkPlanType.ShareWorkPlanItem:
					this.AddDynamicWorkItemSix(viewModel, DynamicWorkItemSixStyle.LastHyperLink);
					break;
				}
			}
		}
		private void ProjectManagementTypeHandler(BaseDynamicWorkObjViewModel viewModel)
		{
			ProjectManagementType type;
			if (!System.Enum.TryParse<ProjectManagementType>(viewModel.BaseDynamicWorkObj.operationType.ToString(), out type))
			{
				ServiceUtil.Instance.Logger.Error("ProjectManagementType 转换异常");
			}
			else
			{
				switch (type)
				{
				case ProjectManagementType.CreateProject:
					this.AddDynamicWorkItemFour(viewModel, DynamicWorkItemFourStyle.LastHyperLink);
					break;
				case ProjectManagementType.CreateSubProject:
					this.AddDynamicWorkItemFour(viewModel, DynamicWorkItemFourStyle.LastHyperLink);
					break;
				case ProjectManagementType.FinishProject:
					this.AddDynamicWorkItemFive(viewModel, DynamicWorkItemFiveStyle.FourHyperLink);
					break;
				case ProjectManagementType.PublishProjectTask:
					this.AddDynamicWorkItemSix(viewModel, DynamicWorkItemSixStyle.LastHyperLink);
					break;
				case ProjectManagementType.ModifyProjectTask:
					this.AddDynamicWorkItemFour(viewModel, DynamicWorkItemFourStyle.LastHyperLink);
					break;
				case ProjectManagementType.SubmitWork:
					this.AddDynamicWorkItemSeven(viewModel, DynamicWorkItemSevenStyle.FourHyperLink);
					break;
				case ProjectManagementType.CheckWorkOk:
					this.AddDynamicWorkItemSeven(viewModel, DynamicWorkItemSevenStyle.SixHyperlink);
					break;
				case ProjectManagementType.CheckWorkNg:
					this.AddDynamicWorkItemSeven(viewModel, DynamicWorkItemSevenStyle.SixHyperlink);
					break;
				case ProjectManagementType.LeaveMessage:
					this.AddDynamicWorkItemFive(viewModel, DynamicWorkItemFiveStyle.FourHyperLink);
					break;
				case ProjectManagementType.ModifyWork:
					this.AddDynamicWorkItemFive(viewModel, DynamicWorkItemFiveStyle.FourHyperLink);
					break;
				case ProjectManagementType.PaushProject:
					this.AddDynamicWorkItemFour(viewModel, DynamicWorkItemFourStyle.LastHyperLink);
					break;
				case ProjectManagementType.ResumeProject:
					this.AddDynamicWorkItemFour(viewModel, DynamicWorkItemFourStyle.LastHyperLink);
					break;
				case ProjectManagementType.StopProject:
					this.AddDynamicWorkItemFour(viewModel, DynamicWorkItemFourStyle.LastHyperLink);
					break;
				case ProjectManagementType.StartProject:
					this.AddDynamicWorkItemFour(viewModel, DynamicWorkItemFourStyle.LastHyperLink);
					break;
				case ProjectManagementType.DeleteProject:
					this.AddDynamicWorkItemFour(viewModel, DynamicWorkItemFourStyle.None);
					break;
				case ProjectManagementType.PaushProjectTask:
					this.AddDynamicWorkItemFour(viewModel, DynamicWorkItemFourStyle.LastHyperLink);
					break;
				case ProjectManagementType.ResumeProjectTask:
					this.AddDynamicWorkItemFour(viewModel, DynamicWorkItemFourStyle.LastHyperLink);
					break;
				case ProjectManagementType.StopProjectTask:
					this.AddDynamicWorkItemFour(viewModel, DynamicWorkItemFourStyle.LastHyperLink);
					break;
				case ProjectManagementType.StartProjectTask:
					this.AddDynamicWorkItemFour(viewModel, DynamicWorkItemFourStyle.LastHyperLink);
					break;
				case ProjectManagementType.DeleteProjectTask:
					this.AddDynamicWorkItemFour(viewModel, DynamicWorkItemFourStyle.None);
					break;
				case ProjectManagementType.TwoClientPublishTopic:
					this.AddDynamicWorkItemSix(viewModel, DynamicWorkItemSixStyle.FourLastHyperLink);
					break;
				case ProjectManagementType.InsidePublishTopic:
					this.AddDynamicWorkItemSix(viewModel, DynamicWorkItemSixStyle.FourLastHyperLink);
					break;
				case ProjectManagementType.GiveAuthorization:
					this.AddDynamicWorkItemFive(viewModel, DynamicWorkItemFiveStyle.FourHyperLink);
					break;
				case ProjectManagementType.CancelAuthorization:
					this.AddDynamicWorkItemFive(viewModel, DynamicWorkItemFiveStyle.FourHyperLink);
					break;
				case ProjectManagementType.ModiyAuthorization:
					this.AddDynamicWorkItemFive(viewModel, DynamicWorkItemFiveStyle.FourHyperLink);
					break;
				}
			}
		}
		private void AddDynamicWorkItemFiveTwo(BaseDynamicWorkObjViewModel viewModel, DynamicWorkItemFiveStyle style)
		{
			if (viewModel.RepalceStrs.Count == 3 && viewModel.UnReplaceStrs.Count == 1)
			{
				DynamicWorkItemFive row = new DynamicWorkItemFive(viewModel.ActionHourMinute, viewModel.RepalceStrs[0], viewModel.UnReplaceStrs[0], viewModel.RepalceStrs[1], viewModel.RepalceStrs[2], viewModel.Url);
				row.UseStyle(style);
				this.RowGroup.Rows.Add(row);
			}
			else
			{
				viewModel.UnReplaceStrs.Clear();
				viewModel.UnReplaceStrs.Add("确认项目");
				viewModel.RepalceStrs.Clear();
				viewModel.RepalceStrs.Add("项目管理者");
				viewModel.RepalceStrs.Add("项目名称");
				viewModel.UnReplaceStrs.Add("完成");
				DynamicWorkItemFive row = new DynamicWorkItemFive(viewModel.ActionHourMinute, viewModel.RepalceStrs[0], viewModel.UnReplaceStrs[0], viewModel.RepalceStrs[1], viewModel.RepalceStrs[2], viewModel.Url);
				row.UseStyle(style);
				this.RowGroup.Rows.Add(row);
			}
		}
		private void AddDynamicWorkItemFive(BaseDynamicWorkObjViewModel viewModel, DynamicWorkItemFiveStyle style)
		{
			if (viewModel.RepalceStrs.Count == 2 && viewModel.UnReplaceStrs.Count == 2)
			{
				DynamicWorkItemFive row = new DynamicWorkItemFive(viewModel.ActionHourMinute, viewModel.RepalceStrs[0], viewModel.UnReplaceStrs[0], viewModel.RepalceStrs[1], viewModel.UnReplaceStrs[1], viewModel.Url);
				row.UseStyle(style);
				this.RowGroup.Rows.Add(row);
			}
			else
			{
				viewModel.UnReplaceStrs.Clear();
				viewModel.UnReplaceStrs.Add("确认项目");
				viewModel.UnReplaceStrs.Add("完成");
				viewModel.RepalceStrs.Clear();
				viewModel.RepalceStrs.Add("项目管理者");
				viewModel.RepalceStrs.Add("项目名称");
				DynamicWorkItemFive row = new DynamicWorkItemFive(viewModel.ActionHourMinute, viewModel.RepalceStrs[0], viewModel.UnReplaceStrs[0], viewModel.RepalceStrs[1], viewModel.UnReplaceStrs[1], viewModel.Url);
				row.UseStyle(style);
				this.RowGroup.Rows.Add(row);
			}
		}
		private void AddDynamicWorkItemFour(BaseDynamicWorkObjViewModel viewModel, DynamicWorkItemFourStyle style)
		{
			if (viewModel.RepalceStrs.Count == 2 && viewModel.UnReplaceStrs.Count == 1)
			{
				DynamicWorkItemFour row = new DynamicWorkItemFour(viewModel.ActionHourMinute, viewModel.RepalceStrs[0], viewModel.UnReplaceStrs[0], viewModel.RepalceStrs[1]);
				row.UseStyle(style);
				this.RowGroup.Rows.Add(row);
			}
			else
			{
				viewModel.UnReplaceStrs.Clear();
				viewModel.UnReplaceStrs.Add("添加部门");
				viewModel.RepalceStrs.Clear();
				viewModel.RepalceStrs.Add("系统管理员");
				viewModel.RepalceStrs.Add("部门名称");
				DynamicWorkItemFour row = new DynamicWorkItemFour(viewModel.ActionHourMinute, viewModel.RepalceStrs[0], viewModel.UnReplaceStrs[0], viewModel.RepalceStrs[1]);
				row.UseStyle(style);
				this.RowGroup.Rows.Add(row);
			}
		}
		private void WorkCooperationTypeHandler(BaseDynamicWorkObjViewModel viewModel)
		{
			WorkCooperationType type;
			if (!System.Enum.TryParse<WorkCooperationType>(viewModel.BaseDynamicWorkObj.operationType.ToString(), out type))
			{
				ServiceUtil.Instance.Logger.Error("WorkCooperationType 转换异常");
			}
			else
			{
				switch (type)
				{
				case WorkCooperationType.PUBLISH_WORK_TASK:
					this.AddDynamicWorkItemSix(viewModel, DynamicWorkItemSixStyle.LastHyperLink);
					break;
				case WorkCooperationType.SUBMIT_WORK:
					this.AddDynamicWorkItemSeven(viewModel, DynamicWorkItemSevenStyle.FourHyperLink);
					break;
				case WorkCooperationType.CHECK_WORK_OK:
					this.AddDynamicWorkItemSeven(viewModel, DynamicWorkItemSevenStyle.SixHyperlink);
					break;
				case WorkCooperationType.CHECK_WORK_NG:
					this.AddDynamicWorkItemSix(viewModel, DynamicWorkItemSixStyle.LastHyperLink);
					break;
				case WorkCooperationType.PUBLISH_REQUISTION:
					this.AddDynamicWorkItemFour(viewModel, DynamicWorkItemFourStyle.LastHyperLink);
					break;
				case WorkCooperationType.CHECKRE_QUISITION_OK:
					this.AddDynamicWorkItemSix(viewModel, DynamicWorkItemSixStyle.LastHyperLink);
					break;
				case WorkCooperationType.CHECKRE_QUISITION_NG:
					this.AddDynamicWorkItemSix(viewModel, DynamicWorkItemSixStyle.LastHyperLink);
					break;
				}
			}
		}
		private void AddDynamicWorkItemSix(BaseDynamicWorkObjViewModel viewModel, DynamicWorkItemSixStyle style)
		{
			if (viewModel.RepalceStrs.Count == 3 && viewModel.UnReplaceStrs.Count == 2)
			{
				DynamicWorkItemSix row = new DynamicWorkItemSix(viewModel.ActionHourMinute, viewModel.RepalceStrs[0], viewModel.UnReplaceStrs[0], viewModel.RepalceStrs[1], viewModel.UnReplaceStrs[1], viewModel.RepalceStrs[2], viewModel.Url);
				row.UseStyle(style);
				this.RowGroup.Rows.Add(row);
			}
			else
			{
				viewModel.UnReplaceStrs.Clear();
				viewModel.UnReplaceStrs.Add("未通过");
				viewModel.UnReplaceStrs.Add("递交的工作简报");
				viewModel.RepalceStrs.Clear();
				viewModel.RepalceStrs.Add("发布人");
				viewModel.RepalceStrs.Add("执行人");
				viewModel.RepalceStrs.Add("任务名称");
				DynamicWorkItemSix row = new DynamicWorkItemSix(viewModel.ActionHourMinute, viewModel.RepalceStrs[0], viewModel.UnReplaceStrs[0], viewModel.RepalceStrs[1], viewModel.UnReplaceStrs[1], viewModel.RepalceStrs[2], viewModel.Url);
				row.UseStyle(style);
				this.RowGroup.Rows.Add(row);
			}
		}
		private void AddDynamicWorkItemSevenTwo(BaseDynamicWorkObjViewModel viewModel, DynamicWorkItemSevenStyle style)
		{
			if (viewModel.RepalceStrs.Count == 4 && viewModel.UnReplaceStrs.Count == 2)
			{
				DynamicWorkItemSeven row = new DynamicWorkItemSeven(viewModel.ActionHourMinute, viewModel.RepalceStrs[0], viewModel.UnReplaceStrs[0], viewModel.RepalceStrs[1], viewModel.UnReplaceStrs[1], viewModel.RepalceStrs[2], viewModel.RepalceStrs[3], viewModel.Url);
				row.UseStyle(style);
				this.RowGroup.Rows.Add(row);
			}
			else
			{
				viewModel.UnReplaceStrs.Clear();
				viewModel.UnReplaceStrs.Add("在以下目录中上传了");
				viewModel.UnReplaceStrs.Add("文件");
				viewModel.RepalceStrs.Clear();
				viewModel.RepalceStrs.Add("上传者");
				viewModel.RepalceStrs.Add("文件数量");
				viewModel.RepalceStrs.Add("文件名");
				viewModel.RepalceStrs.Add("目录名称");
				DynamicWorkItemSeven row = new DynamicWorkItemSeven(viewModel.ActionHourMinute, viewModel.RepalceStrs[0], viewModel.UnReplaceStrs[0], viewModel.RepalceStrs[1], viewModel.UnReplaceStrs[1], viewModel.RepalceStrs[2], viewModel.RepalceStrs[3], viewModel.Url);
				row.UseStyle(style);
				this.RowGroup.Rows.Add(row);
			}
		}
		private void AddDynamicWorkItemSeven(BaseDynamicWorkObjViewModel viewModel, DynamicWorkItemSevenStyle style)
		{
			if (viewModel.RepalceStrs.Count == 3 && viewModel.UnReplaceStrs.Count == 3)
			{
				DynamicWorkItemSeven row = new DynamicWorkItemSeven(viewModel.ActionHourMinute, viewModel.RepalceStrs[0], viewModel.UnReplaceStrs[0], viewModel.RepalceStrs[1], viewModel.UnReplaceStrs[1], viewModel.RepalceStrs[2], viewModel.UnReplaceStrs[2], viewModel.Url);
				row.UseStyle(style);
				this.RowGroup.Rows.Add(row);
			}
			else
			{
				viewModel.UnReplaceStrs.Clear();
				viewModel.UnReplaceStrs.Add("通过");
				viewModel.UnReplaceStrs.Add("在工作任务");
				viewModel.UnReplaceStrs.Add("中递交的工作简报");
				viewModel.RepalceStrs.Clear();
				viewModel.RepalceStrs.Add("发布人");
				viewModel.RepalceStrs.Add("执行人");
				viewModel.RepalceStrs.Add("任务名称");
				DynamicWorkItemSeven row = new DynamicWorkItemSeven(viewModel.ActionHourMinute, viewModel.RepalceStrs[0], viewModel.UnReplaceStrs[0], viewModel.RepalceStrs[1], viewModel.UnReplaceStrs[1], viewModel.RepalceStrs[2], viewModel.UnReplaceStrs[2], viewModel.Url);
				row.UseStyle(style);
				this.RowGroup.Rows.Add(row);
			}
		}
		public OAAllCurrentWorkDateRow CreateOAAllCurrentWorkDateRow(string date)
		{
			return new OAAllCurrentWorkDateRow
			{
				tbkDateGroup = 
				{
					Text = date
				}
			};
		}
		private void ViewMoreHandler(object sender, MouseButtonEventArgs e)
		{
		}
        //[System.Diagnostics.DebuggerNonUserCode]
        //public void InitializeComponent()
        //{
        //    if (!this._contentLoaded)
        //    {
        //        this._contentLoaded = true;
        //        Uri resourceLocater = new Uri("/IDKin.IM.Windows;component/comps/oa/currentwork/oaallcurrentwork.xaml", UriKind.Relative);
        //        Application.LoadComponent(this, resourceLocater);
        //    }
        //}
        //[EditorBrowsable(EditorBrowsableState.Never), System.Diagnostics.DebuggerNonUserCode]
        //void IComponentConnector.Connect(int connectionId, object target)
        //{
        //    switch (connectionId)
        //    {
        //    case 1:
        //        this.RowGroup = (TableRowGroup)target;
        //        break;
        //    case 2:
        //        ((TextBlock)target).MouseLeftButtonDown += new MouseButtonEventHandler(this.ViewMoreHandler);
        //        break;
        //    default:
        //        this._contentLoaded = true;
        //        break;
        //    }
        //}
	}
}
