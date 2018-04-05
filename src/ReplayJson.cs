using Newtonsoft.Json.Linq;

namespace W0T.ReplayAnalyzer
{
    //   R E P L A Y J S O N
    //   The ReplayJson class contains the both
    //   json blocks and the value if the replay file is ok

    public class ReplayJson
    {
        public int isFileOK;
        public JObject BlockOne;
        public JArray BlockTwo;
    }
}
