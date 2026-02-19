using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ApplePicker : MonoBehaviour
{
    // basket variables
    public GameObject basketPrefab;
    public int numBaskets = 3;
    public float basketBottomY = -14f;
    public float basketSpacingY = 2f;
    public List<GameObject> basketList;

    // evil crow variables
    public GameObject birdPrefab;
    public float minBirdDelay = 5f;
    public float maxBirdDelay = 15f;
        // screen shake variables
    private float shakeDuration = 0f;
    private float shakeMagnitude = 0f;
    private float shakeTimer = 0f;
    private Vector3 originalPos;

    

    // Start is called before the first frame update
    void Start()
    {
        // add baskets (num of lives)
        basketList = new List<GameObject>();

        for (int i = 0; i < numBaskets; i++)
        {
            GameObject tBasketGO = Instantiate<GameObject>(basketPrefab);
            Vector3 pos = Vector3.zero;
            pos.y = basketBottomY + (basketSpacingY * i);
            tBasketGO.transform.position = pos;
            basketList.Add(tBasketGO);
        }

        // if in hard mode, spawn crows randomly
        if (SceneManager.GetActiveScene().name == "Hard_Mode")
        {
            Invoke("SpawnBird", Random.Range(minBirdDelay, maxBirdDelay));
        }


    }

    public void AppleMissed()
    {
        // destory all fall apples
        GameObject[] tAppleArray = GameObject.FindGameObjectsWithTag("Apple");
        foreach (GameObject tGO in tAppleArray)
        {
            Destroy(tGO);
        }
        // destroy one of the baskets
        // get index of last basket in list
        int basketIndex = basketList.Count-1;
        // get reference to that basket
        GameObject tBasketGO = basketList[basketIndex];
        // remove basket from the list and destroy
        basketList.RemoveAt(basketIndex);
        Destroy(tBasketGO);

        Debug.Log("APPLE DROPPED, LIFE LOST");

        if (basketList.Count == 0)
        {
            SceneManager.LoadScene("Scenes/Main_Menu");
        }
    }

    // Update is called once per frame
    void Update()
    {
        // esc to main menu
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            SceneManager.LoadScene("Scenes/Main_Menu"); 
        }

        // Handle screen shake when crow is spawned
        if (shakeTimer < shakeDuration)
        {
            shakeTimer += Time.deltaTime;

            float x = Random.Range(-1f, 1f) * shakeMagnitude;
            float y = Random.Range(-1f, 1f) * shakeMagnitude;

            transform.localPosition = originalPos + new Vector3(x, y, 0);
        }
        else if (shakeDuration > 0f)
        {
            // Reset position once shake is done
            transform.localPosition = originalPos;
            shakeDuration = 0f;
        }


    }

    // evil crow spawn function
    void SpawnBird()
    {
        Vector3 spawnPos = new Vector3(-25f, Random.Range(3f, 8f), 0f);
        Instantiate(birdPrefab, spawnPos, Quaternion.identity);
        Debug.Log("EVIL CROW SPAWNED");

        // screen shake for effect
        StartShake(0.5f, 0.6f);

        // spawn next crow
        Invoke("SpawnBird", Random.Range(minBirdDelay, maxBirdDelay));
    }

    void StartShake(float duration, float magnitude)
    {
        shakeDuration = duration;
        shakeMagnitude = magnitude;
        shakeTimer = 0f;
        originalPos = transform.localPosition;
    }

}
