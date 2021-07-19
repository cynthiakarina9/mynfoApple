using Newtonsoft.Json;
using System;

namespace mynfoApple.Domain
{
    public class User
    {
        public int UserId { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Email { get; set; }
        public string ImagePath { get; set; }
        public int UserTypeId { get; set; }

        public bool Share { get; set; }
        public int Edad { get; set; }

        public string Ubicacion { get; set; }

        public string Ocupacion { get; set; }

        public int Conexiones { get; set; }

        public virtual DateTime Fecha { get; set; }

        [JsonIgnore]
        public virtual UserType UserType { get; set; }

        public byte[] ImageArray { get; set; }

        public string Password { get; set; }

        public string ImageFullPath
        {
            get
            {
                if (string.IsNullOrEmpty(ImagePath))
                {
                    return "noimage";
                }
                return string.Format(
                    "https://mynfoapi.azurewebsites.net/{0}",
                    ImagePath.Substring(1));
            }
        }

        public string FullName
        {
            get
            {
                return string.Format("{0} {1}", this.FirstName, this.LastName);
            }
        }
       /* [JsonIgnore]
        public virtual ICollection<ProfileEmail> ProfilePhone { get; set; }

        [JsonIgnore]
        public virtual ICollection<ProfilePhone> ProfileEmail { get; set; }

        [JsonIgnore]
        public virtual ICollection<Box> Box { get; set; }

        [JsonIgnore]
        public virtual ICollection<ProfileSM> ProfileSM { get; set; }*/
    }
}