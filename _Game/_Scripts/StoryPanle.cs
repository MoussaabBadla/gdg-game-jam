using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
public class StoryPanle : MonoBehaviour
{
    public Image image;
    public Text text;
    public float time;
    public float fadeSpeed;
    bool fade;
    public UnityEvent Faded = new UnityEvent();
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(Wait());
    }
    IEnumerator Wait()
    {
        yield return new WaitForSeconds(time);
        fade = true;    
    }
    // Update is called once per frame
    void Update()
    {
        if (fade)
        {
            image.color = new Color(image.color.r, image.color.g, image.color.b, Mathf.Lerp(image.color.a, 0f, fadeSpeed * Time.deltaTime));
            text.color = new Color(text.color.r, text.color.g, text.color.b, Mathf.Lerp(text.color.a, 0f, fadeSpeed * Time.deltaTime));
            if (image.color.a<0.1f)
            {
                Faded?.Invoke();
                Destroy(gameObject);
            }
        }
    }
}
