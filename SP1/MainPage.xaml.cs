
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

//add this

using Q42.WinRT.Data;
using System.Diagnostics;





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
            //moved
            //moved

        }


        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            await configRequest();
            await discoverRequest();

        }


        public configRootObject conf = new configRootObject();

        public async Task configRequest()
        {
            var client = new HttpClient();
            try {
                
                var api_url = new Uri("https://api.themoviedb.org/3/configuration?api_key=783af1800cdfe954a484c64b70f4d0aa", UriKind.Absolute);
                var res = await DataCache.GetAsync("config_cache",
                    async () =>
                    {
                        var tmp_res = await client.GetStringAsync(api_url);
                        configRootObject tmp_conf = JsonConvert.DeserializeObject<configRootObject>(tmp_res);
                        return tmp_conf;
                    },
                    expireDate: DateTime.Now.AddDays(2)
                    );
                conf = res;
            }

            catch
            {
                //write what to do when there's no cache and internet here
                
            }         
            
        }

        


        public async Task discoverRequest()
        {
            var client = new HttpClient();
            
            try
            {
                discoverRootObject discoverRes = new discoverRootObject();
                var api_url = new Uri("https://api.themoviedb.org/3/discover/movie?sort_by=popularity.desc&api_key=783af1800cdfe954a484c64b70f4d0aa", UriKind.Absolute);
           
                var res = await DataCache.GetAsync("discover_cache_file_name_here",
                    async () =>
                    {
                        var tmp_res = await client.GetStringAsync(api_url);
                        discoverRootObject dis_res_tmp = JsonConvert.DeserializeObject<discoverRootObject>(tmp_res);
                        return dis_res_tmp;
                    });
                discoverRes = res;
                foreach (discoverResult movie in discoverRes.results)
                {
                    movie.poster_full_url = new Uri(conf.images.base_url + conf.images.poster_sizes[1] + movie.poster_path);
                }


                MoviesGridView.ItemsSource = discoverRes.results;

            }
            catch
            {
                //decide what to do here
            }          
        }

    }
}
