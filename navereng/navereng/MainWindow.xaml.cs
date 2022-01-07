using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace navereng
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly HttpClient client = new HttpClient();
        private NotifyIcon notifyIcon = new NotifyIcon();
        public MainWindow()
        {
            InitializeComponent();

            notifyIcon.Icon = new System.Drawing.Icon("navereng.ico");
            notifyIcon.Visible = true;
            notifyIcon.Text = "Navereng";
            notifyIcon.Click += NotifyIcon_Click;

            notifyIcon.ContextMenuStrip = new ContextMenuStrip();
            notifyIcon.ContextMenuStrip.Items.Add("Exit", System.Drawing.Image.FromFile("exit.ico"), NotifyIconExit_Click);
        }

        private void NotifyIconExit_Click(object? sender, EventArgs e)
        {
            this.Close();
        }
        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
            e.Cancel = true;
        }
        private void NotifyIcon_Click(object? sender, EventArgs e)
        {
            this.WindowState = WindowState.Normal;
            this.Activate();
        }

        private async void TextBox_KeyUp(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key != Key.Enter) return;
            MeaningListView.Items.Clear();  

            string word = WordTextBox.Text;
            var res = await client.GetStringAsync($"https://ac-dict.naver.com/enko/ac?st=11&r_lt=11&q={word}");
            var rawJson = JsonConvert.DeserializeObject(res);
            var json = JObject.Parse(rawJson.ToString());

            foreach(var junk in json["items"][0])
            {
                var meanings = junk[2][0].ToString().Split(", ").ToList();
                meanings.ForEach(x => {
                    x.Trim();
                    if(x.Length > 0)
                        MeaningListView.Items.Add(x);
                });
            }
            
        }
    }
}
