namespace mynfo.Domain
{
    using Newtonsoft.Json;
    public class UserTags
    {
        public int UserTagsId { get; set; }

        public int UserId { get; set; }

        [JsonIgnore]
        public virtual User User { get; set; }

        public int TagId { get; set; }
        
        public string Name { get; set; }
    }
}