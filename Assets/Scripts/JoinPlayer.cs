using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class JoinPlayer : MonoBehaviour
{
    [SerializeField]
    GameObject UserReady01 = null;

    [SerializeField]
    GameObject UserReady02 = null;

    [SerializeField]
    GameObject User01 = null;

    [SerializeField]
    GameObject User02 = null;

    [SerializeField]
    Vector3 user01Pos = new Vector3(-5, 1, 0);

    [SerializeField]
    Vector3 user02Pos = new Vector3(5, 1, 0);

    [SerializeField]
    Material player01Material = null;

    [SerializeField]
    Material player02Material = null;

    [SerializeField]
    CountDownUI countDownUI = null;

    [SerializeField]
    Sprite target = null;

    bool isFirstJoin = false;

    bool isSecondJoin = false;

    int children;

    PlayerInputManager playerInputManager;

    // Start is called before the first frame update
    void Start()
    {
        playerInputManager = GetComponent<PlayerInputManager>();
        playerInputManager.playerPrefab.transform.position = user01Pos;
        children = playerInputManager.playerPrefab.transform.GetChild(0).transform.childCount;
        for (int i = 0; i < children; i++)
        {
            if (playerInputManager.playerPrefab.transform.GetChild(0).transform.
                GetChild(i).GetComponent<SkinnedMeshRenderer>() != null &&
                playerInputManager.playerPrefab.transform.GetChild(0).transform.
                GetChild(i).name != "head_eyes_low")
            {
                playerInputManager.playerPrefab.transform.GetChild(0).transform.
                GetChild(i).GetComponent<SkinnedMeshRenderer>().material = player01Material;
            }
        }

    }

    // Update is called once per frame
    void Update()
    {
        switch (playerInputManager.playerCount)
        {
            case 0:
                break;
            case 1:
                if (!isFirstJoin)
                {
                    isFirstJoin = true;
                    FirstJoin();
                }
                break;
            case 2:
                if (!isSecondJoin)
                {
                    isSecondJoin = true;
                    SecondJoin();
                }
                break;
            default:
                break;
        }
    }

    public void FirstJoin()
    {
        UserReady01.SetActive(false);
        User01.SetActive(true);
        GameObject[] players = GameObject.FindGameObjectsWithTag("PlayerObject");
        players[0].GetComponent<Player>().MultiType = 1;
        StopPlayerScripts(players);
        playerInputManager.playerPrefab.transform.position = user02Pos;
        for (int i = 0; i < children; i++)
        {
            if (playerInputManager.playerPrefab.transform.GetChild(0).transform.
                GetChild(i).GetComponent<SkinnedMeshRenderer>() != null &&
                playerInputManager.playerPrefab.transform.GetChild(0).transform.
                GetChild(i).name != "head_eyes_low")
            {
                playerInputManager.playerPrefab.transform.GetChild(0).transform.
                GetChild(i).GetComponent<SkinnedMeshRenderer>().material = player02Material;
            }
        }
        playerInputManager.playerPrefab.transform.GetChild(6).transform.GetChild(0).
            GetComponent<SpriteRenderer>().sprite = target;
    }

    public void SecondJoin()
    {
        UserReady02.SetActive(false);
        User02.SetActive(true);
        GameObject[] players = GameObject.FindGameObjectsWithTag("PlayerObject");
        players[1].GetComponent<Player>().MultiType = 2;
        StopPlayerScripts(players);
        countDownUI.CountStart(User01, User02);

        playerInputManager.GetComponent<PlayerInputManager>().DisableJoining();
    }

    private void StopPlayerScripts(GameObject[] Players)
    {
        foreach (var player in Players)
        {
            player.GetComponent<Player>().enabled = false;
            player.transform.GetChild(0).GetComponent<Animator>().enabled = false;
        }
    }

    public void StartPlayerScripts(GameObject[] Players)
    {
        foreach (var player in Players)
        {
            player.GetComponent<Player>().enabled = true;
            player.transform.GetChild(0).GetComponent<Animator>().enabled = true;
        }
    }

    public void HidePlayerUI(GameObject user01, GameObject user02)
    {
        user01.SetActive(false);
        user02.SetActive(false);
    }
}
