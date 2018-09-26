using System.Net;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using Microsoft.AspNetCore.SignalR.Client;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace UWP
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        private readonly HubConnection hub;

        public MainPage()
        {
            this.InitializeComponent();

            ServicePointManager.ServerCertificateValidationCallback +=
                (sender, certificate, chain, sslPolicyErrors) => true;

            hub = new HubConnectionBuilder()
                .WithUrl("http://localhost:49791/hubs/status")
                .Build();

            this.DataContext = new MainViewModel(hub);
        }

        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            await hub.StartAsync();
            base.OnNavigatedTo(e);            
        }
    }
}
