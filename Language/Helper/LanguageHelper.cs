using System;
using System.Windows;
namespace IDKin.IM.Language.Helper
{
	public class LanguageHelper
	{
		public static void LoadLanguageFile(string languagefileName)
		{
			Application.Current.Resources.MergedDictionaries[0] = new ResourceDictionary
			{
				Source = new Uri(languagefileName, UriKind.RelativeOrAbsolute)
			};
		}
	}
}
