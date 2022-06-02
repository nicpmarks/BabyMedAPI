using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;


namespace BabyMedsAPI.Models
{
	public class Medicine
    {
		[Key]
    	[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string Name { get; set; }
		
        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
}