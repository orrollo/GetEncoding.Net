using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace SimplifiedApp
{
    class CharDictionary : Dictionary<char,int>
    {
        
    }

    class Program
    {
        static void Main(string[] args)
        {
            var dic = new Dictionary<char, CharDictionary>();
            var cnt = new CharDictionary();

            int totalCount = 0;

            var files = Directory.GetFiles(@"..\\..\\..\\texts", "*.txt");
            var enc = Encoding.GetEncoding(1251);
            foreach (var file in files)
            {
                var txt = File.ReadAllText(file, enc).ToLower();
                for (int i = 1; i < txt.Length; i++)
                {
                    char c1 = txt[i - 1], c2 = txt[i];
                    if (!IsAplha(c1) || !IsAplha(c2)) continue;
                    if (!dic.ContainsKey(c1)) dic[c1] = new CharDictionary();
                    if (!dic[c1].ContainsKey(c2)) dic[c1][c2] = 0;
                    dic[c1][c2]++;

                    if (!cnt.ContainsKey(c1)) cnt[c1] = 0;
                    cnt[c1]++;

                    totalCount++;
                }
            }
            //
            var border = totalCount / 100;
            // generate code for the tree
            var sb = new StringBuilder();
            sb.AppendLine("public static int MarkBigram(char c1,char c2)");
            sb.AppendLine("{");
            sb.AppendLine("  var mark = 0;");

            var isFirstC1 = true;
            var keysC1 = GetKeys(cnt);
            var limitNumber = 15;

            for (var idx1 = 0; idx1 < keysC1.Count; idx1++)
            {
                var c1 = keysC1[idx1];
                sb.AppendLine(string.Format("  {1}if (c1=='{0}') // cnt={2}", c1, isFirstC1 ? "" : "else ", cnt[c1]));
                sb.AppendLine("  {");
                var isFirstC2 = true;
                var keysC2 = GetKeys(dic[c1]);
                var markLimit = cnt[c1] / 100;
                for (var idx2 = 0; idx2 < keysC2.Count; idx2++)
                {
                    //if (idx2 >= limitNumber) break;
                    var c2 = keysC2[idx2];
                    var value = dic[c1][c2];
                    if (markLimit > value) break;
                    sb.AppendLine(string.Format("    {2}if (c2=='{0}') mark = {1};", c2, value, isFirstC2 ? "" : "else "));
                    isFirstC2 = false;
                }

                sb.AppendLine("  }");
                isFirstC1 = false;
            }

            sb.AppendLine("  return mark;");
            sb.AppendLine("}");

            File.WriteAllText("marks.cs", sb.ToString());
        }

        private static List<char> GetKeys(CharDictionary cnt)
        {
            var keysC1 = cnt.Keys.ToList();
            keysC1.Sort((a, b) => cnt[b].CompareTo(cnt[a]));
            return keysC1;
        }

        private static bool IsAplha(char ch)
        {
            return ch >= 'а' && ch <= 'я';
        }
    }
}
