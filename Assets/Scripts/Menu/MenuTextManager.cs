using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MenuTextManager : MonoBehaviour
{
    [SerializeField] private SpriteRenderer img; 
    [SerializeField] private TextMeshProUGUI name; 
    [SerializeField] private TextMeshProUGUI description;
    [SerializeField] private int lvlRequired;

    [Header("Only For Dark Knight Image")]
    [SerializeField] private GameObject[] darkKnightParts;

    private void Awake()
    {
        //If player has not beaten enough levels, do not display text and images
        if(PlayerPrefs.GetInt("lvlsBeaten") < lvlRequired)
        {
            if(img != null)
                img.color = Color.black;
            
            if(name != null)
            {
                name.text = "?";
                name.color = Color.black;
            }
            
            if(description != null)
            {
                description.text = "Complete level " + lvlRequired + " to unlock!";
                description.color = Color.black;
            }
            
            if(darkKnightParts != null)
            {
                foreach (GameObject part in darkKnightParts)
                {
                    SpriteRenderer sprite = part.GetComponent<SpriteRenderer>();
                    sprite.color = Color.black;
                }
            }
            
        }
    }
}
