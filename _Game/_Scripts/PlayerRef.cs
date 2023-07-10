using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRef : MonoBehaviour
{
  public static PlayerRef instance;
    private void Awake()
    {
        if (instance == null) instance = this;
        else
            Destroy(instance);
    }
}
