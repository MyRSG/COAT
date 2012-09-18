using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace COAT.ViewModel
{
    public class Counter
    {
        public int Time { get; set; }
        public int Count
        {
            get
            {
                return _Count++ / Time;
            }
        }

        private int _Count = 0;
    }
}