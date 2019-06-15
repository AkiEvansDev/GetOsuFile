using NAudio.Wave;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using System.Windows.Threading;

namespace GetOsuFile
{
    public partial class MusicPlayer : UserControl
    {
        public SongImageInfo SongInfo;
        public bool IsPause;
        private bool IsDrag;
        private bool IsLoop;
        private DispatcherTimer Timer;
        private WaveOutEvent Wave;
        private AudioFileReader Audio;

        public MusicPlayer()
        {
            InitializeComponent();
            IsEnabled = false;
            IsPause = true;
            IsDrag = false;
            IsLoop = false;
            Wave = new WaveOutEvent();
            Timer = new DispatcherTimer();
            Timer.Tick += Timer_Tick;
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            SongPosition.Value = Audio.CurrentTime.TotalSeconds;
            TextPosition.Text = Audio.GetCurrentTimeString();
        }

        public void Refresh()
        {
            if (Wave != null)
            {
                Wave.PlaybackStopped -= Wave_PlaybackStopped;
                Wave.Stop();
                Wave.Dispose();
            }
            if (Audio != null)
                Audio.Dispose();
            Wave = null;
            Audio = null;
            Wave = new WaveOutEvent();
            Wave.PlaybackStopped += Wave_PlaybackStopped;
            IsEnabled = false;
            Preview.Source = null;
            SongPosition.Value = 0;
            SongLength.Text = "00:00";
            TextPosition.Text = "00:00";
            SongName.Text = "Песня";
            IsDrag = false;
            IsPause = true;
            PlayIcon.Kind = MaterialDesignThemes.Wpf.PackIconKind.Play;
        }

        private void Wave_PlaybackStopped(object sender, StoppedEventArgs e)
        {
            if (!IsPause)
            {
                PlayIcon.Kind = MaterialDesignThemes.Wpf.PackIconKind.Play;
                Timer.Stop();
                IsPause = !IsPause;
                if (IsLoop)
                    SetSong(Info.Control.NextSong(SongInfo.GetName()));
            }
        }

        public void SetSong(SongImageInfo info)
        {
            Refresh();
            if (info.IsNull)
                return;
            SongInfo = info;
            Audio = new AudioFileReader(SongInfo.GetSongPath())
            {
                Volume = (float)SongVolume.Value
            };
            SongPosition.Maximum = Audio.TotalTime.TotalSeconds;
            SongLength.Text = Audio.GetTotalTimeString();
            SongName.Text = SongInfo.GetName();
            Wave.Init(Audio);
            BitmapImage image = new BitmapImage();
            image.BeginInit();
            image.UriSource = new Uri(SongInfo.GetImagePath());
            image.DecodePixelWidth = 60;
            image.EndInit();
            Preview.Source = image;
            IsEnabled = true;
            PlayOrPause_Click(null, new RoutedEventArgs());
        }

        private void PlayOrPause_Click(object sender, RoutedEventArgs e)
        {
            if (IsPause)
            {
                PlayIcon.Kind = MaterialDesignThemes.Wpf.PackIconKind.Pause;
                if (SongPosition.Value == SongPosition.Maximum)
                    SongPosition.Value = 0;
                if (Audio != null)
                    Audio.SetPosition(SongPosition.Value);
                Wave.Play();
                Timer.Start();
            }
            else
            {
                PlayIcon.Kind = MaterialDesignThemes.Wpf.PackIconKind.Play;
                Wave.Pause();
                Timer.Stop();
            }
            IsPause = !IsPause;
        }

        private void SongPosition_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (SongPosition.Value > SongPosition.Maximum)
                SongPosition.Value = SongPosition.Maximum - 1;
            if (!IsDrag && Audio != null)
                Audio.SetPosition(SongPosition.Value);
            TextPosition.Text = WaveStreamExtensions.GetMinutesAndSecondsString(SongPosition.Value);
        }

        private void SongPosition_DragStarted(object sender, System.Windows.Controls.Primitives.DragStartedEventArgs e)
        {
            IsDrag = true;
        }

        private void SongPosition_DragCompleted(object sender, System.Windows.Controls.Primitives.DragCompletedEventArgs e)
        {
            if (Audio != null)
                Audio.SetPosition(SongPosition.Value);
            IsDrag = false;
            TextPosition.Text = WaveStreamExtensions.GetMinutesAndSecondsString(SongPosition.Value);
        }

        public void Dispose()
        {
            if (Wave != null)
                Wave.Dispose();
            if (Audio != null)
                Audio.Dispose();
            Wave = null;
            Audio = null;
        }

        private void SongVolume_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (Audio != null)
                Audio.Volume = (float)SongVolume.Value;
            if (SongVolume.Value > 0.7)
                VolumeIcon.Kind = MaterialDesignThemes.Wpf.PackIconKind.VolumeHigh;
            else if (SongVolume.Value > 0.3)
                VolumeIcon.Kind = MaterialDesignThemes.Wpf.PackIconKind.VolumeMedium;
            else if (SongVolume.Value > 0)
                VolumeIcon.Kind = MaterialDesignThemes.Wpf.PackIconKind.VolumeLow;
            else if (SongVolume.Value == 0)
                VolumeIcon.Kind = MaterialDesignThemes.Wpf.PackIconKind.VolumeMute;
        }

        private void Dowload_Click(object sender, RoutedEventArgs e)
        {
            if (!SongInfo.IsNull)
                Info.SaveSong(SongInfo);
        }

        private void Loop_Click(object sender, RoutedEventArgs e)
        {
            if (IsLoop)
                Loop.Opacity = 0.5;
            else
                Loop.Opacity = 1;
            IsLoop = !IsLoop;
        }
    }
}
