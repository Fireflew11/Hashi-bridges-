public class Edge
{
    public Island Destination { get; }
    public int BridgeCount { get; set; }

    public Edge(Island destination, int bridgeCount)
    {
        Destination = destination;
        BridgeCount = bridgeCount;
    }
}