using GetOsuFile.OsuFiles;
using System;
using System.IO;

namespace GetOsuFile
{
    public static class Info
    {
        public static string OsuSong;
        public static string DownloadImagePath;
        public static string DownloadSongPath;
        public static string DownloadFolderSongPath;
        public static OsuControl Control { get; private set; }
        public static SaveMessage Message;

        public static void SetControl(string osuSongs, int minWidth, int minHeight)
        {
            Control = new OsuControl(osuSongs, minWidth, minHeight);
        }

        private static void Copy(string file, string path, string name)
        {
            int count = 1;
            try
            {
                string extension = Path.GetExtension(file);
                string newFullPath = Path.Combine(path, name + extension);
                while (File.Exists(newFullPath))
                {
                    string tempFileName = string.Format("{0}({1})", name, count++);
                    newFullPath = Path.Combine(path, tempFileName + extension);
                }
                File.Copy(file, newFullPath);
                Message("Сохранено: " + name, newFullPath);
            }
            catch
            {
                Message("Ошибка сохранения " + name, "");
            }
        }

        private static void CopyDir(string fromDir, string toDir)
        {
            Directory.CreateDirectory(toDir);
            foreach (string s1 in Directory.GetFiles(fromDir))
            {
                string s2 = toDir + '\\' + Path.GetFileName(s1);
                File.Copy(s1, s2);
            }
            foreach (string s in Directory.GetDirectories(fromDir))
            {
                CopyDir(s, toDir + '\\' + Path.GetFileName(s));
            }
        }

        public static void SaveImage(SongImageInfo image)
        {
            Copy(image.GetImagePath(), DownloadImagePath, image.GetName());
        }

        public static void SaveSong(SongImageInfo song)
        {
            Copy(song.GetSongPath(), DownloadSongPath, song.GetName());
        }

        public static void SaveFolderSong(SongImageInfo folder)
        {
            try
            {
                CopyDir(OsuSong + '\\' + folder.Folder, DownloadFolderSongPath + '\\' + folder.Folder);
                Message("Сохранено: " + folder.Folder, DownloadFolderSongPath + '\\' + folder.Folder);
            }
            catch (Exception e)
            {
                Message(e.Message, "");
            }
        }
    }
}
