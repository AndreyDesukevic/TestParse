using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text.Json.Serialization;

namespace TestTaskParse
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var txtReader = new TxtReader();
            var searchWords = txtReader.Read(Path.GetFullPath("../../../Keys/Keys.txt")).Result;
            var dataList = new List<Rootobject>();
            var getRequest = new GetRequest();
            foreach (var word in searchWords)
            {
                getRequest.Run(word);
                var rootobject = JsonConvert.DeserializeObject<Rootobject>(getRequest.Response);
                dataList.Add(ChangePrices(rootobject));
            }

            try
            {
                using (var helper = new ExcelHelper())
                {
                    if(helper.Open(filePath: Path.GetFullPath("../../../Excel/ParseTest.xlsx")))
                    {
                        helper.CreateWorkSheets(searchWords);
                        foreach (var data in dataList)
                        {
                            var workSheet = helper.GetWorksheetByName(data.metadata.name);
                            helper.Set(data.data, workSheet);
                        }

                        helper.Save();
                    }
                }
           
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
           
        }
        public static Rootobject ChangePrices(Rootobject rootobject)
        {
            rootobject.data.products.ForEach(x =>
            {
                x.priceU = int.Parse(x.priceU.ToString().Substring(0, x.priceU.ToString().Length - 2));
            });

            return rootobject;
        }

    }
}
