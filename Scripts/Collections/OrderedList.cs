using System.Collections;

namespace JimmysUnityUtilities.Collections;

/// <summary>
/// A version of List<T> where the items always remain sorted/ordered.
/// Unlike SortedSet<T>, allows fast access by index.
/// Also unlike SortedSet<T>, allows duplicates.
/// </summary>
public sealed class OrderedList<T> : IReadOnlyList<T>
{
    private readonly List<T> _BackingList;
    private readonly IComparer<T>? Comparer;

    public OrderedList(IComparer<T>? comparer = null)
    {
        _BackingList = new();
        Comparer = comparer;
    }

    public OrderedList(int capacity, IComparer<T>? comparer = null)
    {
        _BackingList = new(capacity);
        Comparer = comparer;
    }

    public OrderedList(IEnumerable<T> contents, IComparer<T>? comparer = null)
    {
        _BackingList = new(contents);
        Comparer = comparer;

        _BackingList.Sort(Comparer);
    }

    public OrderedList(ReadOnlySpan<T> contents, IComparer<T>? comparer = null)
    {
        _BackingList = new(contents.Length);
        Comparer = comparer;

        for (int i = 0; i < contents.Length; i++)
            _BackingList.Add(contents[i]);

        _BackingList.Sort(Comparer);
    }

    public OrderedList(ReadOnlyMemory<T> contents, IComparer<T>? comparer = null) : this(contents.Span, comparer) { }


    public void Add(T item)
    {
        int index = _BackingList.BinarySearch(item, Comparer);

        if (index < 0)
            index = ~index; // Bitwise complement gives insertion point when item not found
        
        _BackingList.Insert(index, item);
    }
    
    public void AddRange(IEnumerable<T> items)
    {
        _BackingList.AddRange(items);
        _BackingList.Sort(Comparer);
    }

    public bool Remove(T item)
    {
        int index = _BackingList.BinarySearch(item, Comparer);
        if (index < 0)
            return false;
            
        _BackingList.RemoveAt(index);
        return true;
    }

    public int IndexOf(T item)
    {
        int index = _BackingList.BinarySearch(item, Comparer);
        return index >= 0 ? index : -1;
    }

    public bool Contains(T item)
        => _BackingList.BinarySearch(item, Comparer) >= 0;

    public void Clear() => _BackingList.Clear();

    public T this[int index] => _BackingList[index];
    public int Count => _BackingList.Count;

    public IEnumerator<T> GetEnumerator() => _BackingList.GetEnumerator();
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public T[] ToArray() => _BackingList.ToArray();
    public Memory<T> ToMemory() => new(ToArray());
}