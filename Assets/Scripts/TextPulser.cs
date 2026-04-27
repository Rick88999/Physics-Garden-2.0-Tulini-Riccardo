using UnityEngine;
using TMPro; 

public class TextPulser : MonoBehaviour
{
    public TextMeshProUGUI textToPulse; 

    public float speed = 1.5f;          
    public float minAlpha = 0.2f;       
    public float maxAlpha = 1.0f;       

    void Update()
    {
        if (textToPulse != null)
        {
            Color currentColor = textToPulse.color;

            float pulseValue = Mathf.PingPong(Time.time * speed, maxAlpha - minAlpha);

            currentColor.a = minAlpha + pulseValue;

            textToPulse.color = currentColor;
        }
    }
}