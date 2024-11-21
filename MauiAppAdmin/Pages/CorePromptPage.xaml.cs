namespace MauiAppAdmin.Pages;

public partial class CorePromptPage : ContentPage
{
    private TaskCompletionSource<string> _taskCompletionSource;

    public CorePromptPage()
	{
		InitializeComponent();
        _taskCompletionSource = new TaskCompletionSource<string>();
    }

    private async void OnSubmitClicked(object sender, EventArgs e)
	{
        string password = PasswordEntry.Text;
        ErrorLabel.IsVisible = false;

        if (string.IsNullOrEmpty(password) || password.Length < 6)
        {
            ErrorLabel.Text = "La contraseña debe tener al menos 6 caracteres.";
            ErrorLabel.IsVisible = true;
            return;
        }

        _taskCompletionSource.SetResult(password);
        await Navigation.PopModalAsync();
    }

    private async void OnCancelClicked(object sender, EventArgs e)
	{
        _taskCompletionSource.SetResult(null);
        await Navigation.PopModalAsync();
    }

    public Task<string> GetPasswordAsync()
    {
        return _taskCompletionSource.Task;
    }

    protected override void OnDisappearing()
    {
        base.OnDisappearing();

        if (!_taskCompletionSource.Task.IsCompleted)
        {
            _taskCompletionSource.SetResult(null);
        }
    }
}