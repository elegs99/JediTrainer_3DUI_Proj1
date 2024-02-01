using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.InputSystem;

public class FutureMovement : MonoBehaviour
{
    public GameObject cubePrefab;
    public float simulationTime = 5f;
    public float timeScale = 0.1f;
    public InputActionReference secondaryButtonAction;
    private List<GameObject> originalCubes = new();
    private List<GameObject> cloneCubes = new();
    private Dictionary<GameObject, Queue<Vector3>> futurePaths = new();
    private bool isSimulating = false;


    private void Awake()
    {
        secondaryButtonAction.action.started += OnSecondaryButtonPress;
    }

    void Start()
    {

    }

    private void OnEnable()
    {
        secondaryButtonAction.action.Enable();
    }

    private void OnDisable()
    {
        secondaryButtonAction.action.started -= OnSecondaryButtonPress;
        secondaryButtonAction.action.Disable();
    }

    private void OnSecondaryButtonPress(InputAction.CallbackContext context)
    {
        if (!isSimulating)
        {
            originalCubes = GameObject.FindGameObjectsWithTag("Enemy").ToList();

            StartCoroutine(SimulateFuture());
        }
    }

    IEnumerator SimulateFuture()
    {
        isSimulating = true;
        foreach (var clone in cloneCubes)
        {
            Destroy(clone);
        }
        cloneCubes.Clear();
        futurePaths.Clear();

        FreezeOriginalCubes();
        CloneCubes();
        Time.timeScale = timeScale;

        float timer = 0;
        while (timer < simulationTime)
        {
            RecordClonePositions();
            timer += (Time.deltaTime / timeScale);
            yield return null;
        }

        Time.timeScale = 1.0f;
        DestroyClones();
        StartCoroutine(ApplyFutureMovement());
        isSimulating = false;
    }

    void FreezeOriginalCubes()
    {
        foreach (var cube in originalCubes)
        {
            var cubeMovement = cube.GetComponent<CubeMovement>();
            if (cubeMovement != null)
            {
                cubeMovement.FreezeMovement();
            }
        }
    }

    void CloneCubes()
    {
        foreach (var originalCube in originalCubes)
        {
            var clone = Instantiate(originalCube, originalCube.transform.position, Quaternion.identity);
            cloneCubes.Add(clone);
            futurePaths.Add(originalCube, new Queue<Vector3>());

            var renderer = clone.GetComponent<Renderer>();
            if (renderer != null)
            {
                Material cloneMaterial = new Material(renderer.material);
                cloneMaterial.SetFloat("_Mode", 3);
                cloneMaterial.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
                cloneMaterial.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
                cloneMaterial.SetInt("_ZWrite", 0);
                cloneMaterial.DisableKeyword("_ALPHATEST_ON");
                cloneMaterial.EnableKeyword("_ALPHABLEND_ON");
                cloneMaterial.DisableKeyword("_ALPHAPREMULTIPLY_ON");
                cloneMaterial.renderQueue = 3000;

                Color color = cloneMaterial.color;
                color.a = 0.4f; // Set alpha to 0.4
                cloneMaterial.color = color;

                renderer.material = cloneMaterial;
            }
        }
    }


    void RecordClonePositions()
    {
        foreach (var clone in cloneCubes)
        {
            var originalCube = originalCubes[cloneCubes.IndexOf(clone)];
            futurePaths[originalCube].Enqueue(clone.transform.position);
        }
    }

    void DestroyClones()
    {
        foreach (var clone in cloneCubes)
        {
            Destroy(clone);
        }
        cloneCubes.Clear();
    }

    IEnumerator ApplyFutureMovement()
    {
        float catchUpDuration = simulationTime * (1f - timeScale);

        foreach (var cube in originalCubes)
        {
            cube.GetComponent<CubeMovement>().StartMovement();
        }

        float catchUpTimer = 0;
        while (catchUpTimer < catchUpDuration)
        {
            foreach (var cube in originalCubes)
            {
                if (futurePaths[cube].Count > 0)
                {
                    var targetPosition = futurePaths[cube].Dequeue();
                    float catchUpSpeedFactor = 1f / timeScale;

                    cube.transform.position = Vector3.Lerp(cube.transform.position, targetPosition, catchUpSpeedFactor * Time.deltaTime);
                }
            }
            catchUpTimer += Time.deltaTime;
            yield return null;
        }

        while (futurePaths[originalCubes[0]].Count > 0)
        {
            foreach (var cube in originalCubes)
            {
                if (futurePaths[cube].Count > 0)
                {
                    var targetPosition = futurePaths[cube].Dequeue();
                    cube.transform.position = Vector3.Lerp(cube.transform.position, targetPosition, 0.1f);
                }
            }
            yield return null;
        }

        foreach (var cube in originalCubes)
        {
            var cubeMovement = cube.GetComponent<CubeMovement>();
            if (cubeMovement != null)
            {
                cubeMovement.UnfreezeMovement();
            }
        }
    }

}
