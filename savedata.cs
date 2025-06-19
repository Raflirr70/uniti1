using UnityEngine;
using TMPro;

public class savedata : MonoBehaviour
{
    public GameObject PosisiPlayer;
    public GameObject menuUI; // Drag UI menu ke sini lewat inspector
    public TextMeshProUGUI gold;

    private bool isMenuVisible = false;
    void init()
    {
        PlayerPrefs.SetInt("sapu", 0);
        PlayerPrefs.SetInt("penyiram", 0);
        PlayerPrefs.SetInt("sarungTangan", 0);
        PlayerPrefs.SetInt("masker", 0);
        PlayerPrefs.SetInt("ransel", 0);
        PlayerPrefs.SetInt("score", 50000);
        PlayerPrefs.SetInt("psp", 0);
        PlayerPrefs.SetInt("jam", 2);
        PlayerPrefs.SetInt("menit", 0);
        PlayerPrefs.SetInt("waitSapu", 0);
        PlayerPrefs.SetString("EventSampah", " ");
        PlayerPrefs.Save();
    }
    void Start() // <== perbaiki kapitalisasi
    {
        // init();
        PosisiPlayer.transform.position = new Vector3(PlayerPrefs.GetFloat("PX"), 0.384022f, PlayerPrefs.GetFloat("PZ"));
        // Inisialisasi UI
        menuUI.SetActive(isMenuVisible);
    }

    void Update()
    {
        menuUI.SetActive(isMenuVisible);
        if (isMenuVisible == false)
        {
            PlayerPrefs.SetInt("MenuInventori", 0);
            PlayerPrefs.Save();
        }
        else
        {
            PlayerPrefs.SetInt("MenuInventori", 1);
            PlayerPrefs.Save();
        }
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            gold.text = PlayerPrefs.GetInt("score").ToString();
            isMenuVisible = !isMenuVisible;
            if (isMenuVisible) Time.timeScale = 0f;
            else Time.timeScale = 1f;
        }
    }
}
