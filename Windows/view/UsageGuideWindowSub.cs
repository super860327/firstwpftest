using IDKin.IM.Log;
using IDKin.IM.Windows.Util;
using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Imaging;
namespace IDKin.IM.Windows.View
{
    public partial class UsageGuideWindowSub : Window
    {
        private ILogger logger = null;
        private int _currentImageIndex = -1;
        private BitmapImage[] _images = null;
        
        //private bool _contentLoaded;
        protected internal int CurrentImageIndex
        {
            get
            {
                return this._currentImageIndex;
            }
            set
            {
                if (value < 0 || value >= this._images.Length)
                {
                    throw new System.ArgumentOutOfRangeException("索引越界");
                }
                this._currentImageIndex = value;
                this.imgBorder.ImageSource = this._images[this._currentImageIndex];
                if (this._currentImageIndex == this._images.Length - 1)
                {
                    this.btnNext.IsEnabled = false;
                }
                else
                {
                    this.btnNext.IsEnabled = true;
                }
                if (this._currentImageIndex > 0)
                {
                    this.btnPrevious.IsEnabled = true;
                }
                else
                {
                    this.btnPrevious.IsEnabled = false;
                }
            }
        }
        public UsageGuideWindowSub()
        {
            this.InitializeComponent();
            this.InitData();
            this._images = ServiceUtil.Instance.ImageService.GetFreshManGuidImages();
            if (this._images == null)
            {
                this._images = new BitmapImage[0];
            }
            if (this._images.Length == 0)
            {
                this.btnNext.IsEnabled = false;
                this.btnPrevious.IsEnabled = false;
            }
            else
            {
                this.btnNext_Click(null, null);
            }
        }
        private void InitData()
        {
            this.logger = ServiceUtil.Instance.Logger;
        }
        private void btnPrevious_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                this.CurrentImageIndex--;
            }
            catch (System.ArgumentOutOfRangeException ex)
            {
                this.logger.Error(ex.ToString());
            }
        }
        private void btnNext_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                this.CurrentImageIndex++;
            }
            catch (System.ArgumentOutOfRangeException ex)
            {
                this.logger.Error(ex.ToString());
            }
        }
        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            base.Close();
        }
    }
}
