using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace Algorithm_Visualizer
{
    public static class Constants
    {
        /// <summary>
        /// RedrawDelay
        /// </summary>
        public const int RedrawDelay = 7;

        /// <summary>
        /// Bar width in px
        /// </summary>
        public static int BarWidth { get; set; } = 10;

        /// <summary>
        /// Bar margin in px
        /// </summary>
        public const int BarMargin = 5;



        public const int InitialArrLen = 30;
    }
}
