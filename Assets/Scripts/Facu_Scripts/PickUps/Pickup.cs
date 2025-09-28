using UnityEngine;

public class Pickup : MonoBehaviour
{
    [Header("Item to pickup reference")]
    [SerializeField] private Item item;


    private Inventory _inventory;
    private PlayerManager _playerManager;
    private CheckPointManager _pickUpsManager;


    private void Start()
    {
        _pickUpsManager = GameManager.instance.CheckPointManager;
        _inventory = GameManager.instance.Inventory;
       _playerManager = GameManager.instance.PlayerManager;
    }


    // Si el objeto que entra en el trigger es el jugador,
    // intenta agregar el item al inventario y destruye el objeto si se agrega con exito
    private void OnTriggerEnter(Collider other)
    {
        if (_playerManager.CompareLayer(other.gameObject.layer))
        {
            if(_inventory.AddItem(item))
            {
                _pickUpsManager.DeletePickUp(gameObject.name);
                gameObject.SetActive(false);

            }
        }
    }
}
