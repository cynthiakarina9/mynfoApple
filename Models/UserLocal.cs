namespace mynfo.Models
{
    using SQLite;
    using System;

    public class UserLocal
    {
        [PrimaryKey]
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

        public DateTime Fecha { get; set; }

        public string Password { get; set; }

        public bool MostrarTutorial { get; set; }

        public string ImageFullPath
        {
            get
            {
                if (string.IsNullOrEmpty(ImagePath))
                {
                    return "no_image";
                }

                if (this.UserTypeId == 1)
                {
                    return string.Format(
                        "https://mynfoapi.azurewebsites.net/{0}",
                        ImagePath.Substring(1));
                }

                return ImagePath;
            }
        }

        public string FullName
        {
            get
            {
                return string.Format("{0} {1}", this.FirstName, this.LastName);
            }
        }

        public override int GetHashCode()
        {
            return UserId;
        }
    }
}