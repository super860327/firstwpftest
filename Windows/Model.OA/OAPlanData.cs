using System;
using System.ComponentModel;
namespace IDKin.IM.Windows.Model.OA
{
	public class OAPlanData : INotifyPropertyChanged
	{
		private string title;
		private string fullTitle;
		private string time;
		private string nextRemind;
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
		public string FullTitle
		{
			get
			{
				return this.fullTitle;
			}
			set
			{
				this.fullTitle = value;
				this.OnPropertyChanged("FullTitle");
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
		public string NextRemind
		{
			get
			{
				return this.nextRemind;
			}
			set
			{
				this.nextRemind = value;
				this.OnPropertyChanged("NextRemind");
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
