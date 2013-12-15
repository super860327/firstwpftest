using System;
using System.ComponentModel;
using System.Windows;
namespace IDKin.IM.Windows.Model.OA
{
	public class OAWorkflowData : INotifyPropertyChanged
	{
		private string title;
		private Visibility important;
		private Visibility urgent;
		private string url;
		private string username;
		private string createtime;
		private string project;
		private string time;
		private string description;
		private Visibility type;
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
		public Visibility Important
		{
			get
			{
				return this.important;
			}
			set
			{
				this.important = value;
				this.OnPropertyChanged("Important");
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
		public string Username
		{
			get
			{
				return this.username;
			}
			set
			{
				this.username = value;
				this.OnPropertyChanged("Username");
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
		public string Project
		{
			get
			{
				return this.project;
			}
			set
			{
				this.project = value;
				this.OnPropertyChanged("Project");
			}
		}
		public string Time
		{
			get
			{
				return this.time;
			}
			set
			{
				this.time = value;
				this.OnPropertyChanged("Time");
			}
		}
		public string Description
		{
			get
			{
				return this.description;
			}
			set
			{
				this.description = value;
				this.OnPropertyChanged("Description");
			}
		}
		public Visibility Type
		{
			get
			{
				return this.type;
			}
			set
			{
				this.type = value;
				this.OnPropertyChanged("Type");
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
