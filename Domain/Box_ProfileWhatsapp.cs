namespace mynfoApple.Domain
{
    using Newtonsoft.Json;
    public class Box_ProfileWhatsapp
    {
        public int Box_ProfileWhatsappId { get; set; }

        public int BoxId { get; set; }
        [JsonIgnore]
        public virtual Box Box { get; set; }

        public int ProfileWhatsappId { get; set; }

        [JsonIgnore]
        public virtual ProfileWhatsapp ProfileWhatsapp { get; set; }
    }
}