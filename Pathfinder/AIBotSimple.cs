using System;
using System.Collections.Generic;
using System.Linq;
using System.Diagnostics;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Audio;
using System.IO;

namespace Pathfinder
{
    class AIBotSimple : AiBotBase
    {
        public AIBotSimple(int x, int y) : base(x, y)
        {
        }

        protected override void ChooseNextGridLocation(Level level, Player plr)
        {
            Coord2 newPos;
            bool ok = false;
            newPos = GridPosition;

            while (!ok)
            {
                if (plr.GridPosition.X > GridPosition.X)
                {
                    newPos.X += 1;
                }
                else if (plr.GridPosition.X < GridPosition.X)
                {
                    newPos.X -= 1;
                }
                else if (plr.GridPosition.Y > GridPosition.Y)
                {
                    newPos.Y += 1;
                }
                else if (plr.GridPosition.Y < GridPosition.Y)
                {
                    newPos.Y -= 1;
                }
                ok = SetNextGridPosition(newPos, level);
            }
        }
    }
}
