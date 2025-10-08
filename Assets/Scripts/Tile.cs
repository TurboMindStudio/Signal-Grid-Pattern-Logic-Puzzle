using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Tile : MonoBehaviour, IPointerClickHandler
{
    [Header("Tile Visuals")]
    public Image tileImage;
    public Color onColor = Color.yellow;
    public Color offColor = Color.black;

    [Header("State Info")]
    public bool IsOn = false;

    private int tileIndex;
    private SignalGridManager gridManager;

   
    public void Init(int index, SignalGridManager manager)
    {
        tileIndex = index;
        gridManager = manager;
        SetState(Random.value > 0.5f);
    }

   
    public void OnPointerClick(PointerEventData eventData)
    {
        if (gridManager != null)
            gridManager.ToggleGroup(tileIndex);
    }

    // Flip the tile ON/OFF
    public void Toggle()
    {
        IsOn = !IsOn;
        UpdateVisual();
    }

    // Update color based on state
    void UpdateVisual()
    {
        if (tileImage != null)
            tileImage.color = IsOn ? onColor : offColor;
    }

 
    public void SetState(bool state)
    {
        IsOn = state;
        UpdateVisual();
    }
}
