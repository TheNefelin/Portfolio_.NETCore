namespace MauiAppAdmin.Pages;

public partial class ChatPage : ContentPage
{
    private TaskCompletionSource<string> _taskCompletionSource = new();

    public ChatPage()
	{
		InitializeComponent();
	}

	private async void OnClick(object sender, EventArgs e)
	{
        _taskCompletionSource.SetResult(Txt2.Text);
        await Navigation.PopModalAsync();
    }

    public async Task<string> GetCompletionTask()
    {
        return await _taskCompletionSource.Task;
    }
}