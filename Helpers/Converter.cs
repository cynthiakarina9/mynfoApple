using Foundation;
using mynfoApple.Domain;
using mynfoApple.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UIKit;

namespace mynfoApple.Helpers
{
    public class Converter
    {
        #region User
        public static UserLocal ToUserLocal(User _user)
        {
            return new UserLocal
            {
                Email = _user.Email,
                FirstName = _user.FirstName,
                ImagePath = _user.ImagePath,
                LastName = _user.LastName,
                UserId = _user.UserId,
                UserTypeId = _user.UserTypeId,
                Share = _user.Share,
                Edad = _user.Edad,
                Ubicacion = _user.Ubicacion,
                Ocupacion = _user.Ocupacion,
                Conexiones = _user.Conexiones,
                Fecha = _user.Fecha
            };
        }
        #endregion
    }
}