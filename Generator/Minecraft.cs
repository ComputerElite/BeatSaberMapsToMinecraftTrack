using System;
using System.Collections.Generic;
using Vectors;

namespace Minecraft
{
    public class Block
    {
        public string block { get; set; } = "grass";
        public int x { get; set; } = 0;
        public int y { get; set; } = 0;
        public int z { get; set; } = 0;

        public Block(int x, int y, int z, string block)
        {
            this.x = x;
            this.y = y;
            this.z = z;
            this.block = block;
        }

        public Block() { }

        public Vector3 GetCoords()
        {
            return new Vector3(x, y, z);
        }

        public override string ToString()
        {
            return "setblock " + x + " " + y + " " + z + " " + block;
        }
    }

    public class Structure
    {
        public List<Block> blocks { get; set; } = new List<Block>();

        public void OffsetBy(Vector3 offset)
        {
            foreach(Block b in blocks)
            {
                b.x += offset.x;
                b.y += offset.y;
                b.z += offset.z;
            }
        }

        public void AddBlock(Block block)
        {
            blocks.Add(block);
        }

        public void InsertBlock(int index, Block block)
        {
            blocks.Insert(index, block);
        }

        public Structure Mirror()
        {
            for(int i = 0; i < blocks.Count; i++) 
            {
                blocks[i].block = MirrorDirection(blocks[i].block);
                blocks[i].x *= -1;
            }
            return this;
        }

        public Structure Merge(Structure toMerge)
        {
            foreach (Block b in toMerge.blocks) this.AddBlock(b);
            return this;
        }

        public static Structure ConcretePowerLauncher()
        {
            Structure s = new Structure();
            s.AddBlock(new Block(0, 0, 0, "repeater[facing=north]"));
            s.AddBlock(new Block(0, -1, 0, "stone"));
            s.AddBlock(new Block(0, 0, 3, "stone"));
            s.AddBlock(new Block(0, 1, 3, "redstone_wire[south=side,north=side,east=side,west=side]"));
            s.AddBlock(new Block(0, -1, 2, "stone"));
            s.AddBlock(new Block(0, 0, 2, "comparator[facing=north]"));
            s.AddBlock(new Block(0, 1, 2, "stone"));
            s.AddBlock(new Block(0, 0, 1, "stone"));
            s.AddBlock(new Block(1, -1, 1, "stone"));
            s.AddBlock(new Block(1, 0, 1, "comparator[facing=east]"));
            s.AddBlock(new Block(1, 1, 1, "stone"));
            s.AddBlock(new Block(2, 0, 1, "stone"));
            s.AddBlock(new Block(2, 1, 1, "redstone_wire"));
            s.AddBlock(new Block(1, 1, 3, "stone"));
            s.AddBlock(new Block(1, 0, 3, "redstone_wire"));
            s.AddBlock(new Block(1, -1, 3, "stone"));
            s.AddBlock(new Block(2, 2, 3, "stone"));
            s.AddBlock(new Block(2, 1, 3, "redstone_wall_torch[facing=east]"));
            s.AddBlock(new Block(2, -1, 3, "repeater[facing=west,delay = 2]"));
            s.AddBlock(new Block(2, -2, 3, "stone"));
            s.AddBlock(new Block(3, 1, 3, "piston[facing=east]"));
            s.AddBlock(new Block(3, -1, 3, "repeater[facing=west,delay = 4]"));
            s.AddBlock(new Block(3, -2, 3, "stone"));
            s.AddBlock(new Block(4, 2, 3, "red_concrete_powder"));
            s.AddBlock(new Block(4, -1, 3, "slime_block"));
            s.AddBlock(new Block(4, -2, 3, "sticky_piston[facing=up]"));
            s.AddBlock(new Block(0, 1, 1, "redstone_wire[south=side,north=side,east=side,west=side]"));
            return s;
        }

        /// <summary>
        /// Offset to powered Block; Loads a powerable beacon
        /// </summary>
        /// <returns></returns>
        public static Structure PowerableBeacon()
        {
            //Needs 4 sec to power on = 16 blocks
            Structure s = new Structure();
            s.AddBlock(new Block(0, 0, 6,"sticky_piston[facing=east]"));
            s.AddBlock(new Block(1, 0, 6, "diamond_block"));
            s.AddBlock(new Block(3, 0, 6, "diamond_block"));
            s.AddBlock(new Block(4, 0, 6, "diamond_block"));
            s.AddBlock(new Block(2, 0, 7, "diamond_block"));
            s.AddBlock(new Block(3, 0, 7, "diamond_block"));
            s.AddBlock(new Block(4, 0, 7, "diamond_block"));
            s.AddBlock(new Block(2, 0, 5, "diamond_block"));
            s.AddBlock(new Block(3, 0, 5, "diamond_block"));
            s.AddBlock(new Block(4, 0, 5, "diamond_block"));
            s.AddBlock(new Block(3, 1, 6, "beacon"));
            s.AddBlock(new Block(3, 2, 6, "red_stained_glass"));
            s.AddBlock(new Block(0, 0, 5, "stone"));
            s.AddBlock(new Block(0, 1, 5, "redstone_wire"));
            s.AddBlock(new Block(0, 0, 3, "stone"));
            s.AddBlock(new Block(0, 0, 1, "stone"));
            s.AddBlock(new Block(0, 1, 1, "redstone_wire"));
            s.AddBlock(new Block(0, 1, 4, "stone"));
            s.AddBlock(new Block(0, 2, 4, "redstone_wire"));
            s.AddBlock(new Block(0, 1, 3, "stone"));
            s.AddBlock(new Block(0, 1, 2, "stone"));
            s.AddBlock(new Block(0, 2, 1, "stone"));
            s.AddBlock(new Block(0, -1, 2, "stone"));
            s.AddBlock(new Block(0, -1, 4, "stone"));
            s.AddBlock(new Block(0, 0, 2, "comparator[facing=north]"));
            s.AddBlock(new Block(0, 0, 4, "comparator[facing=north]"));
            s.AddBlock(new Block(0, 2, 2, "comparator[facing=south]"));
            s.AddBlock(new Block(0, 2, 3, "comparator[facing=south]"));
            s.AddBlock(new Block(0, -1, 0, "stone"));
            s.AddBlock(new Block(0, 0, 0, "repeater[facing=north]"));
            return s;
        }

        public static Structure RedstoneConnection(Vector3 from, Vector3 to)
        {
            Structure s = new Structure();
            to = to - from;
            from = new Vector3();
            s.AddBlock(new Block(from.x, from.y - 1, from.z, "stone"));
            s.AddBlock(new Block(from.x, from.y, from.z, "redstone_wire"));
            int i = 0;
            while (from.x != to.x && from.y != to.y)
            {
                string block = "redstone_wire";
                if(to.x > from.x && to.y == from.y)
                {
                    from.x++;
                    if (i >= 10)
                    {
                        block = "repeater";
                        i = 0;
                    }
                }
                else if (to.x < from.x && to.y == from.y)
                {
                    from.x--;
                    if (i >= 10)
                    {
                        block = "repeater";
                        i = 0;
                    }
                }
                else if(to.x > from.x)
                {
                    from.x++;
                } else 
                {
                    from.x--;
                }
                if(to.y > from.y)
                {
                    from.y++;
                } else
                {
                    from.y--;
                }
                s.AddBlock(new Block(from.x, from.y - 1, from.z, "stone"));
                s.AddBlock(new Block(from.x, from.y, from.z, block));
                i++;
            }
            while(from.z != to.z)
            {
                string block = "redstone_wire";
                if (to.z > from.z)
                {
                    from.z++;
                }
                else
                {
                    from.z--;
                }
                if (i >= 10)
                {
                    block = "repeater";
                    i = 0;
                }
                s.AddBlock(new Block(from.x, from.y - 1, from.z, "stone"));
                s.AddBlock(new Block(from.x, from.y, from.z, block));
                i++;
            }
            return s;
        }

        public static Structure StaticTracklight()
        {
            Structure s = new Structure();
            s.AddBlock(new Block(0, -1, 0, "glowstone"));
            s.AddBlock(new Block(0, 0, 0, "red_stained_glass"));
            s.AddBlock(new Block(-1, -1, 0, "glowstone"));
            s.AddBlock(new Block(-1, 0, 0, "red_stained_glass"));
            s.AddBlock(new Block(1, -1, 0, "glowstone"));
            s.AddBlock(new Block(1, 0, 0, "red_stained_glass"));
            return s;
        }

        public Structure changeColor(string color)
        {
            for (int i = 0; i < blocks.Count; i++)
            {

                if (blocks[i].block.ToLower().Contains("wire")) continue;
                blocks[i].block = blocks[i].block.Replace("red", color);
                blocks[i].block = blocks[i].block.Replace("blue", color);
            }
            return this;
        }

        public string MirrorDirection(string direction)
        {
            if (direction.Contains("wire")) return direction;
            if (direction.Contains("west")) return direction.Replace("west", "east");
            if (direction.Contains("east")) return direction.Replace("east", "west");
            
            if (direction.Contains("blue")) return direction.Replace("blue", "red");
            if (direction.Contains("red")) return direction.Replace("red", "blue");
            return direction;
        }

        public string FlipDirection(string direction)
        {
            if (direction.Contains("up")) return direction.Replace("up", "down");
            if (direction.Contains("down")) return direction.Replace("down", "up");
            if (direction.Contains("north")) return direction.Replace("north", "south");
            if (direction.Contains("south")) return direction.Replace("south", "north");
            if (direction.Contains("west")) return direction.Replace("west", "east");
            if (direction.Contains("east")) return direction.Replace("east", "west");
            return direction;
        }
    }

    public class World
    {
        public List<Block> blocks { get; set; } = new List<Block>();
        
        public void AddBlock(Block block)
        {
            blocks.Add(block);
        }

        public void InsertBlock(int index, Block block)
        {
            blocks.Insert(index, block);
        }

        public void PlaceStructure(Structure structure, Vector3 point)
        {
            structure.OffsetBy(point);
            foreach (Block b in structure.blocks) this.blocks.Add(b);
        }

        public override string ToString()
        {
            string outp = "";
            foreach(Block b in blocks)
            {
                outp += b.ToString() + "\n";
            }
            return outp;
        }
    }
}