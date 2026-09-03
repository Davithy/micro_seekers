using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class WinManager : MonoBehaviour
{
    public int totalItems = 3;
    private int foundItems = 0;
    public GameObject winText;

    public bool HasWon { get; private set; } = false;

    public void FindItem()
    {
        foundItems++;
        Debug.Log("Item found. " + foundItems + "/" + totalItems);

        if (foundItems >= totalItems)
        {
            WinGame();
        }
    }

    public void WinGame()
    {
        Debug.Log("Winner");
        HasWon = true;
        winText.gameObject.SetActive(true);
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}