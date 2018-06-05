using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ImageWebService.Models
{
    public class Student
    {
        public Student(int id, string f, string l)
        {
            ID = id;
            FirstName = f;
            LastName = l;
        }

        public int ID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
}