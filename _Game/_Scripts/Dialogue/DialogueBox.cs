using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class DialogueBox : MonoBehaviour
{
   
   
   public Text textDisplayer;
    public Text nameleft;
    public Text nameright;
    public Image characterImageright;
   public Image characterImageleft;

    public void Show(string text, Sprite sprite, bool right, string speakerName)
    {
        GetComponent<Image>().color = new Color(GetComponent<Image>().color.r, GetComponent<Image>().color.g, GetComponent<Image>().color.b,1);
        textDisplayer.GetComponent<ArabicFixer>().fixedText= text;
        characterImageright.sprite= sprite;
        characterImageleft.sprite = sprite;
        if (!right)
        {
            characterImageright.enabled= false;
            characterImageleft.enabled = true;
            textDisplayer.alignment = TextAnchor.UpperLeft;
            nameleft.GetComponent<ArabicFixer>().fixedText = speakerName;
            nameright.GetComponent<ArabicFixer>().fixedText = " ";
        }
        else
        {
            characterImageright.enabled = true;
            characterImageleft.enabled = false;
            textDisplayer.alignment = TextAnchor.UpperRight;
            nameleft.GetComponent<ArabicFixer>().fixedText = " ";
            nameright.GetComponent<ArabicFixer>().fixedText = speakerName;
        }
       
     
    }
 



}
