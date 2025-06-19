using UnityEngine;
using TMPro;

public class day : MonoBehaviour
{
    public EventTrashTrigger ts;

    // Untuk Merubah Warna Langit
    public Camera mainCamera;
    private Color targetSkyColor;

    // Untuk Text Jam
    public TextMeshProUGUI clockText;
    public float keJam = 30f;
    private float clockTimer = 0f;
    private int jam = 5;
    private int menit = 0;
    private int tampilanMenitTerakhir = -1;

    // Untuk Merubah Cahaya
    public Light L1;
    public Light L2;

    public enum TimeOfDay
    {
        Pagi,
        Siang,
        Sore,
        Malam
    }
    public TimeOfDay currentTime;

    // Untuk Transisi Waktu
    public float transitionSpeed = 1f;

    private Color targetColor;
    private float targetIntensity;

    void Start()
    {
        // Load waktu jika tersedia di PlayerPrefs
        if (PlayerPrefs.HasKey("jam") && PlayerPrefs.HasKey("menit"))
        {
            jam = PlayerPrefs.GetInt("jam");
            menit = PlayerPrefs.GetInt("menit");
        }

        SetLighting(currentTime);
        ApplyTargetImmediately();
        UpdateClockDisplay();
    }

    void Update()
    {
        Timer();
        UpdateTimeOfDay();

        // Transisi lighting
        L1.color = Color.Lerp(L1.color, targetColor, Time.deltaTime * transitionSpeed);
        L2.color = Color.Lerp(L2.color, targetColor, Time.deltaTime * transitionSpeed);
        L1.intensity = Mathf.Lerp(L1.intensity, targetIntensity, Time.deltaTime * transitionSpeed);
        L2.intensity = Mathf.Lerp(L2.intensity, targetIntensity, Time.deltaTime * transitionSpeed);

        // Transisi warna langit
        if (mainCamera != null)
        {
            mainCamera.backgroundColor = Color.Lerp(mainCamera.backgroundColor, targetSkyColor, Time.deltaTime * transitionSpeed);
        }
    }

    void Timer()
    {
        clockTimer += Time.deltaTime;

        float menitsPerdetik = 60f / keJam;
        int tambahMenit = Mathf.FloorToInt(clockTimer * menitsPerdetik);
        menit += tambahMenit;
        clockTimer -= tambahMenit / menitsPerdetik;

        if (menit >= 60)
        {
            menit -= 60;
            jam++;
            if (jam >= 24)
            {
                jam = 0;
            }
        }

        if (menit % 10 == 0 && menit != tampilanMenitTerakhir)
        {
            SaveTime();
            UpdateClockDisplay();
            tampilanMenitTerakhir = menit;
        }
    }

    void UpdateClockDisplay()
    {
        string jamStr = jam.ToString("D2");
        string menitStr = menit.ToString("D2");
        clockText.text = jamStr + " : " + menitStr;
    }

    void UpdateTimeOfDay()
    {
        TimeOfDay newTime;

        int totalmenit = jam * 60 + menit;

        if (totalmenit == 300)
        {
            PlayerPrefs.SetString("EventSampah", "");
            PlayerPrefs.Save();
        }

        if (totalmenit >= 300 && totalmenit < 420)          // 5:00 - 6:59
            newTime = TimeOfDay.Pagi;
        else if (totalmenit >= 420 && totalmenit <= 990)    // 7:00 - 16:30
            newTime = TimeOfDay.Siang;
        else if (totalmenit >= 991 && totalmenit <= 1080)   // 16:31 - 18:00
            newTime = TimeOfDay.Sore;
        else                                                // 18:01 - 4:59
            newTime = TimeOfDay.Malam;

        if (newTime != currentTime)
        {
            currentTime = newTime;
            SetLighting(currentTime);
        }
    }

    void SetLighting(TimeOfDay time)
    {
        switch (time)
        {
            case TimeOfDay.Pagi:
                targetSkyColor = new Color(0.8f, 0.7f, 0.5f);
                targetColor = new Color(1f, 0.85f, 0.6f);
                targetIntensity = 1.5f;
                break;
            case TimeOfDay.Siang:
                targetSkyColor = new Color(0.53f, 0.81f, 0.92f);
                targetColor = Color.white;
                targetIntensity = 3f;
                break;
            case TimeOfDay.Sore:
                targetSkyColor = new Color(1f, 0.4f, 0.2f);
                targetColor = new Color(1f, 0.5f, 0.3f);
                targetIntensity = 1.5f;
                break;
            case TimeOfDay.Malam:
                targetSkyColor = new Color(0.05f, 0.05f, 0.1f);
                targetColor = new Color(0.2f, 0.3f, 0.6f);
                targetIntensity = 0f;
                break;
        }
    }

    void ApplyTargetImmediately()
    {
        L1.color = targetColor;
        L2.color = targetColor;
        L1.intensity = targetIntensity;
        L2.intensity = targetIntensity;
    }

    // Menyimpan waktu ke PlayerPrefs
    public void SaveTime()
    {
        PlayerPrefs.SetInt("jam", jam);
        PlayerPrefs.SetInt("menit", menit);
        PlayerPrefs.Save();
    }

    // Reset waktu dari PlayerPrefs
    public void ResetTime()
    {
        PlayerPrefs.DeleteKey("jam");
        PlayerPrefs.DeleteKey("menit");
    }
}
