using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.Resources;
using System.Runtime.CompilerServices;
namespace IDKin.IM.Windows.Properties
{
	[GeneratedCode("System.Resources.Tools.StronglyTypedResourceBuilder", "4.0.0.0"), System.Diagnostics.DebuggerNonUserCode, System.Runtime.CompilerServices.CompilerGenerated]
	internal class Resources
	{
		private static System.Resources.ResourceManager resourceMan;
		private static System.Globalization.CultureInfo resourceCulture;
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		internal static System.Resources.ResourceManager ResourceManager
		{
			get
			{
				if (object.ReferenceEquals(Resources.resourceMan, null))
				{
					System.Resources.ResourceManager temp = new System.Resources.ResourceManager("IDKin.IM.Windows.Properties.Resources", typeof(Resources).Assembly);
					Resources.resourceMan = temp;
				}
				return Resources.resourceMan;
			}
		}
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		internal static System.Globalization.CultureInfo Culture
		{
			get
			{
				return Resources.resourceCulture;
			}
			set
			{
				Resources.resourceCulture = value;
			}
		}
		internal static string Changelog
		{
			get
			{
				return Resources.ResourceManager.GetString("Changelog", Resources.resourceCulture);
			}
		}
		internal static string chinese
		{
			get
			{
				return Resources.ResourceManager.GetString("chinese", Resources.resourceCulture);
			}
		}
		internal static Icon groupHeaderIcon
		{
			get
			{
				object obj = Resources.ResourceManager.GetObject("groupHeaderIcon", Resources.resourceCulture);
				return (Icon)obj;
			}
		}
		internal static Bitmap login
		{
			get
			{
				object obj = Resources.ResourceManager.GetObject("login", Resources.resourceCulture);
				return (Bitmap)obj;
			}
		}
		internal static Icon notifyIcon
		{
			get
			{
				object obj = Resources.ResourceManager.GetObject("notifyIcon", Resources.resourceCulture);
				return (Icon)obj;
			}
		}
		internal static Icon notifyIcon_empty
		{
			get
			{
				object obj = Resources.ResourceManager.GetObject("notifyIcon_empty", Resources.resourceCulture);
				return (Icon)obj;
			}
		}
		internal static Icon notifyIcon_gray
		{
			get
			{
				object obj = Resources.ResourceManager.GetObject("notifyIcon_gray", Resources.resourceCulture);
				return (Icon)obj;
			}
		}
		internal static Icon speaker
		{
			get
			{
				object obj = Resources.ResourceManager.GetObject("speaker", Resources.resourceCulture);
				return (Icon)obj;
			}
		}
		internal Resources()
		{
		}
	}
}
