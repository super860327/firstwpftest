using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Media;
namespace IDKin.IM.Windows.BindingVO
{
	public class RequestResultVO : INotifyPropertyChanged
	{
		private Visibility reason;
		private Visibility isSure;
		private ImageSource icon;
		public event PropertyChangedEventHandler PropertyChanged;
		public Visibility Reason
		{
			get
			{
				return this.reason;
			}
			set
			{
				this.reason = value;
				this.OnPropertyChanged("Reason");
			}
		}
		public Visibility IsSure
		{
			get
			{
				return this.isSure;
			}
			set
			{
				this.isSure = value;
				this.OnPropertyChanged("IsSure");
			}
		}
		public ImageSource Icon
		{
			get
			{
				return this.icon;
			}
			set
			{
				this.icon = value;
				this.OnPropertyChanged("Icon");
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
