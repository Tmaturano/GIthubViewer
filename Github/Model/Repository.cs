using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Github.Model
{
    public class Repository
    {
        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }

        [JsonProperty(PropertyName = "full_name")]
        public string FullName { get; set; }

        [JsonProperty(PropertyName = "description")]
        public string Description { get; set; }

        [JsonProperty(PropertyName = "fork")]
        public bool Forked { get; set; }

        public string CompleteDescription
        {
            get
            {
                return Forked ? $"{FullName} - Forked" : FullName;
            }
        }
    }
}
