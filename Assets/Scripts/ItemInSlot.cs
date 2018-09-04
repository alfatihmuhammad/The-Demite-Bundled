using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemInSlot : MonoBehaviour {

    Image itemImage;

    // Use this for initialization
    void Start () {
        itemImage = transform.GetChild(0).GetComponentInChildren<Image>();
        Debug.Log("NAMANYA "+itemImage.sprite.name);

    }
	
	// Update is called once per frame
	void Update () {
        
	}

    public void ItemOnClick() {
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
