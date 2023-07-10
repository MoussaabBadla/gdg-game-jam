using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class PuzzleLock : MonoBehaviour
{
    public string Code;
    public int firstNumber;
    public int secondNumber;
    public int thirdNumber;

    public TextMeshProUGUI num1;
    public TextMeshProUGUI num2;
    public TextMeshProUGUI num3;
    public GameObject Cover;
    public AudioSource audioSource;
    public SceneLoader sceneLoader;
    public void Rotate(int index)
    {
        switch (index)
        {

            case 0:
                firstNumber++;
                break;
            case 1:
                secondNumber++;
                break;


            case 2:
                thirdNumber++;
                break;
            default:
                break;
        }
        firstNumber= Mathf.Clamp(firstNumber, 0, 9);
        secondNumber = Mathf.Clamp(secondNumber, 0, 9);
        thirdNumber= Mathf.Clamp(thirdNumber, 0, 9);
        num1.text = firstNumber.ToString();
        num2.text = secondNumber.ToString();
        num3.text = thirdNumber.ToString();

        if (CheckNumber())
        {
            PuzzleSolved();

        }
    }
    public void AntiRotate(int index)
    {
        switch (index)
        {

            case 0:
                firstNumber--;
                break;
            case 1:
                secondNumber--;
                break;


            case 2:
                thirdNumber--;
                break;
            default:
                break;
        }
        firstNumber = Mathf.Clamp(firstNumber, 0, 9);
        secondNumber = Mathf.Clamp(secondNumber, 0, 9);
        thirdNumber = Mathf.Clamp(thirdNumber, 0, 9);
        num1.text = firstNumber.ToString();
        num2.text = secondNumber.ToString();
        num3.text = thirdNumber.ToString();
        if (CheckNumber())
        {
            PuzzleSolved();

        }
    }
    public void PuzzleSolved()
    {
        Cover.SetActive(true);
        audioSource.Play();
        StartCoroutine(wait());
    }
    IEnumerator wait()
    {
        yield return new WaitForSeconds(3f);
        sceneLoader.LoadScene();
    }
    public bool CheckNumber()
    {
         int num =Int16.Parse(Code);
        print(num);
        int one  = (num / (int)Math.Pow(10, 3 - 1)) % 10;
        int two  = (num / (int)Math.Pow(10, 2- 1)) % 10;
        int three  = (num / (int)Math.Pow(10, 1 - 1)) % 10;
        print(one);
        print(two);
        print(three);
        if (one==firstNumber && two== secondNumber && three == thirdNumber)
        {

            return true;
        }
        return false;
    }
    



}
