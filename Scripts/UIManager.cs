using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField] private Text time;
    [SerializeField] private Text rings;

    [SerializeField] private PlatformerCharacterController controller;
    // Start is called before the first frame update

    // Update is called once per frame
    void Update()
    {
        time.text=(int)Time.time+"";
        rings.text=controller.ringCount+"";
    }
}
