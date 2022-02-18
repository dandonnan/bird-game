namespace BirdGame.AI
{
    using BirdGame.Enums;
    using BirdGame.World;
    using Microsoft.Xna.Framework;
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// A network of nodes for the AI to navigate between.
    /// </summary>
    internal class NodeNetwork
    {
        /// <summary>
        /// A singleton for the node network that makes it accessible elsewhere.
        /// </summary>
        private static NodeNetwork nodeNetwork;

        /// <summary>
        /// A dictionary of nodes in the network. The structure is [Id, Node]
        /// in order to get a node based on it's id.
        /// </summary>
        private readonly Dictionary<string, Node> nodes;

        /// <summary>
        /// Create a node network. This is private to force it to be
        /// created through the Initialise method.
        /// </summary>
        private NodeNetwork()
        {
            nodes = PopulateDictionary();

            nodeNetwork = this;
        }

        /// <summary>
        /// Initialise the node network by calling the constructor.
        /// </summary>
        public static void Initialise()
        {
            if (nodeNetwork == null)
            {
                new NodeNetwork();
            }
        }

        /// <summary>
        /// Get a list of nodes that are in the network.
        /// This gets the values out of the dictionary.
        /// </summary>
        /// <returns>A list of nodes in the network.</returns>
        public static List<Node> GetNodes()
        {
            return nodeNetwork.nodes.Values.ToList();
        }

        /// <summary>
        /// Find a node in the network by it's id.
        /// </summary>
        /// <param name="id">The identifier of the node.</param>
        /// <returns>The node with the given id, or null if not found.</returns>
        public static Node FindNode(string id)
        {
            nodeNetwork.nodes.TryGetValue(id, out Node node);

            return node;
        }

        /// <summary>
        /// Get a route for an AI to navigate starting at the given node.
        /// </summary>
        /// <param name="node">The node to navigate from.</param>
        /// <returns>A list of nodes.</returns>
        public static List<Node> GetRouteFromNode(Node node)
        {
            return GetRouteFromId(node.Id);
        }

        /// <summary>
        /// Get a route for an AI to navigate starting at the given spawn point.
        /// </summary>
        /// <param name="spawnPoint">The spawn point to navigate from.</param>
        /// <returns>A list of nodes.</returns>
        public static List<Node> GetRouteFromSpawnPoint(SpawnPoint spawnPoint)
        {
            return GetRouteFromId(spawnPoint.Id);
        }

        /// <summary>
        /// Get a route for an AI to navigate based on the id of a node.
        /// </summary>
        /// <param name="id">The node identifier.</param>
        /// <returns>A list of nodes.</returns>
        private static List<Node> GetRouteFromId(string id)
        {
            List<Node> nodes = new List<Node>();

            // Find the node with the given id
            Node firstNode = FindNode(id);

            // Add the node to the list, as the starting point
            nodes.Add(firstNode);

            // Find another 20 nodes to connect to - 20 is just
            // an arbitrary number as it's likely the AI will have
            // despawned before they hit it
            for (int i = 0; i < 20; i++)
            {
                // From the current / latest node added to the list, get the
                // first node from it's list of connected nodes - this should
                // become the next node
                Node nextNode = nodes.Last().ConnectedNodes.First();

                // If not at the start
                if (i > 0)
                {
                    // Find the node before the current one
                    Node previousNode = nodes[i - 1];

                    // If the next node is the same as the previous node
                    // then use the last node from the current / latest node's
                    // connected nodes list instead, otherwise the AI will
                    // walk between two spots
                    if (nextNode == previousNode)
                    {
                        nextNode = nodes.Last().ConnectedNodes.Last();
                    }
                }

                // Add the next node to the list
                nodes.Add(nextNode);
            }

            return nodes;
        }

        /// <summary>
        /// Get a list of nodes on the outer street.
        /// </summary>
        /// <returns>A list of nodes.</returns>
        private static List<Node> GetOuterNodes()
        {
            // Use the route builder to build up a list of nodes at the given co-ordinates
            // These don't match in-game positions - they are multiplied by the tile size later
            List<Node> outerNodes = NodeRouteBuilder.Start(new Node("OffscreenTopLeft", new Vector2(6, -1)))
                                                    .AddNode(new Node("CarParkTopRight", new Vector2(6, 6)))
                                                    .AddNode(new Node("CarParkTopLeft", new Vector2(-1, 6)))
                                                    .AddNode(new Node("CarParkBottomLeft", new Vector2(-1, 15)))
                                                    .AddNode(new Node("CarParkBottomRight", new Vector2(6, 15)))
                                                    .AddNode(new Node("BottomStreetLeft", new Vector2(6, 18)))
                                                    .AddNode(new Node("LeftBeach", new Vector2(8, 18)))
                                                    .AddNode(new Node("MiddleBeach", new Vector2(16, 18)))
                                                    .AddNode(new Node("RightBeach", new Vector2(25, 18)))
                                                    .AddNode(new Node("OffscreenBottomRight", new Vector2(33, 18)))
                                                    .AddNode(new Node("OffscreenBottomLeft", new Vector2(33, 16)))
                                                    .AddNode(new Node("ParkBottomRight", new Vector2(30, 16)))
                                                    .AddNode(new Node("ParkBottomMiddle", new Vector2(25, 16)))
                                                    .AddNode(new Node("ParkBottomLeft", new Vector2(20, 16)))
                                                    .AddNode(new Node("ParkMiddleLeft", new Vector2(20, 13)))
                                                    .AddNode(new Node("ParkTopLeft", new Vector2(20, 9)))
                                                    .AddNode(new Node("TopStreetRight", new Vector2(14, 9)))
                                                    .AddNode(new Node("TopStreetMiddleRight", new Vector2(14, 8)))
                                                    .AddNode(new Node("TopStreetMiddleLeft", new Vector2(13, 7)))
                                                    .AddNode(new Node("TopStreetMiddleTop", new Vector2(12, 6)))
                                                    .AddNode(new Node("TopStreetLeft", new Vector2(8, 6)))
                                                    .AddNode(new Node("OffscreenTopRight", new Vector2(8, -1)))
                                                    .End();

            // Create some extra nodes for instead the park
            Node parkRightMiddle = new Node("ParkMiddleRight", new Vector2(30, 12));
            Node parkTopRight = new Node("ParkTopRight", new Vector2(30, 9));
            Node parkMiddle = new Node("ParkMiddle", new Vector2(25, 12.5f));

            // Add connections for the park nodes
            parkRightMiddle.AddConnection(parkMiddle);
            parkRightMiddle.AddConnection(parkTopRight);
            parkRightMiddle.AddConnection(outerNodes[11]);

            parkTopRight.AddConnection(parkRightMiddle);
            parkTopRight.AddConnection(outerNodes[15]);

            parkMiddle.AddConnection(parkRightMiddle);
            parkMiddle.AddConnection(outerNodes[13]);
            parkMiddle.AddConnection(outerNodes[14]);

            // Add the park nodes into the list
            outerNodes.Add(parkRightMiddle);
            outerNodes.Add(parkTopRight);
            outerNodes.Add(parkMiddle);

            return outerNodes;
        }

        /// <summary>
        /// Get a list of nodes from the inner street.
        /// </summary>
        /// <returns>A list of nodes.</returns>
        private static List<Node> GetInnerNodes()
        {
            // Use the route builder to build up a list of nodes at the given co-ordinates
            // These don't match in-game positions - they are multiplied by the tile size later
            return NodeRouteBuilder.Start(new Node("ShopTopLeft", new Vector2(8, 8)))
                                   .AddNode(new Node("ShopBottomLeft", new Vector2(8, 16)))
                                   .AddNode(new Node("ShopBottomRight", new Vector2(18, 16)))
                                   .AddNode(new Node("ShopTopRight", new Vector2(18, 11)))
                                   .AddNode(new Node("ShopRightCorner", new Vector2(12, 11)))
                                   .AddNode(new Node("ShopCornerMiddle", new Vector2(12, 9)))
                                   .AddNode(new Node("ShopLeftCorner", new Vector2(10.5f, 9)))
                                   .End();
        }

        /// <summary>
        /// Populate the node network dictionary.
        /// </summary>
        /// <returns>A dictionary of nodes.</returns>
        private Dictionary<string, Node> PopulateDictionary()
        {
            // Get 
            List<Node> outerNodesNorth = MapNodesToWorldPositions(GetOuterNodes(), NodeDirection.North);
            List<Node> outerNodesSouth = MapNodesToWorldPositions(GetOuterNodes(), NodeDirection.South);
            List<Node> innerNodesNorth = MapNodesToWorldPositions(GetInnerNodes(), NodeDirection.North);
            List<Node> innerNodesSouth = MapNodesToWorldPositions(GetInnerNodes(), NodeDirection.South);

            outerNodesSouth.Reverse();
            innerNodesSouth.Reverse();

            List<Node> allNodes = new List<Node>();
            allNodes.AddRange(outerNodesNorth);
            allNodes.AddRange(outerNodesSouth);
            allNodes.AddRange(innerNodesNorth);
            allNodes.AddRange(innerNodesSouth);

            return allNodes.ToDictionary(n => n.Id);
        }

        /// <summary>
        /// Map nodes from their co-ordinates into game positions.
        /// </summary>
        /// <param name="nodes">The list of nodes.</param>
        /// <param name="direction">The direction of the nodes.</param>
        /// <returns>A list of nodes.</returns>
        private static List<Node> MapNodesToWorldPositions(List<Node> nodes, NodeDirection direction)
        {
            // Multiply the co-ordinates by 32 to turn them into game positions
            int multiplier = 32;
            
            // Add an offset from the position to spawn the AI from
            // If heading north, the offset is 8. If south, it will be 24
            int offset = direction == NodeDirection.North ? 8 : 24;

            // Get the direction to append the id to make it unique
            string name = direction == NodeDirection.North ? "North" : "South";

            // Go through the list of nodes and update the names and positions
            foreach (Node node in nodes)
            {
                node.Id += name;

                // The co-ordinates are actually off by 1 because code starts at 0,
                // but human brains start at 1 and it was too much effort to change the lists above
                node.Position = new Vector2(((node.Position.X - 1) * multiplier) + offset, ((node.Position.Y - 1) * multiplier) + offset);
            }

            return nodes;
        }
    }
}
