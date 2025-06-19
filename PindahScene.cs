using UnityEngine;
using UnityEngine.SceneManagement; // Penting untuk ganti scene

public class PindahScene : MonoBehaviour
{
    // Nama scene tujuan (harus sudah ditambahkan di Build Settings)
    private string Pindah;

    // Fungsi ini otomatis dipanggil saat objek dengan collider menyentuh trigger
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) // Pastikan Player punya tag "Player"
        {
            Debug.Log("Player masuk trigger: " + gameObject.name);
            if (gameObject.name == "PintuToko")
            {
                PlayerPrefs.SetFloat("PX", -1.957242f);
                PlayerPrefs.SetFloat("PZ", 6.607297f);
                PlayerPrefs.Save();
                Debug.Log("Player masuk trigger: " + gameObject.name);
                Pindah = "Kantor";
            }
            else if (gameObject.name == "PindahKota")
            {
                Pindah = "Kota";
            }
            else if(gameObject.name == "PintuRS")
            {
                PlayerPrefs.SetFloat("PX", 7.831213f);
                PlayerPrefs.SetFloat("PZ", 5.682653f);
                PlayerPrefs.Save();
                Pindah = "RumahSakit";
            }

            SceneManager.LoadScene(Pindah);

        }
    }
}