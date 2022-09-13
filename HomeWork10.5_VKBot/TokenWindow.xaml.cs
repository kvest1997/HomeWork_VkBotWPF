using System.IO;
using System.Text;
using System.Windows;

namespace HomeWork10._5_VKBot
{
    /// <summary>
    /// Логика взаимодействия для TokenWindow.xaml
    /// </summary>
    public partial class TokenWindow : Window
    {
        MainWindow mw;

        public TokenWindow()
        {
            InitializeComponent();
            string line;
            if (File.Exists("token.txt"))
            {
                using (StreamReader streamReader = new StreamReader("token.txt"))
                {
                    line = streamReader.ReadToEnd();

                }

                if (line != null)
                {
                    mw = new MainWindow();
                    mw.Show();
                    this.Close();

                }

            }
            else
                File.Create("token.txt");
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (File.Exists("token.txt"))
            {
                using (StreamWriter streamWriter = new StreamWriter("token.txt", true, Encoding.UTF8))
                {
                    streamWriter.Write(TextToken.Text);
                }
                mw = new MainWindow();
                mw.Show();
                this.Close();
            }
            else
                MessageBox.Show("Файл не создан!!!");


        }
    }
}
