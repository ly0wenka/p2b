using System;
using TMPro;
using UnityEngine;

public sealed class ScoreCounter : MonoBehaviour
{
    public static ScoreCounter Instance { get; private set; }
    public int _score;
    [SerializeField] private TextMeshProUGUI scoreText;

    public int Score
    {
        get => _score;
        set
        {
            if (_score == value)
            {
                return;
            }

            _score = value;
            scoreText.SetText($"Score = {_score}");
        }
    }

    private void Awake() => Instance = this;
}