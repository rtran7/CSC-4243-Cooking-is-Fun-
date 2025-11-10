using UnityEngine;

public class CookingMeterController : MonoBehaviour
{
    [Header("Needle")]
    public RectTransform needle;                 // Canvas/MeterGroupUI/MeterDial/Needle

    [Header("Angles (deg)")]
    [Tooltip("Angle when value = 0")]
    public float minAngle = -135f;
    [Tooltip("Angle when value = 1")]
    public float maxAngle = 135f;

    [Header("Motion")]
    [Tooltip("Use a constant clockwise spin instead of ping-pong.")]
    public bool continuousSpin = false;
    [Tooltip("Degrees per second when continuousSpin = true")]
    public float degreesPerSecond = 200f;

    [Tooltip("Oscillations (there and back) per second when not continuous.")]
    public float oscillationsPerSecond = 0.6f;
    [Tooltip("Easing curve for ping-pong motion (x=0..1)")]
    public AnimationCurve easing = AnimationCurve.Linear(0, 0, 1, 1);

    /// <summary>Current normalized value [0..1]. 0 = minAngle, 1 = maxAngle.</summary>
    public float CurrentValue { get; private set; }

    float _t;
    float _spinAngle;     // for continuous spin
    bool _frozen;

    void Reset()
    {
        if (needle == null)
            needle = GetComponentInChildren<RectTransform>();
    }

    void Update()
    {
        if (_frozen || needle == null) return;

        if (continuousSpin)
        {
            _spinAngle += degreesPerSecond * Time.deltaTime;
            // Map a 360° cycle to 0..1 and then to min..max
            float cycle = Mathf.Repeat(_spinAngle / 360f, 1f);
            CurrentValue = cycle; // 0..1 clockwise
        }
        else
        {
            // 0..1..0 repeating
            _t += Time.deltaTime * oscillationsPerSecond;
            float pingpong = Mathf.PingPong(_t, 1f);
            CurrentValue = Mathf.Clamp01(easing.Evaluate(pingpong));
        }

        float ang = Mathf.Lerp(minAngle, maxAngle, CurrentValue);
        needle.localRotation = Quaternion.Euler(0, 0, ang);
    }

    public void Freeze()   { _frozen = true; }
    public void Unfreeze() { _frozen = false; }

    /// <summary>Force the needle to a value [0..1].</summary>
    public void SetValue(float v01)
    {
        CurrentValue = Mathf.Clamp01(v01);
        if (needle != null)
        {
            float ang = Mathf.Lerp(minAngle, maxAngle, CurrentValue);
            needle.localRotation = Quaternion.Euler(0, 0, ang);
        }
    }
}
