using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ControlMinigame2Menu : MonoBehaviour
{
    public Text status;
    public Text lvl;

    public void Serang()
    {
        SceneManager.LoadScene("Minigame2");
    }
    public void Upgrade()
    {
        SceneManager.LoadScene("Minigame2Upgrade");
    }
    public void Keluar()
    {
        SceneManager.LoadScene("Kota");
    }

    void Start()
    {
        lvl.text = "Level "+PlayerPrefs.GetInt("playerLevel");
        status.text = "";
        status.text += "HP\t\t: " + PlayerPrefs.GetFloat("playerHp").ToString("F2")+"\n";
        status.text += "Atk\t\t: " + PlayerPrefs.GetFloat("playerAtk").ToString("F2")+"\n";
        status.text += "Exp\t\t: " + PlayerPrefs.GetFloat("playerXp").ToString("F2") + " / 10";
    }
}

