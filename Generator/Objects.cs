using Minecraft;
using System.Collections.Generic;
using Vectors;

namespace Objects
{
    public class Object
    {
        public Vector3 position = new Vector3();
        public int xSize = 0;
        public int ySize = 0;
        public int zSize = 0;
        public int xSizeOffset = 0; //from 0 0 0
        public int ySizeOffset = 0; //from 0 0 0
        public int zSizeOffset = 0; //from 0 0 0

        public static Object FromStructure(Structure s)
        {
            Object b = new Object();
            int xmax = 0;
            int xmin = 0;
            int ymax = 0;
            int ymin = 0;
            int zmax = 0;
            int zmin = 0;
            foreach (Block bl in s.blocks)
            {
                if (bl.x > xmax) xmax = bl.x;
                else if (bl.x < xmin) xmin = bl.x;
                if (bl.y > ymax) ymax = bl.y;
                else if (bl.y < ymin) ymin = bl.y;
                if (bl.z > zmax) zmax = bl.z;
                else if (bl.z < zmin) zmin = bl.z;
            }
            b.xSize = xmax - xmin;
            b.ySize = ymax - ymin;
            b.zSize = zmax - zmin;
            b.xSizeOffset = xmin;
            b.ySizeOffset = ymin;
            b.zSizeOffset = zmin;
            return b;
        }
    }

    public class ZObjectCounters
    {
        public int commandBlocks = 0;
    }

    public class ObjectPlacer
    {
        public List<ZObjectCounters> zObjectCounters = new List<ZObjectCounters>();
        public List<int> YOffsets = new List<int>();

        public ObjectPlacer(int trackLength)
        {
            for (int i = 0; i <= trackLength; i++)
            {
                zObjectCounters.Add(new ZObjectCounters());
                YOffsets.Add(0);
            }
        }

        public int GetYOffset(int z)
        {
            return YOffsets[z];
        }

        public void SetYOffset(int z, int offset)
        {
            YOffsets[z] = offset;
        }

        public int GetYOffsetCommandBlock(int z)
        {
            zObjectCounters[z].commandBlocks++;
            return zObjectCounters[z].commandBlocks * -1;
        }

        public static int GetNearestRailZ(int z)
        {
            return z % 2 == 0 ? z - 1 : z;
        }

        public static string GetRailType(int z)
        {
            return z % 2 == 0 ? "powered_rail" : "detector_rail";
        }
    }
}