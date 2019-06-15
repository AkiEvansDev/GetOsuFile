using System;
using NAudio.Wave;

namespace GetOsuFile
{
    public static class WaveStreamExtensions
    {
        public static void SetPosition(this WaveStream strm, long position)
        {
            long adj = position % strm.WaveFormat.BlockAlign;
            long newPos = Math.Max(0, Math.Min(strm.Length, position - adj));
            strm.Position = newPos;
        }
        
        public static void SetPosition(this WaveStream strm, double seconds)
        {
            strm.SetPosition((long)(seconds * strm.WaveFormat.AverageBytesPerSecond));
        }
        
        public static void SetPosition(this WaveStream strm, TimeSpan time)
        {
            strm.SetPosition(time.TotalSeconds);
        }
        
        public static void Seek(this WaveStream strm, double offset)
        {
            strm.SetPosition(strm.Position + (long)(offset * strm.WaveFormat.AverageBytesPerSecond));
        }

        public static string GetMinutesAndSecondsString(TimeSpan timeSpan)
        {
            string res;
            int m = timeSpan.Minutes;
            int s = timeSpan.Seconds;
            if (m > 9)
                res = m.ToString() + ":";
            else
                res = "0" + m.ToString() + ":";
            if (s > 9)
                res += s.ToString();
            else
                res += "0" + s.ToString();
            return res;
        }

        public static string GetMinutesAndSecondsString(double seconds)
        {
            TimeSpan time = new TimeSpan(0, 0, (int)seconds);
            return GetMinutesAndSecondsString(time);
        }

        public static string GetCurrentTimeString(this WaveStream strm)
        {
            return GetMinutesAndSecondsString(strm.CurrentTime);
        }

        public static string GetTotalTimeString(this WaveStream strm)
        {
            return GetMinutesAndSecondsString(strm.TotalTime);
        }
    }
}
