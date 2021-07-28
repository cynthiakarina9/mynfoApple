namespace mynfo.Domain
{
    using Newtonsoft.Json;
    using System.Collections.Generic;
    public class ProfileWhatsapp
    {
        public int ProfileWhatsappId { get; set; }
        
        public string Name { get; set; }
        
        public string Number { get; set; }

        public int UserId { get; set; }

        [JsonIgnore]
        public virtual User User { get; set; }

        [JsonIgnore]
        public virtual ICollection<Box_ProfileWhatsapp> Box_ProfileWhatsapp { get; set; }

        [JsonIgnore]
        public virtual bool Exist { get; set; }
    }
}