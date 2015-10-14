using System.Collections.Generic;
using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace Aditi.GIS.Foundation.Services.Contracts
{
    public class Response<T>
    {
        public Response()
        {
            // this.Links = new List<string>();
        }

        public Response(T result)
            : this()
        {
            this.Result = result;
        }

        public T Result { get; set; }
        // public string Etag { get; set; }
        // [JsonProperty(PropertyName = "links")]
        // [DataMember(Name = "links")]
        // [JsonIgnore]
        // public List<string> Links { get; set; }
        // 
        // public void AddLink(string link)
        // {
        //     this.Links.Add(link);
        // }
    }
}
