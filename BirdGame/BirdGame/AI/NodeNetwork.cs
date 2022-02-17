namespace BirdGame.AI
{
    using BirdGame.Enums;
    using BirdGame.World;
    using Microsoft.Xna.Framework;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    internal class NodeNetwork
    {
        private static NodeNetwork nodeNetwork;

        private readonly Dictionary<string, Node> nodes;

        private NodeNetwork()
        {
            nodes = PopulateDictionary();

            nodeNetwork = this;
        }

        public static void Initialise()
        {
            if (nodeNetwork == null)
            {
                new NodeNetwork();
            }
        }

        public static List<Node> GetNodes()
        {
            return nodeNetwork.nodes.Values.ToList();
        }

        public static Node FindNode(string id)
        {
            nodeNetwork.nodes.TryGetValue(id, out Node node);

            return node;
        }

        public static List<Node> GetRouteFromSpawnPoint(SpawnPoint spawnPoint)
        {
            List<Node> nodes = new List<Node>();

            Node firstNode = FindNode(spawnPoint.Id);

            nodes.Add(firstNode);

            for (int i = 0; i < 20; i++)
            {
                Node nextNode = nodes.Last().ConnectedNodes.First();

                if (i > 0)
                {
                    Node previousNode = nodes[i - 1];

                    if (nextNode == previousNode)
                    {
                        nextNode = nodes.Last().ConnectedNodes.Last();
                    }
                }

                nodes.Add(nextNode);
            }

            return nodes;
        }

        private static List<Node> GetOuterNodes()
        {
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

            Node parkRightMiddle = new Node("ParkMiddleRight", new Vector2(30, 12));
            Node parkTopRight = new Node("ParkTopRight", new Vector2(30, 9));
            Node parkMiddle = new Node("ParkMiddle", new Vector2(25, 12.5f));

            parkRightMiddle.AddConnection(parkMiddle);
            parkRightMiddle.AddConnection(parkTopRight);
            parkRightMiddle.AddConnection(outerNodes[11]);

            parkTopRight.AddConnection(parkRightMiddle);
            parkTopRight.AddConnection(outerNodes[15]);

            parkMiddle.AddConnection(parkRightMiddle);
            parkMiddle.AddConnection(outerNodes[13]);
            parkMiddle.AddConnection(outerNodes[14]);

            outerNodes.Add(parkRightMiddle);
            outerNodes.Add(parkTopRight);
            outerNodes.Add(parkMiddle);

            return outerNodes;
        }

        private static List<Node> GetInnerNodes()
        {
            return NodeRouteBuilder.Start(new Node("ShopTopLeft", new Vector2(8, 8)))
                                   .AddNode(new Node("ShopBottomLeft", new Vector2(8, 16)))
                                   .AddNode(new Node("ShopBottomRight", new Vector2(18, 16)))
                                   .AddNode(new Node("ShopTopRight", new Vector2(18, 11)))
                                   .AddNode(new Node("ShopRightCorner", new Vector2(12, 11)))
                                   .AddNode(new Node("ShopCornerMiddle", new Vector2(12, 9)))
                                   .AddNode(new Node("ShopLeftCorner", new Vector2(10.5f, 9)))
                                   .End();
        }

        private Dictionary<string, Node> PopulateDictionary()
        {
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

        private static List<Node> MapNodesToWorldPositions(List<Node> nodes, NodeDirection direction)
        {
            int multiplier = 32;
            int offset = direction == NodeDirection.North ? 8 : 24;
            string name = direction == NodeDirection.North ? "North" : "South";

            foreach (Node node in nodes)
            {
                node.Id += name;
                node.Position = new Vector2(((node.Position.X - 1) * multiplier) + offset, ((node.Position.Y - 1) * multiplier) + offset);
            }

            return nodes;
        }
    }
}
