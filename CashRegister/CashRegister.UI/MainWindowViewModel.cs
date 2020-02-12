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

        /// <summary>
        /// The list of items in the basket.
        /// </summary>
        private ObservableCollection<ReceiptLineViewModel> receiptLines = new ObservableCollection<ReceiptLineViewModel>();
        public ObservableCollection<ReceiptLineViewModel> ReceiptLines
        {
            get { return receiptLines; }
            set { SetProperty(ref receiptLines, value); }
        }

        public decimal TotalSum => ReceiptLines.Sum(item => item.TotalPrice);

        //
        // Commands
        //
        public DelegateCommand<int?> AddToBasketCommand { get; }
        public DelegateCommand CheckoutCommand { get; }

        public MainWindowViewModel()
        {
            AddToBasketCommand = new DelegateCommand<int?>(OnAddToBasket);
            CheckoutCommand = new DelegateCommand(async () => await OnCheckout(), () => ReceiptLines.Count > 0);

            ReceiptLines.CollectionChanged += (_, __) =>
            {
                CheckoutCommand.RaiseCanExecuteChanged();
                RaisePropertyChanged(nameof(TotalSum));
            };
        }

        public void OnAddToBasket(int? productId)
        {
            var product = products.FirstOrDefault(p => p.ID == productId);
            if (product == default)
            {
                return;
            }

            var basketItem = receiptLines.FirstOrDefault(b => b.ProductID == productId);
            if (basketItem == default)
            {
                // Item hast not been added before
                receiptLines.Add(new ReceiptLineViewModel
                {
                    ProductID = product.ID,
                    Name = product.Name,
                    TotalPrice = product.UnitPrice,
                    Amount = 1
                });
            }
            else
            {
                // Item has already been added to the list
                basketItem.Amount += 1;
                basketItem.TotalPrice += product.UnitPrice;

                RaisePropertyChanged(nameof(TotalSum));
            }
        }

        public async Task OnCheckout()
        {
            var dto = ReceiptLines.Select(b => new ReceiptLineDto
            {
                ProductId = b.ProductID,
                Amount = b.Amount
            }).ToList();

            using (var content = new StringContent(JsonSerializer.Serialize(dto), Encoding.UTF8, "application/json"))
            {
                var response = await RetryPolicy.ExecuteAndCaptureAsync(async () => await _client.PostAsync("/api/receipts", content));

                response.Result.EnsureSuccessStatusCode();
            }

            ReceiptLines.Clear();
        }

        public async Task LoadProductsAsync()
        {
            // Use the retry policy
            var products = await RetryPolicy.ExecuteAndCaptureAsync(async () => await _client.GetStringAsync("/api/products"));

            // Set the products
            Products = JsonSerializer.Deserialize<ObservableCollection<Product>>(products.Result, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });
        }
    }
}
