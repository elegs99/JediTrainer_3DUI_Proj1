using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TransitionToAnotherScene : MonoBehaviour {

    [SerializeField] private int nextScene;

	// Use this for initialization
	void Start () {
        StartCoroutine(LaunchScene());
	}
	
    IEnumerator LaunchScene()
    {
        yield return new WaitForSeconds(30f);
        SceneManager.LoadScene(nextScene);
    }
}
