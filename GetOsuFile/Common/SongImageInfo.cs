using System.IO;

namespace GetOsuFile
{
    public struct SongImageInfo
    {
        public string Folder;
        public string Song;
        public string Image;
        public bool IsNull;
        
        public SongImageInfo(string folder, string song, string image, bool isNull = false)
        {
            Folder = folder;
            Song = song;
            Image = image;
            IsNull = isNull;
        }

        public string GetImagePath()
        {
            return Path.Combine(Info.OsuSong, Folder + '\\' + Image);
        }

        public string GetSongPath()
        {
            return Path.Combine(Info.OsuSong, Folder + '\\' + Song);
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

        public static SongImageInfo GetNullInfo()
        {
            return new SongImageInfo(null, null, null, true);
        }
    }
}
