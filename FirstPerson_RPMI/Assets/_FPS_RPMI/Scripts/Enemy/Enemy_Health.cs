using UnityEngine;

public class Enemy_Health : MonoBehaviour
{
    [Header("Health System Management")]
    [SerializeField] int maxHealth = 100; //Vida máxima del enemigo
    [SerializeField] int health; //Vida actual del enemigo

    [Header("Feedback Configuration")]
    [SerializeField] Material damagedMat; //Material feedback al daño
    [SerializeField] GameObject deathVfx; //Efecto de particulas de muerte
    [SerializeField] MeshRenderer enemyRend; //Ref al componente que dibuja los materiales del enemigo en pantalla
    Material baseMat; //Almacén del material base del enemigo


    private void Awake()
    {
        //enemyRend = GetComponent<MeshRenderer>(); 
        health = maxHealth; //La vida se pone al máximo
        baseMat = enemyRend.material; //Se referencia al material base
    }
    // Update is called once per frame
    void Update()
    {
        if (health <= 0)
        {
            health = 0; //La vida no puede bajar de cero
            deathVfx.SetActive(true);
            deathVfx.transform.position = transform.position;
            gameObject.SetActive(false); //El enemigo se apaga = muere
        }
    }

    public void TakeDamage(int damage)
    {
        health -= damage; //Quita una cantidad de vida determinada al enemigo
        enemyRend.material = damagedMat; //Se cambia al material de feedback de daño
        Invoke(nameof(ResetEnemyMaterial), 0.1f); //Espera de tiempo que permite ver el parpadeo
    }

    void ResetEnemyMaterial()
    {
        //Devuelve el material del enemigo a su material original
        enemyRend.material = baseMat; 
    }
}
