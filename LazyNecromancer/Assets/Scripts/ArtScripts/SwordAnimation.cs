using System.Collections;
using System.Threading.Tasks;
using UnityEngine;

public class SwordAnimation : MonoBehaviour
{
    [SerializeField] public float duration = .15f;

    [Space(10)]

    [SerializeField] float minArc = 0;
    [SerializeField] float maxArc = 125;
    [SerializeField] AnimationCurve arcOverTime;

    [Space(10)]

    [SerializeField] float minRadius = 0;
    [SerializeField] float maxRadius = 0.2f;
    [SerializeField] AnimationCurve radiusOverTime;

    [Space(10)]

    [SerializeField] public float arcStartPos = 45;
    [SerializeField] float arcDistance = 270;
    [SerializeField] AnimationCurve rotationOverTime;

    Vector3 pivotPoint;

    public void TestAnimation()
    {
        transform.localPosition = pivotPoint;
        PlayAnimation();
    }

    public void PlayAnimation()
    {
        StartCoroutine(_PlayAnimation());
    }

    IEnumerator _PlayAnimation()
    {
        pivotPoint = transform.localPosition;
        float elapsedTime = 0;

        while (elapsedTime < duration)
        {
            float precent = elapsedTime / duration;

            setTransform(precent);

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        setTransform(1);
    }

    void setTransform(float time)
    {
        float arc = minArc + maxArc * arcOverTime.Evaluate(time);
        float radius = minRadius + maxRadius * radiusOverTime.Evaluate(time);
        float rotation = arcStartPos + arcDistance * rotationOverTime.Evaluate(time);

        Vector3 position = Vector3.zero;
        arc *= Mathf.Deg2Rad;

        position.x = Mathf.Cos(arc);
        position.y = Mathf.Sin(arc);
        transform.localPosition = pivotPoint + position * radius;
        transform.rotation = Quaternion.Euler(0, 0, rotation);
    }
}
