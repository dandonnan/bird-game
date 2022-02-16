namespace BirdGame.AI
{
    using System.Collections.Generic;
    using System.Linq;

    internal class NodeRouteBuilder
    {
        private readonly List<Node> nodes;

        private NodeRouteBuilder(Node node)
        {
            nodes = new List<Node>
            {
                node
            };
        }

        public static NodeRouteBuilder Start(Node node)
        {
            return new NodeRouteBuilder(node);
        }

        public NodeRouteBuilder AddNode(Node newNode)
        {
            nodes.Last().AddConnection(newNode);

            nodes.Add(newNode);

            return this;
        }

        public List<Node> End()
        {
            nodes.First().AddConnection(nodes.Last());

            return nodes;
        }
    }
}
