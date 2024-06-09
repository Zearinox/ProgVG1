using UnityEngine;
using UnityEngine.UI;

public class ScoreBar : MonoBehaviour
{
    public Image scoreBarImage;
    private float maxScore = 10f;
    private float currentScore;

    void Start()
    {
        currentScore = 0;
        UpdateScoreBar();
    }

    public void UpdateScore(float score)
    {
        currentScore = Mathf.Clamp(score, 0, maxScore);
        UpdateScoreBar();
    }

    private void UpdateScoreBar()
    {
        if (scoreBarImage != null)
        {
            scoreBarImage.fillAmount = currentScore / maxScore;
        }
    }
    
 
}