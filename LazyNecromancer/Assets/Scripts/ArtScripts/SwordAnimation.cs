using System.Collections;
using System.Threading.Tasks;
using UnityEngine;

public class SwordAnimation : MonoBehaviour
{
    [SerializeField] float duration = .5f;

    [Space(10)]

    [SerializeField] float minArc = 0;
    [SerializeField] float maxArc = 180;
    [SerializeField] AnimationCurve arcOverTime;

    [Space(10)]

    [SerializeField] float minRadius = 1;
    [SerializeField] float maxRadius = 1;
    [SerializeField] AnimationCurve radiusOverTime;

    [Space(10)]

    [SerializeField] float minRotation = -90;
    [SerializeField] float maxRotation = 90;
    [SerializeField] AnimationCurve rotationOverTime;

    Vector3 pivotPoint;

    public void TestAnimation()
    {
        transform.position = pivotPoint;
        PlayAnimation();
    }

    public void PlayAnimation()
    {
        StartCoroutine(_PlayAnimation());
    }

    IEnumerator _PlayAnimation()
    {
        pivotPoint = transform.position;
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
        float rotation = minRotation + maxRotation * rotationOverTime.Evaluate(time);

        Vector3 position = Vector3.zero;
        arc *= Mathf.Deg2Rad;

        position.x = Mathf.Cos(arc);
        position.y = Mathf.Sin(arc);
        transform.position = pivotPoint + position * radius;
        transform.rotation = Quaternion.Euler(0, 0, rotation);
    }
}
