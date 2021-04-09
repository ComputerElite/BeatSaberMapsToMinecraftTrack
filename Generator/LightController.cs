using System.Collections.Generic;

namespace BeatSaber
{
    public class LightController
    {
        public string leftLightsColor = "red";
        public string rightLightsColor = "blue";
        public string ringLightsColor = "blue";
        public string centerLightsColor = "blue";
        public string backLightsColor = "blue";
        public int lastLeftLights = 0;
        public int lastRightLights = 0;
        public int lastRingLights = 0;
        public int lastCenterLights = 0;
        public int lastBackLights = 0;
        public static List<int> redValues = new List<int>() { 5, 6, 7 };
        public static List<int> blueValues = new List<int>() { 2, 3, 4 };
        public static List<int> flashValues = new List<int>() { 3, 7 };
        public static List<int> turnOnValues = new List<int>() { 1, 2, 5, 6 };

        
    }
}