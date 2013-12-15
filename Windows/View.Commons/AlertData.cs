using System;
using System.ComponentModel;
namespace IDKin.IM.Windows.View.Commons
{
	public class AlertData : INotifyPropertyChanged
	{
		private string caption;
		private string message;
		public event PropertyChangedEventHandler PropertyChanged;
		public string Caption
		{
			get
			{
				return this.caption;
			}
			set
			{
				this.caption = value;
				this.OnPropertyChanged("Caption");
			}
		}
		public string Message
		{
			get
			{
				return this.message;
			}
			set
			{
				this.message = value;
				this.OnPropertyChanged("Message");
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
