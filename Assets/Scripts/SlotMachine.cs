using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class SlotMachine : MonoBehaviour
{
    public Button spinButton;
    public TMP_InputField rtpInputField;
    public TextMeshProUGUI resultText;
    public TextMeshProUGUI balanceText;
    public TextMeshProUGUI machineBalanceText;
    public TextMeshProUGUI rtpDisplayText;

    public float playerBalance = 1000f;
    public float machineBalance = 5000f;
    public float betAmount = 100f;
    public float defaultRTP = 80f;

    private float currentRTP;

    void Start()
    {
        currentRTP = defaultRTP;
        UpdateUI();
        spinButton.onClick.AddListener(OnSpinClick);
        rtpInputField.onEndEdit.AddListener(OnRTPChanged);
    }

    void UpdateUI()
    {
        balanceText.text = "Your Balance: " + playerBalance;
        machineBalanceText.text = "Machine Balance: " + machineBalance;
        rtpDisplayText.text = "Current RTP: " + currentRTP + "%";
    }

    void OnRTPChanged(string input)
    {
        if (float.TryParse(input, out float newRTP))
        {
            currentRTP = Mathf.Clamp(newRTP, 0f, 100f);
            resultText.text = "RTP Updated to " + currentRTP + "%";
        }
        else
        {
            resultText.text = "Invalid RTP value!";
        }

        UpdateUI();
    }

    void OnSpinClick()
    {
        if (playerBalance < betAmount)
        {
            resultText.text = "Not enough balance!";
            return;
        }

        spinButton.interactable = false;
        playerBalance -= betAmount;
        machineBalance += betAmount;
        UpdateUI();
        resultText.text = "Spinning...";
        StartCoroutine(SpinResult());
    }

    IEnumerator SpinResult()
    {
        yield return new WaitForSeconds(3f);

        float randomValue = Random.Range(0f, 100f);

        if (randomValue <= currentRTP)
        {
            float winAmount = betAmount * (currentRTP / 100f);

            if (machineBalance >= winAmount)
            {
                playerBalance += winAmount;
                machineBalance -= winAmount;
                resultText.text = "You Win " + winAmount + "!";
            }
            else
            {
                resultText.text = "Machine is out of money!";
            }
        }
        else
        {
            resultText.text = "You Lose!";
        }

        UpdateUI();
        spinButton.interactable = true;
    }
}
