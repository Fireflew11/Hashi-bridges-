using System;
using System.Collections.Generic;

public class Island
{
    public int X { get; }
    public int Y { get; }
    public int BridgesNeeded { get; set; }
    public List<Edge> Connections { get; } = new List<Edge>();

    public Island(int x, int y, int bridgesNeeded)
    {
        X = x;
        Y = y;
        BridgesNeeded = bridgesNeeded;
    }

    public void Connect(Island other)
    {
        if (other == null)
        {
            throw new ArgumentNullException(nameof(other));
        }

        Edge bridge = Connections.Find(e => e.Destination == other);
        
        if(bridge == null)
        {
            Connections.Add(new Edge(other, 1));
            other.Connections.Add(new Edge(this, 1));
        }
        else
        {
            bridge.BridgeCount = 2;
            other.Connections.Find(e => e.Destination == this).BridgeCount = 2;
        }
    }

    public void Disconnect(Island other)
    {
        if (other == null)
        {
            throw new ArgumentNullException(nameof(other));
        }

        Edge bridge = Connections.Find(e => e.Destination == other);

        if(bridge != null)
        {
            Connections.Remove(bridge);
            other.Connections.Remove(other.Connections.Find(e => e.Destination == this));
        }
    }

    internal void AddConnection(Island newIsland, int numberOfConnections)
    {
        if (numberOfConnections <= 0)
        {
            throw new ArgumentException("Number of connections must be greater than zero.", nameof(numberOfConnections));
        }

        if (newIsland == null)
        {
            throw new ArgumentNullException(nameof(newIsland));
        }

        Connections.Add(new Edge(newIsland, numberOfConnections));
        newIsland.Connections.Add(new Edge(this, numberOfConnections));
    }

    public int GetBridgeCount(Island toIsland)
    {
        foreach(Edge edge in Connections)
        {
            if(edge.Destination == toIsland)
            {
                return edge.BridgeCount;
            }
        }

        return 0;
    }

    public int GetBridgeCount()
    {
        int bridgeCount = 0;

        foreach(Edge edge in Connections)
        {
            bridgeCount += edge.BridgeCount;
        }

        return bridgeCount;
    }
}