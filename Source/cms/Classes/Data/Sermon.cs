using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace Csbs.Classes.Data
{
    public class Sermon
    {
        public String AudioUrl { get; set; }
        public DateTime Date { get; set; }
        public String Speaker { get; set; }
        public String Synopsis { get; set; }
        public String PhotoUrl { get; set; }

        public String Semester
        {
            get
            {
                StringBuilder sb = new StringBuilder();
                sb.Append(Date.Year);
                sb.Append(Date.Month < 7 ? " Spring" : " Fall");
                return sb.ToString();
            }
        }

        public String Heading { get; set; }
    }

    public class SermonComparer : System.Collections.IComparer
    {
        public int Compare(object x, object y)
        {
            if (x == null || y == null)
                return -1;

            Sermon v1 = x as Sermon;
            Sermon v2 = y as Sermon;

            if (v1.Semester == null && v2.Semester == null)
                return 0;

            return v1.Semester.CompareTo(v2.Semester);
        }
    }
}