namespace mynfo.Domain
{
    using Newtonsoft.Json;
    using System.Collections.Generic;

    public class RedSocial
    {        
        public int RedSocialId { get; set; }
                
        public string Name { get; set; }

        [JsonIgnore]
        public virtual ICollection<ProfileSM> ProfileSM { get; set; }
    }
}