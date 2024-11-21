using ClassLibrary_DTOs.PasswordManager;

namespace MauiAppAdmin.Pages;

public partial class CoreFormPage : ContentPage
{
    private TaskCompletionSource<(CoreDTO, string)> _taskCompletionSource;
    private readonly CoreDTO _coreDTO;

    public CoreFormPage(CoreDTO coreDTO)
	{
		InitializeComponent();

        _taskCompletionSource = new TaskCompletionSource<(CoreDTO, string)>();
        _coreDTO = coreDTO;
        
        LoadPage();
	}

    private async void LoadPage()
    {
        if (_coreDTO != null)
        {
            IdEntry.Text = _coreDTO.Id.ToString();
            Data01Entry.Text = _coreDTO.Data01;
            Data02Entry.Text = _coreDTO.Data02;
            Data03Entry.Text = _coreDTO.Data03;
            BtnClick.Text = "Modificar";

            ErrorLabel.Text = "Procura que los datos a modificar están Desencriptado.";
            ErrorFrame.IsVisible = true;
        }
        else
        {
            IdEntry.Text = "0";
            BtnClick.Text = "Guardar";
        }
    }

    private async void OnSave(object sender, EventArgs e)
	{
        int id = Int32.Parse(IdEntry.Text);
        string data01 = Data01Entry.Text;
        string data02 = Data02Entry.Text;
        string data03 = Data03Entry.Text;
        string password = PasswordEntry.Text;

        if (string.IsNullOrWhiteSpace(data01) || string.IsNullOrWhiteSpace(data02) || string.IsNullOrWhiteSpace(data03) || string.IsNullOrWhiteSpace(password))
        {
            ErrorLabel.Text = "Debes Rellenar Todos los Campos.";
            ErrorFrame.IsVisible = true;
            return;
        }

        CoreDTO coreDTO = new()
        {
            Id = id,
            Data01 = data01,
            Data02 = data02,
            Data03 = data03,
        };

        //var passwordPromp = new CorePromptPage();
        //await Navigation.PushModalAsync(passwordPromp);
        //var password = await passwordPromp.GetPasswordAsync();

        _taskCompletionSource.SetResult((coreDTO, password));
        await Navigation.PopAsync();
    }

    public async Task<(CoreDTO, string)> GetCompletionTask()
    {
        return await _taskCompletionSource.Task;
    }

    protected override void OnDisappearing()
    {
        base.OnDisappearing();

        if (!_taskCompletionSource.Task.IsCompleted)
        {
            _taskCompletionSource.SetResult((null, null));
        }
    }
}