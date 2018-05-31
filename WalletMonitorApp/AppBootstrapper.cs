using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Windows;
using Caliburn.Micro;
using Ninject;
using Ninject.Planning.Bindings.Resolvers;
using WalletMonitorApp.Log;
using WalletMonitorApp.ViewModels;
using WalletMonitorServices;

namespace WalletMonitorApp
{
    public class AppBootstrapper : BootstrapperBase
    {
        private Boolean _isInitialized;         
        private Logger logger = new Logger(typeof(AppBootstrapper));

        public IKernel Kernel
        {
            get
            {
                return App.Kernel;
            }
            set
            {
                App.Kernel = value;
            }
        }
        public AppBootstrapper()
        {            
            Initialize();
        }

        protected override void Configure()
        {
            try
            {
                Kernel = new StandardKernel();
                Kernel.Components.RemoveAll<IMissingBindingResolver>();
                Kernel.Components.Add<IMissingBindingResolver, DefaultValueBindingResolver>();

                var httpClient = new HttpClient();
                httpClient.DefaultRequestHeaders.Add("User-Agent", "CryptoCore Explorer exchange statistics");
                httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("text/html"));
                httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/xhtml+xml"));

                Kernel.Bind<HttpClient>().ToConstant(httpClient).InSingletonScope();
                Kernel.Bind<PoolingService>().ToSelf().InSingletonScope();
                Kernel.Bind<LoginService>().ToSelf().InSingletonScope();
                Kernel.Bind<WalletService>().ToSelf().InSingletonScope();
                Kernel.Bind<MainViewModel>().ToSelf().InSingletonScope();
                Kernel.Bind<AddAddressViewModel>().ToSelf().InSingletonScope();
                Kernel.Bind<DonateViewModel>().ToSelf().InSingletonScope();
                Kernel.Bind<IWindowManager>().To<WindowManager>().InSingletonScope();
                Kernel.Bind<IEventAggregator>().To<EventAggregator>().InSingletonScope();
                _isInitialized = true;

            }
            catch (Exception ex)
            {
                logger.LogException(ex);
                MessageBox.Show(ex.Message);                
                Environment.Exit(0);
            }
            
        }


        protected override void OnStartup(object sender, StartupEventArgs e)
        {
            if (_isInitialized)
            {
                DisplayRootViewFor<MainViewModel>();
            }
        }
        protected override void OnExit(object sender, EventArgs e)
        {
            logger.LogInfoMessage("Application exit");
            Kernel.Dispose();
            base.OnExit(sender, e);
            Environment.Exit(0);
        }

        protected override object GetInstance(Type service, string key)
        {
            if (service == null)
                throw new ArgumentNullException("service");

            return Kernel.Get(service);
        }

        protected override IEnumerable<object> GetAllInstances(Type service)
        {
            return Kernel.GetAll(service);
        }

        protected override void BuildUp(object instance)
        {
            Kernel.Inject(instance);
        }
    }
}
