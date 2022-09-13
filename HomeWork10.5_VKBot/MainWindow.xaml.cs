using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using Timer = System.Threading.Timer;

namespace HomeWork10._5_VKBot
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        readonly VkBotClient client;
        PageMessage PageMessage { get; set; }

        public MainWindow()
        {
            InitializeComponent();

            client = new VkBotClient(this);

            UsersList.ItemsSource = client.user;
        }


        private void Update()
        {
            client.MessageListener(UsersList.SelectedIndex);

            int i = UsersList.SelectedIndex;

            List<MessageLog> tempMessage = new List<MessageLog>();
            PageMessage = new PageMessage(tempMessage);
            foreach (var item in client.BotMessageLog)
            {
                if (item.Id == client.user[i].Id)
                {
                    tempMessage.Add(item);
                }
            }
            if (UsersList.SelectedIndex != -1)
                MainFrame.Content = PageMessage.Content;
        }

        private void UsersList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Update();
        }


        private void Button_Click(object sender, RoutedEventArgs e)
        {
            client.SendMessage(txtMsgSend.Text, targetId.Text);

            Update();
        }

        private void updateBtn_Click(object sender, RoutedEventArgs e)
        {
            Update();
            client.SaveToJson(client.user[UsersList.SelectedIndex]);
        }
    }
}
