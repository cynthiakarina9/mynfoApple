namespace mynfoApple.Domain
{
    using Newtonsoft.Json;
    using System.Collections.Generic;
    public class ProfileSM
    {        
        public int ProfileMSId { get; set; }

        public string link { get; set; }

        public string ProfileName { get; set; }

        public int UserId { get; set; }

        [JsonIgnore]
        public virtual User User { get; set; }

        public int RedSocialId { get; set; }

        [JsonIgnore]
        public virtual RedSocial RedSocial { get; set; }

        [JsonIgnore]
        public virtual ICollection<Box_ProfileSM> Box_ProfileSM { get; set; }

        [JsonIgnore]
        public virtual bool Exist { get; set; }
    }
}