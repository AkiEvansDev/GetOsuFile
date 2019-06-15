using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace GetOsuFile
{
    public partial class ImageCard : UserControl
    {
        public SongImageInfo ImageInfo;
        public static SelectObj ImageClick;
        public static SelectObj SongClick;

        public ImageCard()
        {
            InitializeComponent();
        }

        public void SetUpdateEvent(Action action)
        {
            Preview.LayoutUpdated += (o, e) => { action(); };
        }

        public void Update(SongImageInfo info)
        {
            Dowload.IsEnabled = false;
            Music.IsEnabled = false;
            if (info.IsNull)
            {
                Visibility = Visibility.Hidden;
                return;
            }
            else if (Visibility == Visibility.Hidden)
                Visibility = Visibility.Visible;
            ImageInfo = info;
            ImageName.Content = ImageInfo.GetName();
            BitmapImage image = new BitmapImage();
            image.BeginInit();
            image.UriSource = new Uri(ImageInfo.GetImagePath());
            image.DecodePixelWidth = 196;
            image.EndInit();
            Preview.Source = image;
            Dowload.IsEnabled = true;
            Music.IsEnabled = true;
        }

        private void Preview_MouseLeftButtonUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (!ImageInfo.IsNull)
                ImageClick(ImageInfo);
        }

        private void Dowload_Click(object sender, RoutedEventArgs e)
        {
            if (!ImageInfo.IsNull)
                Info.SaveImage(ImageInfo);
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            Info.SaveFolderSong(ImageInfo);
        }

        private void Music_Click(object sender, RoutedEventArgs e)
        {
            SongClick(ImageInfo);
        }
    }
}
