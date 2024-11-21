namespace MauiAppAdmin.Pages;

public partial class TestPage : ContentPage
{
	public TestPage()
	{
		InitializeComponent();
	}

	private async void OnClick(object sender, EventArgs e)
	{
		var page = new ProfilePage();
		await Navigation.PushAsync(page);
		var (txt1, txt2) = await page.GetCompletionTask();

		Txt1.Text = txt1;
        Txt2.Text = txt2;
    }
}