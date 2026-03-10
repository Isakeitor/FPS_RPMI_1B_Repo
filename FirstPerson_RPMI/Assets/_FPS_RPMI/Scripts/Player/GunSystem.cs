using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class GunSystem : MonoBehaviour
{
    #region General Variables
    [Header("General References")]
    [SerializeField] Camera fpsCam; //Ref si disparamos desde el centro de la cam
    [SerializeField] Transform shootPoint; //Ref si disparamos desde la punta del ca??n
    [SerializeField] LayerMask impactLayer; //Layer con la que interact?a el raycast
    RaycastHit hit; //Almac?n de la informaci?n de los objetos con los que el Raycast puede chocar

    [Header("Weapon Parameters")]
    [SerializeField] int damage = 10; //Da?o del arma por bala
    [SerializeField] float range = 100f; //Distancia m?xima de disparo
    [SerializeField] float spread = 0; //Radio de dispersi?n del disparo
    [SerializeField] float shootingCooldown = 0.2f; //Tiempo entre disparos
    [SerializeField] float reloadTime = 1.5f; //Tiempo de recarga en segundos
    [SerializeField] bool allowButtonHold = false; //Si el disparo se ejecuta por click (false) o por mantener (true)

    [Header("Bullet Management")]
    [SerializeField] int ammoSize = 30; //Cantidad max de balas/cargador
    [SerializeField] int bulletsPerTap = 1; //Cantidad de balas disparadas por cada ejecuci?n del disparo
    [SerializeField] int bulletsLeft; //Cantidad de balas dentro del cargador

    [Header("Feedback References")]
    [SerializeField] GameObject impactEffect; //Ref al VFX de impacto de bala

    [Header("Dev - Gun State Bools")]
    [SerializeField] bool shooting; //Indica si estamos disparando
    [SerializeField] bool canShoot; //Indica si podemos disparar en X momento del juego
    [SerializeField] bool reloading; //Indica si estamos en proceso de recarga

    #endregion

    private void Awake()
    {
        bulletsLeft = ammoSize; //Al iniciar la partida, tenemos el cargador lleno
        canShoot = true; //Al iniciar la partida, podemos disparar
    }

    // Update is called once per frame
    void Update()
    {
        if (canShoot && shooting && !reloading && bulletsLeft > 0)
        {
            //Inicializar el proceso de disparo
            StartCoroutine(ShootRoutine());
        }
    }

    IEnumerator ShootRoutine()
    {
        canShoot = false; //Primera capa de seguridad que evita que apilemos disparos
        if (!allowButtonHold) shooting = false; //Configuraci?n del disparo por tap
        for (int i = 0; i < bulletsPerTap; i++)
        {
            if (bulletsLeft <= 0) break; //Segunda prevenci?n de errores

            Shoot(); //Disparo en s? = Raycast que permite da?o
            bulletsLeft--; //Quita una bala del cargador actual
        }

        yield return new WaitForSeconds(shootingCooldown); //Ejecuci?n de la espera entre disparos
        canShoot = true; //Se devuelve la posibilidad de disparar
    }

    void Shoot()
    {
        //ESTE ES EL M?TODO M?S IMPORTANTE
        //SE DEFINE DISPARO POR RAYCAST -> UTILIZABLE POR CUALQUIER MEC?NICA

        //Almacenar la direcci?n de disparo y modificarla en caso de haber dispersi?n
        Vector3 direction = fpsCam.transform.forward;

        //A?adir dispersi?n aleatoria seg?n el valor de spread
        direction.x += Random.Range(-spread, spread);
        direction.y += Random.Range(-spread, spread);

        //DECLARACI?N DEL RAYCAST
        //Physics.Raycast(Origen del rayo, direcci?n, almac?n de la info del impacto, longitud del rayo, layer con la que impacta el rayo)
        if (Physics.Raycast(fpsCam.transform.position, direction, out hit, range, impactLayer))
        {
            //AQU? PODEMOS CODEAR TODOS LOS EFECTOS QUE QUIERO PARA LA INTERACCI?N
            Debug.Log(hit.collider.name);
            if (hit.collider.CompareTag("Enemy"))
            {
                EnemyHealth enemyHealth = hit.collider.GetComponent<EnemyHealth>();
                enemyHealth.TakeDamage(damage);
            }
        }
    }

    IEnumerator ReloadRoutine()
    {
        reloading = true; //Se activa modo recarga = no se puede stackear la recarga
        //Aqu? ir?a la llamada a la animaci?n de recarga
        yield return new WaitForSeconds(reloadTime);
        bulletsLeft = ammoSize; //Se efect?a la recarga a nivel datos
        reloading = false;
    }

    void Reload()
    {
        if (bulletsLeft < ammoSize && !reloading)
        {
            StartCoroutine(ReloadRoutine());
        }
    }

    #region Input Methods

    public void OnShoot(InputAction.CallbackContext context)
    {
        //El sistema de input debe comprobar si el disparo es por tap o por mantener
        if (allowButtonHold)
        {
            //Modo mantener ON
            shooting = context.ReadValueAsButton();
        }
        else
        {
            //Modo tap ON
            if (context.performed) shooting = true;
        }

    }

    public void OnReload(InputAction.CallbackContext context)
    {
        if (context.performed) Reload();
    }

    #endregion
}
