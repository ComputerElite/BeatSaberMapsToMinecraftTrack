using System.Collections.Generic;

namespace BeatSaber
{
    //read more here https://bsmg.wiki/mapping/map-format.html
    public class DiffFile
    {
        //Only 2.0.0 supported
        public string _version { get; set; } = "2.0.0";
        public List<Event> _events { get; set; } = new List<Event>();
        public List<Note> _notes { get; set; } = new List<Note>();
        public List<Obstacle> _obstacles { get; set; } = new List<Obstacle>();
    }

    public class Obstacle
    {
        //in beats
        public decimal _time { get; set; } = 0.0m;
        public int _lineIndex { get; set; } = 0;
        //0, full height wall; 1, crouch/duck wall
        public int _type { get; set; } = 0;
        public decimal _duration { get; set; } = 0.0m;
        public int _width { get; set; } = 0;

    }

    public class Event
    {
        //in beats
        public decimal _time { get; set; } = 0.0m;
        public int _lineIndex { get; set; } = 0;
        public int _lineLayer { get; set; } = 0;
        /*
        0	Controls lights in the Back Lasers group.
        1	Controls lights in the Ring Lights group.
        2	Controls lights in the Left Rotating Lasers group.
        3	Controls lights in the Right Rotating Lasers group.
        4	Controls lights in the Center Lights group.
        5	(unused) Controls boost light colors (secondary colors).
        6	Unused.
        7	Unused.
        8	Creates one ring spin in the environment. Is not affected by _value.
        9	Controls zoom for applicable rings. Is not affected by _value.
        10	(unused) Official BPM Changes.
        11	Unused.
        12	Controls rotation speed for applicable lights in Left Rotating Lasers.
        13	Controls rotation speed for applicable lights in Right Rotating Lasers.
        14	(Previously unused) 360/90 Early rotation. Rotates future objects, while also rotating objects at the same time.
        15	(Previously unused) 360/90 Late rotation. Rotates future objects, but ignores rotating objects at the same time.*/
        public int _type { get; set; } = 0;
        /*
        ###Lights
        0	Turns the light group off.
        1	Changes the lights to blue, and turns the lights on.
        2	Changes the lights to blue, and flashes brightly before returning to normal.
        3	Changes the lights to blue, and flashes brightly before fading to black.
        4	Unused.
        5	Changes the lights to red, and turns the lights on.
        6	Changes the lights to red, and flashes brightly before returning to normal.
        7	Changes the lights to red, and flashes brightly before fading to black.*/

        public int _value { get; set; } = 0;
        //0, up; 1, down; 2, left; 3, right; 4, up left; 5, up right; 6, down left; 7 , down right; 8, any (dot note)
        public int _cutDirection { get; set; } = 0;


        //Light management
        public static List<int> redValues = new List<int>() { 5, 6, 7 };
        public static List<int> blueValues = new List<int>() { 2, 3, 4 };
        public static List<int> flashValues = new List<int>() { 3, 7 };
        public static List<int> turnOnValues = new List<int>() { 1, 2, 5, 6 };

        public string GetValueColor()
        {
            if (redValues.Contains(this._value)) return "red";
            else if (blueValues.Contains(this._value)) return "blue";
            return "red";
        }

        public bool ShouldLightFlash()
        {
            if (flashValues.Contains(this._value)) return true;
            return false;
        }

        public bool ShouldLightsStayOn()
        {
            if (turnOnValues.Contains(this._value)) return true;
            return false;
        }
    }

    public class Note
    {
        //in beats
        public decimal _time { get; set; } = 0.0m;
        public int _lineIndex { get; set; } = 0;
        public int _lineLayer { get; set; } = 0;
        //0, left red; 1, right blue; 2, unused; 3, Bomb
        public int _type { get; set; } = 0;
        //0, up; 1, down; 2, left; 3, right; 4, up left; 5, up right; 6, down left; 7 , down right; 8, any (dot note)
        public int _cutDirection { get; set; } = 0;

        public string GetBlock()
        {
            if(this._type == 0)
            {
                //red
                switch(this._cutDirection)
                {
                    case 0:
                        return "light_blue_wool";
                    case 1:
                        return "green_concrete";
                    case 2:
                        return "light_blue_concrete";
                    case 3:
                        return "green_wool";
                    case 4:
                        return "lime_concrete";
                    case 5:
                        return "lime_wool";
                    case 6:
                        return "light_gray_wool";
                    case 7:
                        return "light_gray_concrete";
                    case 8:
                        return "magenta_concrete";
                }
            } else if(this._type == 1)
            {
                //blue
                switch (this._cutDirection)
                {
                    case 0:
                        return "magenta_wool";
                    case 1:
                        return "orange_concrete";
                    case 2:
                        return "orange_wool";
                    case 3:
                        return "pink_concrete";
                    case 4:
                        return "red_concrete";
                    case 5:
                        return "red_wool";
                    case 6:
                        return "purple_wool";
                    case 7:
                        return "purple_concrete";
                    case 8:
                        return "white_concrete";
                }
            } else if(this._type == 3)
            {
                return "white_wool";
            }
            return "air";
        }
    }
}