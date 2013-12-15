using System;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
namespace IDKin.IM.Windows.Util
{
	public class ValidationUtil
	{
		public static bool IsSharePath(string fullName)
		{
			bool result;
			if (!string.IsNullOrEmpty(fullName))
			{
				Regex regexShareFile = new Regex("\\\\\\\\\\d+\\.\\d+\\.\\d+\\.\\d+");
				MatchCollection matchsShareFilePath = regexShareFile.Matches(fullName);
				if (matchsShareFilePath.Count > 0)
				{
					result = true;
					return result;
				}
			}
			result = false;
			return result;
		}
		public static bool Jid(string str)
		{
			bool result;
			if (string.IsNullOrEmpty(str))
			{
				result = false;
			}
			else
			{
				int at = str.IndexOf('@');
				int res = str.IndexOf('/');
				result = (at >= 0 && res >= 0);
			}
			return result;
		}
		public static long ChangeUid(string userId)
		{
			long result;
			if (!string.IsNullOrEmpty(userId))
			{
				try
				{
					result = System.Convert.ToInt64(userId);
					return result;
				}
				catch (System.Exception)
				{
					result = 0L;
					return result;
				}
			}
			result = 0L;
			return result;
		}
		public static bool IsUid(uint uid)
		{
			return uid > 0u;
		}
		public static bool IsGid(uint gid)
		{
			return gid > 0u;
		}
		public static bool IsValidEmail(string strIn)
		{
			return Regex.IsMatch(strIn, "^(?(\")(\".+?\"@)|(([0-9a-zA-Z]((\\.(?!\\.))|[-!#\\$%&'\\*\\+/=\\?\\^`\\{\\}\\|~\\w])*)(?<=[0-9a-zA-Z])@))(?(\\[)(\\[(\\d{1,3}\\.){3}\\d{1,3}\\])|(([0-9a-zA-Z][-\\w]*[0-9a-zA-Z]\\.)+[a-zA-Z]{2,6}))$");
		}
		public static bool IsValid(DependencyObject node)
		{
			bool result;
			if (node != null)
			{
				if (Validation.GetHasError(node))
				{
					if (node is IInputElement)
					{
						Keyboard.Focus((IInputElement)node);
					}
					result = false;
					return result;
				}
			}
			foreach (object subnode in LogicalTreeHelper.GetChildren(node))
			{
				if (subnode is DependencyObject)
				{
					if (!ValidationUtil.IsValid((DependencyObject)subnode))
					{
						result = false;
						return result;
					}
				}
			}
			result = true;
			return result;
		}
	}
}
