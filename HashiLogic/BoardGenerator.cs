using System;
using System.Collections.Generic;
using System.Linq;

public class BoardGenerator
{
    private Random rand = new Random();
    private HashSet<(int x, int y)> unavailablePositions = new HashSet<(int x, int y)>();
    private HashSet<(int x, int y)> nearIslandsUnavailable = new HashSet<(int x, int y)>();

    public HashiBoard GenerateSolvableBoard(int width, int height)
    {
        HashiBoard board = CreateBoard(width, height);

        ClearConnections(board);

        return board;
    }

    private HashiBoard CreateBoard(int width, int height)
    {
        HashiBoard board = new HashiBoard(width, height);
        HashSet<Island> islands = new HashSet<Island>();
        int firstIslandX = rand.Next(0, width);
        int firstIslandY = rand.Next(0, height);
        Island first = new Island(firstIslandX, firstIslandY, 0);
        nearIslandsUnavailable.Add((firstIslandX, firstIslandY));
        nearIslandsUnavailable.Add((firstIslandX + 1, firstIslandY));
        nearIslandsUnavailable.Add((firstIslandX - 1, firstIslandY));
        nearIslandsUnavailable.Add((firstIslandX, firstIslandY + 1));
        nearIslandsUnavailable.Add((firstIslandX, firstIslandY - 1));
        unavailablePositions.Add((firstIslandX, firstIslandY));
        islands.Add(first);
        board.AddIsland(first);
        int unsuccesfulAttempts = 0;

        while (islands.Count <= width * height / 3)
        {
            Island randIsland = islands.ElementAt(rand.Next(0, islands.Count));
            BuildIslands(board, islands, randIsland, width, height, ref unsuccesfulAttempts);

            if(unsuccesfulAttempts == 200)
            {
                break;
            }
        }

        foreach (var island in board.Islands)
        {
            foreach(var connection in island.Connections)
            {
                island.BridgesNeeded += connection.BridgeCount;
            }
        }

        ClearConnections(board);
        foreach (var island in board.Islands)
        {
            board.UnavailablePositions.Add((island.X, island.Y));
        }

        return board;
    }

    private void BuildIslands(HashiBoard board, HashSet<Island> islands, Island curIsland, int width, int height, ref int unsuccessfulAttempts)
    {
        var directions = new List<(int, int)> { (1, 0), (-1, 0), (0, 1), (0, -1) };
        var direction = directions[rand.Next(0, 4)];
        int newX = curIsland.X + direction.Item1 * rand.Next(2, 5);
        int newY = curIsland.Y + direction.Item2 * rand.Next(2, 5);
        fixOverTheMapToEdge(newX, width);
        fixOverTheMapToEdge(newY, height);

        if (unavailablePositions.Contains((newX, newY)) || newX < 0 || newX >= width || newY < 0 || newY >= height)
        {
            unsuccessfulAttempts++;
            return;
        }
        else if (IsCrossing(direction, curIsland, (newX, newY)))
        {
            unsuccessfulAttempts++;
            return;
        }
        else if (nearIslandsUnavailable.Contains((newX, newY)))
        {
            unsuccessfulAttempts++;
            return;
        }

        Island newIsland = new Island(newX, newY, 0);
        int numberOfConnections = rand.Next(1, 3);
        curIsland.AddConnection(newIsland, numberOfConnections);
        board.AddIsland(newIsland);
        islands.Add(newIsland);
        nearIslandsUnavailable.Add((newX, newY));
        nearIslandsUnavailable.Add((newX + 1, newY));
        nearIslandsUnavailable.Add((newX - 1, newY));
        nearIslandsUnavailable.Add((newX, newY + 1));
        nearIslandsUnavailable.Add((newX, newY - 1));
        unavailablePositions.Add((newX, newY));
        // Add all the points from the current island to the new island to the unavailable positions
        unsuccessfulAttempts = 0;
        AddConnectionsToUnavailablePositions(curIsland);

    }

    private int fixOverTheMapToEdge(int coordinate, int measure)
    {
        if (coordinate < 0)
        {
            return 0;
        }
        else if (coordinate >= measure)
        {
            return measure - 1;
        }
        return coordinate;
    }

    private void AddConnectionsToUnavailablePositions(Island curIsland)
    {
        foreach (var connection in curIsland.Connections)
        {

            int startX = curIsland.X;
            int startY = curIsland.Y;
            int endX = connection.Destination.X;
            int endY = connection.Destination.Y;

            int stepX = Math.Sign(endX - startX);
            int stepY = Math.Sign(endY - startY);

            if (stepX == 0)
            {
                for (int y = startY; y != endY; y += stepY)
                {
                    unavailablePositions.Add((startX, y));
                }
            }
            else
            {
               for (int x = startX; x != endX; x += stepX)
               {
                   unavailablePositions.Add((x, startY));
               }
            }
        }
    }

    private bool IsCrossing((int, int) direction, Island curIsland, (int, int) newIsland)
    {
        int stepX = Math.Sign(direction.Item1);
        int stepY = Math.Sign(direction.Item2);

        if(direction.Item1 == 0)
        {
            for (int y = curIsland.Y + stepY; y != newIsland.Item2; y += stepY)
            {
                if (unavailablePositions.Contains((curIsland.X, y)))
                {
                    return true;
                }
            }
        }
        else
        {
            for (int x = curIsland.X + stepX; x != newIsland.Item1; x += stepX)
            {
                if (unavailablePositions.Contains((x, curIsland.Y)))
                {
                    return true;
                }
            }
        }

        return false;
    }

    private void ClearConnections(HashiBoard board)
    {
        foreach (var island in board.Islands)
        {
           island.Connections.Clear();
        }
    }
}