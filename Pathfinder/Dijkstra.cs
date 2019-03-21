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
    class Dijkstra
    {
        // WHETHER OR NOT THE LOCATION IS CLOSED
        public bool[,] closed;

        // VALUE FOR EACH LOCATION
        public float[,] cost;

        // LINK FOR EACH LOCATION = COORDS OF A NEIGHBOURING LOCATION
        public Coord2[,] link;

        // WHETHER OR NOT A LOCATION IS IN THE FINAL PATH
        public bool[,] inPath;

        public Dijkstra()
        {
            closed = new bool[40, 40];
            cost = new float[40, 40];
            link = new Coord2[40, 40];
            inPath = new bool[40, 40];
        }

        // INITIALISE NODES AS OPEN
        public void Build(Level level, AiBotBase bot, Player plr)
        {
            for (int i = 0; i < 40; i++)
            {
                for (int j = 0; j < 40; j++)
                {
                    closed[i, j] = false;
                    //i++; j++;
                }
            }

            // INITIALISE NODES COST TO HIGH VALUE
            for (int i = 0; i < 40; i++)
            {
                for (int j = 0; j < 40; j++)
                {
                    cost[i, j] = 1000000;
                    //i++; j++;
                }
            }

            // INITIALISE LINK VALUES
            for (int i = 0; i < 40; i++)
            {
                for (int j = 0; j < 40; j++)
                {
                    link[i, j] = new Coord2(-1, -1);
                    //i++; j++;
                }
            }

            // INITIALISE INPATH VALUES AS FALSE
            for (int i = 0; i < 40; i++)
            {
                for (int j = 0; j < 40; j++)
                {
                    inPath[i, j] = false;
                    //i++; j++;
                }
            }

            closed[bot.GridPosition.X, bot.GridPosition.Y] = false;
            cost[bot.GridPosition.X, bot.GridPosition.Y] = 0;

            Coord2 tempPos;
            tempPos.X = 0; tempPos.Y = 0;

            // WHILE THE PLAYERS GRID POSITION IS NOT CLOSED
            while (tempPos != plr.GridPosition)
            {
                float comparisonVal = 1000000;

                for (int i = 0; i < 40; i++)
                {
                    for (int j = 0; j < 40; j++)
                    {
                        if (cost[i, j] < comparisonVal && closed[i, j] == false && level.ValidPosition(new Coord2(i, j)))
                        {
                            comparisonVal = cost[i, j];
                            tempPos.X = i; tempPos.Y = j;
                        }
                    }
                }
                // MARKING THE LOCATION AS CLOSED
                closed[tempPos.X, tempPos.Y] = true;

                float pairCost = 0;

                // GO THROUGH NEIGHBOURING COORDS
                for (int i = tempPos.X - 1; i < tempPos.X + 2; i++)
                {
                    for (int j = tempPos.Y - 1; j < tempPos.Y + 2; j++)
                    {
                        if (level.ValidPosition(new Coord2(i, j)) && !closed[i, j])
                        {
                            // SETTING COST VALUE FOR MOVING TO NEW GRID COORD
                            if (i != tempPos.X && j != tempPos.Y)
                            {
                                // DIAGONALLY
                                pairCost = 1.4f;
                            }
                            else if ((i != tempPos.X && j == tempPos.Y) || (i == tempPos.X && j != tempPos.Y))
                            {
                                // UP, DOWN, LEFT AND RIGHT
                                pairCost = 1.0f;
                            }
                            else
                            {
                                // NO MOVEMENT - BLOCKED PATH
                                pairCost = 0.0f;
                            }

                            if (cost[tempPos.X, tempPos.Y] + pairCost < cost[i, j])
                            {
                                cost[i, j] = cost[tempPos.X, tempPos.Y] + pairCost;

                                link[i, j] = tempPos;
                            }
                        }
                    }
                }
            }

            bool done = false;
            Coord2 nextClosed = plr.GridPosition;

            while (!done)
            {
                inPath[nextClosed.X, nextClosed.Y] = true;
                nextClosed = link[nextClosed.X, nextClosed.Y];

                if (nextClosed == bot.GridPosition)
                {
                    done = true;
                }
            }
        }
    }
}
