using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{

    [SerializeField] private Item[] _defaultItems; // Items por defecto al iniciar el juego
    [SerializeField] private int[] _quantity;

    private Dictionary<Item,int> _items = new Dictionary<Item, int>();


    
    public delegate void OnItemChanged(); // Define un delegado para notificar cambios en el inventario
    public OnItemChanged onItemChangedCallback; // Evento que se invoca cuando el inventario cambia
    public Dictionary<Item, int> Items => _items; 

  
    public void addDefaultItems()
    {
        if (_defaultItems.Length < 1) return;
        if (_defaultItems.Length != _quantity.Length)
        {
            Debug.LogError("Default items and quantity arrays must have the same length");
            return;
        }
        // Agrega los items por defecto al inventario

        for (int i = 0; i < _defaultItems.Length; i++)
        {
            for (int j = 0; j < _quantity[i]; j++)
            {
                if (_defaultItems[i] == null) break;
                if (_quantity[i] > _defaultItems[i].MaxStack)
                    _quantity[i] = _defaultItems[i].MaxStack;
                AddItem(_defaultItems[i]);

            }
        }
    }

    public void ResetInventory()
    {
        _items.Clear();
        onItemChangedCallback?.Invoke();
        addDefaultItems();
    }

    // Agrega un item al inventario, si ya existe y no esta en su maximo de usos, aumenta el uso en 1.
    public bool AddItem(Item item)
    {

        if (_items.ContainsKey(item))
        {
            if (_items[item] < item.MaxStack)
            {
                _items[item]++;
                onItemChangedCallback?.Invoke();
                return true;
            }
            else
            {
                return false;
            }
        }
        else
        {
            _items.Add(item, 1);
            onItemChangedCallback?.Invoke();
            return true;
        }
    }
    // Remueve un item del inventario, si tiene usos, reduce el uso en 1.
    public void RemoveItem(Item item)
    {
        if (_items.ContainsKey(item))
        {
            if (_items[item] > 1)
            {
                _items[item]--;
            }
            else
            {
                _items[item] = 0;
            }
            onItemChangedCallback?.Invoke();
        }


    }


}
