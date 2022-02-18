namespace BirdGame.AI
{
    using Microsoft.Xna.Framework;
    using System.Collections.Generic;

    /// <summary>
    /// A Node used for AI characters to move between.
    /// </summary>
    internal class Node
    {
        /// <summary>
        /// An Id / name to identify the node.
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// The position of the node.
        /// </summary>
        public Vector2 Position { get; set; }

        /// <summary>
        /// A list of nodes that this node is connected to.
        /// </summary>
        public List<Node> ConnectedNodes { get; set; }

        /// <summary>
        /// Node constructor.
        /// </summary>
        /// <param name="id">An identifier for the node.</param>
        /// <param name="position">The position of the node.</param>
        public Node(string id, Vector2 position)
        {
            Id = id;
            Position = position;
            ConnectedNodes = new List<Node>();
        }

        /// <summary>
        /// Connect another node to this node.
        /// </summary>
        /// <param name="node">The node to connect.</param>
        public void AddConnection(Node node)
        {
            ConnectedNodes.Add(node);
        }
    }
}
