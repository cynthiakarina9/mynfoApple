namespace mynfo.Domain
{
    using Newtonsoft.Json;
    using System;
    using System.Collections.Generic;
    public class Box
    {
        public int BoxId { get; set; }
        
        public string Name { get; set; }

        public bool BoxDefault { get; set; }

        public int UserId { get; set; }

        public virtual DateTime Time { get; set; }

        public string ColorBox { get; set; }

        [JsonIgnore]
        public virtual User User { get; set; }

        [JsonIgnore]
        public virtual ICollection<Box_ProfileSM> Box_ProfileSM { get; set; }

        [JsonIgnore]
        public virtual ICollection<Box_ProfileEmail> Box_ProfileEmail { get; set; }

        [JsonIgnore]
        public virtual ICollection<Box_ProfilePhone> Box_ProfilePhone { get; set; }

        [JsonIgnore]
        public virtual ICollection<Box_ProfileWhatsapp> Box_ProfileWhatsapp { get; set; }
    }
}