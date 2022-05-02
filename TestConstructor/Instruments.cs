using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestSystemLibrary;

namespace TestConstructor
{
    public class Instruments
    {
        public static Test test { get; set; }
        public static bool isSaved { get; set; }
        public static List<string> testsNames { get; set; }
        public static bool isOpened { get; set; }
        public static string pathToFolder { get; set; }
        public static string currentPathToTest { get; set; }
        public static bool IsFormEmpty { get; set; }
    }
}
