using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ImageWebService.Models
{
    public class Photo
    {
        public Photo(string iRelPath, string tRelPath, string name, string year, string month, string details)
        {
            ImagePath = iRelPath;
            ThumbnailPath = tRelPath;
            Name = name;
            Year = year;
            Month = month;
            Details = details;     
        }
        public string ThumbnailPath { get; set; }
        public string ImagePath { get; set; }
        public string Name { get; set; }
        public string Year { get; set; }
        public string Month { get; set; }
        public string Details { get; set; }
    }
}