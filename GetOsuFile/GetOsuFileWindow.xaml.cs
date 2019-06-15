using System;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Imaging;

namespace GetOsuFile
{
    public partial class GetOsuFileWindow
    {
        public GetOsuFileWindow()
        {
            InitializeComponent();
            Settings.RefreshInfo = SetInfo;
            SetInfo();
        }

        private void MetroWindow_Loaded(object sender, RoutedEventArgs e)
        {
            ImageCard.ImageClick = SelectImage;
            ImageCard.SongClick = Player.SetSong;
            Info.Message = AddMessage;
        }

        public void SetInfo()
        {
            if (Settings.IsPath)
            {
                ImagePage.IsEnabled = false;
                AsyncWork.AsyncAction(
                    delegate ()
                    {
                        Info.SetControl(Settings.OsuSongs, 700, 600);
                    },
                delegate ()
                {
                    Dispatcher.Invoke(delegate ()
                    {
                        ImagePage.SetOsuImage(Info.Control);
                    });
                });
            }
        }

        private void ColorZone_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }
        
        private void SelectImage(SongImageInfo info)
        {
            DialogHeader.Header = info.GetName();
            BitmapImage image = new BitmapImage();
            image.BeginInit();
            image.UriSource = new Uri(info.GetImagePath());
            image.EndInit();
            DialogImage.Source = image;
            ImageDialog.IsOpen = true;
        }

        private void DialogClose_Click(object sender, RoutedEventArgs e)
        {
            ImageDialog.IsOpen = false;
        }

        private void AddMessage(string message, string path)
        {
            if (string.IsNullOrEmpty(path))
            {
                Message.MessageQueue.Enqueue(message);
                return;
            }
            Message.MessageQueue.Enqueue(message, "ПЕРЕЙТИ", () => 
            {
                System.Diagnostics.Process.Start("explorer.exe", "/select, \"" + path + "\"");
            });
        }

        private void MetroWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            Player.Dispose();
            Settings.Save();
        }
    }
}
