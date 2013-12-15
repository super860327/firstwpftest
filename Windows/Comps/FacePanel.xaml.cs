using IDKin.IM.ImageService;
using IDKin.IM.Util;
using IDKin.IM.Windows.Util;
using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
namespace IDKin.IM.Windows.Comps
{
	[GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
	public partial class FacePanel : Popup, System.IDisposable//, IComponentConnector
	{
		private delegate void SelectFace(object sender, System.EventArgs e);
		private AnimatedImage[] animateFaces = null;
		private int num = 66;
		public bool first = true;
		private ChatComponent chatComp = null;
		private IImageService imageService = ServiceUtil.Instance.ImageService;
		private string[] tooltip = new string[]
		{
			"微笑",
			"调皮",
			"惊喜",
			"呲牙",
			"偷笑",
			"胜利",
			"哈哈",
			"思考",
			"奋斗",
			"惊讶",
			"坏笑",
			"阴险",
			"感动",
			"撇嘴",
			"受伤",
			"委屈",
			"快哭了",
			"大哭",
			"流泪",
			"可爱",
			"害羞",
			"色",
			"亲亲",
			"飞吻",
			"尴尬",
			"勾引",
			"我爱你",
			"抠鼻",
			"抓狂",
			"生气",
			"咒骂",
			"发火",
			"再见",
			"加油",
			"棒",
			"OK",
			"NO",
			"差劲",
			"弱",
			"闭嘴",
			"疑问",
			"喇叭",
			"吃饭",
			"困",
			"睡",
			"嘘",
			"鄙视",
			"冷汗",
			"吓",
			"白眼",
			"傲慢",
			"呆",
			"晕",
			"吐",
			"饿",
			"酷",
			"投降",
			"刺杀",
			"菜刀",
			"敲打",
			"炸弹",
			"衰",
			"惊恐",
			"骷髏",
			"金子",
			"贊"
		};
		//internal IDKinWrapPanel2 wrapPanel;
        ////private bool _contentLoaded;
		public FacePanel(ChatComponent chatComp)
		{
			this.InitializeComponent();
			this.chatComp = chatComp;
		}
		private void Image_MouseDown(object sender, System.EventArgs e)
		{
			Image image = sender as Image;
			if (image != null)
			{
				Image newImage = new Image();
				newImage.Source = image.Source;
				newImage.DataContext = image.DataContext;
				this.chatComp.InsertImage(newImage);
				base.IsOpen = false;
			}
		}
		private void Popup_Opened(object sender, System.EventArgs e)
		{
			if (this.animateFaces == null && this.first)
			{
				for (int i = 0; i < this.num; i++)
				{
					AnimatedImage animatedImage = this.imageService.GetAnimatebyIDImage(i);
					if (animatedImage != null)
					{
						animatedImage.MouseDown += new MouseButtonEventHandler(this.AnimatedImage_MouseDown);
						if (this.tooltip.Length > i)
						{
							animatedImage.ToolTip = this.tooltip[i];
						}
						animatedImage.Stretch = Stretch.None;
						animatedImage.DataContext = i;
						this.wrapPanel.Children.Add(animatedImage);
					}
				}
				this.first = false;
			}
		}
		public void AnimatedImage_MouseDown(object sender, MouseButtonEventArgs e)
		{
			try
			{
				AnimatedImage image = sender as AnimatedImage;
				if (image != null)
				{
					int i = int.Parse(image.DataContext.ToString());
					AnimatedImage newimage = this.imageService.GetAnimatebyIDImage(i);
					if (newimage != null)
					{
						newimage.DataContext = image.DataContext;
						UIControlUtil.Instance.AnimateImageProcessor(newimage);
						this.chatComp.InsertAnimatedImage(newimage);
						base.IsOpen = false;
					}
				}
			}
			catch (System.Exception)
			{
			}
		}
		public void Dispose()
		{
			this.imageService.RemoveFaces();
			this.imageService.RemoveAnimatedFaces();
		}
        //[System.Diagnostics.DebuggerNonUserCode]
        //public void InitializeComponent()
        //{
        //    if (!this._contentLoaded)
        //    {
        //        this._contentLoaded = true;
        //        Uri resourceLocater = new Uri("/IDKin.IM.Windows;component/comps/facepanel.xaml", UriKind.Relative);
        //        Application.LoadComponent(this, resourceLocater);
        //    }
        //}
        //[System.Diagnostics.DebuggerNonUserCode]
        ////internal System.Delegate _CreateDelegate(System.Type delegateType, string handler)
        //{
        //    return System.Delegate.CreateDelegate(delegateType, this, handler);
        //}
        //[EditorBrowsable(EditorBrowsableState.Never), System.Diagnostics.DebuggerNonUserCode]
        //void IComponentConnector.Connect(int connectionId, object target)
        //{
        //    switch (connectionId)
        //    {
        //    case 1:
        //        ((FacePanel)target).Opened += new System.EventHandler(this.Popup_Opened);
        //        break;
        //    case 2:
        //        this.wrapPanel = (IDKinWrapPanel2)target;
        //        break;
        //    default:
        //        this._contentLoaded = true;
        //        break;
        //    }
        //}
	}
}
