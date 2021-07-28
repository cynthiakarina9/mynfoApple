namespace mynfo.Domain
{
    using Newtonsoft.Json;
    using System.Collections.Generic;

    public class UserType
    {
        public int UserTypeId { get; set; }

        public string Name { get; set; }

        [JsonIgnore]

        public virtual ICollection<User> Users { get; set; }
    }
}