namespace BirdGame.World
{
    using BirdGame.AI;
    using Microsoft.Xna.Framework;
    using System.Collections.Generic;

    internal class SpawnPoint
    {
        private readonly Vector2 position;

        private readonly bool allowStatic;

        private readonly string id;

        private bool occupied;

        public SpawnPoint(string id, Vector2 position, bool allowStatic)
        {
            this.id = id;
            this.position = position;
            this.allowStatic = allowStatic;
            occupied = false;
        }

        public string Id => id;

        public Vector2 Position => position;

        public bool Static => allowStatic;

        public bool Occupied => occupied;

        public void Occupy()
        {
            occupied = true;
        }

        public void Vacate()
        {
            occupied = false;
        }

        public static List<SpawnPoint> SpawnPoints = GetSpawnPoints();

        private static List<SpawnPoint> GetSpawnPoints()
        {
            List<SpawnPoint> spawnPoints = new List<SpawnPoint>
            {
                new SpawnPoint("CarParkStaticLeft", new Vector2(0, 404), true),
                new SpawnPoint("CarParkStaticMiddle", new Vector2(64, 404), true),
                new SpawnPoint("CarParkStaticRight", new Vector2(128, 404), true),
            };

            List<Node> nodes = NodeNetwork.GetNodes();

            foreach (Node node in nodes)
            {
                spawnPoints.Add(new SpawnPoint(node.Id, node.Position, false));
            }

            return spawnPoints;
        }
    }
}
