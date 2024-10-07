using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [SerializeField] private GameData _gameData;
    
    public int Difficulty { get => _gameData.difficulty; }
    public int GlobalGas { get => _gameData.globalGas; }
    private void Awake()
    {
        instance = this;
        
        DontDestroyOnLoad(this.gameObject);
    }

    public void ChangeDifficulty(bool addDiff)
    {
        if (addDiff)
        {
            _gameData.difficulty ++;
        }
        else
        {
            _gameData.difficulty = 1;
        }
    }

    public bool UseGas(int valueToChange)
    {
        if (valueToChange > 0)
        {
            valueToChange = -valueToChange;
        }
        
        if (_gameData.globalGas + valueToChange >= 0)
        {
            return true;
        }
        else
        {
            return false;
        }        
    }

    public void AddGas(int valueToChange)
    {
        _gameData.globalGas += valueToChange;
        if (_gameData.globalGas > 9999) _gameData.globalGas = 9999;
    }
}
