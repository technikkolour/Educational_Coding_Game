using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransition : MonoBehaviour
{
    public Animator Animator;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void FadetoScene(string SceneName)
    {
        Animator.SetTrigger("Fade");
        SceneManager.LoadScene(SceneName);
    }
    public void OnFadeComplete()
    {

    }
}
