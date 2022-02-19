namespace BirdGame.World
{
    using BirdGame.AI;
    using Microsoft.Xna.Framework;
    using System.Collections.Generic;

    /// <summary>
    /// A spawn point.
    /// </summary>
    internal class SpawnPoint
    {
        /// <summary>
        /// The position of the spawn point.
        /// </summary>
        private readonly Vector2 position;

        /// <summary>
        /// Whether the spawn point allows static / non-moving objects to be spawned there.
        /// </summary>
        private readonly bool allowStatic;

        /// <summary>
        /// The id / name of the spawn point.
        /// </summary>
        private readonly string id;

        /// <summary>
        /// Whether the spawn point is occupied.
        /// </summary>
        private bool occupied;

        /// <summary>
        /// The constructor for the spawn point.
        /// </summary>
        /// <param name="id">The id / name of the spawn point.</param>
        /// <param name="position">The position of the spawn point.</param>
        /// <param name="allowStatic">Whether the spawn point allows static / non-moving objects
        /// to be spawned there.</param>
        public SpawnPoint(string id, Vector2 position, bool allowStatic)
        {
            this.id = id;
            this.position = position;
            this.allowStatic = allowStatic;
            occupied = false;
        }

        /// <summary>
        /// A publically accessible id / name of the spawn point.
        /// </summary>
        public string Id => id;

        /// <summary>
        /// A publically accessible position of the spawn point.
        /// </summary>
        public Vector2 Position => position;

        /// <summary>
        /// A publically accessible property saying whether the spawn point allows
        /// static / non-moving objects to spawn there.
        /// </summary>
        public bool Static => allowStatic;

        /// <summary>
        /// A publically accessible property saying whether the spawn point is occupied.
        /// </summary>
        public bool Occupied => occupied;

        /// <summary>
        /// Set the spawn point to be occupied.
        /// </summary>
        public void Occupy()
        {
            occupied = true;
        }

        /// <summary>
        /// Set the spawn point to be vacated / empty.
        /// </summary>
        public void Vacate()
        {
            occupied = false;
        }

        /// <summary>
        /// A list of spawn points.
        /// </summary>
        public static List<SpawnPoint> SpawnPoints = GetSpawnPoints();

        /// <summary>
        /// Get a list of spawn points.
        /// </summary>
        /// <returns>A list of spawn points.</returns>
        private static List<SpawnPoint> GetSpawnPoints()
        {
            // Set the initial list of spawn points to be in the car park and static
            List<SpawnPoint> spawnPoints = new List<SpawnPoint>
            {
                new SpawnPoint("CarParkStaticLeft", new Vector2(0, 404), true),
                new SpawnPoint("CarParkStaticMiddle", new Vector2(64, 404), true),
                new SpawnPoint("CarParkStaticRight", new Vector2(128, 404), true),
            };

            // Get a list of nodes from the node network
            List<Node> nodes = NodeNetwork.GetNodes();

            // Go through each node in the node network
            foreach (Node node in nodes)
            {
                // Create a spawn point at each node, but don't allow them to have static objects spawn there
                spawnPoints.Add(new SpawnPoint(node.Id, node.Position, false));
            }

            return spawnPoints;
        }
    }
}
