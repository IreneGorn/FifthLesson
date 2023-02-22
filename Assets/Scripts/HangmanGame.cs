using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

public class HangmanGame : MonoBehaviour
{
    [FormerlySerializedAs("textField")] [SerializeField] private TextMeshProUGUI _textField;
    [FormerlySerializedAs("hp")] [SerializeField] private int _hp = 6;
    [FormerlySerializedAs("hpImage")] [SerializeField] private GameObject[] _hpImage;
    [FormerlySerializedAs("wrongLetters")] [SerializeField] private TextMeshProUGUI _wrongLetters;
    [FormerlySerializedAs("endPanel")] [SerializeField] private GameObject _endPanel;
    [FormerlySerializedAs("resultText")] [SerializeField] private TextMeshProUGUI _resultText;
    [FormerlySerializedAs("hitPointsText")] [SerializeField] private TextMeshProUGUI _hitPointsText;
    [FormerlySerializedAs("hintText")] [SerializeField] private TextMeshProUGUI _hintText;

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
        //_hintText.text = _words[_wordToGuess];

        var randomIndex = Random.Range(0, _words.Length);
        _wordToGuess = _words[randomIndex];
        _hintText.text = _hints[randomIndex];
        
        _hitPointsText.text = "Hit points: " + _hp;
    }

    private void OnGUI()
    {
        var e = Event.current;
        if (!e.isKey) return; 
        
        if (e.keyCode == KeyCode.None || _lastKeyPressed == e.keyCode || _endPanel.activeSelf) return;
        
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
            _hp -= 1;
            ChangeHpImage(_hp);
            
            if (_hp <= 0)
            {
                _endPanel.SetActive(true);
                
                _resultText.text = "You lose!";
            }
            else
            {
                _wrongLetters.text += pressedKeyString + " ";
                _hitPointsText.text = "Hit points: " + _hp;
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
            _endPanel.SetActive(true);
            _resultText.text = "Congratulations! You win!";
        }

        _textField.text = stringToPrint;
    }

    private void ChangeHpImage(int hpValue)
    {
        foreach (var image in _hpImage)
        {
            image.SetActive(hpValue.ToString() == image.name);
        }
    }

    
}
