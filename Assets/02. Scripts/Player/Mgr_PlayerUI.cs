using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Mgr_PlayerUI : MonoBehaviour
{
    Mgr_Player Player;
    RectTransform rect;

    public GameObject Damage_Text;
    public float DamageForceMax = 1f;


    // Start is called before the first frame update
    void Start()
    {
        Player = GetComponentInParent<Mgr_Player>();
        rect = GetComponent<RectTransform>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Player.isFacingL)
        {
           rect.localRotation = Quaternion.Euler(0, 180, 0);
        }
        else
        {
            rect.localRotation = Quaternion.Euler(0, 0, 0);
        }
    }

    public void DamageTextShow(int Damage)
    {
        GameObject GO = Instantiate(Damage_Text, this.transform);
        GO.transform.position = this.transform.position;
        Destroy(GO, 2f);

        


        Vector2 Force = new Vector2(Random.Range(-DamageForceMax, DamageForceMax), Random.Range(2, DamageForceMax));

        GO.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        GO.GetComponent<Rigidbody2D>().velocity = Force;

        GO.GetComponent<Text>().text = Damage.ToString();
    }
}
