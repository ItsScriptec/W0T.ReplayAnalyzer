using System;
using System.IO;
using System.Text;
using Newtonsoft.Json.Linq;

namespace W0T.ReplayAnalyzer 
{
    //   C O N V E R T E R
    //   The converter gets the json blocks from the replay file
    //   and return them as ReplayJson object 

    public class Converter
    {
        // variables
        string filePath = "";

        // Constructor
        public Converter(string replayFilePath)
        {
            this.filePath = replayFilePath;
        }

        // Converts the Replay to ReplayData -> JObject(Block 1), JArray(Block 2)
        // and gives the information if the file is OK (1 = no, 2 = yes)
        public ReplayJson ToJson()
        {
            byte[] mybyte = new byte[4];
            byte[] blockOne;
            byte[] blockTwo;

            using (FileStream reader = new FileStream(filePath, FileMode.Open, FileAccess.Read))
            {
                // not important
                reader.Read(mybyte, 0, 4);

                // check if file is ok -> 1 = no / 2 = yes              
                reader.Read(mybyte, 0, 4);
                int isFileOK = BitConverter.ToInt32(mybyte, 0);

                // get the length of the first json Block
                reader.Read(mybyte, 0, 4);
                int lengthBlockOne = BitConverter.ToInt32(mybyte, 0);

                // get the first json Block and convert it to a JObject
                blockOne = new byte[lengthBlockOne];
                reader.Read(blockOne, 0, lengthBlockOne);
                JObject blockOneResult = JObject.Parse(Encoding.UTF8.GetString(blockOne));

                // get the length of the second json Block
                reader.Read(mybyte, 0, 4);
                int lengthBlockTwo = BitConverter.ToInt32(mybyte, 0);

                // get the second json Block and convert it to a JArray
                blockTwo = new byte[lengthBlockTwo];
                reader.Read(blockTwo, 0, lengthBlockTwo);
                JArray blockTwoResult = JArray.Parse(Encoding.UTF8.GetString(blockTwo));

                // return the results as ReplayJson object
                return new ReplayJson()
                {
                    isFileOK = isFileOK,
                    BlockOne = blockOneResult,
                    BlockTwo = blockTwoResult
                };
            }
        }
    }
}
