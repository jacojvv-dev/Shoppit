using Nest;

namespace ApplicationCore.Models
{
    public class ElasticProductMetadata
    {
        [Keyword] 
        public string Key { get; set; }
        [Keyword]
        public string Value { get; set; }

        public ElasticProductMetadata()
        {
        }

        public ElasticProductMetadata(string key, string value)
        {
            Key = key;
            Value = value;
        }
    }
}