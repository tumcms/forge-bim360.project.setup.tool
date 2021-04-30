namespace AdskConstructionCloudBreakdown
{
    public class Company
    {
        private string Name { get; set; }
        private string Trade { get; set; }

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