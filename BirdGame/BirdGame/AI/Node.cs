namespace BirdGame.AI
{
    using Microsoft.Xna.Framework;
    using System.Collections.Generic;

    internal class Node
    {
        public string Id { get; set; }

        public Vector2 Position { get; set; }

        public List<Node> ConnectedNodes { get; set; }

        public Node(string id, Vector2 position)
        {
            Id = id;
            Position = position;
            ConnectedNodes = new List<Node>();
        }

        public void AddConnection(Node node)
        {
            ConnectedNodes.Add(node);
        }
    }
}
