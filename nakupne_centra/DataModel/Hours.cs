namespace nakupne_centra.DataModel
{
    public class Hours
    {
        public Hours(string monday, string tuesday, string wednesday, string thursday, string friday, string saturday, string sunday)
        {
            this.Monday = monday;
            this.Tuesday = tuesday;
            this.Wednesday = wednesday;
            this.Thursday = thursday;
            this.Friday = friday;
            this.Saturday = saturday;
            this.Sunday = sunday;
        }

        public string Monday { get; private set; }
        public string Tuesday { get; private set; }
        public string Wednesday { get; private set; }
        public string Thursday { get; private set; }
        public string Friday { get; private set; }
        public string Saturday { get; private set; }
        public string Sunday { get; private set; }
    }
}
