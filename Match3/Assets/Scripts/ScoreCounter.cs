using TMPro;
using UnityEngine;

public sealed class ScoreCounter : MonoBehaviour
{
    public static ScoreCounter Instance { get; private set; }
    public int score;
    [SerializeField] private TextMeshProUGUI scoreText;

    public int Score
    {
        get => score;
        set
        {
            if (score == value)
            {
                return;
            }

            score = value;
            scoreText.SetText($"Score = {score}");
        }
    }

    private void Awake() => Instance = this;
}