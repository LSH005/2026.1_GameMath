using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(LineRenderer))]
public class LerpGunManager : MonoBehaviour
{
    [Header("레이저")]
    public float chargeDuration = 1.0f;
    public float shootInterval = 0.3f;
    public float widthMultiplier = 0.2f;
    public Transform laserStartPoint;

    [Header("컴포넌트")]
    public LineRendering line;
    public SLerp cam;
    public LayerMask layer;

    [Header("적")]
    public Transform[] allPath;
    public LerpEnemy enemyPrefabs;

    LineRenderer lr;
    Coroutine rayCoroutine;

    private void Awake()
    {
        lr = GetComponent<LineRenderer>();
        lr.positionCount = 0;
    }

    private void Start()
    {
        for (int i = 0; i < 3; i++)
        {
            LerpEnemy enemy = Instantiate(enemyPrefabs, Vector3.one * 99f, Quaternion.identity);
            enemy.Poses = allPath;
            enemy.manager = this;
        }
    }

    public void OnRightClick(InputValue value)
    {
        if (!value.isPressed) return;
        Ray ray = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());

        if (Physics.Raycast(ray, out RaycastHit hit, 50f, layer))
        {
            if (hit.collider.CompareTag("Enemy"))
            {
                line.StartRendering(hit.transform);
                cam.StartTracking(hit.transform);
                if (hit.transform.TryGetComponent<LerpEnemy>(out LerpEnemy enemy))
                {
                    if (rayCoroutine != null) StopCoroutine(rayCoroutine);
                    rayCoroutine = StartCoroutine(RayShot(enemy));
                }
            }
        }
        else
        {
            line.StopRendering();
            cam.StopTracking();
            if (rayCoroutine != null) StopCoroutine(rayCoroutine);
            lr.positionCount = 0;
        }
    }

    IEnumerator RayShot(LerpEnemy target)
    {
        yield return new WaitForSeconds(chargeDuration);

        if (!target) yield break;
        lr.positionCount = 2;
        while (true)
        {
            float time = 0;
            float laserExpirationTime = shootInterval / 2f;
            lr.SetPosition(1, target.transform.position);
            target.GetDamage();
            while (time < laserExpirationTime)
            {
                lr.SetPosition(0, laserStartPoint.position);
                lr.widthMultiplier = ThatLerp(widthMultiplier, 0, time / laserExpirationTime);
                time += Time.deltaTime;
                yield return null;
            }

            lr.widthMultiplier = 0;
            if (target == null) break;
            yield return new WaitForSeconds(laserExpirationTime);
        }

        line.StopRendering();
        cam.StopTracking();
        lr.positionCount = 0;
        rayCoroutine = null;
    }

    float ThatLerp(float start, float end, float t) => (1f - t) * start + t * end;

    public void KilledEnemy()
    {
        LerpEnemy enemy = Instantiate(enemyPrefabs, Vector3.one * 99f, Quaternion.identity);
        enemy.Poses = allPath;
        enemy.manager = this;
    }
}
