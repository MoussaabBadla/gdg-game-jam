using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : Interactable
{

    public GameObject PuzzleUI;
    bool interacted;
    public override void OnInteract()
    {
        PuzzleUI.SetActive(true);
        PlayerRef.instance.GetComponent<PlayerMovement>().FreezMotion();
        interacted = true;
    }
    private void Update()
    {
        if (interacted && Input.GetKeyDown(KeyCode.Escape))
        {
            interacted = false;
            PuzzleUI.SetActive(false);
            PlayerRef.instance.GetComponent<PlayerMovement>().UnFreezMotion();
        }
    }

}
