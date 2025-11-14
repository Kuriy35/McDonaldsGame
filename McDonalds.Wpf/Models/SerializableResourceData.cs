using System.Collections.Generic;
using System.Runtime.Serialization;

namespace McDonalds.Models.Core
{
    [DataContract]
    public class SerializableResourceData
    {
        [DataMember]
        public Dictionary<string, int> Resources { get; set; } = new Dictionary<string, int>();
    }
}