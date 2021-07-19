using Foundation;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UIKit;

namespace mynfoApple.Domain
{
    public class UserType
    {
        public int UserTypeId { get; set; }

        public string Name { get; set; }

        [JsonIgnore]

        public virtual ICollection<User> Users { get; set; }
    }
}