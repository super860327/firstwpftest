using System;
using System.ComponentModel;
using System.Diagnostics;
namespace IDKin.IM.Windows.ViewModel
{
    public abstract class ViewModelBase : INotifyPropertyChanged, System.IDisposable
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public virtual string DisplayName
        {
            get;
            protected set;
        }
        protected virtual bool ThrowOnInvalidPropertyName
        {
            get;
            private set;
        }
        [System.Diagnostics.Conditional("DEBUG"), System.Diagnostics.DebuggerStepThrough]
        public void VerifyPropertyName(string propertyName)
        {
            if (TypeDescriptor.GetProperties(this)[propertyName] == null)
            {
                string msg = "Invalid property name: " + propertyName;
                if (this.ThrowOnInvalidPropertyName)
                {
                    throw new System.Exception(msg);
                }
                Debug.Fail(msg);
            }
        }
        protected virtual void OnPropertyChanged(string propertyName)
        {
            this.VerifyPropertyName(propertyName);
            PropertyChangedEventHandler handler = this.PropertyChanged;
            if (handler != null)
            {
                PropertyChangedEventArgs e = new PropertyChangedEventArgs(propertyName);
                handler(this, e);
            }
        }
        public void Dispose()
        {
            this.OnDispose();
        }
        protected virtual void OnDispose()
        {
        }
        ~ViewModelBase()
        {
            //try
            //{
            string msg = string.Format("{0} ({1}) ({2}) Finalized", base.GetType().Name, this.DisplayName, this.GetHashCode());
            Debug.WriteLine(msg);
            //}
            //finally
            //{
            //    base.Finalize();
            //}
        }
    }
}
