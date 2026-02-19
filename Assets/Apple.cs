using UnityEngine;
using UnityEngine.SceneManagement;

public class Apple : MonoBehaviour
{
    public static float bottomY = -20f;

    private string currentScene;

   // Update is called once per frame
    void Update()
    {
        // destroy apple when out of bounds
        if(transform.position.y < bottomY)
        {
            Destroy(this.gameObject);

            // get referene to apple picket
            ApplePicker apScript = Camera.main.GetComponent<ApplePicker>();
            // call apple destroyed
            apScript.AppleMissed();
        }
    }
}
