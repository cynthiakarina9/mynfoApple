namespace mynfo.Domain
{
    using Newtonsoft.Json;
    public class Box_ProfileSM
    {
        public int Box_ProfileSMId { get; set; }

        public int BoxId { get; set; }
        [JsonIgnore]
        public virtual Box Box { get; set; }

        public int ProfileMSId { get; set; }

        [JsonIgnore]
        public virtual ProfileSM ProfileSM { get; set; }
    }
}