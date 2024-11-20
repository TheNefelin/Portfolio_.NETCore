using ClassLibrary_DTOs.PasswordManager;
using MauiAppAdmin.Services;
using System.Collections.ObjectModel;

namespace MauiAppAdmin.Pages;

public partial class CorePage : ContentPage
{
    private readonly ApiCoreService _apiCoreService;
    public ObservableCollection<CoreDTO> CoreData { get; set; } = new();
    private ObservableCollection<CoreDTO> FilteredCoreData { get; set; }

    public CorePage(ApiCoreService apiCoreService)
	{
		InitializeComponent();

        BindingContext = this;
        _apiCoreService = apiCoreService;
    }

    private async void OnGetAll(object sender, EventArgs e)
	{
        var frame = (Frame)sender;
        loading.IsVisible = true;
        frame.IsEnabled = false;

        await ButtonAnimation(frame);
        await OnDownloadData();

        loading.IsVisible = false;
        frame.IsEnabled = true;
    }

    private async void OnDecryptData(object sender, EventArgs e)
    {
        bool status = true;

        var frame = (Frame)sender;
        loading.IsVisible = true;
        frame.IsEnabled = false;

        status = true;
        if (CoreData.Count == 0)
        {
            await DisplayAlert("Error", "La Lista esta Vacía.", "Ok");
            status = false;
        }

        for (int i = 0; i < CoreData.Count; i++)
        {
            if (!IsBase64String(CoreData[i].Data01))
            {
                await DisplayAlert("Error", "La Lista ya está Descifrado.", "Ok");
                status = false;
            }
        }

        if (status)
        {
            var passwordPromp = new CorePromptPage();
            await Navigation.PushModalAsync(passwordPromp);
            var password = await passwordPromp.GetPasswordAsync();
            
            await DisplayAlert("#", password, "ok");
        }

        loading.IsVisible = false;
        frame.IsEnabled = true;
    }

    private async void OnCreate(object sender, EventArgs e)
	{
        var frame = (Frame)sender;
        loading.IsVisible = true;
        frame.IsEnabled = false;

        loading.IsVisible = false;
        frame.IsEnabled = true;
    }

    private async Task OnDownloadData()
    {
        SecretsCollectionView.ItemsSource = null;
        CoreData.Clear();

        var resultApi = await _apiCoreService.GetAll();
        var sortData = resultApi.Data.OrderBy(dt => dt.Data01).ToList();

        foreach (var data in sortData)
        {
            CoreData.Add(data);
        }

        FilteredCoreData = new ObservableCollection<CoreDTO>(CoreData);

        SecretsCollectionView.ItemsSource = FilteredCoreData;
    }

    private void OnSearchTextChanged(object sender, TextChangedEventArgs e)
    {
        var searchText = e.NewTextValue;

        FilteredCoreData.Clear();

        if (string.IsNullOrEmpty(searchText))
        {
            foreach (var item in CoreData)
            {
                FilteredCoreData.Add(item); // Restauramos todos los elementos.
            }
        }
        else
        {
            var filteredList = CoreData.Where(item => item.Data01.Contains(searchText, StringComparison.OrdinalIgnoreCase));

            foreach (var item in filteredList)
            {
                FilteredCoreData.Add(item);
            }
        }
    }

    public bool IsBase64String(string base64String)
    {
        if (string.IsNullOrEmpty(base64String))
        {
            return false;
        }

        Span<byte> buffer = new Span<byte>(new byte[base64String.Length]);
        return Convert.TryFromBase64String(base64String, buffer, out _);
    }

    private async Task ButtonAnimation(Frame frame)
    {
        await frame.ScaleTo(0.90, 100);
        await frame.ScaleTo(1, 100);
    }
}