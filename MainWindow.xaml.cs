using Microsoft.AspNetCore.SignalR.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;


namespace WPFboxApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        HubConnection connection;
        public MainWindow()
        {
            InitializeComponent();

            connection = new HubConnectionBuilder()
                .WithUrl("http://localhost:5295/chatHub")
                .Build();

            connection.Closed += async (error) =>
            {
                await Task.Delay(new Random().Next(0, 5) * 1000);
                await connection.StartAsync();
            };
        }

        

       

     
        private async void submitButton_Click(object sender, RoutedEventArgs e)
        {
            connection.On<string, string>("ReceiveMessage", (user, message) =>
            {
                this.Dispatcher.Invoke(() =>
                {
                    var newMessage = $"{user}: {message}";
                    messagesList.Items.Add(newMessage);
                });
            });

            try
            {
                await connection.StartAsync();
                messagesList.Items.Add("Connection started");
                connectButton.IsEnabled = false;
                sendButton.IsEnabled = true;
            }
            catch (Exception ex)
            {
                messagesList.Items.Add(ex.Message);
            }
        }

        private async void endTask_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                await connection.InvokeAsync("SendMessage",
                    userTextBox.Text, messageTextBox.Text);
            }
            catch (Exception ex)
            {
                messagesList.Items.Add(ex.Message);
            }
        }

        private async void userTextbox_TextChanged(object sender, TextChangedEventArgs e)
        {
          
        }

        private void messagesList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private async void testButton_Click(object sender, RoutedEventArgs e)
        {
            string text = userTextBox.Text;
            try
            {
                //variable to establish identity of client
                //variable to establish program name
                //server needs to take identity of client and program to be UnInstalled
                //command line prompt to install for that specific client
                //usernames and password credentials for each individual user
                await connection.InvokeAsync("UnInstall",
                    userTextBox.Text, messageTextBox.Text);
                Console.WriteLine("I have received message one");
            }
            catch (Exception ex)
            {
                messagesList.Items.Add(ex.Message);
                Console.WriteLine("invalid message");
            }
        }
    }
}
