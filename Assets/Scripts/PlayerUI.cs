using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUI : MonoBehaviour
{
    // �A�C�e��UI
    [SerializeField]
    Image LTitemUI = null;
    [SerializeField]
    Image RTitemUI = null;

    // �\������A�C�e���C���[�W
    [SerializeField]
    Sprite[] itemSprites = null;



    // Start is called before the first frame update
    void Start()
    {
        RTitemUI.GetComponent<Image>().enabled = false;
        LTitemUI.GetComponent<Image>().enabled = false;
    }

    // Update is called once per frame
    public void ShowPlayerUI(Item LItem, Item RItem)
    {
        if (RItem != null)
        {
            Debug.Log(RItem.name);
            RTitemUI.GetComponent<Image>().sprite = itemSprites[RItem.no - 1];
            RTitemUI.GetComponent<Image>().enabled = true;
        }
        else
        {
            RTitemUI.GetComponent<Image>().enabled = false;
        }
        if (LItem != null)
        {
            Debug.Log(LItem.name);
            LTitemUI.GetComponent<Image>().sprite = itemSprites[LItem.no - 1];
            LTitemUI.GetComponent<Image>().enabled = true;
        }
        else
        {
            LTitemUI.GetComponent<Image>().enabled = false;
        }
    }
}
