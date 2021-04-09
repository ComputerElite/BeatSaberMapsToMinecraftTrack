using BeatSaber;
using BeatSaverAPI;
using Minecraft;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Vectors;

namespace BeatSaberMap_to_MCMap
{
    class Program
    {
        public static BeatSaberSong infodat = new BeatSaberSong();
        public static DiffFile diffdat = new DiffFile();
        public static decimal secsOffset = 0.0m;
        public static decimal beatSecondRatio = 120.0m;
        public static int bps = 8;

        static void Main(string[] args)
        {
            World w = new World();
            Console.Write("Info.dat: ");
            String info = Console.ReadLine().Replace("\"", "");
            Stopwatch s = Stopwatch.StartNew();
            if(info == "")
            {
                Console.WriteLine("Generating empty quader");
                for (int h = 4; h < 20; h++)
                {
                    for (int wi = -10; wi < 10; wi++)
                    {
                        for(int z = 0; z < 3000; z++)
                        {
                            w.AddBlock(new Block(wi, h, z, "air"));
                        }
                    }
                }
            } else
            {
                infodat = JsonSerializer.Deserialize<BeatSaberSong>(File.ReadAllText(info));
                if(infodat._version != "2.0.0")
                {
                    Console.WriteLine("Only BeatMap Version 2.0.0 is supported natively. Continue at own risk. Do you want to continue (y/n)?");
                    if (Console.ReadLine().ToLower() != "y") return;
                }
                Console.Write("Difficulty: ");
                String diff = Console.ReadLine().Replace("\"", "");
                Console.Write("Beat Craft modus (y/n)?: ");
                bool beatcraft = Console.ReadKey().KeyChar == 'y';
                Console.WriteLine();
                s.Restart();
                diffdat = JsonSerializer.Deserialize<DiffFile>(File.ReadAllText(diff));
                //minecart: 8 Blocks/s
                //order by time
                diffdat._notes.OrderBy(x => x._time);
                diffdat._events.OrderBy(x => x._time);
                diffdat._obstacles.OrderBy(x => x._time);

                //Get max beat
                decimal maxNoteBeat = diffdat._notes.Count > 0 ? diffdat._notes[diffdat._notes.Count - 1]._time : 0.0m;
                decimal maxEventBeat = diffdat._events.Count > 0 ? diffdat._events[diffdat._events.Count - 1]._time : 0.0m;
                decimal maxObstacleBeat = diffdat._obstacles.Count > 0 ? diffdat._obstacles[diffdat._obstacles.Count - 1]._time : 0.0m;
                decimal maxBeat = maxNoteBeat;
                if (maxEventBeat > maxBeat) maxBeat = maxEventBeat;
                if (maxObstacleBeat > maxBeat) maxBeat = maxObstacleBeat;
                int xStart = -2;
                int yStart = 14;
                int zStart = 0;
                int railHeight = 10;
                secsOffset = infodat._songTimeOffset;

                beatSecondRatio = 60.0m / infodat.BPM;
                Console.WriteLine("Generating rails for " + maxBeat + " beats");
                for (int i = 0; i < (maxBeat * beatSecondRatio + secsOffset) * bps; i++)
                {
                    w.AddBlock(new Block(0, railHeight, i,  "stone replace"));
                    w.AddBlock(new Block(0, railHeight + 1, i, i % 2 == 0 ? "powered_rail replace" : "detector_rail replace"));
                }
                if(beatcraft)
                {
                    Console.WriteLine("Generating blocks for " + diffdat._notes.Count + " Notes");
                    foreach (Note n in diffdat._notes)
                    {
                        Block b = new Block();
                        b.x = xStart + n._lineIndex;
                        b.y = yStart + n._lineLayer;
                        b.z = GetBlockFromBeat(n._time);
                        b.block = n._type == 0 ? "dandelion" : "blue_orchid";
                        if (b.block.Contains("powder"))
                        {
                            w.InsertBlock(0, new Block(b.x, b.y - 1, b.z, "barrier replace"));
                        }
                        w.AddBlock(b);
                    }
                } else
                {
                    Console.WriteLine("Generating blocks for " + diffdat._obstacles.Count + " obstacles");
                    foreach (Obstacle o in diffdat._obstacles)
                    {
                        List<int> heights = new List<int>();
                        heights.Add(yStart + 1);
                        heights.Add(yStart + 2);
                        if (o._type == 0) heights.Add(yStart);

                        List<int> widths = new List<int>();
                        for (int i = 0; i < o._width; i++)
                        {
                            widths.Add(xStart + o._lineIndex + i);
                        }
                        List<Vector2> flat = new List<Vector2>();
                        foreach (int h in heights)
                        {
                            foreach (int wi in widths)
                            {
                                flat.Add(new Vector2(wi, h));
                            }
                        }
                        int durationInBlocks = (int)Math.Round(o._duration * beatSecondRatio * bps);
                        int baseBlock = GetBlockFromBeat(o._time);
                        foreach (Vector2 v in flat)
                        {
                            for (int i = 0; i < durationInBlocks; i++)
                            {
                                w.AddBlock(new Block(v.x, v.y, baseBlock + i, "red_stained_glass keep"));
                            }

                        }
                    }
                    Console.WriteLine("Generating blocks for " + diffdat._notes.Count + " Notes");
                    foreach (Note n in diffdat._notes)
                    {
                        Block b = new Block();
                        b.x = xStart + n._lineIndex;
                        b.y = yStart + n._lineLayer;
                        b.z = GetBlockFromBeat(n._time);
                        b.block = n.GetBlock();
                        if (b.block.Contains("powder"))
                        {
                            w.InsertBlock(0, new Block(b.x, b.y - 1, b.z, "barrier replace"));
                        }
                        w.AddBlock(b);
                    }

                    LightController l = new LightController();
                    Console.WriteLine("Generating Blocks for " + diffdat._events.Count + " Events");
                    foreach (Event e in diffdat._events)
                    {
                        if (e._type == 0)
                        {
                            //Controls lights in the Back Lasers group.
                        }
                        else if (e._type == 1)
                        {
                            //Controls lights in the Ring Lights group.
                        }
                        else if (e._type == 2)
                        {
                            //Controls lights in the Left Rotating Lasers group.
                            l.leftLightsColor = e.GetValueColor();
                            if (e.ShouldLightFlash())
                            {
                                int block = GetBlockFromBeat(e._time);
                                if (block - l.lastLeftLights < 13) continue;
                                l.lastLeftLights = block;
                                w.PlaceStructure(Structure.ConcretePowerLauncher(), new Vector3(2, railHeight - 3, block + 7));
                                w.PlaceStructure(Structure.RedstoneConnection(new Vector3(0, railHeight - 1, block % 2 == 0 ? block - 1 : block), new Vector3(2, railHeight - 3, block + 6)), new Vector3(0, railHeight - 1, block % 2 == 0 ? block - 1 : block));
                            }
                        }
                        else if (e._type == 3)
                        {
                            //Controls lights in the Right Rotating Lasers group.
                            l.rightLightsColor = e.GetValueColor();
                            if (e.ShouldLightFlash())
                            {
                                int block = GetBlockFromBeat(e._time);
                                if (block - l.lastRightLights < 13) continue;
                                l.lastRightLights = block;
                                w.PlaceStructure(Structure.ConcretePowerLauncher().Mirror(), new Vector3(-2, railHeight - 3, block + 7));
                                w.PlaceStructure(Structure.RedstoneConnection(new Vector3(0, railHeight - 1, block % 2 == 0 ? block - 1 : block), new Vector3(-2, railHeight - 3, block + 6)), new Vector3(0, railHeight - 1, block % 2 == 0 ? block - 1 : block));
                            }
                        }
                        else if (e._type == 4)
                        {
                            //Controls lights in the Center Lights group.
                            l.centerLightsColor = e.GetValueColor();
                            if (e.ShouldLightFlash()) w.PlaceStructure(Structure.StaticTracklight().changeColor(l.centerLightsColor), new Vector3(0, railHeight, GetBlockFromBeat(e._time)));
                        }
                        else if (e._type == 5)
                        {
                            //(Previously unused) Controls boost light colors (secondary colors).
                        }
                        else if (e._type == 8)
                        {
                            //Creates one ring spin in the environment. Is not affected by _value.
                        }
                        else if (e._type == 9)
                        {
                            //Controls zoom for applicable rings. Is not affected by _value.
                        }
                        //case 10:
                        //    //(unused)BPM Changes
                        //    break;
                        //case 12:
                        //    //(unused)Controls rotation speed for applicable lights in Left Rotating Lasers.
                        //    break;
                        //case 13:
                        //    //(unused)Controls rotation speed for applicable lights in Right Rotating Lasers.
                        //    break;
                    }
                }
            }
                
            File.WriteAllText("D:\\bs.txt", w.ToString());
            s.Stop();
            
            
            
            Console.WriteLine("Generated " + w.blocks.Count + " Blocks to place in " + s.ElapsedMilliseconds + " ms");
            Console.ReadLine();
        }

        public static int GetBlockFromBeat(decimal beat)
        {
            return (int)Math.Round((beat * beatSecondRatio + secsOffset) * bps);
        }
    }
}
