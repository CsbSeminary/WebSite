using System;
using System.Collections.Generic;
using System.IO;
using System.Web;

namespace Csbs.Classes.Data
{
    public static class SermonHelper
    {
        public static List<Sermon> FilterSermons(String semester, IEnumerable<Sermon> list)
        {
            List<Sermon> sermons = new List<Sermon>();

            foreach (var sermon in list)
            {
                if (semester == null || semester == sermon.Semester)
                    sermons.Add(sermon);
            }

            return sermons;
        }

        public static List<Semester> CreateSemesters(IEnumerable<Sermon> sermons, String currentSemester)
        {
            List<Semester> semesters = new List<Semester>();

            foreach (var sermon in sermons)
            {
                Semester semester = new Semester();
                semester.Name = sermon.Semester;
                semester.NavigateUrl = "/cms/pages/viewcontrol.aspx?action=view-chapel&semester=" + HttpUtility.UrlEncode(semester.Name);

                if (currentSemester != semester.Name && !semesters.Contains(semester))
                    semesters.Add(semester);
            }

            return semesters;
        }

        public static List<Sermon> GetSermons(String relativePath, String physicalPath)
        {
            var sermons = new List<Sermon>();
            
            var directory = new DirectoryInfo(physicalPath);
            FileInfo[] files = directory.GetFiles("*.mp3");

            Comparison<FileInfo> comparison = (a, b) => String.Compare(a.Name, b.Name) > 0 ? -1 : 1;
            Array.Sort(files, comparison);

            foreach (FileInfo file in files)
            {
                var sermon = new Sermon();
                sermon.AudioUrl = relativePath + file.Name;
                String indexPath = String.Format("{0}{1}", physicalPath, file.Name.Replace(".mp3", ".txt"));
                GetSermonInfo(sermon, indexPath);
                sermons.Add(sermon);
            }

            return sermons;
        }

        private static void GetSermonInfo(Sermon sermon, String path)
        {
            if (!File.Exists(path))
                return;

            String[] lines = File.ReadAllLines(path);
            foreach (String line in lines)
            {
                String[] tokens = line.Split(new[] { '=' }, StringSplitOptions.RemoveEmptyEntries);
                if (tokens.Length == 2)
                    SetPropertyValue(sermon, tokens[0].Trim(), tokens[1].Trim());
            }
        }

        private static void SetPropertyValue(Sermon sermon, String name, String value)
        {
            switch (name)
            {
                case "Date":
                    DateTime date;
                    if (DateTime.TryParse(value, out date))
                        sermon.Date = date;
                    else
                        sermon.Date = DateTime.Today;
                    break;

                case "Speaker":
                    sermon.Speaker = value;
                    break;
                case "Synopsis":
                    sermon.Synopsis = value;
                    break;
                case "Photo":
                    sermon.PhotoUrl = value;
                    break;
                case "Heading":
                    sermon.Heading = value;
                    break;
            }
        }
    }
}