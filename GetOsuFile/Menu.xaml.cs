using MaterialDesignThemes.Wpf;
using Microsoft.Win32;
using System;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Runtime.Serialization.Formatters.Binary;

namespace GetOsuFile
{
    public partial class Menu : UserControl
    {
        public string OsuPath;
        public string OsuSongs;
        public string ImagePath;
        public string SongPath;
        public string FolderSongPath;
        public bool IsPath;
        public Action RefreshInfo;

        public Menu()
        {
            InitializeComponent();
            IsPath = false;
            PathSelect.ActionCommand = new PathCommand(SelectOsuPath);
            ImageSelect.ActionCommand = new PathCommand(SelectImagePath);
            SongSelect.ActionCommand = new PathCommand(SelectSongPath);
            FolderSongSelect.ActionCommand = new PathCommand(SelectFolderSongPath);
            Load();
            if (!IsPath)
            {
                try
                {
                    PathImage.Text = Environment.GetFolderPath(Environment.SpecialFolder.MyPictures);
                    PathSong.Text = Environment.GetFolderPath(Environment.SpecialFolder.MyMusic);
                    PathFolderSong.Text = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
                    PathOsu.Text = GetApplictionInstallPath("osu!");
                    IsTruePath();
                }
                catch
                {
                    Info.Message("Ошибка в поиске папок!", "");
                }
            }
        }

        public void Save()
        {
            if (isSave.IsChecked.Value)
            {
                string[] save = new string[4];
                save[0] = ImagePath;
                save[1] = SongPath;
                save[2] = OsuPath;
                save[3] = FolderSongPath;
                BinaryFormatter bf = new BinaryFormatter();
                using (FileStream fs = new FileStream("GetOsuFileSave.bin", FileMode.Create))
                {
                    bf.Serialize(fs, save);
                }
            }
        }

        public void Load()
        {
            if (!File.Exists("GetOsuFileSave.bin"))
            {
                IsPath = false;
                return;
            }
            string[] save = new string[0];
            BinaryFormatter bf = new BinaryFormatter();
            using (FileStream fs = new FileStream("GetOsuFileSave.bin", FileMode.Open))
            {
                save = (string[])bf.Deserialize(fs);
            }
            PathImage.Text = save[0];
            PathSong.Text = save[1];
            PathOsu.Text = save[2];
            PathFolderSong.Text = save[3];
            IsTruePath();
        }

        public void SelectOsuPath()
        {
            System.Windows.Forms.FolderBrowserDialog dialog = new System.Windows.Forms.FolderBrowserDialog
            {
                ShowNewFolderButton = false,
                Description = "Укажите путь к папке osu!"
            };
            if (IsPath)
                dialog.SelectedPath = OsuPath;
            if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                PathOsu.Text = dialog.SelectedPath;
                IsTruePath();
            }
        }

        public void SelectImagePath()
        {
            System.Windows.Forms.FolderBrowserDialog dialog = new System.Windows.Forms.FolderBrowserDialog
            {
                Description = "Укажите путь к папке для сохранения картинок!"
            };
            if (IsPath)
                dialog.SelectedPath = ImagePath;
            if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                PathImage.Text = dialog.SelectedPath;
                IsTruePath();
            }
        }

        public void SelectSongPath()
        {
            System.Windows.Forms.FolderBrowserDialog dialog = new System.Windows.Forms.FolderBrowserDialog
            {
                Description = "Укажите путь к папке для сохранения музыки!"
            };
            if (IsPath)
                dialog.SelectedPath = SongPath;
            if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                PathSong.Text = dialog.SelectedPath;
                IsTruePath();
            }
        }

        public void SelectFolderSongPath()
        {
            System.Windows.Forms.FolderBrowserDialog dialog = new System.Windows.Forms.FolderBrowserDialog
            {
                Description = "Укажите путь к папке для сохранения песен osu!"
            };
            if (IsPath)
                dialog.SelectedPath = FolderSongPath;
            if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                PathFolderSong.Text = dialog.SelectedPath;
                IsTruePath();
            }
        }

        public void IsTruePath()
        {
            OsuPath = PathOsu.Text;
            ImagePath = PathImage.Text;
            SongPath = PathSong.Text;
            FolderSongPath = PathFolderSong.Text;
            if (!string.IsNullOrEmpty(OsuPath))
            {
                OsuSongs = Path.Combine(OsuPath, "Songs");
                if (!Directory.Exists(OsuSongs))
                    OsuSongs = "";
                if (!string.IsNullOrEmpty(OsuSongs))
                {
                    PathError.IsActive = false;
                    PathOsu.Text = OsuPath;
                }
                else
                {
                    OsuPath = "";
                    OsuSongs = "";
                    PathError.IsActive = true;
                }
            }
            else
            {
                OsuPath = "";
                OsuSongs = "";
                PathError.IsActive = true;
            }
            if (!string.IsNullOrEmpty(ImagePath))
            {
                if (!Directory.Exists(ImagePath))
                    ImageError.IsActive = true;
                else
                {
                    PathImage.Text = ImagePath;
                    ImageError.IsActive = false;
                }
            }
            else
                ImageError.IsActive = true;
            if (!string.IsNullOrEmpty(SongPath))
            {
                if (!Directory.Exists(SongPath))
                    SongError.IsActive = true;
                else
                {
                    PathSong.Text = SongPath;
                    SongError.IsActive = false;
                }
            }
            else
                SongError.IsActive = true;
            if (!string.IsNullOrEmpty(FolderSongPath))
            {
                if (!Directory.Exists(FolderSongPath))
                    FolderSongError.IsActive = true;
                else
                {
                    PathFolderSong.Text = FolderSongPath;
                    FolderSongError.IsActive = false;
                }
            }
            else
                FolderSongError.IsActive = true;
            Info.DownloadImagePath = ImagePath;
            Info.DownloadSongPath = SongPath;
            Info.DownloadFolderSongPath = FolderSongPath;
            IsPath = !PathError.IsActive & !ImageError.IsActive & !SongError.IsActive & !FolderSongError.IsActive;
        }

        private void PathOsu_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                IsTruePath();
                PathImage.Focus();
            }
        }

        private void PathImage_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                IsTruePath();
                PathSong.Focus();
            }
        }

        private void PathSong_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                IsTruePath();
                PathFolderSong.Focus();
            }
        }

        private void PathFolderSong_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                IsTruePath();
                PathOsu.Focus();
            }
        }

        public static string GetApplictionInstallPath(string nameOfAppToFind)
        {
            string installedPath;
            string keyName;
            
            keyName = @"SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall";
            installedPath = ExistsInSubKey(Registry.CurrentUser, keyName, "DisplayName", nameOfAppToFind);
            if (!string.IsNullOrEmpty(installedPath))
            {
                return installedPath;
            }

            keyName = @"SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall";
            installedPath = ExistsInSubKey(Registry.LocalMachine, keyName, "DisplayName", nameOfAppToFind);
            if (!string.IsNullOrEmpty(installedPath))
            {
                return installedPath;
            }
            
            keyName = @"SOFTWARE\Wow6432Node\Microsoft\Windows\CurrentVersion\Uninstall";
            installedPath = ExistsInSubKey(Registry.LocalMachine, keyName, "DisplayName", nameOfAppToFind);
            if (!string.IsNullOrEmpty(installedPath))
            {
                return installedPath;
            }

            return string.Empty;
        }
        
        private static string ExistsInSubKey(RegistryKey root, string subKeyName, string attributeName, string nameOfAppToFind)
        {
            RegistryKey subkey;
            string displayName;
            using (RegistryKey key = root.OpenSubKey(subKeyName))
            {
                if (key != null)
                {
                    foreach (string kn in key.GetSubKeyNames())
                    {
                        using (subkey = key.OpenSubKey(kn))
                        {
                            displayName = subkey.GetValue(attributeName) as string;
                            if (displayName != null && displayName.ToLower().StartsWith(nameOfAppToFind))
                            {
                                try
                                {
                                    string path = subkey.GetValue("InstallLocation") as string;
                                    if (!string.IsNullOrEmpty(path))
                                        return path.Substring(0, path.LastIndexOf('\\'));
                                    path = subkey.GetValue("UninstallString") as string;
                                    if (!string.IsNullOrEmpty(path))
                                        return path.Substring(0, path.LastIndexOf('\\'));
                                }
                                catch
                                {
                                    return string.Empty;
                                }
                            }
                        }
                    }
                }
            }
            return string.Empty;
        }
    
        private void isDark_Click(object sender, RoutedEventArgs e)
        {
            new PaletteHelper().SetLightDark(isDark.IsChecked.Value);
        }

        private void Path_LostFocus(object sender, RoutedEventArgs e)
        {
            IsTruePath();
        }

        private void PathOsu_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            SelectOsuPath();
        }

        private void PathImage_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            SelectImagePath();
        }

        private void PathSong_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            SelectSongPath();
        }
        
        private void PathFolderSong_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            SelectFolderSongPath();
        }

        private void Refresh_Click(object sender, RoutedEventArgs e)
        {
            RefreshInfo();
        }

        private void Research_Click(object sender, RoutedEventArgs e)
        {
            PathOsu.Text = GetApplictionInstallPath("osu!");
            IsTruePath();
        }
    }
}
