using System;
using System.Windows;
using System.Windows.Controls;
namespace IDKin.IM.CustomComponents.Controls
{
    public class ImageButton : Button
    {
        public static readonly DependencyProperty DisabledImageProperty;
        public static readonly DependencyProperty HoverImageProperty;
        public static readonly DependencyProperty NormalImageProperty;
        public static readonly DependencyProperty PressedImageProperty;
        public string HoverImage
        {
            get
            {
                return (string)base.GetValue(ImageButton.HoverImageProperty);
            }
            set
            {
                base.SetValue(ImageButton.HoverImageProperty, value);
            }
        }
        public string NormalImage
        {
            get
            {
                return (string)base.GetValue(ImageButton.NormalImageProperty);
            }
            set
            {
                base.SetValue(ImageButton.NormalImageProperty, value);
            }
        }
        public string PressedImage
        {
            get
            {
                return (string)base.GetValue(ImageButton.PressedImageProperty);
            }
            set
            {
                base.SetValue(ImageButton.PressedImageProperty, value);
            }
        }
        public string DisabledImage
        {
            get
            {
                return (string)base.GetValue(ImageButton.DisabledImageProperty);
            }
            set
            {
                base.SetValue(ImageButton.DisabledImageProperty, value);
            }
        }
        static ImageButton()
        {
            ImageButton.DisabledImageProperty = DependencyProperty.Register("DisabledImage", typeof(string), typeof(ImageButton));
            ImageButton.HoverImageProperty = DependencyProperty.Register("HoverImage", typeof(string), typeof(ImageButton));
            ImageButton.NormalImageProperty = DependencyProperty.Register("NormalImage", typeof(string), typeof(ImageButton));
            ImageButton.PressedImageProperty = DependencyProperty.Register("PressedImage", typeof(string), typeof(ImageButton));
            FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof(ImageButton), new FrameworkPropertyMetadata(typeof(ImageButton)));
        }
    }
}
