using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace IoTBackUp
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
            Display();
        }

        SparkFun.SparkFunWeatherSheild sheild = new SparkFun.SparkFunWeatherSheild();
        private async void Display()
        {
            if (await sheild.Setup())
            {
                Temperature.Text = sheild.Temperature.ToString();
                Humdity.Text = sheild.Humidity.ToString();
                PostToAzure();
            }
        }

        private async void PostToAzure()
        {
            var formContent = new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string,string>("DeviceID", Guid.NewGuid().ToString()),
                new KeyValuePair<string,string>("Humidity", sheild.Humidity.ToString()),
                new KeyValuePair<string,string>("Temperature", sheild.Temperature.ToString())
            });

            var client = new HttpClient();
            var response = await client.PostAsync("http://YourWeatherAPI.azurewebsites.net/api/weather", formContent);
        }

    }
}
