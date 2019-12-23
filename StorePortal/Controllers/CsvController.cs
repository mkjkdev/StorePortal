using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using LumenWorks.Framework.IO.Csv;
using System.IO;
using System.Collections;
using System.Linq;
using System;

namespace csvIO.Controllers
{
    [ApiController]
    public class CsvController : ControllerBase
    {
        private readonly ILogger<CsvController> _logger;

        public CsvController(ILogger<CsvController> logger)
        {
            _logger = logger;
        }

        //test method

        //API method that returns Csv object
        [Route("csv")]
        [HttpGet]
        public List<Csv> readCSV()
        {
            List<Csv> myList = new List<Csv>();
            using (CsvReader csv = new CsvReader(new StreamReader("uploadedFiles.csv"), true))
            {
                //replace missing field with "" empty string
                csv.MissingFieldAction = MissingFieldAction.ReplaceByEmpty;

                //gets max fields for each record
                int fieldCount = csv.FieldCount;

                //while has net record
                while (csv.ReadNextRecord())
                {
                    //new object of csv
                    Csv temp = new Csv(fieldCount);
                    //New list to store coulumn names
                    String[] headers = csv.GetFieldHeaders();
                    //iterate over column data in row
                    //add the data to temp object
                    for (int i = 0; i < fieldCount; i++)
                    {
                        temp.data.Add(csv[i]);
                        temp.titles.Add(headers[i]);
                    }

                    myList.Add(temp);
                }
                return myList;
            }
        }
    }
}
