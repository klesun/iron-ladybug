using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Util;
using Random = System.Random;

namespace Assets.Scripts.Util.Misc
{
    // based on http://weblog.jamisbuck.org/2010/12/27/maze-generation-recursive-backtracking
    // generates maze data

    public class MazeGenerator
    {
        Random rnd;

        public MazeGenerator(int randomSeed)
        {
            rnd = new Random(randomSeed);
        }

        private void Shuffle<T>(T[] list)
        {
            for (var i = 0; i < list.Length; i++) {
                var idx = rnd.Next(i, list.Length);
                var tmp = list[idx];
                list[idx] = list[i];
                list[i] = tmp;
            }
        }

        // "Recursive Backtracking" - produces small amount of long paths
        private int PaveTheWayRb(Cell[,] grid, int x0, int y0, int x1, int y1)
        {
            var cell = grid[x0, y0];
            cell.visited = true;
            var forksTillExit = 0;
            var backDirection = cell.ways.FirstOrDefault();
            var turns = 0;
            var directions = (Way.EKind[])Enum.GetValues(typeof(Way.EKind));
            Shuffle(directions);
            foreach(var direction in directions) {
                var way = new Way() {kind = direction};
                var shift = way.Dir();
                var x = x0 + (int)shift.x;
                var y = y0 + (int)shift.y;

                if (x >= 0 && x < grid.GetLength(0) &&
                    y >= 0 && y < grid.GetLength(1) &&
                    grid[x, y].visited != true
                ) {
                    ++turns;
                    cell.ways.Add(way);
                    grid[x, y].ways.Add(way.Opp());
                    var turnForks = PaveTheWayRb(grid, x, y, x1, y1);
                    if (turnForks > 0) {
                        forksTillExit = turnForks;
                        cell.forwardDirection = way;
                    }
                }
            }
            if (x0 == x1 && y0 == y1 || (forksTillExit > 0 && turns > 1)) {
                ++forksTillExit;
                cell.isForked = true;
            }
            cell.backDirection = backDirection ?? new Way(){kind = Way.EKind.S};
            cell.forksTillExit = forksTillExit;

            return forksTillExit;
        }

        public Cell[,] Generate(int w, int h, int x0, int x1)
        {
            x0 = Math.Min(x0, w - 1);
            x1 = Math.Min(x1, w - 1);

            var grid = new Cell[w,h];
            for (var i = 0; i < w; ++i) {
                for (var j = 0; j < h; ++j) {
                    grid[i,j] = new Cell();
                }
            }

            // swapping p1 and p2 so begining was more branched
            PaveTheWayRb(grid, x1, h - 1, x0, 0);

            return grid;
        }

        public static string PrintGrid(Cell[,] grid)
        {
            var result = "";

            for (var x = 0; x < grid.GetLength(0); ++x) {
                result += "__";
            }
            result += '\n';

            for (var y = grid.GetLength(1) - 1; y >= 0; --y) {
                var frameMarked = false;
                for (var x = 0; x < grid.GetLength(0); ++x) {
                    var cell = grid[x,y];
                    var chars = new[] {" ", " "};

                    D.F1<string, D.F1<Cell, string>> colorize = (txt) => (neighbor) =>
                        cell.forksTillExit > 0 || neighbor != null && neighbor.forksTillExit > 0
                            ? "<color=red>" + txt + "</color>"
                            : txt;

                    D.F1<Way.EKind, Cell> at = (kind) => {
                        var shift = (new Way(){kind = kind}).Dir();
                        var x1 = x + (int)shift.x;
                        var y1 = y + (int)shift.y;
                        if (x1 > 0 && x1 < grid.GetLength(0) &&
                            y1 > 0 && y1 < grid.GetLength(1)
                        ) {
                            return grid[x1,y1];
                        } else {
                            return null;
                        }
                    };


                    chars[0] = cell.ways.Any(w => w.kind == Way.EKind.W)
                        ? colorize("_")(at(Way.EKind.S))
                        : colorize("|")(at(Way.EKind.W));

                    chars[1] = cell.ways.Any(w => w.kind == Way.EKind.S)
                        ? " "
                        : colorize("_")(at(Way.EKind.S));

                    frameMarked = cell.forksTillExit > 0;

                    if (cell.forksTillExit > 0 && cell.ways.Count > 2) {
                        chars[1] = "<color=red>" + (cell.forksTillExit % 10) + "</color>";
                    }

                    result += String.Join("", chars);
                }
                result += frameMarked ? "<color=red>|</color>\n" : "|\n";
            }

            return result;
        }

        public class Cell
        {
            public bool visited = false;
            public int forksTillExit = 0;
            public Way backDirection = new Way(){kind = Way.EKind.N};
            public Way forwardDirection = new Way(){kind = Way.EKind.S};
            public bool isForked = false;
            public HashSet<Way> ways = new HashSet<Way>();
        }

        public class Way
        {
            public enum EKind {N, S, E, W};
            static Dictionary<EKind, int> DX = new Dictionary<EKind, int>{
                {EKind.N,  0},
                {EKind.S,  0},
                {EKind.E, +1},
                {EKind.W, -1},
            };
            static Dictionary<EKind, int> DY = new Dictionary<EKind, int>{
                {EKind.N, +1},
                {EKind.S, -1},
                {EKind.E,  0},
                {EKind.W,  0},
            };

            public EKind kind;

            public Vector2 Dir()
            {
                return new Vector2(DX[kind], DY[kind]);
            }

            public Way Opp()
            {
                var OPPOSITE = new Dictionary<EKind, EKind>{
                    {EKind.N, EKind.S},
                    {EKind.S, EKind.N},
                    {EKind.E, EKind.W},
                    {EKind.W, EKind.E},
                };

                return new Way(){kind = OPPOSITE[kind]};
            }

            public int getDegree()
            {
                var degreeByKind = new Dictionary<EKind, int>{
                    {EKind.N, 90},
                    {EKind.S, 270},
                    {EKind.E, 0},
                    {EKind.W, 180},
                };

                return degreeByKind[kind];
            }
        }
    }
}