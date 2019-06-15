using GetOsuFile.OsuFiles;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace GetOsuFile
{
    public partial class ImageList : UserControl
    {
        public OsuControl Images;
        private ImageCard[,] Cards;
        private int Positions;
        private bool WasNext;

        public ImageList()
        {
            InitializeComponent();
            Cards = new ImageCard[2, 12];
            for (int i = 0; i < 12; i++)
            {
                Page1.Children.Add(Cards[0, i] = new ImageCard());
                Page2.Children.Add(Cards[1, i] = new ImageCard());
            }
            Cards[0, 0].SetUpdateEvent(FirstCardUpdate);
            Cards[1, 0].SetUpdateEvent(FirstCardUpdate);
            Cards[0, 11].SetUpdateEvent(LastCardUpdate);
            Cards[1, 11].SetUpdateEvent(LastCardUpdate);
            if (Images == null || !Images.IsSongs)
                IsEnabled = false;
        }

        public void Open()
        {
            if (Positions == -1)
            {
                Positions = 0;
                Slider.SelectedIndex = Positions;
                GoPrev.IsEnabled = false;
                GoNext.IsEnabled = Images.CanGoNext();
            }
            Slider.Focus();
        }

        public void SetOsuImage(OsuControl osuImage)
        {
            GoPrev.IsEnabled = false;
            GoNext.IsEnabled = false;
            IsEnabled = true;
            if (osuImage.IsSongs)
            {
                Images = osuImage;
                Positions = 0;
                Slider.SelectedIndex = 0;
                Cards[0, 0].Update(Images.Select());
                for (int i = 1; i < 12; i++)
                    Cards[0, i].Update(Images.Next());
                WasNext = true;
                GoPrev.IsEnabled = false;
                GoNext.IsEnabled = Images.CanGoNext();
            }
        }

        public void Next()
        {
            Positions = 1 - Positions;
            if (!WasNext)
            {
                Images.LoadPosition();
                WasNext = true;
            }
            Images.SaveNextPosition();
            for (int i = 0; i < 12; i++)
                Cards[Positions, i].Update(Images.Next());
            GoPrev.IsEnabled = true;
            GoNext.IsEnabled = Images.CanGoNext();
        }

        public void Prev()
        {
            Positions = 1 - Positions;
            if (WasNext)
            {
                Images.LoadPosition();
                WasNext = false;
            }
            Images.SavePrevPosition();
            for (int i = 11; i >= 0; i--)
                Cards[Positions, i].Update(Images.Prev());
            GoNext.IsEnabled = true;
            GoPrev.IsEnabled = Images.CanGoPrev();
        }

        private void GoPrev_Click(object sender, RoutedEventArgs e)
        {
            Prev();
        }

        private void GoNext_Click(object sender, RoutedEventArgs e)
        {
            Next();
        }

        private void SearchEnter_Click(object sender, RoutedEventArgs e)
        {
            SetOsuImage(Info.Control.Search(SearchText.Text));
        }

        private void SearchText_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
                SearchEnter_Click(null, new RoutedEventArgs());
        }

        private void UserControl_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Right || e.Key == Key.D)
                if (Images.CanGoNext())
                    Next();
            if (e.Key == Key.Left || e.Key == Key.A)
                if (Images.CanGoPrev())
                    Prev();
        }

        private void FirstCardUpdate()
        {
            if (!WasNext)
                Slider.SelectedIndex = Positions;
        }

        private void LastCardUpdate()
        {
            if (WasNext)
                Slider.SelectedIndex = Positions;
        }
    }
}
