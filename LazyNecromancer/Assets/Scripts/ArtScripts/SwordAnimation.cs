using System.Collections;
using System.Threading.Tasks;
using UnityEngine;

public class SwordAnimation : MonoBehaviour
{
    [SerializeField] SwordAnimationSettings settings;

    Vector3 pivotPoint = Vector3.zero;

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
        Debug.Log(settings.Duration);

        while (elapsedTime < settings.Duration)
        {
            float precent = elapsedTime / settings.Duration;

            setTransform(precent);

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        setTransform(1);
    }

    void setTransform(float time)
    {
        float arc = settings.EvaluateArc(time);
        float radius = settings.EvaluateRadius(time);
        float rotation = settings.EvaluateRotation(time);

        Vector3 position = Vector3.zero;
        arc *= Mathf.Deg2Rad;

        position.x = Mathf.Cos(arc);
        position.y = Mathf.Sin(arc);
        transform.localPosition = pivotPoint + position * radius;
        transform.rotation = Quaternion.Euler(0, 0, rotation);
    }

    public SwordAnimationSettings Settings => settings;
}
