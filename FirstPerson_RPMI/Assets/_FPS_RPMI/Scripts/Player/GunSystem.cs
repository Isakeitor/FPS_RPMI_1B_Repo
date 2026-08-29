using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using TMPro;

public class GunSystem : MonoBehaviour
{
    #region General Variables
    [Header("General References")]
    [SerializeField] Camera fpsCam;
    [SerializeField] Transform shootPoint;
    [SerializeField] LayerMask impactLayer;
    RaycastHit hit;

    [Header("VFX")]
    [SerializeField] GameObject muzzleVFX;
    [SerializeField] GameObject hitVFX;

    [Header("Weapon Parameters")]
    [SerializeField] int damage = 10;
    [SerializeField] float range = 100f;
    [SerializeField] float spread = 0;
    [SerializeField] float shootingCooldown = 0.2f;
    [SerializeField] float reloadTime = 1.5f;
    [SerializeField] bool allowButtonHold = false;

    [Header("Bullet Management")]
    [SerializeField] int ammoSize = 30;
    [SerializeField] int bulletsPerTap = 1;
    [SerializeField] int bulletsLeft;

    [Header("Ammo UI")]
    public GameObject ammoIcon;
    public GameObject noAmmoIcon;
    public TextMeshProUGUI ammoText;

    [Header("Dev - Gun State Bools")]
    [SerializeField] bool shooting;
    [SerializeField] bool canShoot;
    [SerializeField] bool reloading;

    [Header("Zoom")]
    [SerializeField] float normalFOV = 60f;
    [SerializeField] float zoomFOV = 40f;
    [SerializeField] float zoomSpeed = 10f;

    float targetFOV;
    #endregion

    void Awake()
    {
        bulletsLeft = ammoSize;
        canShoot = true;
    }

    void Start()
    {
        targetFOV = normalFOV;
        fpsCam.fieldOfView = normalFOV;

        UpdateAmmoUI();
    }

    void Update()
    {
        if (canShoot && shooting && !reloading && bulletsLeft > 0)
        {
            StartCoroutine(ShootRoutine());
        }

        fpsCam.fieldOfView = Mathf.Lerp(
            fpsCam.fieldOfView,
            targetFOV,
            Time.deltaTime * zoomSpeed
        );

        UpdateAmmoUI();
    }

    #region SHOOTING

    IEnumerator ShootRoutine()
    {
        canShoot = false;

        if (!allowButtonHold)
            shooting = false;

        for (int i = 0; i < bulletsPerTap; i++)
        {
            if (bulletsLeft <= 0) break;

            Shoot();
            bulletsLeft--;
        }

        yield return new WaitForSeconds(shootingCooldown);
        canShoot = true;
    }

    void Shoot()
    {
        // 🔥 MUZZLE FLASH
        if (muzzleVFX != null && shootPoint != null)
        {
            GameObject vfx = Instantiate(
                muzzleVFX,
                shootPoint.position,
                shootPoint.rotation
            );

            Destroy(vfx, 1f);
        }

        Vector3 direction = fpsCam.transform.forward;

        direction.x += Random.Range(-spread, spread);
        direction.y += Random.Range(-spread, spread);

        if (Physics.Raycast(
            fpsCam.transform.position,
            direction,
            out hit,
            range,
            impactLayer))
        {
            // 🔥 IMPACT VFX
            if (hitVFX != null)
            {
                GameObject impact = Instantiate(
                    hitVFX,
                    hit.point,
                    Quaternion.LookRotation(hit.normal)
                );

                Destroy(impact, 2f);
            }

            if (hit.collider.CompareTag("Enemy"))
            {
                EnemyHealth enemyHealth =
                    hit.collider.GetComponent<EnemyHealth>();

                if (enemyHealth != null)
                    enemyHealth.TakeDamage(damage);
            }
        }
    }

    #endregion

    #region RELOAD

    IEnumerator ReloadRoutine()
    {
        reloading = true;

        yield return new WaitForSeconds(reloadTime);

        bulletsLeft = ammoSize;
        reloading = false;
    }

    void Reload()
    {
        if (bulletsLeft < ammoSize && !reloading)
        {
            StartCoroutine(ReloadRoutine());
        }
    }

    public void AddAmmo(int amount)
    {
        bulletsLeft += amount;
        bulletsLeft = Mathf.Clamp(bulletsLeft, 0, ammoSize);
    }

    #endregion

    #region UI

    void UpdateAmmoUI()
    {
        if (ammoText != null)
            ammoText.text = bulletsLeft.ToString();

        if (ammoIcon != null)
            ammoIcon.SetActive(bulletsLeft > 0);

        if (noAmmoIcon != null)
            noAmmoIcon.SetActive(bulletsLeft <= 0);
    }

    #endregion

    #region INPUT

    public void OnShoot(InputAction.CallbackContext context)
    {
        if (allowButtonHold)
        {
            shooting = context.ReadValueAsButton();
        }
        else
        {
            if (context.performed)
                shooting = true;
        }
    }

    public void OnReload(InputAction.CallbackContext context)
    {
        if (context.performed)
            Reload();
    }

    public void OnScroll(InputAction.CallbackContext context)
    {
        float scroll = context.ReadValue<Vector2>().y;

        targetFOV -= scroll * 2f;
        targetFOV = Mathf.Clamp(
            targetFOV,
            zoomFOV,
            normalFOV
        );
    }

    #endregion
}