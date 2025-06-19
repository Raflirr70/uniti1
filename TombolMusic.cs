using UnityEngine;
using UnityEngine.UI;

public class TombolMusic : MonoBehaviour
{
    public AudioSource musicSource;
    public RawImage icon;

    void Start()
    {
        // Ambil status musik dari PlayerPrefs (default ON = 1)
        int musicStatus = PlayerPrefs.GetInt("music", 1); // default = 1 (ON)
        musicSource.mute = (musicStatus == 0); // mute jika 0
        if (PlayerPrefs.GetInt("music") == 0)
            icon.gameObject.SetActive(false);
    }

    // Fungsi ini akan dipanggil dari tombol UI (OnClick)
    public void Toggle()
    {
        int currentStatus = PlayerPrefs.GetInt("music", 1);
        int newStatus = (currentStatus == 1) ? 0 : 1;
        PlayerPrefs.SetInt("music", newStatus);
        PlayerPrefs.Save();
        musicSource.mute = (newStatus == 0); // mute jika 0
        Debug.Log("Music now: " + (newStatus == 1 ? "ON" : "OFF"));
        if(PlayerPrefs.GetInt("music") == 1)
            icon.gameObject.SetActive(true);
        else
            icon.gameObject.SetActive(false);
    }
}
