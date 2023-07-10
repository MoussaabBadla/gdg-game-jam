using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Interactable : MonoBehaviour
{
    public Outline outline;
    private void Start()
    {
        outline = GetComponent<Outline>();
        UnHilight();
    }
    public abstract void OnInteract();
    

    public void Hilight()
    {
        outline.enabled = true;
    }
    public void UnHilight()
    {
        outline.enabled = false;
    }
}
