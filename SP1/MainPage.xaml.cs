
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

using Windows.Web.Http;
using Newtonsoft.Json;
using SP1.models;
using System.Threading.Tasks;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace SP1
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
            configRequest();
            discoverRequest();

        }


        public configRootObject conf = new configRootObject();
        public async Task configRequest()
        {
            var client = new HttpClient();

            //Change YOUR_KEY to your own TMDb key
            var res = await client.GetStringAsync(new Uri("https://api.themoviedb.org/3/configuration?api_key=YOUR_KEY", UriKind.Absolute));
            conf = JsonConvert.DeserializeObject<configRootObject>(res);
            
        }

   

        public async Task discoverRequest()
        {
            var client = new HttpClient();

            //Change your key to your own TMDb key
            var res = await client.GetStringAsync(new Uri("https://api.themoviedb.org/3/discover/movie?sort_by=popularity.desc&api_key=YOUR_KEY", UriKind.Absolute));
            discoverRootObject discoverRes = JsonConvert.DeserializeObject<discoverRootObject>(res);

            
            foreach (discoverResult movie in discoverRes.results)
            {
                movie.poster_full_url = new Uri(conf.images.base_url + conf.images.poster_sizes[1] + movie.poster_path);
            }

            //Binding
            MoviesGridView.ItemsSource = discoverRes.results;
           
        }

    }
}
