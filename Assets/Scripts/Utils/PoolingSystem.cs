using System.Collections;
using System.Collections.Generic;

/// <summary>
/// Custom pooling system to manage generic objects
/// </summary>
/// <typeparam name="T"></typeparam>
public class PoolingSystem <T>
{
    private System.Random _random;

    private List<T> _poolList;
    private List<int> _itemsAvailable;
    private Dictionary<T, int> _itemsIndexes;

    public PoolingSystem(List<T> itemList)
    {
        _random = new System.Random(); //Chooses a random seed by default. This makes sure every playthrough is almost unique
       
        _poolList = itemList;

        _itemsAvailable = new List<int>(_poolList.Count);
        _itemsIndexes = new Dictionary<T, int>(_poolList.Count);

        for (int i = 0; i < _poolList.Count; ++i)
        {
            T item = _poolList[i];
            _itemsIndexes.Add(item, i);
            _itemsAvailable.Add(i);
        }
    }

    public T GetRandomItem()
    {
        if (_itemsAvailable.Count <= 0)
        {
            return default(T);
        }

        int index = _random.Next(0, _itemsAvailable.Count);

        int itemIndex = _itemsAvailable[index];
        _itemsAvailable[index] = _itemsAvailable[_itemsAvailable.Count - 1]; // Swap with last index item
        _itemsAvailable.RemoveAt(_itemsAvailable.Count - 1); // Remove then las index item (this operation is O(1))

        T item = _poolList[itemIndex];

        return item;
    }

    public void ReleaseItem(T item)
    {
        int index;
        _itemsIndexes.TryGetValue(item, out index);

        _itemsAvailable.Add(index);
    }

    public List<T> GetPoolList()
    {
        return _poolList;
    }

    public void ReleaseItems()
    {
        _itemsAvailable.Clear();

        for (int i = 0; i < _poolList.Count; ++i)
        {
            _itemsAvailable.Add(i);
        }
    }
}
