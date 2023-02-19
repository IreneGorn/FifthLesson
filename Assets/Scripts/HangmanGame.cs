using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

public class HangmanGame : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _textField;
    [SerializeField] private int hp = 6;
    [SerializeField] private GameObject[] hpImage;
    [SerializeField] private TextMeshProUGUI _wrongLetters;
    [SerializeField] private GameObject endPanel;
    [SerializeField] private TextMeshProUGUI resultText;
    [SerializeField] private TextMeshProUGUI hitPointsText;

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
        
        hitPointsText.text = "Hit points: " + hp;
    }

    void OnGUI()
    {
        var e = Event.current;
        if (!e.isKey) return; //если клавиша не нажата - выходим из метода
        //Debug.Log("Detected key code: " + e.keyCode);
        
        if (e.keyCode == KeyCode.None || lastKeyPressed == e.keyCode || endPanel.activeSelf) return; //если клавиша "пустая" или такая же, как прошлая, то выходим из метода
        
        ProcessKey(e.keyCode);
        lastKeyPressed = e.keyCode;
    }

    private void ProcessKey(KeyCode key)
    {
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
            ChangeHpImage(hp);
            
            if (hp <= 0)
            {
                endPanel.SetActive(true);
                
                resultText.text = "You lose!";
            }
            else
            {
                _wrongLetters.text += pressedKeyString + " ";
                hitPointsText.text = "Hit points: " + hp;
                //print("Wrong letter! Hp left = " + hp);
            }
        }
        if (wordContainsPressedKey && !letterWasGuessed)
        {
            guessedLetters.Add(pressedKeyString);
        }

        bool entireWordGuessed = true;
        string stringToPrint = "";
        string stringOfWrongLetters = "";
        foreach (var letterInWord in wordUppercase)
        {
            if (guessedLetters.Contains(letterInWord))
            {
                stringToPrint += letterInWord;
            }
            else
            {
                stringToPrint += " _ ";
            }
        }

        if (wordUppercase == stringToPrint)
        {
            endPanel.SetActive(true);
            resultText.text = "Congratulations! You win!";
            print("You Win!");
        }
        
        //print(string.Join(", ", guessedLetters));
        //print(stringToPrint);
        _textField.text = stringToPrint;
    }
    
    void ChangeHpImage(int hpValue)
    {
        foreach (var image in hpImage)
        {
            image.SetActive(hpValue.ToString() == image.name);
        }
    }

    
}
