using System;

namespace ModelTrain
{
    public class ScriptCommand
    {
        public string Name { get; set; }
        public object[] Args { get; set; }

        public ScriptCommand(string name, params object[] args)
        {
            Name = name.Trim().ToLower();
            Args = args ?? new object[0];
        }

        public override string ToString()
        {
            return Args.Length == 0 ? Name : String.Format("{0}({1})", Name, String.Join("; ", Args));
        }
    }
}