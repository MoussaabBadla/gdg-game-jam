using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ocean : MonoBehaviour
{
    public AudioSource AudioSource;
    public float wait = 4f;
    public SceneLoader SceneLoader;
    public GameObject Cover;
    public void Load()
    {
        Cover.SetActive(true);
        AudioSource.Play();
        StartCoroutine(WaitLoad());
    }
    public IEnumerator WaitLoad()
    {
        yield return new WaitForSeconds(wait);
        SceneLoader.LoadScene();    
    }
}
