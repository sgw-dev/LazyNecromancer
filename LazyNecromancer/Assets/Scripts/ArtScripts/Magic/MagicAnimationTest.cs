using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicAnimationTest : MonoBehaviour
{
    public BaseSpell baseSpell;

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            CastSpell();
        }
    }

    public void CastSpell()
    {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = 0;

        BaseSpell temp = Instantiate(baseSpell, transform.position, Quaternion.identity);
        temp.Play(mousePos);
    }
}
