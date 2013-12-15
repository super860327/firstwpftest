using IDKin.IM.Data;
using IDKin.IM.Windows.Util;
using IDKin.IM.Windows.View;
using IN.Helper;
using System;
using System.CodeDom.Compiler;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Windows;
namespace IN
{
    public partial class App : Application
    {
        [Import]
        private IDataService dataService;        
        private KeyboardHook hook;
        private CompositionContainer container;
        
        [Import("IDKin.IM.Windows.View.SystemWindow")]        
        public new Window MainWindow
        {
            get
            {
                return base.MainWindow;
            }
            set
            {
                base.MainWindow = value;
            }
        }

        public App()
        {
            this.hook = new KeyboardHook();
            this.hook.KeyDown += new KeyboardHook.HookEventHandler(this.OnHookKeyDown);
        }
        private void OnHookKeyDown(object sender, HookEventArgs e)
        {
            if (this.dataService.SystemWindow != null)
            {
                SystemWindow sysWindow = this.dataService.SystemWindow as SystemWindow;
                sysWindow.OnHookKeyDownHandler(sender, e);
            }
        }
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            if (this.Compose())
            {
                this.MainWindow.Show();
                this.dataService.SystemWindow = this.MainWindow;
                return;
            }
            base.Shutdown();
        }
        protected override void OnExit(ExitEventArgs e)
        {
            base.OnExit(e);
            if (this.container != null)
            {
                this.container.Dispose();
                this.container = null;
            }
        }
        private bool Compose()
        {
            this.container = new CompositionContainer(new AggregateCatalog
            {
                Catalogs = 
				{
					new AssemblyCatalog(Assembly.GetExecutingAssembly()),
					new DirectoryCatalog(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location))
				}
            }, new ExportProvider[0]);
            CompositionBatch batch = new CompositionBatch();
            batch.AddPart(this);
            try
            {
                this.container.Compose(batch);
            }
            catch (CompositionException ex)
            {
                MessageBox.Show(ex.ToString());
                base.Shutdown(1);
                return false;
            }
            return true;
        }

        [DebuggerNonUserCode, STAThread]
        public static void Main()
        {
            App app = new App();
            app.InitializeComponent();
            app.Run();
        }
    }
}
