using IDKin.IM.Communicate;
using IDKin.IM.Data;
using IDKin.IM.ImageService;
using IDKin.IM.Log;
using System;
namespace IDKin.IM.Windows.Util
{
	public class ServiceUtil
	{
		private static ServiceUtil instance = null;
		public static ServiceUtil Instance
		{
			get
			{
				if (ServiceUtil.instance == null)
				{
					ServiceUtil.instance = new ServiceUtil();
				}
				return ServiceUtil.instance;
			}
		}
		public IDataService DataService
		{
			get;
			set;
		}
		public ISessionService SessionService
		{
			get;
			set;
		}
		public IWSClient WsClient
		{
			get;
			set;
		}
		public ILogger Logger
		{
			get;
			set;
		}
		public IFileService FileService
		{
			get;
			set;
		}
		public IConnection Connection
		{
			get;
			set;
		}
		public IImageService ImageService
		{
			get;
			set;
		}
		public IUtilService utilService
		{
			get;
			set;
		}
		private ServiceUtil()
		{
		}
	}
}
