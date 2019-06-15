using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace GetOsuFile.OsuFiles
{
    public class OsuControl
    {
        public bool IsSongs;
        private int SongsPosition;
        private int SaveSongsPosition;
        private int ImagesPosition;
        private int SaveImagesPosition;
        private SongImage[] Songs;

        public OsuControl() { }

        public OsuControl(SongImage[] songs)
        {
            Songs = songs;
            SongsPosition = 0;
            ImagesPosition = 0;
            SaveSongsPosition = -1;
            SaveImagesPosition = -1;
            IsSongs = Songs.Length > 0;
        }

        public OsuControl(string osuSongs, int minWidth, int minHeight)
        {
            Info.OsuSong = osuSongs;
            string[] files = Directory.GetDirectories(osuSongs);
            Songs = new SongImage[files.Length];
            Parallel.For(0, files.Length, 
                delegate (int i)
                {
                    Songs[i] = new SongImage(files[i], minWidth, minHeight);
                });
            Songs = Songs.Where(x => x.Images.Length > 0 && x.Song != null).OrderBy(x => x.GetName()).ToArray();
            SongsPosition = 0;
            ImagesPosition = 0;
            SaveImagesPosition = -1;
            SaveSongsPosition = -1;
            IsSongs = Songs.Length > 0;
        }

        public void RefreshPosition()
        {
            SongsPosition = 0;
            ImagesPosition = 0;
            SaveSongsPosition = -1;
            SaveImagesPosition = -1;
        }

        public void SavePosition()
        {
            SaveImagesPosition = ImagesPosition;
            SaveSongsPosition = SongsPosition;
        }

        public void SaveNextPosition()
        {
            SaveImagesPosition = ImagesPosition;
            SaveSongsPosition = SongsPosition;
            if (SaveImagesPosition == Songs[SaveSongsPosition].Images.Length - 1)
            {
                SaveSongsPosition++;
                if (SaveSongsPosition == Songs.Length)
                {
                    SaveSongsPosition = Songs.Length - 1;
                    return;
                }
                SaveImagesPosition = 0;
            }
            else
                SaveImagesPosition++;
        }

        public void SavePrevPosition()
        {
            SaveImagesPosition = ImagesPosition;
            SaveSongsPosition = SongsPosition;
            if (SaveImagesPosition == 0)
            {
                SaveSongsPosition--;
                if (SaveSongsPosition < 0)
                {
                    SaveSongsPosition = 0;
                    return;
                }
                SaveImagesPosition = Songs[SaveSongsPosition].Images.Length - 1;
            }
            else
                SaveImagesPosition--;
        }

        public void LoadPosition()
        {
            if (SaveSongsPosition != -1 && SaveImagesPosition != -1)
            {
                ImagesPosition = SaveImagesPosition;
                SongsPosition = SaveSongsPosition;
                SaveSongsPosition = -1;
                SaveImagesPosition = -1;
            }
        }

        public SongImageInfo Select()
        {
            return Songs[SongsPosition].GetInfo(ImagesPosition);
        }

        public SongImageInfo Next()
        {
            if (ImagesPosition == Songs[SongsPosition].Images.Length - 1)
            {
                SongsPosition++;
                if (SongsPosition == Songs.Length)
                {
                    SongsPosition = Songs.Length - 1;
                    return SongImageInfo.GetNullInfo();
                }
                ImagesPosition = 0;
            }
            else
                ImagesPosition++;
            return Songs[SongsPosition].GetInfo(ImagesPosition);
        }

        public SongImageInfo Prev()
        {
            if (ImagesPosition == 0)
            {
                SongsPosition--;
                if (SongsPosition < 0)
                {
                    SongsPosition = 0;
                    return SongImageInfo.GetNullInfo();
                }
                ImagesPosition = Songs[SongsPosition].Images.Length - 1;
            }
            else
                ImagesPosition--;
            return Songs[SongsPosition].GetInfo(ImagesPosition);
        }

        public SongImageInfo NextSong(string name)
        {
            int index = Songs.ToList().FindIndex(x => x.GetName() == name);
            if (index == -1)
                return SongImageInfo.GetNullInfo();
            index++;
            if (index == Songs.Length - 1)
                index = 0;
            return Songs[index].GetInfo(0);
        }

        public bool CanGoNext()
        {
            if (Songs.Length == 0)
                return false;
            if (ImagesPosition + 1 == Songs[SongsPosition].Images.Length && SongsPosition + 1 == Songs.Length)
                return false;
            return true;
        }

        public bool CanGoPrev()
        {
            if (Songs.Length == 0)
                return false;
            if (ImagesPosition == 0 && SongsPosition == 0)
                return false;
            return true;
        }

        public OsuControl Search(string search)
        {
            if (string.IsNullOrWhiteSpace(search))
            {
                RefreshPosition();
                return this;
            }
            search = search.ToLower();
            return new OsuControl(Songs.Where(x => x.GetName().ToLower().Contains(search)).ToArray());
        }
    }
}

