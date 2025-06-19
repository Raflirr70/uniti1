using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class NPCInteraction : MonoBehaviour
{
    private bool isPlayerInRange = false;

    [SerializeField] private GameObject dialogBox;
    [SerializeField] private TextMeshProUGUI dialogText;
    string tempat;
    public string NPC;

    [TextArea(3, 10)]
    public string[] dialogLines;

    private int currentLine = 0;
    private bool isDialogActive = false;

    void Start()
    {
        tempat = SceneManager.GetActiveScene().name;
        PlayerPrefs.SetInt("penjualKantor", 0);
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
                StartDialog();
                Time.timeScale = 0f;
            }
            else {             
                AdvanceDialog();
            }
        }
    }
    void StartDialog()
    {
        if (dialogLines == null || dialogLines.Length == 0) return;

        currentLine = 0;
        if (dialogBox != null) dialogBox.SetActive(true);
        if (dialogText != null) dialogText.text = dialogLines[currentLine];
        isDialogActive = true;
    }

    void AdvanceDialog()
    {
        currentLine++;
        if (currentLine < dialogLines.Length)
        {
            if (dialogText != null) dialogText.text = dialogLines[currentLine];
        }
        else
        {
            EndDialog();
        }
    }

    void EndDialog()
    {
        if (dialogBox != null)
        {
            dialogBox.SetActive(false);
            Time.timeScale = 1f;

            // Pindah scene hanya jika dialog aktif dan nama NPC cocok
            if (isDialogActive && NPC == "penjual")
            {
                if(tempat == "Kantor"){
                    PlayerPrefs.SetInt("penjualKantor", 1);
                    SceneManager.LoadScene("TokoKantor");
                }else{
                    SceneManager.LoadScene("Toko");
                }
            }
        }

        isDialogActive = false;
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
