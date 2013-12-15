using IDKin.IM.Core;
using IDKin.IM.Data;
using IDKin.IM.ImageService;
using IDKin.IM.Log;
using IDKin.IM.Windows.Comps.ChatTab;
using IDKin.IM.Windows.Model;
using IDKin.IM.Windows.Util;
using IDKin.IM.Windows.View;
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Threading;
using IDKin.IM.CustomComponents.Controls;
using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
namespace IDKin.IM.Windows.Comps
{
     
    public partial class TabItemHeaderControl : Border 
    {
        private ImageSource icon;
        private string label;
        [TypeConverter(typeof(LengthConverter))]
        private double iconWidth;
        [TypeConverter(typeof(LengthConverter))]
        private double iconHeight;

        public ImageSource Icon
        {
            get
            {
                return this.icon;
            }
            set
            {
                this.icon = value;
                this.imgIcon.Source = this.icon;
            }
        }
        public string Label
        {
            get
            {
                return this.label;
            }
            set
            {
                this.label = value;
                this.tbkTitle.Text = this.label;
            }
        }
        public double IconWidth
        {
            get
            {
                return this.iconWidth;
            }
            set
            {
                this.iconWidth = value;
                this.imgIcon.Width = this.iconWidth;
            }
        }
        public double IconHeight
        {
            get
            {
                return this.iconHeight;
            }
            set
            {
                this.iconHeight = value;
            }
        }
        public TabItemHeaderControl()
        {
            this.InitializeComponent();
            this.InitEventHandler();
        }
        private void InitEventHandler()
        {
            base.MouseLeftButtonDown += new MouseButtonEventHandler(this.MouseLeftButtonDownHandler);
        }
        private void MouseLeftButtonDownHandler(object sender, MouseButtonEventArgs e)
        {
            this.SetNormalStyle();
        }
        public void SetFlashingStyle()
        {
            CloseableTabItem tabItem = base.Parent as CloseableTabItem;
            tabItem.Style = (Style)tabItem.FindResource("CloseableTabItemFlashingStyle");
        }
        public void SetNormalStyle()
        {
            CloseableTabItem tabItem = base.Parent as CloseableTabItem;
            tabItem.Style = (Style)tabItem.FindResource("CloseableTabItemNormalStyle");
        }
    }
}