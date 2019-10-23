using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Tesseract;

namespace WebApplication6.Classes
{
    public class Field
    {
        public Rect rect { get; set; }
        public bool specialTreatment { get; set; }
        public string xmlField { get; set; }
        public bool isCross { get; set; }
        public string value { get; set; }
        public bool CrossNo { get; set; }

        public Field()
        {
            specialTreatment = false;
            xmlField = "";
            isCross = false;
            value = "";
            CrossNo = false;
        }
    }
}