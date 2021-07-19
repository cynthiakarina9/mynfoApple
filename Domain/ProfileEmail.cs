namespace mynfoApple.Domain
{
    using Newtonsoft.Json;
    using System.Collections.Generic;
    public class ProfileEmail
    {
        public int ProfileEmailId { get; set; }
        
        public string Name { get; set; }
        
        public string Email { get; set; }

        public int UserId { get; set; }

        [JsonIgnore]
        public virtual User User { get; set; }

        [JsonIgnore]
        public virtual ICollection<Box_ProfileEmail> Box_ProfileEmail { get; set; }

        [JsonIgnore]
        public virtual bool Exist { get; set; }
    }
}