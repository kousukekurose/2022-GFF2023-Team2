using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    public int MultiType = 0;
    public bool isFree = true;

    private Vector2 moveInput = Vector2.zero;
    private Vector2 lookInput = Vector2.zero;

    public bool isTraking = false;


    [SerializeField]
    private MoveBehaviour moveBehaviour = null;

    [SerializeField]
    GameObject pouseUI = null;

    [SerializeField]
    Selectable firstSelectable = null;

    [SerializeField]
    PlayerUI playerUI01 = null;
    [SerializeField]
    PlayerUI playerUI02 = null;

    Pause pause = new Pause();

    [SerializeField]
    SearchArea searchArea = null;

    void Start()
    {

    }
    // Update is called once per frame
    void FixedUpdate()
    {
        moveBehaviour.Move(new Vector3(moveInput.x, 0, moveInput.y));
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();
    }

    public void OnLook(InputAction.CallbackContext context)
    {
        lookInput = context.ReadValue<Vector2>();
    }

    public void OnFire(InputAction.CallbackContext context)
    {
        if (context.started)
            moveBehaviour.Fire();
    }

    public void OnAvoidance(InputAction.CallbackContext context)
    {
        if (context.started)
            moveBehaviour.Avoidance();
    }
    public void OnPouse(InputAction.CallbackContext context)
    {
        pause.ControllerPouse(pouseUI, firstSelectable);
    }
    public void OnItemLT(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            if (searchArea.LItem!=null)
            {
                moveBehaviour.UserItem(searchArea.LItem);
                searchArea.SetLTItem();
                ShowPlayerUI();
            }
        }
    }
    public void OnItemRT(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            if (searchArea.RItem!=null)
            {
                moveBehaviour.UserItem(searchArea.RItem);
                searchArea.SetRTItem();
                ShowPlayerUI();
            }
        }
    }
    public void ShowPlayerUI()
    {
        if (MultiType == 1)
        {
            playerUI01.ShowPlayerUI(searchArea.LItem, searchArea.RItem);
        }
        else if (MultiType == 2)
        {
            playerUI02.ShowPlayerUI(searchArea.LItem, searchArea.RItem);
        }
    }
    public void OnTarget(InputAction.CallbackContext context)
    {
        if (context.started)
            searchArea.Target();
    }
}
