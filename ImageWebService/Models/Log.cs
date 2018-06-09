using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ImageWebService.Models
{
    /// <summary>
    /// Enum for log types.
    /// </summary>
    public enum LogType
    {
        WARNING,
        INFO,
        FAIL
    }

    public class Log
    {
        public Log(string t, string m)
        {
            Type = t;
            Message = m;
        }
        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "Type")]
        public String Type { get; set; }
        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "Message")]
        public String Message { get; set; }
    }
}