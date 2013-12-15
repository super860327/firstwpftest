using System;
using System.ComponentModel;
namespace IDKin.IM.Windows.BindingVO
{
	public class LogOnMsg : INotifyPropertyChanged
	{
		private string message;
		public event PropertyChangedEventHandler PropertyChanged;
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
