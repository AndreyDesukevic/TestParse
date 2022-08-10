using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace TestTaskParse
{
    public class TxtReader
    {
        public async Task<List<string>> Read(string path)
        {
            List<string> result = new List<string>();

            using (FileStream fileStream = File.OpenRead(path))
            {
                using (StreamReader reader = new StreamReader(fileStream))
                {
                    string line;
                    while ((line = await reader.ReadLineAsync()) != null)
                    {
                        result.Add(line);
                    }
                }
                return result;
            }
        }

    }
}
