using System;
using System.CodeDom.Compiler;
using System.Collections.Specialized;
using System.Configuration;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Windows.Forms;
namespace IDKin.IM.Windows.Properties
{
	[GeneratedCode("Microsoft.VisualStudio.Editors.SettingsDesigner.SettingsSingleFileGenerator", "10.0.0.0"), System.Runtime.CompilerServices.CompilerGenerated]
	internal sealed class Settings : ApplicationSettingsBase
	{
		private static Settings defaultInstance = (Settings)SettingsBase.Synchronized(new Settings());
		public static Settings Default
		{
			get
			{
				return Settings.defaultInstance;
			}
		}
		[DefaultSettingValue("192.168.1.230"), UserScopedSetting, System.Diagnostics.DebuggerNonUserCode]
		public string Server
		{
			get
			{
				return (string)this["Server"];
			}
			set
			{
				this["Server"] = value;
			}
		}
		[DefaultSettingValue("6200"), UserScopedSetting, System.Diagnostics.DebuggerNonUserCode]
		public int Port
		{
			get
			{
				return (int)this["Port"];
			}
			set
			{
				this["Port"] = value;
			}
		}
		[DefaultSettingValue(""), UserScopedSetting, System.Diagnostics.DebuggerNonUserCode]
		public string Username
		{
			get
			{
				return (string)this["Username"];
			}
			set
			{
				this["Username"] = value;
			}
		}
		[DefaultSettingValue(""), UserScopedSetting, System.Diagnostics.DebuggerNonUserCode]
		public string Password
		{
			get
			{
				return (string)this["Password"];
			}
			set
			{
				this["Password"] = value;
			}
		}
		[DefaultSettingValue("True"), UserScopedSetting, System.Diagnostics.DebuggerNonUserCode]
		public bool SavePassword
		{
			get
			{
				return (bool)this["SavePassword"];
			}
			set
			{
				this["SavePassword"] = value;
			}
		}
		[DefaultSettingValue("1"), UserScopedSetting, System.Diagnostics.DebuggerNonUserCode]
		public int Status
		{
			get
			{
				return (int)this["Status"];
			}
			set
			{
				this["Status"] = value;
			}
		}
		[DefaultSettingValue(""), UserScopedSetting, System.Diagnostics.DebuggerNonUserCode]
		public string ShieldGroups
		{
			get
			{
				return (string)this["ShieldGroups"];
			}
			set
			{
				this["ShieldGroups"] = value;
			}
		}
		[DefaultSettingValue("0"), UserScopedSetting, System.Diagnostics.DebuggerNonUserCode]
		public int FontFamily
		{
			get
			{
				return (int)this["FontFamily"];
			}
			set
			{
				this["FontFamily"] = value;
			}
		}
		[DefaultSettingValue("#000000"), UserScopedSetting, System.Diagnostics.DebuggerNonUserCode]
		public string FontColor
		{
			get
			{
				return (string)this["FontColor"];
			}
			set
			{
				this["FontColor"] = value;
			}
		}
		[DefaultSettingValue("255"), UserScopedSetting, System.Diagnostics.DebuggerNonUserCode]
		public byte FontColorA
		{
			get
			{
				return (byte)this["FontColorA"];
			}
			set
			{
				this["FontColorA"] = value;
			}
		}
		[DefaultSettingValue("0"), UserScopedSetting, System.Diagnostics.DebuggerNonUserCode]
		public byte FontColorR
		{
			get
			{
				return (byte)this["FontColorR"];
			}
			set
			{
				this["FontColorR"] = value;
			}
		}
		[DefaultSettingValue("0"), UserScopedSetting, System.Diagnostics.DebuggerNonUserCode]
		public byte FontColorG
		{
			get
			{
				return (byte)this["FontColorG"];
			}
			set
			{
				this["FontColorG"] = value;
			}
		}
		[DefaultSettingValue("0"), UserScopedSetting, System.Diagnostics.DebuggerNonUserCode]
		public byte FontColorB
		{
			get
			{
				return (byte)this["FontColorB"];
			}
			set
			{
				this["FontColorB"] = value;
			}
		}
		[DefaultSettingValue("1"), UserScopedSetting, System.Diagnostics.DebuggerNonUserCode]
		public int FontSize
		{
			get
			{
				return (int)this["FontSize"];
			}
			set
			{
				this["FontSize"] = value;
			}
		}
		[DefaultSettingValue("0"), UserScopedSetting, System.Diagnostics.DebuggerNonUserCode]
		public int FontWeight
		{
			get
			{
				return (int)this["FontWeight"];
			}
			set
			{
				this["FontWeight"] = value;
			}
		}
		[DefaultSettingValue("0"), UserScopedSetting, System.Diagnostics.DebuggerNonUserCode]
		public int FontStyle
		{
			get
			{
				return (int)this["FontStyle"];
			}
			set
			{
				this["FontStyle"] = value;
			}
		}
		[DefaultSettingValue("normal"), UserScopedSetting, System.Diagnostics.DebuggerNonUserCode]
		public string TextDecoration
		{
			get
			{
				return (string)this["TextDecoration"];
			}
			set
			{
				this["TextDecoration"] = value;
			}
		}
		[DefaultSettingValue(""), UserScopedSetting, System.Diagnostics.DebuggerNonUserCode]
		public string RecentLinkList
		{
			get
			{
				return (string)this["RecentLinkList"];
			}
			set
			{
				this["RecentLinkList"] = value;
			}
		}
		[DefaultSettingValue("True"), UserScopedSetting, System.Diagnostics.DebuggerNonUserCode]
		public bool SystemSetup_Base_AutoLogin
		{
			get
			{
				return (bool)this["SystemSetup_Base_AutoLogin"];
			}
			set
			{
				this["SystemSetup_Base_AutoLogin"] = value;
			}
		}
		[DefaultSettingValue("True"), UserScopedSetting, System.Diagnostics.DebuggerNonUserCode]
		public bool SystemSetup_Base_AutoStart
		{
			get
			{
				return (bool)this["SystemSetup_Base_AutoStart"];
			}
			set
			{
				this["SystemSetup_Base_AutoStart"] = value;
			}
		}
		[DefaultSettingValue("False"), UserScopedSetting, System.Diagnostics.DebuggerNonUserCode]
		public bool SystemSetup_Base_ExitHide
		{
			get
			{
				return (bool)this["SystemSetup_Base_ExitHide"];
			}
			set
			{
				this["SystemSetup_Base_ExitHide"] = value;
			}
		}
		[DefaultSettingValue(""), UserScopedSetting, System.Diagnostics.DebuggerNonUserCode]
		public string SystemSetup_FileTransport_SaveDir
		{
			get
			{
				return (string)this["SystemSetup_FileTransport_SaveDir"];
			}
			set
			{
				this["SystemSetup_FileTransport_SaveDir"] = value;
			}
		}
		[DefaultSettingValue("<?xml version=\"1.0\" encoding=\"utf-16\"?>\r\n<ArrayOfString xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\">\r\n  <string>您好,我现在有事不在,一会再和你联系。</string>\r\n  <string>工作中,请勿打扰。</string>\r\n  <string>我去吃饭了,一会再联系。</string>\r\n</ArrayOfString>"), UserScopedSetting, System.Diagnostics.DebuggerNonUserCode]
		public StringCollection SystemSetup_AutoReply_Message
		{
			get
			{
				return (StringCollection)this["SystemSetup_AutoReply_Message"];
			}
			set
			{
				this["SystemSetup_AutoReply_Message"] = value;
			}
		}
		[DefaultSettingValue("0"), UserScopedSetting, System.Diagnostics.DebuggerNonUserCode]
		public int SystemSetup_AutoReply_Index
		{
			get
			{
				return (int)this["SystemSetup_AutoReply_Index"];
			}
			set
			{
				this["SystemSetup_AutoReply_Index"] = value;
			}
		}
		[DefaultSettingValue("False"), UserScopedSetting, System.Diagnostics.DebuggerNonUserCode]
		public bool SystemSetup_AutoReply_Is
		{
			get
			{
				return (bool)this["SystemSetup_AutoReply_Is"];
			}
			set
			{
				this["SystemSetup_AutoReply_Is"] = value;
			}
		}
		[DefaultSettingValue("True"), UserScopedSetting, System.Diagnostics.DebuggerNonUserCode]
		public bool SystemSetup_MessageNotify_MessageBoxShow
		{
			get
			{
				return (bool)this["SystemSetup_MessageNotify_MessageBoxShow"];
			}
			set
			{
				this["SystemSetup_MessageNotify_MessageBoxShow"] = value;
			}
		}
		[DefaultSettingValue("100"), UserScopedSetting, System.Diagnostics.DebuggerNonUserCode]
		public int SystemSetup_HotKey_SendMessage
		{
			get
			{
				return (int)this["SystemSetup_HotKey_SendMessage"];
			}
			set
			{
				this["SystemSetup_HotKey_SendMessage"] = value;
			}
		}
		[DefaultSettingValue("S"), UserScopedSetting, System.Diagnostics.DebuggerNonUserCode]
		public Keys CutScreen
		{
			get
			{
				return (Keys)this["CutScreen"];
			}
			set
			{
				this["CutScreen"] = value;
			}
		}
		[DefaultSettingValue("Z"), UserScopedSetting, System.Diagnostics.DebuggerNonUserCode]
		public Keys PickupMsg
		{
			get
			{
				return (Keys)this["PickupMsg"];
			}
			set
			{
				this["PickupMsg"] = value;
			}
		}
		[DefaultSettingValue("Ctrl + Alt + S"), UserScopedSetting, System.Diagnostics.DebuggerNonUserCode]
		public string CutScreenString
		{
			get
			{
				return (string)this["CutScreenString"];
			}
			set
			{
				this["CutScreenString"] = value;
			}
		}
		[DefaultSettingValue("Ctrl + Alt + Z"), UserScopedSetting, System.Diagnostics.DebuggerNonUserCode]
		public string PickupMsgString
		{
			get
			{
				return (string)this["PickupMsgString"];
			}
			set
			{
				this["PickupMsgString"] = value;
			}
		}
		[DefaultSettingValue("3"), UserScopedSetting, System.Diagnostics.DebuggerNonUserCode]
		public int CutScreenType
		{
			get
			{
				return (int)this["CutScreenType"];
			}
			set
			{
				this["CutScreenType"] = value;
			}
		}
		[DefaultSettingValue("1"), UserScopedSetting, System.Diagnostics.DebuggerNonUserCode]
		public int CutScreenCAS
		{
			get
			{
				return (int)this["CutScreenCAS"];
			}
			set
			{
				this["CutScreenCAS"] = value;
			}
		}
		[DefaultSettingValue("3"), UserScopedSetting, System.Diagnostics.DebuggerNonUserCode]
		public int PickUpMsgType
		{
			get
			{
				return (int)this["PickUpMsgType"];
			}
			set
			{
				this["PickUpMsgType"] = value;
			}
		}
		[DefaultSettingValue("1"), UserScopedSetting, System.Diagnostics.DebuggerNonUserCode]
		public int PickUpMsgCAS
		{
			get
			{
				return (int)this["PickUpMsgCAS"];
			}
			set
			{
				this["PickUpMsgCAS"] = value;
			}
		}
		[DefaultSettingValue("False"), UserScopedSetting, System.Diagnostics.DebuggerNonUserCode]
		public bool ExitRemind
		{
			get
			{
				return (bool)this["ExitRemind"];
			}
			set
			{
				this["ExitRemind"] = value;
			}
		}
		[DefaultSettingValue("0"), UserScopedSetting, System.Diagnostics.DebuggerNonUserCode]
		public int FastResponseIndex
		{
			get
			{
				return (int)this["FastResponseIndex"];
			}
			set
			{
				this["FastResponseIndex"] = value;
			}
		}
		[DefaultSettingValue("True"), UserScopedSetting, System.Diagnostics.DebuggerNonUserCode]
		public bool IsCutScreenHidenWindow
		{
			get
			{
				return (bool)this["IsCutScreenHidenWindow"];
			}
			set
			{
				this["IsCutScreenHidenWindow"] = value;
			}
		}
		[DefaultSettingValue("<?xml version=\"1.0\" encoding=\"utf-16\"?>\r\n<ArrayOfString xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\">\r\n  <string>收到！</string>\r\n  <string>一会联系！</string>\r\n  <string>请速回复！</string>\r\n  <string>我现在很忙！</string>\r\n  <string>过来一下，有事找！</string>\r\n</ArrayOfString>"), UserScopedSetting, System.Diagnostics.DebuggerNonUserCode]
		public StringCollection SystemSetup_FastReply_Message
		{
			get
			{
				return (StringCollection)this["SystemSetup_FastReply_Message"];
			}
			set
			{
				this["SystemSetup_FastReply_Message"] = value;
			}
		}
		[DefaultSettingValue("0"), UserScopedSetting, System.Diagnostics.DebuggerNonUserCode]
		public int SystemSetup_FastReply_Index
		{
			get
			{
				return (int)this["SystemSetup_FastReply_Index"];
			}
			set
			{
				this["SystemSetup_FastReply_Index"] = value;
			}
		}
		[DefaultSettingValue("True"), UserScopedSetting, System.Diagnostics.DebuggerNonUserCode]
		public bool ShowUsageGuideWindow
		{
			get
			{
				return (bool)this["ShowUsageGuideWindow"];
			}
			set
			{
				this["ShowUsageGuideWindow"] = value;
			}
		}
	}
}
