using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class WinManager : MonoBehaviour
{
    public int totalItems = 3;
    private int foundItems = 0;
    public GameObject winScreen;
    public GameObject restartText;
    public GameObject disconnectText;

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
        StartCoroutine(WinGameCoroutine());
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    IEnumerator WinGameCoroutine()
    {
        
        yield return new WaitForSeconds(1);

        HasWon = true;
        winScreen.gameObject.SetActive(true);

        yield return new WaitForSeconds(0.5f);
        disconnectText.SetActive(true);

        yield return new WaitForSeconds(3);
        disconnectText.SetActive(false);
        restartText.SetActive(true);

    }
}