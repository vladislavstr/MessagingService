using Desktop.Dto;
using Microsoft.AspNetCore.SignalR.Client;

namespace Desktop
{
    public partial class MainPage : ContentPage
    {
        private HubConnection hubConnection;
        public MainPage()
        {
            InitializeComponent();
            hubConnection = GetHubConnection();
        }
        private async void connection_Button_Click(object sender, EventArgs e)
        {
            try
            {
                if (hubConnection.State == HubConnectionState.Disconnected)
                {
                    await hubConnection.StartAsync();
                    connection_Button.Text = "Disconnect";
                    connectionState_Label.Text = "Connected";
                    connectionState_Label.TextColor = Colors.Green;
                }
                else if (hubConnection.State == HubConnectionState.Connected)
                {
                    await hubConnection.StopAsync();
                    connection_Button.Text = "Connect";
                    connectionState_Label.Text = "Disconnected";
                    connectionState_Label.TextColor = Colors.Red;
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error on Connection", ex.Message, "OK");
            }
        }

        private void AppendTextToChat(MessageDto message, Color color)
        {
            MainThread.BeginInvokeOnMainThread(() =>
            {
                var messageLabel = new Label
                {
                    Text = $"Id: {message.Id}\nDate: {message.SavedAt}\nContent: {message.Content}\n",
                    TextColor = color
                };

                chat.Children.Add(messageLabel);
            });
        }

        private HubConnection GetHubConnection()
        {
            string url = "http://localhost:5120/chat";

            HubConnection result = new HubConnectionBuilder()
                .WithUrl(url)
                .WithAutomaticReconnect()
                .Build();

            result.On<MessageDto>("Send", message =>
            {
                AppendTextToChat(message, Colors.Yellow);
            });

            result.Closed += error =>
            {
                try
                {
                    MainThread.BeginInvokeOnMainThread(async () =>
                    {
                        string errorMessage = error != null ? error.Message : "Connection closed normally.";
                        await DisplayAlert("Connection Closed", $"Connection closed with message: {errorMessage}", "OK");
                    });
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error in Closed event handler: {ex}");
                }

                return Task.CompletedTask;
            };

            result.Reconnected += id =>
            {
                DisplayAlert("Reconnected", "Connection reconnected with id: {id}", id, "OK");
                return Task.CompletedTask;
            };

            result.Reconnecting += error =>
            {
                DisplayAlert("Reconnected", "Connection reconnecting with error message: {Message}", error.Message, "OK");
                return Task.CompletedTask;
            };

            return result;
        }
    }
}
