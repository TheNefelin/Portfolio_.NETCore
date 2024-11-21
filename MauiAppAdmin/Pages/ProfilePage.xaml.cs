namespace MauiAppAdmin.Pages;

public partial class ProfilePage : ContentPage
{
    private TaskCompletionSource<(string, string)> _taskCompletionSource = new();

    public ProfilePage()
	{
		InitializeComponent();
    }

    private async void OnClick(object sender, EventArgs e)
    {
        var page = new ChatPage();
        await Navigation.PushModalAsync(page);
        var txt2 = await page.GetCompletionTask();

        _taskCompletionSource.SetResult((TxtProp.Text, txt2));
        await Navigation.PopAsync();
    }

    public async Task<(string, string)> GetCompletionTask()
    {
        return await _taskCompletionSource.Task;
    }
}