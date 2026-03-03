using UnityEngine;
using UnityEngine.InputSystem;

public class GunSystem : MonoBehaviour
{
    #region General Variables
    [Header("General References")]
    [SerializeField] Camera fpsCam; //Ref si disparamos desde el centro de la cam
    [SerializeField] Transform shootPoint; //Ref si disparamos desde el cañon
    [SerializeField] LayerMask impactLayer; //Layer que interactua con el raycast
    RaycastHit hit; //Almacen de la info de los objeos que chocan con el raycast

    [Header("Weapon Parameters")]
    [SerializeField] int damage = 10; //Daño que hace el arma
    [SerializeField] float range = 100f; //Distancia máxima de disparo
    [SerializeField] float spread = 0f; //Radio de dispersion del disparo
    [SerializeField] float shootingCooldown = 0.2f; //Tiempo entre disparos
    [SerializeField] float reloadTime = 1.5f; //Tiempo de recarga en segundos
    [SerializeField] bool allowButtonHold = false; //Si el disparo se ejecuta por (false) o mantener (true)

    [Header("Bullet Management")]
    [SerializeField] int ammoSize = 30; //Cantidad max de balas por cargador
    [SerializeField] int bulletsPerTap = 1; //Cantidad de balas disparadas por cada ejecucion
    int bulletsLeft; //Balas restantes en el cargador

    [Header("Feedback References")]
    [SerializeField] GameObject impactEffect; //Ref al VFX de impacto de bala

    [Header("Dev - Gun State Bools")]
    [SerializeField] bool shooting; //Indica si estamos disparando
    [SerializeField] bool canShoot; //Indica si podemos disparar
    [SerializeField] bool reloading; //Indica si estamos en proceso de recarga

    #endregion

    private void Awake()
    {
        bulletsLeft = ammoSize; //Al iniciar el juego, el cargador esta lleno
        canShoot = true; //Al iniciar el juego, podemos disparar
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void Shoot()
    {
        //ESTE ES EL METODO MAS IMPORTANTE
        //SE DEFINE DISPARO POR RAYCAST -> UTILIZABLE POR CUALQUIER MECANICA

        //Almacenar la direccion del disparo y modificarla
        Vector3 direction = fpsCam.transform.forward;

        //Añadir dispersion aleatoria segun el valor de spread
        direction.x += Random.Range(-spread, spread);
        direction.y += Random.Range(-spread, spread);

        //DECLARACION DEL RAYCAST
        //Physics.Raycast(Origen del rayo, direccon, almacen de la info del impacto, longitud del rayo, layer de inpacto)
        if(Physics.Raycast(fpsCam.transform.position, direction, out hit, range, impactLayer))
        {
            //AQUI PODEMOS CODEAR TODOS LOS EFECTOS DE LA INTERACCION
            Debug.Log(hit.collider.name);
        }
    }

    #region
    public void OnShoot(InputAction.CallbackContext context)
    {
        Shoot();
    }
       
    public void OnReload(InputAction.CallbackContext context)
    {
       
    }
    #endregion
}
