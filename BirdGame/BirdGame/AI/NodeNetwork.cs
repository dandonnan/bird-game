namespace BirdGame.AI
{
    using System.Collections.Generic;

    internal class NodeNetwork
    {
        private static NodeNetwork nodeNetwork;

        private readonly Dictionary<string, Node> nodes;

        private NodeNetwork()
        {
            nodes = new Dictionary<string, Node>();

            nodeNetwork = this;
        }

        public static void Initialise()
        {
            if (nodeNetwork == null)
            {
                new NodeNetwork();
            }
        }

        public static Node FindNode(string id)
        {
            nodeNetwork.nodes.TryGetValue(id, out Node node);

            return node;
        }
    }
}
