namespace BirdGame.AI
{
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// A builder to generate routes of nodes.
    /// The usage is NodeRouteBuilder.Start(firstNode).AddNode(nextNode).End()
    /// </summary>
    internal class NodeRouteBuilder
    {
        /// <summary>
        /// A list of nodes.
        /// </summary>
        private readonly List<Node> nodes;

        /// <summary>
        /// The constructor. This is private to force the creation
        /// through the Start method.
        /// </summary>
        /// <param name="node">The first node in the route.</param>
        private NodeRouteBuilder(Node node)
        {
            nodes = new List<Node>
            {
                node
            };
        }

        /// <summary>
        /// Start a route.
        /// </summary>
        /// <param name="node">The first node.</param>
        /// <returns>A node route builder to allow adding more nodes.</returns>
        public static NodeRouteBuilder Start(Node node)
        {
            return new NodeRouteBuilder(node);
        }

        /// <summary>
        /// Add a node to the route.
        /// </summary>
        /// <param name="newNode">The new node.</param>
        /// <returns>A node route builder to allow adding more nodes.</returns>
        public NodeRouteBuilder AddNode(Node newNode)
        {
            // Get the previous node
            Node lastNode = nodes.Last();

            // If there was a previous node then add connections
            // between the previous node and the new one
            if (lastNode != null)
            {
                lastNode.AddConnection(newNode);

                newNode.AddConnection(lastNode);
            }

            // Add the node to the list
            nodes.Add(newNode);

            return this;
        }

        /// <summary>
        /// End the route and generate the list.
        /// </summary>
        /// <returns>A list of nodes in the route.</returns>
        public List<Node> End()
        {
            // Add a connection between the last node and the first one
            nodes.Last().AddConnection(nodes.First());

            // Add a connection between the first node and the last one
            nodes.First().AddConnection(nodes.Last());

            return nodes;
        }
    }
}
