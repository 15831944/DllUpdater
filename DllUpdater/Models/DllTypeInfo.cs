using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DllUpdater.Models
{
    public class DllTypeInfo
    {
        public DllType DllType { get; set; }
        public bool Enable { get; set; }
        public string Filename { get; set; }
        public string CheckUrl { get; set; }
        public string XPath { get; set; }
        public string XPathData { get; set; }
        public string DownloadUrl { get; set; }
    }
}
