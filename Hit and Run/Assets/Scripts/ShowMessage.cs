using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ShowMessage : MonoBehaviour
{
    public TextMeshProUGUI textMessage;
    public static string message;

    private void Start()
    {
        message = "";
    }

    private void FixedUpdate()
    {
        if (message != null)
        {
            textMessage.text = message;
        }
    }
}
