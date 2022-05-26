using Newtonsoft.Json;

namespace BabyMedsAPI.Models
{
	public class Medicine
    {
        [JsonProperty(PropertyName = "id")]
        public string Id { get; set; }
        public string Name { get; set; }
		
        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
}