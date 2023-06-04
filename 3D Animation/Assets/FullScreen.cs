using UnityEngine;

public class FullScreen : MonoBehaviour
{
    private bool _fullScreen;
    public bool fullScreen
    {
        set
        {
            _fullScreen = value;
            Screen.fullScreen = value;
        }
        get
        {
            return _fullScreen;
        }
    }

    private void Start()
    {
        fullScreen = Screen.fullScreen;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.RightAlt))
        {
            FullScreenMethod();
        }
    }

    public void FullScreenMethod()
    {
        if (fullScreen)
        {
            fullScreen = false;
        }
        else
        {
            fullScreen = true;
        }
    }
}
