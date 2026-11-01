using MauiApp1.ViewModels;
namespace MauiApp1
{
    public partial class MainPage : ContentPage
    {
        private readonly MainViewModel _vm = new();
        private bool _initialized;
        public MainPage()
        {
            InitializeComponent();
            BindingContext = _vm;
        }
        protected override async void OnAppearing()
        { base.OnAppearing();
            if (_initialized) return;
            _initialized = true;
            var xslPath = await CopyAssetAsync("dormitory.xsl");
            _vm.SetXslPath(xslPath);
        }
        private static async Task<string> CopyAssetAsync(string name)
        {
            var target = Path.Combine(FileSystem.AppDataDirectory, name);
            using var source = await FileSystem.OpenAppPackageFileAsync(name);
            using var destination = File.Create(target);
            await source.CopyToAsync(destination);
            return target;
        }
        private async void OnOpenFile(object? sender, EventArgs e)
        {
            var xmlType = new FilePickerFileType(new Dictionary<DevicePlatform, IEnumerable<string>>
            {
                { DevicePlatform.WinUI, new[] { ".xml" } },
                { DevicePlatform.MacCatalyst, new[] { "public.xml" } },
                { DevicePlatform.Android, new[] { "text/xml" } },
                { DevicePlatform.iOS, new[] { "public.xml" } }
            });
            var file = await FilePicker.Default.PickAsync(new PickOptions
            {
                PickerTitle = "Select an XML file",
                FileTypes = xmlType
            });

            if (file != null) _vm.LoadFile(file.FullPath);

        }
        private async void OnLoadXML(object? sender, EventArgs e)
        {
            var path = await CopyAssetAsync("dormitory.xml");
            _vm.LoadFile(path);
        }
        private void OnSearch(object? sender, EventArgs e)
        {
            _vm.Search();
        }
        private void OnTransform(object? sender, EventArgs e)
        {
            _vm.TransformToHtml();
            if (_vm.IsHtmlVisible)
                htmlView.Source = new HtmlWebViewSource { Html = _vm.HtmlSource };
        }
        private void OnClear(object? sender, EventArgs e)
        {
            _vm.Clear();
            htmlView.Source = new HtmlWebViewSource { Html = string.Empty };
        }
        private async void OnExit(object? sender, EventArgs e) => await ConfirmExitAsync();
        private async Task ConfirmExitAsync()
        {
            bool yes = await DisplayAlert("Exit", "Are you sure you want to exit?", "Yes", "No");
            if (yes) Application.Current?.Quit();
        }
        protected override bool OnBackButtonPressed()
        {
            Dispatcher.Dispatch(async () => await ConfirmExitAsync());
            return true; // Prevent default back button behavior
        } } }
