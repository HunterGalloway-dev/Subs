using System.Collections.Generic;

namespace SubscriptionCleanup
{
    public class ResourceList
    {
        public Dictionary<Resource, List<string>> Resources { get; set; }
        public ResourceList()
        {
            Resources = new Dictionary<Resource, List<string>>();
        }

        public void Add(Resource resource, string filePath)
        {
            if(Resources.ContainsKey(resource))
            {
                Resources[resource].Add(filePath);
            } else
            {
                Resources.Add(resource, new List<string>() { filePath });
            }
        }
    }
}
