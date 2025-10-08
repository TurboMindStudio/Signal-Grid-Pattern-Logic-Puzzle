using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using TMPro;

public class UIManager : MonoBehaviour
{
    [Header("References")]
    public SignalGridManager gridManager;
    public TextMeshProUGUI moveText;
    public TextMeshProUGUI timerText;
    public TextMeshProUGUI feedbackText;

    private int moveCount = 0;
    private float timer = 0f;
    private bool timerRunning = true;
    private bool peekUsed = false;

    private void Update()
    {
        if (timerRunning)
        {
            timer += Time.deltaTime;
            UpdateTimerText();
        }
    }

  
    public void RegisterMove(int tilesChanged)
    {
        moveCount++;
        UpdateMoveText();

    }

    public void ShowWinMessage()
    {
        timerRunning = false;
        if (feedbackText != null)
              feedbackText.text = $" You solved it in {moveCount} moves and {timer:F1} seconds!";
    }

    void UpdateMoveText()
    {
        if (moveText != null)
          moveText.text = $"Moves: {moveCount}";
    }

    void UpdateTimerText()
    {
        if (timerText != null)
            
          timerText.text = $"Time: {timer:F1}s";
    }

   


    public void PeekRule()
    {
        if (peekUsed)
        {
            if (feedbackText != null)
                feedbackText.text = "Peek already used!";
            return;
        }

        peekUsed = true;
        StartCoroutine(ShowPeek());
    }

    IEnumerator ShowPeek()
    {
        if (feedbackText != null)
            feedbackText.text = "Revealing one rule briefly...";

        yield return new WaitForSeconds(0.5f);

      
        if (gridManager.tiles == null || gridManager.tiles.Count == 0) yield break;

        int randomIndex = Random.Range(0, gridManager.tiles.Count);
        Tile tile = gridManager.tiles[randomIndex];

        if (tile != null && tile.tileImage != null)
        {
            Color originalColor = tile.tileImage.color;
            tile.tileImage.color = Color.cyan; 
            yield return new WaitForSeconds(1f);
            tile.tileImage.color = originalColor;
        }

        if (feedbackText != null)
            feedbackText.text = "Peek complete!";
    }
}
