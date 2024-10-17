namespace ET
{
    public class IpInfo
    {
        public string ip { get; set; }

        public string hostname { get; set; }

        public string city { get; set; }

        public string region { get; set; }

        public string country { get; set; }

        public string loc { get; set; }
        public string org { get; set; }

        public string postal { get; set; }
    }

    public class CountryLanguage
    {
        public const string RankOthers = "Others";
        public const string Indonesia = "Indonesia";
        public const string Ukraine = "Ukraine";
        public const string Spanish = "Spanish";
        public const string India = "India";
    }
}