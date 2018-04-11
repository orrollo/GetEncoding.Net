using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace ModelTrain
{
    public class BayesModel 
    {
        public int CodeSize { get; set; }
        public Dictionary<int,double> P0 = new Dictionary<int, double>();
        public Dictionary<int,CodeModel> Codes = new Dictionary<int, CodeModel>();

        public BayesModel()
        {
        }

        public BayesModel(BinaryReader reader) : this()
        {
            Load(reader);
        }

        public void Save(Stream stream)
        {
            using (var wrt = new BinaryWriter(stream, Encoding.UTF8, true))
            {
                Save(wrt);
            }
        }

        public void Save(BinaryWriter wrt)
        {
            wrt.Write(CodeSize);
            wrt.Write(P0.Count);
            foreach (var pair in P0)
            {
                wrt.Write(pair.Key);
                wrt.Write(pair.Value);
            }
            wrt.Write(Codes.Count);
            foreach (var pair in Codes)
            {
                wrt.Write(pair.Key);
                var model = pair.Value;
                wrt.Write(model.Count);
                foreach (var pair2 in model)
                {
                    wrt.Write(pair2.Key);
                    wrt.Write(pair2.Value);
                }
            }
            wrt.Flush();
        }

        public void Load(Stream stream)
        {
            using (var rdr = new BinaryReader(stream,Encoding.UTF8,true))
            {
                Load(rdr);
            }
        }

        public void Load(BinaryReader rdr)
        {
            P0.Clear();
            Codes.Clear();
            CodeSize = rdr.ReadInt32();
            int count = rdr.ReadInt32();
            for (int i = 0; i < count; i++)
            {
                int key = rdr.ReadInt32();
                double value = rdr.ReadDouble();
                P0.Add(key, value);
            }
            count = rdr.ReadInt32();
            for (int i = 0; i < count; i++)
            {
                int key = rdr.ReadInt32();
                var codeModel = new CodeModel();
                int count2 = rdr.ReadInt32();
                for (int j = 0; j < count2; j++) codeModel.Add(rdr.ReadInt32(), rdr.ReadDouble());
                Codes[key] = codeModel;
            }
        }

        public Dictionary<int,double> GetMarks(byte[] bytes)
        {
            var ret = new Dictionary<int,double>();

            int pos = 0;
            Func<int> readByte = () =>
            {
                int byteValue = pos < bytes.Length ? bytes[pos] : -1;
                pos++;
                return byteValue;
            };

            foreach (var pair in P0) ret[pair.Key] = Math.Log(pair.Value/(1.0 - pair.Value));
            NGramHelper.ForEachNGram(CodeSize, readByte, (size, value) =>
            {
                if (!Codes.ContainsKey(value)) return;
                foreach (var pair in Codes[value])
                {
                    if (!ret.ContainsKey(pair.Key)) continue;
                    ret[pair.Key] += Math.Log(pair.Value/(1.0 - pair.Value));
                }
            });

            var keys = ret.Keys.ToArray();
            var pSum = 0.0;
            foreach (var key in keys)
            {
                var x = Math.Exp(Math.Min(100.0, ret[key]));
                var p = x / (1.0 + x);
                pSum += p;
                ret[key] = p;
            }
            foreach (var key in keys) ret[key] /= pSum;

            return ret;
        }

        public int GetBestMark(byte[] bytes)
        {
            return GetBestMark(GetMarks(bytes));
        }

        public int GetBestMark(Dictionary<int, double> marks)
        {
            int? ret = null;
            foreach (var pair in marks)
            {
                if (ret != null && marks[ret.Value] >= pair.Value) continue;
                ret = pair.Key;
            }
            return ret ?? -1;
        }
    }
}