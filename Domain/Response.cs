using Foundation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UIKit;

namespace mynfoApple.Domain
{
    public class Response
    {
        public bool IsSuccess { get; set; }

        public string Messagge { get; set; }

        public object Result { get; set; }

    }
}