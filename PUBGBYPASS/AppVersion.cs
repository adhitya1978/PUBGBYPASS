using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace PUBGBYPASSER
{
    public enum ReleaseLevel
    {
        Alpha,
        Beta,
        ReleaseCandidate,
        Stable
    }
    
    public class AppVersion
    {
        int major = 1;
        int minor = 0;
        int revision = 0;
        int build = 1;
        ReleaseLevel releaseLevel;


        public AppVersion(int Major, int Minor, int Revision, int Build, ReleaseLevel level)
        {
            major = Major;
            minor = Minor;
            revision = Revision;
            build = Build;
            releaseLevel = level;
        }

        public System.Collections.Hashtable BuildVersion(string ComparisonPattern)
        {
            string[] patern = ComparisonPattern.Split(',');
            System.Collections.Hashtable version = new System.Collections.Hashtable();

            foreach (string name in patern)
            {
                switch (name)
                {
                    case "minor": version.Add("minor", this.major); break;
                    case "major": version.Add("major", this.major); break;
                    case "revision": version.Add("revision", this.major); break;
                    case "build": version.Add("build", this.major); break;
                }
            }
            return version;
        }

        public string GetVersion
        {
            get {
                return new Version(major, minor, build, revision).ToString();
            }
        }
    }
}
