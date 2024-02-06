using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DemonstrationOfModels : MonoBehaviour {

    [SerializeField] private GameObject[] groups;

    private void Start()
    {
        StartCoroutine(ShowModels());
    }

    IEnumerator ShowModels()
    {
        for (int numModel = 0; numModel < groups.Length; numModel++)
        {
            if (numModel > 0)
                groups[numModel - 1].SetActive(false);
            groups[numModel].SetActive(true);
            yield return new WaitForSeconds(3f);
        }
    }
}
