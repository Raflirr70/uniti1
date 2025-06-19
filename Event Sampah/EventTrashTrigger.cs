using UnityEngine;
using TMPro;

public class EventTrashTrigger : MonoBehaviour
{
    public eventsampah eventsampah1;
    private bool isPlayerInRange = false;

    [SerializeField] private GameObject dialogBox;
    [SerializeField] private TextMeshProUGUI dialogText;

    [TextArea(3, 10)]
    public string[] dialogLines;

    private int currentLine = 0;
    public string statusEvent;
    private bool isDialogActive = false;
    private bool isWaitingForSapu = false; // 👈 Tambahan flag

    void Start()
    {
        
        statusEvent = PlayerPrefs.GetString("EventSampah");
        if (dialogBox != null)
            dialogBox.SetActive(false);
        else
            Debug.LogError("DialogBox belum di-assign!");
    }

    void Update()
    {
        if (isPlayerInRange && Input.GetKeyDown(KeyCode.Space))
        {
            if (!isDialogActive)
            {
                if (PlayerPrefs.GetInt("sapu") == 1)
                {
                    StartDialog();
                    Time.timeScale = 0f;
                    if (statusEvent != "selesai")
                        eventsampah1.TriggerTrashSpawnEvent();
                }
                else
                {
                    if (PlayerPrefs.GetInt("waitSapu") == 0)
                        currentLine = 5;
                    else if (statusEvent == "selesai")
                        currentLine = 4;
                    else
                        currentLine = 8;
                    isWaitingForSapu = true;
                    if (dialogBox != null) dialogBox.SetActive(true);
                    if (dialogText != null) dialogText.text = dialogLines[currentLine];
                    isDialogActive = true;
                    Time.timeScale = 0f;
                    AdvanceDialog();
                }
            }
            else
            {
                AdvanceDialog();
            }
        }
    }

    void StartDialog()
    {
        if (dialogLines == null || dialogLines.Length == 0) return;
        if (statusEvent == "berjalan")
        {
            if (dialogBox != null) dialogBox.SetActive(true);
            if (dialogText != null) dialogText.text = dialogLines[3];
            isDialogActive = true;
        }
        else if (statusEvent == "hadiah")
        {
            PlayerPrefs.SetString("EventSampah", "selesai");
            PlayerPrefs.Save();
            statusEvent = "selesai";
            if (dialogBox != null) dialogBox.SetActive(true);
            if (dialogText != null) dialogText.text = dialogLines[4];
            PlayerPrefs.SetInt("score", PlayerPrefs.GetInt("score") + 500);
            isDialogActive = true;
        }
        else if (statusEvent == "selesai")
        {
            if (dialogBox != null) dialogBox.SetActive(true);
            if (dialogText != null) dialogText.text = dialogLines[5];
            isDialogActive = true;
        }
        else
        {
            statusEvent = "berjalan";
            eventsampah1.hasSpawned = false;
            currentLine = 0;
            if (dialogBox != null) dialogBox.SetActive(true);
            if (dialogText != null) dialogText.text = dialogLines[currentLine];
            isDialogActive = true;
        }
    }

    void AdvanceDialog()
    {
        currentLine++;
        if (currentLine == 8)
        {
            PlayerPrefs.SetInt("waitSapu", 1);
            PlayerPrefs.SetInt("score", PlayerPrefs.GetInt("score") + 400);
            PlayerPrefs.Save();
        }
        if (isWaitingForSapu)
        {

            if (currentLine <= 9)
            {
                if (dialogText != null) dialogText.text = dialogLines[currentLine];
            }
            else
            {
                isWaitingForSapu = false;
                EndDialog();
            }
        }
        else
        {
            if (currentLine < 3)
            {
                if (dialogText != null) dialogText.text = dialogLines[currentLine];
            }
            else
            {
                EndDialog();
            }
        }
    }

    void EndDialog()
    {
        if (dialogBox != null)
        {
            dialogBox.SetActive(false);
            Time.timeScale = 1f;
        }
        isDialogActive = false;
        isWaitingForSapu = false;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInRange = true;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInRange = false;
            EndDialog();
        }
    }
}
