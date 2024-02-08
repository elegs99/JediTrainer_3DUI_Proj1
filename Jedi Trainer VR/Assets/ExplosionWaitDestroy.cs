using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionWaitDestroy : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(Wait2Destroy());
    }

    // Update is called once per frame
    private IEnumerator Wait2Destroy() {
        yield return new WaitForSeconds(2);
        Destroy(gameObject);
    }

}
