using ClassLibrary_DTOs.PasswordManager;
using MauiAppAdmin.Services;
using System.Collections.ObjectModel;

namespace MauiAppAdmin.Pages;

public partial class CorePage : ContentPage
{
    private readonly ApiCoreService _apiCoreService;
    public ObservableCollection<CoreDTO> CoreData { get; set; } = new();
    private ObservableCollection<CoreDTO> FilteredCoreData { get; set; } = new();

    public CorePage(ApiCoreService apiCoreService)
	{
		InitializeComponent();

        BindingContext = this;
        _apiCoreService = apiCoreService;
    }

    private async void OnGetAll(object sender, EventArgs e)
	{
        loading.IsVisible = true;
        var frame = (Frame)sender;
        await DisableControls(frame);

        await OnDownloadData();

        loading.IsVisible = false;
        EnableControls();
    }

    private async void OnDecryptData(object sender, EventArgs e)
    {
        var frame = (Frame)sender;
        await DisableControls(frame);

        if (CoreData.Count == 0)
        {
            await DisplayAlert("Error", "La Lista esta Vacía.", "Ok");
            EnableControls();
            return;
        }

        for (int i = 0; i < CoreData.Count; i++)
        {
            if (!IsBase64String(CoreData[i].Data01))
            {
                await DisplayAlert("Error", "La Lista ya está Descifrado.", "Ok");
                EnableControls();
                return;
            }
        }

        var passwordPromp = new CorePromptPage();
        await Navigation.PushModalAsync(passwordPromp);
        var password = await passwordPromp.GetPasswordAsync();

        if (string.IsNullOrWhiteSpace(password))
        {
            EnableControls();
            return;
        }

        loading.IsVisible = true;
        FilteredCoreData.Clear();

        var result = await _apiCoreService.Login(password);

        if (!result.Success)
        {
            await DisplayAlert($"{result.StatusCode}", $"{result.Message}", "Ok");
            EnableControls();
            return;
        }

        if (!string.IsNullOrEmpty(result.Data?.IV))
        {
            EncryptionService encryptionService = new EncryptionService();

            for (int i = 0; i < CoreData.Count; i++)
            {
                CoreData[i] = encryptionService.DecryptData(CoreData[i], password, result.Data.IV);
            }

            foreach (var coreData in CoreData.OrderBy(e => e.Data01))
            {
                FilteredCoreData.Add(coreData);
            }
        }

        EnableControls();
    }

    private async void OnCreate(object sender, EventArgs e)
	{
        var frame = (Frame)sender;
        await DisableControls(frame);

        var page = new CoreFormPage(null);
        await Navigation.PushAsync(page);
        var (coreDTO, password) = await page.GetCompletionTask();

        loading.IsVisible = true;

        if (coreDTO != null && !string.IsNullOrWhiteSpace(password))
        {
            await OnDownloadData();
        }

        EnableControls();
    }

    private async Task OnDownloadData()
    {
        SecretsCollectionView.ItemsSource = null;
        CoreData.Clear();
        FilteredCoreData.Clear();

        var resultApi = await _apiCoreService.GetAll();

        foreach (var coreData in resultApi.Data)
        {
            CoreData.Add(coreData);
            FilteredCoreData.Add(coreData);
        }

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

    private async Task DisableControls(Frame frame)
    {
        BtnGetAll.IsEnabled = false;
        BtnDecrypt.IsEnabled = false;
        BtnCreate.IsEnabled = false;
        TxtSearch.IsEnabled = false;

        await frame.ScaleTo(0.90, 100);
        await frame.ScaleTo(1, 100);
    }

    private void EnableControls()
    {
        BtnGetAll.IsEnabled = true;
        BtnDecrypt.IsEnabled = true;
        BtnCreate.IsEnabled = true;
        TxtSearch.IsEnabled = true;

        loading.IsVisible = false;
    }

    private bool IsBase64String(string base64String)
    {
        if (string.IsNullOrEmpty(base64String))
        {
            return false;
        }

        Span<byte> buffer = new Span<byte>(new byte[base64String.Length]);
        return Convert.TryFromBase64String(base64String, buffer, out _);
    }
}