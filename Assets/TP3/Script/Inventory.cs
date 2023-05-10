using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{
    [SerializeField] private List<Image> m_InventoryImages;
    [SerializeField] private Sprite bananaImg;
    [SerializeField] private Button m_EndButton;

    public enum ItemType
    {
        Banana,
        Icecube,
    }

    private List<InventoryItem.Item> m_Collection;

    private int m_Capacity;

    void Start()
    {
        m_Collection = new List<InventoryItem.Item>();
        m_Capacity = 8;
        LevelManager.instance.CollectItemAction += CreateItem;
        LevelManager.instance.CollectItemAction += UpdateInventory;
        LevelManager.instance.PauseAction += CanEndGame;
    }

    void Update()
    {
    }

    private void CreateItem(ItemType _item)
    {
        if (_item == ItemType.Banana)
        {
            var newItem = new InventoryItem.Item(bananaImg, "Banana");
            m_Collection.Add(newItem);
        }
    }

    private void UpdateInventory(ItemType _item)
    {
        for (int i = 0; i < m_Collection.Count; i++)
        {
            m_InventoryImages[i].GetComponent<Image>().color = Color.white;
            m_InventoryImages[i].GetComponent<Image>().sprite = m_Collection[i].m_Image;
        }
    }

    private void CanEndGame()
    {
        if (m_Collection.Count == 8)
        {
            m_EndButton.enabled = true;
        }
        else
        {
            m_EndButton.enabled = false;
        }
    }
}