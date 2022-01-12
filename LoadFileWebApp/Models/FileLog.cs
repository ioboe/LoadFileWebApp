using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LoadFileWebApp.Models
{
    public class FileLog
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public TimeSpan Durtation { get; set; }
        public DateTime StartTime { get; set; }
        public string Status { get; set; }
    }
}