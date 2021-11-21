using System;

namespace ApplicationCore.Helpers
{
    public static class ElasticSearchIndexHelper
    {
        public static string GetProductIndexName(string prefix)
        {
            if (string.IsNullOrWhiteSpace(prefix)) throw new ArgumentNullException(nameof(prefix));
            return $"{prefix}-products";
        }
    }
}