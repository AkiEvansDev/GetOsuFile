using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;

namespace GetOsuFile.OsuFiles
{
    public struct SongImage
    {
        private readonly string Folder;
        public string Song;
        public string[] Images;
        
        public SongImage(string path, int minWidth, int minHeight)
        {
            Song = null;
            string[] files = Directory.GetFiles(path);
            List<string> imageFiles = new List<string>();
            long maxFileLength = 0;
            Image image;
            for (int j = 0; j < files.Length; j++)
                if (files[j].EndsWith(".jpg") || files[j].EndsWith(".jpeg") || files[j].EndsWith(".png"))
                {
                    image = Image.FromFile(files[j]);
                    if (image.Width >= minWidth && image.Height >= minHeight)
                        imageFiles.Add(files[j].Substring(files[j].LastIndexOf('\\') + 1));
                }
                else if (files[j].EndsWith(".wav") || files[j].EndsWith(".mp3") || files[j].EndsWith(".flac"))
                {
                    FileInfo file = new FileInfo(files[j]);
                    if (file.Length > maxFileLength)
                    {
                        Song = files[j].Substring(files[j].LastIndexOf('\\') + 1);
                        maxFileLength = file.Length;
                    }
                }
            Images = imageFiles.ToArray();
            Folder = path;
            if (path.Contains('\\') && path.LastIndexOf('\\') + 1 < path.Length)
                Folder = path.Substring(path.LastIndexOf('\\') + 1);
        }

        public SongImageInfo GetInfo(int index)
        {
            return new SongImageInfo(Folder, Song, Images[index]);
        }

        public string GetName()
        {
            string name = Folder;
            int i = name.IndexOf(' ');
            if (i > 0 && i < name.Length)
            {
                if (int.TryParse(name.Substring(0, i), out int n))
                    return name.Substring(i + 1);
            }
            return name;
        }
    }
}
