using MangoDevoluciones.Models;
using MangoDevoluciones.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using ZXing;
using ZXing.Mobile;
using ZXing.Net.Mobile.Forms;

namespace MangoDevoluciones.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class DevolucionesPage : ContentPage
    {
        public ObservableCollection<ItemPrenda> Prendas { get; set; }
        public GridLength HeightRow2 { get; set; }
        public GridLength HeightRow4 { get; set; }
        public bool HasBarcodePedidoScanned { get; set; }


        public ZXing.Net.Mobile.Forms.ZXingScannerView scanner = new ZXing.Net.Mobile.Forms.ZXingScannerView();

        public List<string> UrlImages = new List<string> {
            "https://st.mngbcn.com/rcs/pics/static/T5/fotos/S20/57055943_99.jpg",
            "https://st.mngbcn.com/rcs/pics/static/T5/fotos/S20/53073709_06_B.jpg",
            "https://st.mngbcn.com/rcs/pics/static/T5/fotos/S20/51083028_56_B.jpg",
            "https://st.mngbcn.com/rcs/pics/static/T5/fotos/S20/51083028_70_B.jpg",
            "https://st.mngbcn.com/rcs/pics/static/T5/fotos/S20/51053704_92_B.jpg",
            "https://st.mngbcn.com/rcs/pics/static/T5/fotos/S20/53025747_99_B.jpg",
            "https://st.mngbcn.com/rcs/pics/static/T5/fotos/S20/57075943_43_B.jpg",
            "https://st.mngbcn.com/rcs/pics/static/T5/fotos/S20/57064379_99_B.jpg",
            "https://st.mngbcn.com/rcs/pics/static/T5/fotos/S20/57089202_99_B.jpg",
            "https://st.mngbcn.com/rcs/pics/static/T5/fotos/S20/51065026_95_B.jpg",
            "https://st.mngbcn.com/rcs/pics/static/T5/fotos/S20/51090908_76_B.jpg",
            "https://st.mngbcn.com/rcs/pics/static/T5/fotos/S20/57094380_OR_B.jpg",
            "https://st.mngbcn.com/rcs/pics/static/T5/fotos/S20/51060897_TM_B.jpg",
            "https://st.mngbcn.com/rcs/pics/static/T5/fotos/S20/53007024_50_B.jpg",
            "https://st.mngbcn.com/rcs/pics/static/T5/fotos/S20/57085930_65_B.jpg",
            "https://st.mngbcn.com/rcs/pics/static/T5/fotos/S20/57006710_85_B.jpg",
            "https://st.mngbcn.com/rcs/pics/static/T5/fotos/S20/53015744_14_B.jpg",
            "https://st.mngbcn.com/rcs/pics/static/T5/fotos/S20/57065926_37_B.jpg",
            "https://st.mngbcn.com/rcs/pics/static/T5/fotos/S20/53035035_02_B.jpg",
            "https://st.mngbcn.com/rcs/pics/static/T5/fotos/S20/57067701_30_B.jpg",
            "https://st.mngbcn.com/rcs/pics/static/T5/fotos/S20/57035933_70_B.jpg",
            "https://st.mngbcn.com/rcs/pics/static/T5/fotos/S20/57004381_99_B.jpg",
            "https://st.mngbcn.com/rcs/pics/static/T5/fotos/S20/51073785_09_B.jpg",
            "https://st.mngbcn.com/rcs/pics/static/T5/fotos/S20/51067657_99_B.jpg",
            "https://st.mngbcn.com/rcs/pics/static/T5/fotos/S20/53097655_05_B.jpg"
        };

        public List<Pedido> Pedidos = new List<Pedido>
        {
            new Pedido{ OrderNumber = "46637-691", OrderDate = new DateTime(2019, 09, 04), CustomerName = "Melany Brown", CustomerAddress = "142500 West Smith Street Orlando, FL 32804 USA" },
            new Pedido{ OrderNumber = "70028-773", OrderDate = new DateTime(2019, 09, 11), CustomerName = "Nicholas Williams", CustomerAddress = "3078 Columbia Road 30 Magnolia, AR 71753 USA" },
            new Pedido{ OrderNumber = "13742-633", OrderDate = new DateTime(2019, 09, 18), CustomerName = "Owen Jackson", CustomerAddress = "28958 1500 East Street Walnut, IL 61376 USA" },
            new Pedido{ OrderNumber = "32523-492", OrderDate = new DateTime(2019, 09, 25), CustomerName = "Jason Clark", CustomerAddress = "2445 D Avenue Anacortes, WA 98221, USA" }
        };

        public DevolucionesPage()
        {
            InitializeComponent();
            Prendas = new ObservableCollection<ItemPrenda>();
            var rows = this.gridPrincipal.RowDefinitions;
            HeightRow2 = rows[2].Height;
            HeightRow4 = rows[4].Height;
            rows[2].Height = new GridLength(0, GridUnitType.Star);
            rows[4].Height = new GridLength(0, GridUnitType.Star);
        }

        private void HandleQrPedido_Tapped(object sender, EventArgs e)
        {
            ScanQrCode_Pedido();
        }

        private void HandleQrItem_Tapped(object sender, EventArgs e)
        {
            ScanQrCode_Prenda();
        }

        private async void HandleIconTrash_Tapped(object sender, EventArgs e)
        {
            var response = await DisplayAlert("Warning", "Desea cancelar el pedido?", "Ok", "Cancel");
            if (response)
            {
                CancelPedido();
            }
        }

        private void HandleBtnFinalizarPedido_Clicked(object sender, EventArgs e)
        {
            CancelPedido();
        }

        private void CancelPedido()
        {
            VisibilityStackLayoutResultPedido(false);
            VisibilityStackLayoutAddPrenda(false);
            VisibilityStackLayoutQrPedido(true);
            VisibilityStackLayoutQrPrenda(false);
            this.HasBarcodePedidoScanned = false;
            Prendas = new ObservableCollection<ItemPrenda>();
            RecalculateHeightCollectionViewPrendas();
            GridRowOneMaximumHeight();

            var rows = this.gridPrincipal.RowDefinitions;
            rows[2].Height = new GridLength(0, GridUnitType.Star);
            rows[4].Height = new GridLength(0, GridUnitType.Star);
        }

        private void HandleBtnAddItemPrenda(object sender, EventArgs e)
        {
            ScanQrCode_Prenda();
        }

        private void HandleLabelRemovePrenda_Tapped(object sender, EventArgs e)
        {
            Label label = (Label)sender;
            TapGestureRecognizer gesture = (TapGestureRecognizer)label.GestureRecognizers[0];
            ItemPrenda item = (ItemPrenda)gesture.CommandParameter;
            RemovePrenda(item);
        }

        private void VisibilityStackLayoutResultPedido(bool visible)
        {
            this.slIconTrash.IsVisible = visible;
            this.slResultPedido.IsVisible = visible;
        }

        private void VisibilityStackLayoutQrPedido(bool visible)
        {
            this.slQrPedido.IsVisible = visible;
        }

        private void VisibilityStackLayoutQrPrenda(bool visible)
        {
            this.slQrPrenda.IsVisible = visible;
            this.lblPrenda.IsVisible = visible;
        }

        private void VisibilityStackLayoutAddPrenda(bool visible)
        {
            this.slBtnAddItem.IsVisible = visible;
            this.slPrendas.IsVisible = visible;
            this.slFinalizarDevolucion.IsVisible = visible;
        }

        private async void ScanQrCode_Pedido()
        {
            try
            {
                var scanPage = new ZXingScannerPage();
                ((NavigationPage)Application.Current.MainPage).BarBackgroundColor = Color.Black;

                scanPage.OnScanResult += (result) => {
                    // Stop scanning
                    scanPage.IsScanning = false;

                    // Pop the page and show the result
                    Device.BeginInvokeOnMainThread(() => {
                        Navigation.PopAsync();
                        VisibilityStackLayoutResultPedido(true);
                        VisibilityStackLayoutQrPedido(false);
                        VisibilityStackLayoutQrPrenda(true);
                        GridRowOneMinimumHeight();
                        GeneratePedido(GetRandomPedido());
                        this.HasBarcodePedidoScanned = true;
                    });
                };

                // Navigate to our scanner page
                await Navigation.PushAsync(scanPage);
            }
            catch (Exception)
            {
                throw;
            }
        }

        private async void ScanQrCode_Prenda()
        {
            try
            {
                var scanPage = new ZXingScannerPage();
                ((NavigationPage)Application.Current.MainPage).BarBackgroundColor = Color.Black;

                scanPage.OnScanResult += (result) => {
                    // Stop scanning
                    scanPage.IsScanning = false;

                    // Pop the page and show the result
                    Device.BeginInvokeOnMainThread(() => {
                        Navigation.PopAsync();
                        VisibilityStackLayoutAddPrenda(true);
                        VisibilityStackLayoutQrPrenda(false);
                        AddItemPrendas(result.Text);
                    });
                };

                // Navigate to our scanner page
                await Navigation.PushAsync(scanPage);
            }
            catch (Exception)
            {
                throw;
            }
        }

        private void AddItemPrendas(string item)
        {
            Prendas.Add(new ItemPrenda { Reference = item, UrlImage = GetRandomUrlImage() });
            RecalculateHeightCollectionViewPrendas();
            GridRowsMinimumHeight();
        }

        private void GridRowsMinimumHeight()
        {
            var rows = this.gridPrincipal.RowDefinitions;
            rows[2].Height = HeightRow2;
            rows[4].Height = HeightRow4;
        }

        private void GridRowOneMinimumHeight()
        {
            var rows = this.gridPrincipal.RowDefinitions;
            rows[0].Height = new GridLength(10, GridUnitType.Absolute);
        }

        private void GridRowOneMaximumHeight()
        {
            var rows = this.gridPrincipal.RowDefinitions;
            rows[0].Height = new GridLength(200, GridUnitType.Absolute);
        }

        private void RecalculateHeightCollectionViewPrendas()
        {
            this.cvPrendas.ItemsSource = Prendas;
            this.cvPrendas.HeightRequest = 250 * Prendas.Count();
        }

        private void RemovePrenda(ItemPrenda item)
        {
            Prendas.Remove(item);
            if (Prendas.Count() == 0)
            {
                VisibilityStackLayoutAddPrenda(false);
                VisibilityStackLayoutQrPrenda(true);
            }
            RecalculateHeightCollectionViewPrendas();
        }

        private void GeneratePedido(Pedido pedido)
        {
            this.lblOrderNumber.Text = pedido.OrderNumber;
            this.lblOrderDate.Text = pedido.OrderDate.ToString("dd/MM/yyyy");
            this.lblCustomerName.Text = pedido.CustomerName;
            this.lblCustomerAddress.Text = pedido.CustomerAddress;
        }


        private string GetRandomUrlImage()
        {
            Random random = new Random();
            int index = random.Next(0, UrlImages.Count());
            return UrlImages.ElementAt(index);
        }

        private Pedido GetRandomPedido()
        {
            Random random = new Random();
            int index = random.Next(0, Pedidos.Count());
            return Pedidos.ElementAt(index);
        }


    }
}