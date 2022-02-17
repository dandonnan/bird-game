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
            Node lastNode = nodes.Last();

            if (lastNode != null)
            {
                lastNode.AddConnection(newNode);

                newNode.AddConnection(lastNode);
            }

            nodes.Add(newNode);

            return this;
        }

        public List<Node> End()
        {
            nodes.Last().AddConnection(nodes.First());

            nodes.First().AddConnection(nodes.Last());

            return nodes;
        }
    }
}
