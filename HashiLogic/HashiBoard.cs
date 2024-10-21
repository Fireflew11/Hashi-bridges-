using System;

using System.Collections.Generic;
using System.Drawing;
using System.Text.RegularExpressions;

public class HashiBoard
{
    public int Width { get; }
    public int Height { get; }
    public HashSet<(int, int)> UnavailablePositions { get;} = new HashSet<(int, int)>();
    public List<Island> Islands { get; } = new List<Island>();
    

    public HashiBoard(int width, int height)
    {
        Width = width;
        Height = height;
    }

    public void AddIsland(int x, int y, int bridgesNeeded)
    {
        Islands.Add(new Island(x, y, bridgesNeeded));
    }

    public void AddIsland(Island island)
    {
        Islands.Add(island);
    }

    public void AddBridgeToUnavailable(Point fromIsland, Point toIsland)
    {
        int startX = fromIsland.X;
        int startY = fromIsland.Y;
        int endX = toIsland.X;
        int endY = toIsland.Y;
        int stepX = Math.Sign(endX - startX);
        int stepY = Math.Sign(endY - startY);

        if (startX == endX)
        {
            for (int y = startY; y != endY; y += stepY)
            {
                UnavailablePositions.Add((startX, y));
            }
        }
        else if (startY == endY)
        {
            for (int x = startX; x != endX; x += stepX)
            {
                UnavailablePositions.Add((x, startY));
            }
        }
    }

    public void RemoveBridgeFromUnavailable(Point fromIsland, Point toIsland)
    {
        int startX = fromIsland.X;
        int startY = fromIsland.Y;
        int endX = toIsland.X;
        int endY = toIsland.Y;
        int stepX = Math.Sign(endX - startX);
        int stepY = Math.Sign(endY - startY);

        if (startX == endX)
        {
            for (int y = startY + stepY; y != endY; y += stepY)
            {
                UnavailablePositions.Remove((startX, y));
            }
        }
        else if (startY == endY)
        {
            for (int x = startX + stepX; x != endX; x += stepX)
            {
                UnavailablePositions.Remove((x, startY));
            }
        }
    }

    public void ResetUnavailablePositions()
    {
        UnavailablePositions.Clear();

        foreach (Island island in Islands)
        {
            UnavailablePositions.Add((island.X, island.Y));
        }
    }

    public bool IsSolved()
    {
        foreach (Island island in Islands)
        {
            if (island.BridgesNeeded != island.GetBridgeCount())
            {
                return false;
            }
        }
        return IsConnectedGraph();

    }

    private bool IsConnectedGraph()
    {
        if (Islands.Count == 0)
            return true; // An empty board is trivially connected

        // Start DFS from the first island
        HashSet<Island> visited = new HashSet<Island>();
        Stack<Island> stack = new Stack<Island>();
        stack.Push(Islands[0]); // Start DFS from the first island
        visited.Add(Islands[0]);

        while (stack.Count > 0)
        {
            Island current = stack.Pop();

            // Traverse all connected islands (edges)
            foreach (Edge edge in current.Connections)
            {
                Island neighbor = edge.Destination;

                if (!visited.Contains(neighbor))
                {
                    visited.Add(neighbor);
                    stack.Push(neighbor);
                }
            }
        }

        // Check if all islands have been visited (i.e., the graph is connected)
        return visited.Count == Islands.Count;
    }
}