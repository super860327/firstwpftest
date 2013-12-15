using IDKin.IM.Log;
using IDKin.IM.Windows.Util;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
namespace IDKin.IM.Windows.View.Commons
{
	public class CanvasCustom : Canvas
	{
		public delegate void CacheImageDelegate(double x, double y, double width, double height);
		public CanvasCustom.CacheImageDelegate CacheImage;
		public Thickness MoveArea;
		private ILogger logger = ServiceUtil.Instance.Logger;
		private double size = 4.0;
		private Rect leftTop;
		private Rect rightTop;
		private Rect leftBottom;
		private Rect rightBottom;
		private Rect border = new Rect(0.0, 0.0, 0.0, 0.0);
		private Point p = default(Point);
		private bool whetherSelected = false;
		private bool mouseOnLeftTop;
		private bool mouseOnLeftBottom;
		private bool mouseOnRightTop;
		private bool mouseOnRightBottom;
		private bool changeSizeMode;
		private bool moveMode;
		private Pen borderPen = new Pen(Brushes.Red, 0.5);
		private Pen rectPen = new Pen(Brushes.Blue, 0.5);
		public Rect LeftTop
		{
			get
			{
				return this.leftTop;
			}
		}
		public Rect RightTop
		{
			get
			{
				return this.rightTop;
			}
		}
		public Rect LeftBottom
		{
			get
			{
				return this.leftBottom;
			}
		}
		public Rect RightBottom
		{
			get
			{
				return this.rightBottom;
			}
		}
		public bool WhetherSelected
		{
			get
			{
				return this.whetherSelected;
			}
		}
		public bool ChangeSizeMode
		{
			get
			{
				return this.changeSizeMode;
			}
			set
			{
				this.changeSizeMode = value;
				this.moveMode = !value;
			}
		}
		public bool MoveMode
		{
			get
			{
				return this.moveMode;
			}
			set
			{
				this.moveMode = value;
				this.changeSizeMode = !value;
			}
		}
		public bool MouseOnLeftTop
		{
			get
			{
				return this.mouseOnLeftTop;
			}
			set
			{
				this.mouseOnLeftTop = value;
				if (value)
				{
					this.mouseOnLeftBottom = false;
					this.mouseOnRightTop = false;
					this.mouseOnRightBottom = false;
				}
			}
		}
		public bool MouseOnLeftBottom
		{
			get
			{
				return this.mouseOnLeftBottom;
			}
			set
			{
				this.mouseOnLeftBottom = value;
				if (value)
				{
					this.mouseOnLeftTop = false;
					this.mouseOnRightTop = false;
					this.mouseOnRightBottom = false;
				}
			}
		}
		public bool MouseOnRightTop
		{
			get
			{
				return this.mouseOnRightTop;
			}
			set
			{
				this.mouseOnRightTop = value;
				if (value)
				{
					this.mouseOnLeftTop = false;
					this.mouseOnLeftBottom = false;
					this.mouseOnRightBottom = false;
				}
			}
		}
		public bool MouseOnRightBottom
		{
			get
			{
				return this.mouseOnRightBottom;
			}
			set
			{
				this.mouseOnRightBottom = value;
				if (value)
				{
					this.mouseOnLeftTop = false;
					this.mouseOnLeftBottom = false;
					this.mouseOnRightTop = false;
				}
			}
		}
		public CanvasCustom()
		{
			this.leftTop = new Rect(0.0 - this.size, 0.0 - this.size, this.size, this.size);
			this.rightTop = new Rect(base.Width - this.size, 0.0 - this.size, this.size, this.size);
			this.leftBottom = new Rect(0.0 - this.size, base.Height - this.size, this.size, this.size);
			this.rightBottom = new Rect(base.Width - this.size, base.Height - this.size, this.size, this.size);
		}
		protected override void OnMouseDown(MouseButtonEventArgs e)
		{
			base.OnMouseDown(e);
			if (e.LeftButton == MouseButtonState.Pressed)
			{
				this.p.X = e.GetPosition(null).X;
				this.p.Y = e.GetPosition(null).Y;
				this.whetherSelected = true;
				base.Cursor = Cursors.Hand;
			}
		}
		protected override void OnMouseMove(MouseEventArgs e)
		{
			try
			{
				bool flag = 1 == 0;
				base.OnMouseMove(e);
				if (e.LeftButton == MouseButtonState.Pressed)
				{
					if (this.whetherSelected)
					{
						double top = base.Margin.Top + e.GetPosition(null).Y - this.p.Y;
						double left = base.Margin.Left + (e.GetPosition(null).X - this.p.X);
						if (left < 0.0)
						{
							left = 0.0;
						}
						else
						{
							if (left + base.Width > base.MaxWidth)
							{
								left = base.MaxWidth - base.Width;
							}
						}
						if (top < 0.0)
						{
							top = 0.0;
						}
						else
						{
							if (top + base.Height > base.MaxHeight)
							{
								top = base.MaxHeight - base.Height;
							}
						}
						if (left < this.MoveArea.Left)
						{
							left = this.MoveArea.Left;
						}
						if (top < this.MoveArea.Top)
						{
							top = this.MoveArea.Top;
						}
						if (left + base.Width > this.MoveArea.Right)
						{
							left = this.MoveArea.Right - base.Width;
						}
						if (top + base.Height > this.MoveArea.Bottom)
						{
							top = this.MoveArea.Bottom - base.Height;
						}
						base.Margin = new Thickness(left, top, 0.0, 0.0);
						this.p.X = e.GetPosition(null).X;
						this.p.Y = e.GetPosition(null).Y;
						this.CacheImage(base.Margin.Left - this.MoveArea.Left, base.Margin.Top - this.MoveArea.Top, base.Width, base.Height);
					}
				}
			}
			catch (System.Exception ex)
			{
				this.logger.Error(ex.ToString());
			}
		}
		protected override void OnMouseUp(MouseButtonEventArgs e)
		{
			this.whetherSelected = false;
			base.OnMouseUp(e);
			base.Cursor = Cursors.Arrow;
			this.moveMode = true;
		}
		protected override void OnRender(DrawingContext dc)
		{
			base.OnRender(dc);
			this.rightTop.X = base.Width;
			this.leftBottom.Y = base.Height;
			this.rightBottom.X = base.Width;
			this.rightBottom.Y = base.Height;
			this.border.Width = base.Width;
			this.border.Height = base.Height;
			dc.DrawRectangle(null, this.borderPen, this.border);
			dc.DrawRectangle(null, this.rectPen, this.leftTop);
			dc.DrawRectangle(null, this.rectPen, this.rightTop);
			dc.DrawRectangle(null, this.rectPen, this.leftBottom);
			dc.DrawRectangle(null, this.rectPen, this.rightBottom);
		}
	}
}
