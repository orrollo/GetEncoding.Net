using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace ModelTrain
{
    public static class SrciptToCs
    {
        private static int _level = 0;

        public static string Fmt(this string msg, params object[] args)
        {
            return args == null || args.Length == 0 ? msg : string.Format(msg, args);
        }

        static StringBuilder sb = new StringBuilder();

        public static void Line(this List<string> ret, string msg, params object[] args)
        {
            sb.Clear();
            if (_level > 0) for (int i = 0; i < _level; i++) sb.Append("  ");
            sb.Append(Fmt(msg, args));
            ret.Add(sb.ToString());
        }

        public static List<string> Process(List<ScriptCommand> script, string fnName = "process")
        {
            var ret = new List<string>();
            var unknown = new HashSet<string>();
            _level = 0;

            int ngsize = 1;

            foreach (var command in script)
            {
                var name = command.Name;
                if (name == "start")
                {
                    ret.Line("public static void {0}(byte[] bytes, out int codePage, out double prob) {{".Fmt(fnName));
                    _level++;
                    ret.Line("int ngsize, ngram, idx = 0;");
                }
                else if (name == "finish")
                {
                    _level--;
                    ret.Line("}");
                }
                else if (name == "begin_switch")
                {
                    var op = (string)command.Args[0];
                    ret.Line("switch({0}) {{", op);
                    _level++;
                }
                else if (name == "end_switch")
                {
                    _level--;
                    ret.Line("}");
                }
                else if (name == "begin_case")
                {
                    var op = (int)command.Args[0];
                    ret.Line("case {0}:", op);
                    _level++;
                }
                else if (name == "end_case")
                {
                    ret.Line("break;");
                    _level--;
                }
                else if (name == "set_ngram")
                {
                    ngsize = (int)command.Args[0];
                    ret.Line("ngsize={0};", ngsize);
                }
                else if (name == "reset_rates")
                {
                    ret.Line("var rates = new Dictionary<int,double>();");
                }
                else
                {
                    if (name == "init_rate")
                    {
                        ret.Line("rates[{0}] = {1};", command.Args[0], ((double)command.Args[1]).ToString(CultureInfo.InvariantCulture));
                    }
                    else if (name == "begin_loop")
                    {
                        ret.Line("while (true) {");
                        _level++;
                    }
                    else if (name == "end_loop")
                    {
                        ret.Line("idx++;");
                        _level--;
                        ret.Line("}");
                    }
                    else if (name == "get_ngram")
                    {
                        ret.Line("if ((idx+ngsize)<=bytes.Length) {");
                        _level++;
                        ret.Line("ngram=0;");
                        for (int i = 0; i < ngsize; i++) ret.Line("ngram=(ngram<<8)+bytes[idx+{0}];", i);
                        _level--;
                        ret.Line("} else {");
                        _level++;
                        ret.Line("ngram=-1;");
                        _level--;
                        ret.Line("}");
                    }
                    else if (name == "if_break")
                    {
                        var op = (string) command.Args[0];
                        var arg = (int)command.Args[1];
                        ret.Line("if (ngram{0}{1}) break;", op, arg);
                    }
                    else if (name == "if_begin")
                    {
                        var op = (string)command.Args[0];
                        var arg = (int)command.Args[1];
                        ret.Line("if (ngram{0}{1}) {{", op, arg);
                        _level++;
                    }
                    else if (name == "if_else")
                    {
                        _level--;
                        ret.Line("} else {");
                        _level++;
                    }
                    else if (name == "if_end")
                    {
                        _level--;
                        ret.Line("}");
                    }
                    else if (name == "inc_rate")
                    {
                        var cp = (int)command.Args[0];
                        var value = (double)command.Args[1];
                        ret.Line("rates[{0}]+={1};", cp, value.ToString(CultureInfo.InvariantCulture));
                    }  
                    else if (name == "find_best_rate")
                    {
                        ret.Line("codePage = -1;");
                        ret.Line("prob=0.0;");
                        ret.Line("foreach (var pair in rates)");
                        ret.Line("{");
                        _level++;
                        ret.Line("if (codePage != -1 && rates[codePage] >= pair.Value) continue;");
                        ret.Line("codePage = pair.Key;");
                        _level--;
                        ret.Line("}");
                        ret.Line("if (codePage != -1) prob=rates[codePage];");
                    } 
                    else if (name == "convert_rates")
                    {
                        ret.Line("var keys = rates.Keys.ToArray();");
                        ret.Line("var pSum = 0.0;");
                        ret.Line("foreach (var key in keys)");
                        ret.Line("{");
                        _level++;
                        ret.Line("var x = Math.Exp(Math.Min(100.0, rates[key]));");
                        ret.Line("var p = x / (1.0 + x);");
                        ret.Line("pSum += p;");
                        ret.Line("rates[key] = p;");
                        _level--;
                        ret.Line("}");
                        ret.Line("foreach (var key in keys) rates[key] /= pSum;");
                    }
                    else
                    {
                        if (!unknown.Contains(name)) unknown.Add(name);
                    }
                }
            }

            foreach (var name in unknown)
            {
                ret.Add(string.Format("// unknown: {0}", name));
            }

            return ret;
        }
    }
}