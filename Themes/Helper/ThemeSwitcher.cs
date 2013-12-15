using System;
using System.Windows;
namespace IDKin.IM.Theme.Helper
{
	public class ThemeSwitcher
	{
		public static void LoadSkin(ThemeEnum style, FrameworkElement element)
		{
			element.Resources.MergedDictionaries.Add(ThemeSwitcher.GetSkinResourceDictionary(style));
		}
		public static void UnloadSkin(ThemeEnum style, FrameworkContentElement element)
		{
			element.Resources.MergedDictionaries.Remove(ThemeSwitcher.GetSkinResourceDictionary(style));
		}
		public static ResourceDictionary GetSkinResourceDictionary(ThemeEnum style)
		{
			Uri uri = null;
			switch (style)
			{
			case ThemeEnum.Classic:
				uri = new Uri("/PresentationFramework.Classic, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35;component/themes/classic.xaml", UriKind.RelativeOrAbsolute);
				break;
			case ThemeEnum.Royale:
				uri = new Uri("/PresentationFramework.Royale, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35;component/themes/royale.normalcolor.xaml", UriKind.RelativeOrAbsolute);
				break;
			case ThemeEnum.Luna:
				uri = new Uri("/PresentationFramework.Luna, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35;component/themes/luna.normalcolor.xaml", UriKind.RelativeOrAbsolute);
				break;
			case ThemeEnum.Luna_HomeStead:
				uri = new Uri("/PresentationFramework.Luna, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35;component/themes/luna.homestead.xaml", UriKind.RelativeOrAbsolute);
				break;
			case ThemeEnum.Luna_MetalLic:
				uri = new Uri("/PresentationFramework.Luna, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35;component/themes/luna.metallic.xaml", UriKind.RelativeOrAbsolute);
				break;
			case ThemeEnum.Aero:
				uri = new Uri("/PresentationFramework.Aero, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35;component/themes/aero.normalcolor.xaml", UriKind.RelativeOrAbsolute);
				break;
			}
			return Application.LoadComponent(uri) as ResourceDictionary;
		}
	}
}
