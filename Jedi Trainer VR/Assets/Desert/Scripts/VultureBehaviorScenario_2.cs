using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class VultureBehaviorScenario_2 : MonoBehaviour {

    [SerializeField] private Text[] textInfoPanel;          // reference Panel Canvas
    [SerializeField] private Transform[] lodTransforms;     // reference Transform Vulture

    [SerializeField] private Animator animator;             // reference Animator
    [SerializeField] private GameObject group;

    private void Start()
    {
        foreach (Text text in textInfoPanel)
        {
            text.canvasRenderer.SetAlpha(0.0f);
        }

        StartCoroutine(BehaviorScenario());                 // Launch behavior scenario
    }

    // Behavior scenario Object
    private IEnumerator BehaviorScenario()
    {
        int countAnimations = 0;
        while (countAnimations < 6)
        {
            yield return new WaitForSeconds(3f);
            countAnimations++;
            animator.SetTrigger("Next Animation");
        }

        yield return new WaitForSeconds(3f);
        

        for (int indexLodTransform = 0; indexLodTransform < lodTransforms.Length; indexLodTransform++)
        {
            lodTransforms[indexLodTransform].gameObject.SetActive(true);
            textInfoPanel[indexLodTransform].CrossFadeAlpha(1f, 3f, false);

            StartCoroutine(LerpChagePositionLodTransform(indexLodTransform));
        }

        yield return new WaitForSeconds(5f);

        group.SetActive(true);
        gameObject.SetActive(false);
    }

    private IEnumerator LerpChagePositionLodTransform(int indexLod)
    {
        float offset = 0f;
        switch (indexLod)
        {
            case 0: offset = -12; break;
            case 1: offset = -4; break;
            case 2: offset = 4; break;
            case 3: offset = 12; break;
        }

        float timeElapsed = 0;
        while (timeElapsed < 1)
        {
            yield return null;
            timeElapsed += Time.deltaTime;
            lodTransforms[indexLod].position = new Vector3(timeElapsed * offset, 0, 0);
        }
    }
}
