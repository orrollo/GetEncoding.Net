using System;
using System.IO;

namespace ModelTrain
{
    public static class NGramHelper
    {
        public static int ForEachNGram(MemoryStream stream, int ngSize, Action<int, int> ngProc)
        {
            stream.Position = 0;
            Func<int> readByte = () => stream.Position < stream.Length ? stream.ReadByte() : -1;
            return ForEachNGram(ngSize, readByte, ngProc);
        }

        public static int ForEachNGram(int ngSize, Func<int> readByte, Action<int, int> ngProc)
        {
            int idx = 0, mask = (1 << (8 * ngSize)) - 1, cur = 0;
            while (true)
            {
                var bt = readByte();
                if (bt == -1) break;
                idx++;
                cur = ((cur << 8) & mask) | bt;
                if (idx >= ngSize) ngProc(ngSize, cur);
            }
            var count = Math.Max(0, idx - ngSize + 1);
            return count;
        }
    }    
}

