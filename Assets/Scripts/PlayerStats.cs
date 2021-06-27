using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerStats
{
    private int _score;
    public int score
    {
        get
        {
            return _score;
        }
        set
        {
            _score = value;
        }
    }

    private int _asteroidsHit;
    public int asteroidsHit
    {
        get
        {
            return  _asteroidsHit;
        }
        set
        {
            _asteroidsHit = value;
        }
    }

    private int _lives;
    public int lives
    {
        get
        {
            return _lives;
        }
        set
        {
            _lives = value;
        }
    }

    public PlayerStats(int s, int a)
    {
        _score = s;
        _asteroidsHit = a;
        _lives = 3;
    }

    public void AsteroidHit(Asteroid ast)
    {
        asteroidsHit++;
        switch (ast.size)
        {
            case AsteroidSize.small:
                _score += 100;
                break;
            case AsteroidSize.medium:
                _score += 75;
                break;
            default:
                _score += 50;
                break;
        }
    }
}
