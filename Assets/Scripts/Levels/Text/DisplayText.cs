using UnityEngine;
using TMPro;

public class DisplayText : MonoBehaviour
{
    [SerializeField] private float xPos, yPos, xExitPos, yExitPos;
    [SerializeField] private bool enableRange;

    private bool xTrue, yTrue, xExitTrue, yExitTrue;
    private bool exit, doNotExit;

    private TextMeshProUGUI text;

    private void Awake()
    {
        text = gameObject.GetComponent<TextMeshProUGUI>();

        if (xPos == 0)
            xTrue = true;
        if (yPos == 0)
            yTrue = true;
        if (xExitPos == 0 || yExitPos == 0)
            doNotExit = true;

        exit = false;
    }

    private void Update()
    {
        if (((enableRange && Mathf.Abs(PlayerProperties.position.x - xPos) <= 1.5f) || 
                (!enableRange && PlayerProperties.position.x >= xPos) || xTrue) && 
            ((enableRange && Mathf.Abs(PlayerProperties.position.y - yPos) <= 1.5f) ||
                (!enableRange && PlayerProperties.position.y >= yPos) || yTrue) && !exit)
        {
            if (!xTrue && Time.timeSinceLevelLoad < 0.5f) return;

            xTrue = true;
            yTrue = true;
        }

        if (xTrue && yTrue)
            text.enabled = true;

        if ((PlayerProperties.position.x >= xExitPos || xExitTrue) && (PlayerProperties.position.y >= yExitPos || yExitTrue) && !doNotExit)
        {
            xExitTrue = true;
            yExitTrue = true;
        }

        if (xExitTrue && yExitTrue)
        {
            text.enabled = false;
            exit = true;
        }    
    }
}
