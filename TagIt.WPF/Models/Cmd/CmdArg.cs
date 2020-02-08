namespace TagIt.WPF.Models.Cmd
{
    public struct CmdArg
    {
        public CmdArg(string name, string value)
        {
            Name = name;
            Value = value;
        }

        public string Name { get; private set; }
        public string Value { get; private set; }

        public static CmdArg Named(string name) => new CmdArg(name, null);
        public static CmdArg Valued(string value) => new CmdArg(null, value);
    }
}
