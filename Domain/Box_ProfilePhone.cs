namespace mynfoApple.Domain
{
    using Newtonsoft.Json;
    public class Box_ProfilePhone
    {
        public int Box_ProfilePhoneId { get; set; }

        public int BoxId { get; set; }
        [JsonIgnore]
        public virtual Box Box { get; set; }

        public int ProfilePhoneId { get; set; }

        [JsonIgnore]
        public virtual ProfilePhone ProfilePhone { get; set; }
    }
}