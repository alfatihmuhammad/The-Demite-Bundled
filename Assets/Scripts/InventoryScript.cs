using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UiImage = UnityEngine.UI.Image;
using UnityEngine.UI;

public class InventoryScript : MonoBehaviour
{

    Image itemImage;

    // Use this for initialization
    void Start()
    {
        itemImage = transform.GetChild(0).GetComponentInChildren<Image>();
        Debug.Log("NAMANYA " + itemImage.sprite.name);
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void ItemOnClick()
    {
        if (itemImage.sprite == null)
        {
            Debug.Log("EMPTY");
        }
        else
        {
            Debug.Log("NOT EMPTY");
        }
    }
}


    //public UnityEngine.UI.Image[] image = new UiImage[10];
    //public int inventory_active;

    //void Start()
    //{
    //    inventory_active = 0;
    //}

    //void Update()
    //{

    //}

    //public void ThePocket(int poc)
    //{
    //    if (poc == 3)
    //    {
    //        //image3.color = Color.white;
    //        //image8.color = image5.color = Color.grey;
    //    }
    //    else if (poc == 5)
    //    {
    //        //image5.color = Color.white;
    //        //image8.color = image3.color = Color.grey;
    //    }
    //    else if (poc == 8)
    //    {
    //        //image8.color = Color.white;
    //        //image3.color = image5.color = Color.grey;
    //    }
    //    inventory_active = poc;
    //}
//}
