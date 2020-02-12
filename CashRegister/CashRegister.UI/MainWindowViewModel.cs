using Prism.Mvvm;
using System;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Polly;
using System.Collections.ObjectModel;
using Prism.Commands;
using CashRegister.API;
using System.Text.Json;

namespace CashRegister.UI
{
    public class MainWindowViewModel : BindableBase
    {
        private readonly HttpClient _client = new HttpClient
        {
            BaseAddress = new Uri("http://localhost:5000"),
            Timeout = TimeSpan.FromSeconds(5)
        };

        private readonly AsyncPolicy RetryPolicy = Policy.Handle<HttpRequestException>().RetryAsync(5);

        /// <summary>
        /// List of products which can be bought.
        /// </summary>
        private ObservableCollection<Product> products;
        public ObservableCollection<Product> Products
        {
            get { return products; }
            set { SetProperty(ref products, value); }
        }

        public MainWindowViewModel()
        {

        }



        public async Task LoadProductsAsync()
        {
            // Use the retry policy
            var products = await RetryPolicy.ExecuteAndCaptureAsync(async () => await _client.GetStringAsync("/api/products"));

            // Set the products
            Products = JsonSerializer.Deserialize<ObservableCollection<Product>>(products.Result);
        }
    }
}
