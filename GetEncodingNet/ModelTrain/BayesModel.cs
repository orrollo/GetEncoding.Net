using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Policy;
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

        public List<ScriptCommand> ToScript()
        {
            var ret = new List<ScriptCommand>();

            Func<double, double> pp = x => Math.Log(x/(1.0 - x));

            ret.Add(new ScriptCommand("start"));
            ret.Add(new ScriptCommand("set_ngram", CodeSize));
            // init of rates table
            ret.Add(new ScriptCommand("reset_rates"));
            foreach (var pair in P0) ret.Add(new ScriptCommand("init_rate", pair.Key, pp(pair.Value)));

            // main procedure
            ret.Add(new ScriptCommand("begin_loop","lp000"));
            ret.Add(new ScriptCommand("get_ngram"));
            ret.Add(new ScriptCommand("if_break", "==", -1));
            // comparing ngram
            ret.Add(new ScriptCommand("begin_switch", "ngram"));
            foreach (var pair in Codes)
            {
                ret.Add(new ScriptCommand("begin_case", pair.Key));
                foreach (var info in pair.Value) ret.Add(new ScriptCommand("inc_rate", info.Key, pp(info.Value)));
                ret.Add(new ScriptCommand("end_case", pair.Key));
            }
            ret.Add(new ScriptCommand("end_switch", "ngram"));

            //var cds = Codes.Keys.ToArray();
            //Array.Sort(cds);

            //Action<int, int> rec = null;

            //rec = (l, r) =>
            //{
            //    if ((r - l + 1) > 4)
            //    {
            //        int m = (l + r) >> 1;
            //        ret.Add(new ScriptCommand("if_begin", "<", cds[m]));
            //        rec(l, m - 1);
            //        ret.Add(new ScriptCommand("if_else"));
            //        rec(m, r);
            //        ret.Add(new ScriptCommand("if_end"));
            //    }
            //    else
            //    {
            //        for (int i = l; i <= r; i++)
            //        {
            //            var code = cds[i];
            //            var dic = Codes[code];
            //            ret.Add(new ScriptCommand("if_begin", "==", code));
            //            foreach (var info in dic) ret.Add(new ScriptCommand("inc_rate", info.Key, pp(info.Value)));
            //            ret.Add(new ScriptCommand("if_end"));
            //        }
            //    }
            //};

            //rec(0, cds.Length - 1);

            ret.Add(new ScriptCommand("end_loop","lp000"));
            ret.Add(new ScriptCommand("convert_rates"));
            ret.Add(new ScriptCommand("find_best_rate"));
            ret.Add(new ScriptCommand("finish"));
            return ret;
        }
    }
}