using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Text;
using CsvHelper;
using CsvHelper.Configuration;

namespace AdskConstructionCloudBreakdown
{
    public class SerializationParser
    {
        public SerializationParser()
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public List<Bim360Project> LoadBim360ProjectsFromCsv()
        {
            return new List<Bim360Project>();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public List<Bim360Project> LoadBim360ProjectsFromJSON()
        {
            throw new NotImplementedException("this is openSource. Do it yourself. ");
        }

    }
}