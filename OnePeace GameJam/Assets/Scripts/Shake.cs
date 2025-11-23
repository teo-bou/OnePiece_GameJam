using UnityEngine;

public class Shake : MonoBehaviour
{
    [Header("Magnitudes")]
    public Vector3 positionMagnitude = new Vector3(0.1f, 0.1f, 0.1f);
    public Vector3 rotationMagnitude = new Vector3(1f, 1f, 1f); // degrees
    public Vector3 scaleMagnitude = new Vector3(0.02f, 0.02f, 0.02f); // relative

    [Header("Timing")]
    public float speed = 1f;      // how fast the noise moves
    public float smooth = 8f;     // how quickly transform follows the target offsets

    [Header("Options")]
    public bool useLocal = true;
    public bool randomizeSeedOnStart = true;

    Vector3 initialPos;
    Quaternion initialRot;
    Vector3 initialScale;

    float sx, sy, sz;
    float rx, ry, rz;
    float scx, scy, scz;

    void Start()
    {
        initialPos = useLocal ? transform.localPosition : transform.position;
        initialRot = useLocal ? transform.localRotation : transform.rotation;
        initialScale = transform.localScale;

        // seeds for Perlin noise so each axis is different
        if (randomizeSeedOnStart)
        {
            sx = Random.Range(0f, 1000f);
            sy = Random.Range(0f, 1000f);
            sz = Random.Range(0f, 1000f);
            rx = Random.Range(0f, 1000f);
            ry = Random.Range(0f, 1000f);
            rz = Random.Range(0f, 1000f);
            scx = Random.Range(0f, 1000f);
            scy = Random.Range(0f, 1000f);
            scz = Random.Range(0f, 1000f);
        }
    }

    void Update()
    {
        float t = Time.time * speed;

        // position offsets
        Vector3 posOffset = new Vector3(
            (Mathf.PerlinNoise(sx, t) - 0.5f) * 2f * positionMagnitude.x,
            (Mathf.PerlinNoise(sy, t) - 0.5f) * 2f * positionMagnitude.y,
            (Mathf.PerlinNoise(sz, t) - 0.5f) * 2f * positionMagnitude.z
        );

        // rotation offsets (degrees)
        Vector3 rotOffset = new Vector3(
            (Mathf.PerlinNoise(rx, t) - 0.5f) * 2f * rotationMagnitude.x,
            (Mathf.PerlinNoise(ry, t) - 0.5f) * 2f * rotationMagnitude.y,
            (Mathf.PerlinNoise(rz, t) - 0.5f) * 2f * rotationMagnitude.z
        );

        // scale offsets (additive relative)
        Vector3 scaleFactor = new Vector3(
            1f + (Mathf.PerlinNoise(scx, t) - 0.5f) * 2f * scaleMagnitude.x,
            1f + (Mathf.PerlinNoise(scy, t) - 0.5f) * 2f * scaleMagnitude.y,
            1f + (Mathf.PerlinNoise(scz, t) - 0.5f) * 2f * scaleMagnitude.z
        );

        // targets
        Vector3 targetPos = initialPos + posOffset;
        Quaternion targetRot = initialRot * Quaternion.Euler(rotOffset);
        Vector3 targetScale = Vector3.Scale(initialScale, scaleFactor);

        // apply with smoothing
        float dt = Mathf.Clamp01(Time.deltaTime * smooth);
        if (useLocal)
        {
            transform.localPosition = Vector3.Lerp(transform.localPosition, targetPos, dt);
            transform.localRotation = Quaternion.Slerp(transform.localRotation, targetRot, dt);
        }
        else
        {
            transform.position = Vector3.Lerp(transform.position, targetPos, dt);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRot, dt);
        }
        transform.localScale = Vector3.Lerp(transform.localScale, targetScale, dt);
    }
}
