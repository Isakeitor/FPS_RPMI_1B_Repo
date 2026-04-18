using UnityEngine;

public class Rotation : MonoBehaviour
{
    public Vector3 velocidadRotacion = new Vector3(0, 0, -20); // grados por segundo

    void Update()
    {
        transform.Rotate(velocidadRotacion * Time.deltaTime);
    }
}