using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;

public class HangmanGame : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI textField;
    [SerializeField] private int hp = 6;
    [SerializeField] private GameObject[] hpImage;
    [SerializeField] private TextMeshProUGUI wrongLetters;
    [SerializeField] private GameObject endPanel;
    [SerializeField] private TextMeshProUGUI resultText;
    [SerializeField] private TextMeshProUGUI hitPointsText;
    [SerializeField] private TextMeshProUGUI hintText;

    /*private readonly Dictionary<string, string> _words = new Dictionary<string, string>()
    {
        ["Cat"] = "Pet",
        ["Time"] = "What can fly without wings?",
        ["Unity"] = "Cross-platform game engine",
        ["Chair"] = "What has legs but cannot walk?",
        ["Fire"] = "If I drink, I die. If I eat, I am fine. What am I?"
    };
    private string[] _keys;*/

    private readonly string[] _words = {
        "Cat",
        "Time",
        "Unity",
        "Chair",
        "Fire"
    };

    private readonly string[] _hints = {
        "Pet",
        "What can fly without wings?",
        "Cross-platform game engine",
        "What has legs but cannot walk?",
        "If I drink, I die. If I eat, I am fine. What am I?"
    };

    private readonly List<char> _guessedLetters = new List<char>();
    private readonly List<char> _wrongTriedLetters = new List<char>();

    private string _wordToGuess = "";
    
    private KeyCode _lastKeyPressed;

    private void Start()
    {
        // _keys = _words.Keys.ToArray();
        // var randomKey = Random.Range(0, _keys.Length);
        // _wordToGuess = _keys[randomKey];
        //hintText.text = _words[_wordToGuess];

        var randomIndex = Random.Range(0, _words.Length);
        _wordToGuess = _words[randomIndex];
        hintText.text = _hints[randomIndex];
        
        hitPointsText.text = "Hit points: " + hp;
    }

    private void OnGUI()
    {
        var e = Event.current;
        if (!e.isKey) return; 
        
        if (e.keyCode == KeyCode.None || _lastKeyPressed == e.keyCode || endPanel.activeSelf) return;
        
        ProcessKey(e.keyCode);
        _lastKeyPressed = e.keyCode;
    }

    private void ProcessKey(KeyCode key)
    {
        var pressedKeyString = key.ToString()[0];

        var wordUppercase = _wordToGuess.ToUpper();
        var wordContainsPressedKey = wordUppercase.Contains(pressedKeyString);
        bool letterWasGuessed = _guessedLetters.Contains(pressedKeyString);

        if (!wordContainsPressedKey && !_wrongTriedLetters.Contains(pressedKeyString))
        {
            _wrongTriedLetters.Add(pressedKeyString);
            hp -= 1;
            ChangeHpImage(hp);
            
            if (hp <= 0)
            {
                endPanel.SetActive(true);
                
                resultText.text = "You lose!";
            }
            else
            {
                wrongLetters.text += pressedKeyString + " ";
                hitPointsText.text = "Hit points: " + hp;
            }
        }
        if (wordContainsPressedKey && !letterWasGuessed)
        {
            _guessedLetters.Add(pressedKeyString);
        }

        var stringToPrint = "";
        foreach (var letterInWord in wordUppercase)
        {
            if (_guessedLetters.Contains(letterInWord))
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
        }

        textField.text = stringToPrint;
    }

    private void ChangeHpImage(int hpValue)
    {
        foreach (var image in hpImage)
        {
            image.SetActive(hpValue.ToString() == image.name);
        }
    }

    
}
