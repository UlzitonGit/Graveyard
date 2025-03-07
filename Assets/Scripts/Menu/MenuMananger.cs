using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuMananger : MonoBehaviour
{
    public void Play(string _scene)
    {
        SceneManager.LoadScene( _scene );
    }
}
