using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace AndroidCompound5.BusinessObject.DTOs
{

    public class RootObject
    {
        [JsonProperty("rows")]
        public List<ResultApiDto> Rows { get; set; }
    }


    public class ResultApiDto
    {
        public string pbt { get; set; }
        public string registration { get; set; }
        public string date { get; set; }
        public string start { get; set; }
        public string end { get; set; }
        public string expired { get; set; }
    }
}