using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryPanel : MonoBehaviour
{
    public Button Btn_back;


    // Start is called before the first frame update
    void Start()
    {
        if (Btn_back != null)
        {
            Btn_back.onClick.AddListener(() =>
            {
                this.gameObject.SetActive(false);
            });
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
