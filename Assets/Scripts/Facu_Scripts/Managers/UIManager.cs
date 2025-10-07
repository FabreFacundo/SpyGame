using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    

    [SerializeField] private GameObject _controlMenu;
    [SerializeField] private GameObject _inGameMenu;
    [SerializeField] private GameObject _HUD;
    [SerializeField] private float _popUpMessageTime = 2;
    // private MainMenu _mainMenu
    private TMP_Text _interactMessage;
    private PlayerInputs _inputs;
    private GameObject _activeMenu;
    

    public TMP_Text InteractMessage { get { return _interactMessage; } }
    public GameObject ControlMenu
    {
        get { 
            if(_controlMenu == null)
            {
                return FindFirstObjectByType<ControlMenu>(FindObjectsInactive.Include).gameObject;
            }
            return _controlMenu; }
    }
    public GameObject InGameMenu
    {
        get
        {
            if (_inGameMenu == null)
            {
                return FindFirstObjectByType<InGameMenu>(FindObjectsInactive.Include).gameObject;
            }
            return _inGameMenu;
        }
    }
    private void Start()
    {
        _inputs = GameManager.instance.Inputs;
        SceneManager.sceneLoaded += (scene, mode) =>
        {
            SearchRefereeces();
        };
        SearchRefereeces();
    }
    private void Update()
    {

        if(_inputs.IsEscapeClicked)
        {
            ActiveMenu(_activeMenu);
        }
    }

    public void PopUpMessage(string message)
    {
        _interactMessage.text = message;
        _interactMessage.enabled = true;
    }

    public void PopUpMessageTimed(string message)
    {
        PopUpMessage(message);
        Invoke(nameof(HideMessage), _popUpMessageTime);

    }

    public void HideMessage()
    {
        _interactMessage.enabled = false;
    }


    private void SearchRefereeces()
    {
        _inGameMenu = FindFirstObjectByType<InGameMenu>(FindObjectsInactive.Include).gameObject;
        _controlMenu = FindFirstObjectByType<ControlMenu>(FindObjectsInactive.Include).gameObject;
        _HUD = GameObject.Find("HUD");
        _interactMessage = GameObject.Find("Interact_message").GetComponent<TMP_Text>();
        _activeMenu = InGameMenu;
    }

    public void ActiveMenu(GameObject activeMenu)
    {
       
        if(activeMenu == null) SearchRefereeces();
        else _activeMenu = activeMenu;

        if (!_activeMenu.activeSelf)
        {
            Time.timeScale = 0;
            _inputs.ChangeCursorLockState(CursorLockMode.Confined);
            _activeMenu.SetActive(true);
        }
        else
        {
            Time.timeScale = 1;
            _inputs.ChangeCursorLockState(CursorLockMode.Locked);
            _activeMenu.SetActive(false);
        }
    }

}
