using UnityEngine;

public class CajaFuerte : Interactable
{
    public int points = 50;

    public override void Interact()
    {
        GameManager.instance.AddPoints(points);
        GameManager.instance.EndGame();
    }
}