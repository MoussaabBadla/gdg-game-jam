using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Book : Interactable
{
    public Image page;
    public Sprite texture;
    public GameObject UI;
    public override void OnInteract()
    {
        UI.SetActive(true);
        page.sprite= texture;
        PlayerRef.instance.GetComponent<PlayerMovement>().FreezMotion();
    }
}
