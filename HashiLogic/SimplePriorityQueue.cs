using System.Collections.Generic;
using System.Linq;

public class SimplePriorityQueue<T>
{
    private List<(T item, int priority)> elements = new List<(T, int)>();

    public int Count => elements.Count;

    public void Enqueue(T item, int priority)
    {
        elements.Add((item, priority));
        elements = elements.OrderBy(e => e.priority).ToList(); // Keep the list sorted by priority
    }

    public T Dequeue()
    {
        var bestItem = elements[0].item; // Get the item with the highest priority (lowest value)
        elements.RemoveAt(0); // Remove it from the list
        return bestItem;
    }
}
