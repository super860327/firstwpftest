using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Media;
namespace IDKin.IM.Windows.Model.OA
{
	public class OANoticeData : INotifyPropertyChanged
	{
		private string title;
		private string name;
		private string createtime;
		private Visibility urgent;
		private Brush isRead;
		private string url;
		public event PropertyChangedEventHandler PropertyChanged;
		public string Title
		{
			get
			{
				return this.title;
			}
			set
			{
				this.title = value;
				this.OnPropertyChanged("Title");
			}
		}
		public string Name
		{
			get
			{
				return this.name;
			}
			set
			{
				this.name = value;
				this.OnPropertyChanged("Name");
			}
		}
		public string Createtime
		{
			get
			{
				return this.createtime;
			}
			set
			{
				this.createtime = value;
				this.OnPropertyChanged("Createtime");
			}
		}
		public Visibility Urgent
		{
			get
			{
				return this.urgent;
			}
			set
			{
				this.urgent = value;
				this.OnPropertyChanged("Urgent");
			}
		}
		public Brush IsRead
		{
			get
			{
				return this.isRead;
			}
			set
			{
				this.isRead = value;
				this.OnPropertyChanged("IsRead");
			}
		}
		public string Url
		{
			get
			{
				return this.url;
			}
			set
			{
				this.url = value;
				this.OnPropertyChanged("Url");
			}
		}
		private void OnPropertyChanged(string propertyName)
		{
			if (this.PropertyChanged != null)
			{
				this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
			}
		}
	}
}
