using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class submitname : MonoBehaviour
{
    public TextMeshProUGUI user_name;
    public TMP_InputField user_inputField;
    public void setname()
    {
        user_name.text = user_inputField.text;
    }



}
