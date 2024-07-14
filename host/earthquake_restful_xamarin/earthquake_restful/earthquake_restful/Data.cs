using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace earthquake_restful
{
    class Data
    {
        /*class to encapsulate the variables to be called by RecycleViewAdapter*/
        
        public string encap_viewDate { get; set; }
        public string encap_viewTime { get; set; }
        public string encap_viewInt { get; set; }

    }
}