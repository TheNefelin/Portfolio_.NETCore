using ClassLibrary_DTOs;
using ClassLibrary_DTOs.PasswordManager;
using MauiAppAdmin.Services;
using System.Collections.ObjectModel;

namespace MauiAppAdmin.Pages;

public partial class CorePage : ContentPage
{
    private readonly ApiCoreService _apiCoreService;
    public ObservableCollection<CoreDTO> CoreData { get; set; } = new();
    private ObservableCollection<CoreDTO> BaseCoreData { get; set; } = new();

    public CorePage(ApiCoreService apiCoreService)
	{
		InitializeComponent();

        BindingContext = this;
        _apiCoreService = apiCoreService;
        //CoreCollectionView.ItemsSource = CoreData;
    }

    private async void OnNewPass(object sender, EventArgs e)
    {
        this.IsEnabled = false;

        var frame = (Frame)sender;
        await ButtonAnimation(frame);

        var newPassword = new CorePassPage(_apiCoreService);
        await Navigation.PushAsync(newPassword);

        this.IsEnabled = true;
    }

    private async void OnClear(object sender, EventArgs e)
    {
        this.IsEnabled = false;

        var frame = (Frame)sender;
        await ButtonAnimation(frame);

        CoreData.Clear();

        this.IsEnabled = true;
        loading.IsVisible = false;
    }

    private async void OnGetAll(object sender, EventArgs e)
	{
        this.IsEnabled = false;

        var frame = (Frame)sender;
        await ButtonAnimation(frame);
        
        loading.IsVisible = true;

        await DownloadData();

        this.IsEnabled = true;
        loading.IsVisible = false;
    }

    private async void OnDecryptData(object sender, EventArgs e)
    {
        this.IsEnabled = false;

        var frame = (Frame)sender;
        await ButtonAnimation(frame);

        if (CoreData.Count == 0)
        {
            await DisplayAlert("Error", "La Lista esta Vacía.", "Ok");
            this.IsEnabled = true;
            return;
        }

        for (int i = 0; i < CoreData.Count; i++)
        {
            if (!IsBase64String(CoreData[i].Data01))
            {
                await DisplayAlert("Error", "La Lista ya está Descifrado.", "Ok");
                this.IsEnabled = true;
                return;
            }
        }

        var passwordPromp = new CorePromptPage();
        await Navigation.PushModalAsync(passwordPromp);
        var password = await passwordPromp.GetPasswordAsync();

        if (string.IsNullOrWhiteSpace(password))
        {
            this.IsEnabled = true;
            return;
        }

        loading.IsVisible = true;

        var resultApi = await _apiCoreService.Login(password);

        if (!resultApi.Success)
        {
            await DisplayAlert($"{resultApi.StatusCode}", $"{resultApi.Message}", "Ok");
            this.IsEnabled = true;
            return;
        }

        if (!string.IsNullOrEmpty(resultApi.Data?.IV))
        {
            EncryptionService encryptionService = new EncryptionService();

            var decryptedData = CoreData
                .Select(data => encryptionService.DecryptData(data, password, resultApi.Data.IV))
                .OrderBy(e => e.Data01)
                .ToList();

            CoreData.Clear();

            foreach (var item in decryptedData)
            {
                CoreData.Add(item);
            }
        }

        this.IsEnabled = true;
        loading.IsVisible = false;
    }

    private async void OnCreate(object sender, EventArgs e)
	{
        this.IsEnabled = false;

        var frame = (Frame)sender;
        await ButtonAnimation(frame);

        await SaveData(null);

        this.IsEnabled = true;
        loading.IsVisible = false;
    }

    private async void OnEdit(object sender, EventArgs e)
    {
        this.IsEnabled = false;
        var button = sender as Button;

        if (button?.BindingContext is CoreDTO coreDTO)
        {
            if (IsBase64String(coreDTO.Data01))
            {
                await DisplayAlert("Error", "El Item Debe Estar Descifrado.", "Ok");
            } else
            {
                await SaveData(coreDTO);
            }
        }

        loading.IsVisible = false;
        this.IsEnabled = true;
    }

    private async void OnDelete(object sender, EventArgs e)
    {
        loading.IsVisible = true;

        this.IsEnabled = false;
        var button = sender as Button;

        bool response = await DisplayAlert("Advertencia", "Estas Seguro de Eliminar Este Elemento?.", "SI", "NO");

        if (button?.BindingContext is CoreDTO coreDTO && response)
        {
            var result = await _apiCoreService.Delete(coreDTO.Id);
            CoreData.Remove(coreDTO);
        }

        loading.IsVisible = false;
        this.IsEnabled = true;
    }

    private async Task SaveData(CoreDTO coreDTO)
    {
        var page = new CoreFormPage(coreDTO);
        await Navigation.PushAsync(page);
        var (newCoreDTO, password) = await page.GetCompletionTask();

        if (newCoreDTO == null || string.IsNullOrWhiteSpace(password))
        {
            return;
        }

        var resultApi = await _apiCoreService.Login(password);

        if (!resultApi.Success)
        {
            await DisplayAlert($"Error {resultApi.StatusCode}", resultApi.Message, "Ok");
            return;
        }

        if (string.IsNullOrEmpty(resultApi.Data.IV))
        {
            return;
        }

        loading.IsVisible = true;

        EncryptionService encryptionService = new EncryptionService();
        newCoreDTO = encryptionService.EncryptData(newCoreDTO, password, resultApi.Data.IV);

        ResultApiDTO<CoreDTO> resultApiDTO;

        if (newCoreDTO.Id == 0)
        {
            resultApiDTO = await _apiCoreService.Create(newCoreDTO);
        }
        else
        {
            resultApiDTO = await _apiCoreService.Edit(newCoreDTO);
        }

        if (!resultApiDTO.Success) {
            await DisplayAlert($"Error {resultApi.StatusCode}", resultApi.Message, "Ok");
            return;
        }

        await DownloadData();
        loading.IsVisible = false;
    }

    private void OnSearchTextChanged(object sender, TextChangedEventArgs e)
    {
        var searchText = e.NewTextValue;

        CoreData.Clear();

        if (string.IsNullOrEmpty(searchText))
        {
            foreach(var item in BaseCoreData)
            {
                CoreData.Add(item);
            }
        }
        else
        {
            var filteredList = BaseCoreData.Where(item => item.Data01.Contains(searchText, StringComparison.OrdinalIgnoreCase));

            foreach (var item in filteredList)
            {
                CoreData.Add(item);
            }
        }
    }

    private async Task DownloadData()
    {
        CoreData.Clear();

        var resultApi = await _apiCoreService.GetAll();

        foreach (var coreData in resultApi.Data)
        {
            CoreData.Add(coreData);
            BaseCoreData.Add(coreData);
        }
    }

    private async Task ButtonAnimation(Frame frame)
    {
        await frame.ScaleTo(0.90, 100);
        await frame.ScaleTo(1, 100);
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