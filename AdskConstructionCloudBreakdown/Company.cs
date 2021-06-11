namespace AdskConstructionCloudBreakdown
{
    public class Company
    {
        public string Name { get; set; }

        public TradeEnum Trade { get; set; }

        //Constructor
        public Company()
        {
            Name = string.Empty;
        }

        public Company(string name)
        {
            this.Name = name;
        }

        public Company(string name, string trade)
        {
            this.Name = name;
            this.Trade = Selection.SelectTrade(trade);
        }
    }
}