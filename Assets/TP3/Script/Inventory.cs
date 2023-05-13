using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using TP3.Script;
using UnityEngine;
using UnityEngine.UI;


public struct Item
{
    public readonly Sprite m_Image;
    public Inventory.ItemType type;

    public Item(Sprite _image, Inventory.ItemType _type)
    {
        m_Image = _image;
        type = _type;
    }
}


public class Inventory : MonoBehaviour
{
    private List<Image> m_InventoryImages;
    [SerializeField] private Sprite bananaImg;
    [SerializeField] private Sprite lightningImg;

    public enum ItemType
    {
        Banana,
        Lightning,
    }


    private int m_Capacity;

    private void Awake()
    {
        LevelManager.SubscribeGameAction += SubscribeAll;
        LevelManager.DisableGameAction += UnsubsribeAll;
    }

    void Start()
    {
        m_InventoryImages = new List<Image>();

        LevelManager.m_DescriptionBox = GameObject.Find("DescriptiveBox");
        LevelManager.m_InventoryBox = GameObject.Find("Inventory");
        LevelManager.m_PauseScreen = GameObject.Find("PauseScreen");
        LevelManager.m_EndButton = GameObject.Find("EndButton");
        LevelManager.m_EndButton.GetComponent<Button>().enabled = false;
        var imgList = GameObject.FindGameObjectsWithTag("ItemSlot");
        foreach (var image in imgList)
        {
            m_InventoryImages.Add(image.GetComponent<Image>());
        }
        
        LevelManager.SubscribeAll();
        LevelManager.Init();
    }

    void Update()
    {
    }

    private void Init()
    {
    }

    private void SubscribeAll()
    {
        LevelManager.CollectItemAction += CreateItem;
        LevelManager.CollectItemAction += UpdateInventory;
    }

    private void UnsubsribeAll()
    {
        LevelManager.CollectItemAction -= CreateItem;
        LevelManager.CollectItemAction -= UpdateInventory;
    }

    public void CreateItem(ItemType item)
    {
        if (item == ItemType.Banana)
        {
            var newItem = new Item(bananaImg, ItemType.Banana);
            LevelManager.m_Collection.Add(newItem);
        }

        if (item == ItemType.Lightning)
        {
            var newItem = new Item(lightningImg, ItemType.Lightning);
            LevelManager.m_Collection.Add(newItem);
        }

        CanEndGame();
    }

    private void UpdateInventory(ItemType item)
    {
        if (LevelManager.m_Collection.Count == 0) return;
        if (m_InventoryImages.Count == 0)
        {
            foreach (var image in GameObject.FindGameObjectsWithTag("ItemSlot"))
            {
                m_InventoryImages.Add(image.GetComponent<Image>());
            }
        }
        for (int i = 0; i < LevelManager.m_Collection.Count; i++)
        {
            m_InventoryImages[i].color = Color.white;
            m_InventoryImages[i].sprite = LevelManager.m_Collection[i].m_Image;
        }
    }

    private void CanEndGame()
    {
        if (LevelManager.m_Collection.Count == 8)
        {
            if (!LevelManager.GetGameWon()) AudioManager.instance.PlaySound(SoundClip.Win, 1.0f);
            LevelManager.m_EndButton.GetComponent<Button>().enabled = true;
            LevelManager.SetGameWon(true);
        }
    }
}