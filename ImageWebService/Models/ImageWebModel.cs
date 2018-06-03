using ImageWebService.Communication;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ImageWebService.Models
{
    public class ImageWebModel
    {

        public ImageWebModel()
        {
            Active = ClientConnection.Connect();
        }

        public WebChannel ClientConnection
        {
            get
            {
                return WebChannel.Instance;
            }
        }

        [Required]
        [Display(Name = "LastName")]
        public Boolean Active { get; set; }
    }
}