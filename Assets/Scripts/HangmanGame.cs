using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;

public class HangmanGame : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _textField;
    [SerializeField] private int hp = 6;
    [SerializeField] private GameObject[] hpImage;

    
    private List<char> guessedLetters = new List<char>();
    private List<char> wrongTriedLetters = new List<char>();

    private string[] words =
    {
        "Cat",
        "Lesson",
        "Time",
        "Unity" 
    };

    private string wordToGuess = "";
    
    private KeyCode lastKeyPressed;

    private void Start()
    {
        var randomIndex = Random.Range(0, words.Length);
        
        wordToGuess = words[randomIndex];
    }

    void OnGUI()
    {
        var e = Event.current;
        if (!e.isKey) return; //если клавиша не нажата - выходим из метода
        //Debug.Log("Detected key code: " + e.keyCode);
        
        if (e.keyCode == KeyCode.None || lastKeyPressed == e.keyCode) return; //если клавиша "пустая" или такая же, как прошлая, то выходим из метода
        
        ProcessKey(e.keyCode);
        lastKeyPressed = e.keyCode;
    }

    private void ProcessKey(KeyCode key)
    {
        var word = "Time";
        
        //print("Word to guess is: " + word);
        print("Key pressed: " + key);

        var pressedKeyString = key.ToString()[0];

        var wordUppercase = wordToGuess.ToUpper();
        var wordContainsPressedKey = wordUppercase.Contains(pressedKeyString);
        bool letterWasGuessed = guessedLetters.Contains(pressedKeyString);

        if (!wordContainsPressedKey && !wrongTriedLetters.Contains(pressedKeyString))
        {
            wrongTriedLetters.Add(pressedKeyString);
            hp -= 1;
            ChangeHPImage(hp);
            
            if (hp <= 0)
            {
                print("You lose!");
            }
            else
            {
                print("Wrong letter! Hp left = " + hp);
            }
        }
        if (wordContainsPressedKey && !letterWasGuessed)
        {
            guessedLetters.Add(pressedKeyString);
        }

        bool entireWordGuessed = true;
        string stringToPrint = "";
        foreach (var letterInWord in wordUppercase)
        {
            if (guessedLetters.Contains(letterInWord))
            {
                stringToPrint += letterInWord;
            }
            else
            {
                stringToPrint += "_";
            }
        }

        if (wordUppercase == stringToPrint)
        {
            print("You Win!");
        }
        
        //print(string.Join(", ", guessedLetters));
        print(stringToPrint);
        _textField.text = stringToPrint;
    }
    
    void ChangeHPImage(int hpValue)
    {
        foreach (var image in hpImage)
        {
            image.SetActive(hpValue.ToString() == image.name);
        }
    }
}
