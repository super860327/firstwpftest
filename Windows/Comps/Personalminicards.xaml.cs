using IDKin.IM.Core;
using IDKin.IM.Windows.Model;
using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Markup;
namespace IDKin.IM.Windows.Comps
{
	[GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
    public partial class Personalminicards : Popup//, IComponentConnector
	{
		private Staff staff = null;
		private bool isCardClose = false;
		//internal Image imgIcon;
		//internal TextBlock tbkName;
		//internal TextBlock tbkNameID;
		//internal TextBlock tbkSignature;
		//internal TextBlock tbkPhone;
		//internal TextBlock tbkExtension;
		//internal TextBlock tbkMobile;
		//internal TextBlock tbkEmail;
        ////private bool _contentLoaded;
		public Personalminicards()
		{
			this.InitializeComponent();
		}
		public void InitData(Staff staff)
		{
			if (staff != null)
			{
				this.staff = staff;
				this.imgIcon.Source = staff.HeaderImage;
				this.tbkNameID.Text = string.Concat(new object[]
				{
					staff.Name,
					"[",
					staff.Uid,
					"]"
				});
				this.tbkSignature.Text = staff.Signature;
				if (staff.ShowScope == 1)
				{
					this.tbkMobile.Text = staff.Mobile;
					this.tbkPhone.Text = staff.Telephone;
					this.tbkExtension.Text = staff.Extension;
					this.tbkEmail.Text = staff.Email;
				}
			}
		}
		private void Hyperlink_Click(object sender, MouseButtonEventArgs e)
		{
			if (this.staff != null)
			{
			}
		}
		private void Popup_MouseLeave(object sender, MouseEventArgs e)
		{
			if (this.isCardClose)
			{
				base.IsOpen = false;
				this.isCardClose = false;
			}
		}
		public void ClosePopup(Point p)
		{
			string direction = DataModel.Instance.Direction;
			if (direction != null)
			{
				if (!(direction == "left"))
				{
					if (direction == "right")
					{
						if (p.X > 50.0 && p.Y > 10.0)
						{
							this.isCardClose = true;
						}
						else
						{
							base.IsOpen = false;
						}
					}
				}
				else
				{
					if (p.X < 0.0 && p.Y > 10.0)
					{
						this.isCardClose = true;
					}
					else
					{
						base.IsOpen = false;
					}
				}
			}
		}
        //[System.Diagnostics.DebuggerNonUserCode]
        //public void InitializeComponent()
        //{
        //    if (!this._contentLoaded)
        //    {
        //        this._contentLoaded = true;
        //        Uri resourceLocater = new Uri("/IDKin.IM.Windows;component/comps/personalminicards.xaml", UriKind.Relative);
        //        Application.LoadComponent(this, resourceLocater);
        //    }
        //}
        //[EditorBrowsable(EditorBrowsableState.Never), System.Diagnostics.DebuggerNonUserCode]
        //void IComponentConnector.Connect(int connectionId, object target)
        //{
        //    switch (connectionId)
        //    {
        //    case 1:
        //        ((Personalminicards)target).MouseLeave += new MouseEventHandler(this.Popup_MouseLeave);
        //        break;
        //    case 2:
        //        this.imgIcon = (Image)target;
        //        break;
        //    case 3:
        //        this.tbkName = (TextBlock)target;
        //        break;
        //    case 4:
        //        this.tbkNameID = (TextBlock)target;
        //        this.tbkNameID.MouseDown += new MouseButtonEventHandler(this.Hyperlink_Click);
        //        break;
        //    case 5:
        //        this.tbkSignature = (TextBlock)target;
        //        break;
        //    case 6:
        //        this.tbkPhone = (TextBlock)target;
        //        break;
        //    case 7:
        //        this.tbkExtension = (TextBlock)target;
        //        break;
        //    case 8:
        //        this.tbkMobile = (TextBlock)target;
        //        break;
        //    case 9:
        //        this.tbkEmail = (TextBlock)target;
        //        break;
        //    default:
        //        this._contentLoaded = true;
        //        break;
        //    }
        //}
	}
}
