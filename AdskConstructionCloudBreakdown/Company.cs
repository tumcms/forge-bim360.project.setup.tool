namespace AdskConstructionCloudBreakdown
{
    public class Company
    {
        public string Name { get; set; }

        //ToDo: adjust Trade to tradeenum
        public string Trade { get; set; }

        //Constructor
        public Company()
        {
            Name = string.Empty;
            Trade = string.Empty;

        }

        public Company(string name)
        {
            this.Name = name;
        }

        public Company(string name, string trade)
        {
            this.Name = name;
            this.Trade = trade;
        }
    }
}