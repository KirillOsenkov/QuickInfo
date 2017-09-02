namespace QuickInfo
{
    public class Unit
    {
        public string[] Names { get; set; }

        public Unit(params string[] names)
        {
            this.Names = names;
        }

        public override string ToString()
        {
            return Names[0];
        }
    }
}
