using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryItem : MonoBehaviour
{
    
    public struct Item
    {
        public readonly Sprite m_Image;
        public string name;

        public Item(Sprite _image, string _name)
        {
            m_Image = _image;
            name = _name;
        }
    }
    
}