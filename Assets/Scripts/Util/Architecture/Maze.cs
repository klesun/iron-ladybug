using System;
using System.Collections.Generic;
using Assets.Scripts.Util.Logic;
using Assets.Scripts.Util.Misc;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;

namespace Assets.Scripts.Util.Architecture
{
    [ExecuteInEditMode]
    [SelectionBase]
    public class Maze : MonoBehaviour
    {
        const float REVALIDATION_PERIOD = 0.1f;

        public int width = 10;
        public int height = 10;
        public int entrancePlace = 2;
        public int exitPlace = 2;
        public int randomSeed = 1337;

        // mandatory
        public TransformListener wallSample;
        // mandatory
        public GameObject cont;
        // optional
        public Text pseudoGraphicHolder = null;
        // optional
        public TransformListener floorSample = null;

        double? lastValidatedOn = null;
        bool revalidationRequested = false;

        #if UNITY_EDITOR
        void OnValidate()
        {
            if (wallSample!= null && cont != null) {
                UnityEditor.EditorApplication.delayCall += () => revalidationRequested = true;
                wallSample.onChange = () => revalidationRequested = true;
                if (floorSample != null) {
                    floorSample.onChange = () => revalidationRequested = true;
                }
            }
        }
        #endif

        void Update()
        {
            var now = System.DateTime.Now.Ticks / 10000000d; // microsoft is microsoft
            if (revalidationRequested && (lastValidatedOn == null || now - lastValidatedOn > REVALIDATION_PERIOD)) {
                revalidationRequested = false;
                lastValidatedOn = now;
                Renew ();
            }
        }

        void Renew()
        {
            if (this == null) {
                // well, it complains about it being
                // destroyed when i starts the game
                return;
            }
            if (Application.isPlaying) {
                return;
            }

            EmptyCont ();
            FillCont ();
        }

        void EmptyCont()
        {
            var deadmen = new List<GameObject>();
            foreach (Transform ch in cont.transform) {
                deadmen.Add (ch.gameObject);
            }
            deadmen.ForEach (DestroyImmediate);
        }

        void FillCont()
        {
            var gen = new MazeGenerator(randomSeed);
            var grid = gen.Generate(width, height, entrancePlace, exitPlace);

            if (pseudoGraphicHolder) {
                pseudoGraphicHolder.text = MazeGenerator.PrintGrid(grid);
            }

            var wallSize = wallSample.GetComponent<Collider>().bounds.size;
            var spacing = wallSample.transform.localScale.x - wallSample.transform.localScale.z;

            // drawing two walls on intersections for now, cuz later
            for (var x = 0; x < grid.GetLength(0); ++x) {
                for (var y = 0; y < grid.GetLength(1); ++y) {
                    var cell = grid[x,y];
                    foreach (MazeGenerator.Way.EKind kind in Enum.GetValues(typeof(MazeGenerator.Way.EKind))) {
                        if (!cell.ways.Any(w => w.kind == kind) &&
                            !(y == 0 && x == entrancePlace) &&
                            !(y == grid.GetLength(1) - 1 && x == exitPlace)
                        ) {
                            var shift = (new MazeGenerator.Way() {kind = kind}).Dir() * 0.5f;
                            var wall = Instantiate(wallSample.gameObject);
                            wall.name = "_generated_wall_" + x + "_" + y + "_" + kind;
                            wall.transform.SetParent(cont.gameObject.transform);
                            wall.transform.localRotation = Quaternion.Euler(0, shift.x != 0 ? 90 : 0, 0);
                            wall.transform.localPosition =
                                (new Vector3(x + shift.x, 0, y + shift.y)) * spacing +
                                Vector3.up * wallSize.y / 2;

                        }
                    }

                    if (floorSample != null) {
                        var floor = Instantiate(floorSample.gameObject);
                        floor.name = "_generated_floor_" + x + "_" + y;
                        floor.transform.SetParent (cont.gameObject.transform);
                        floor.transform.localRotation = Quaternion.identity;
                        floor.transform.localPosition = (new Vector3(x, 0, y)) * spacing;

                        if (cell.isForked) {

                        }
                    }
                }
            }
        }
    }
}
