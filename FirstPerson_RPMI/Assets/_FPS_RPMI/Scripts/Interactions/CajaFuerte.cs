using UnityEngine;
using UnityEngine.SceneManagement;

public class CajaFuerte : Interactable
{
    public string victorySceneName = "Victory";

    public override void Interact()
    {
        SceneManager.LoadScene(victorySceneName);
    }
}