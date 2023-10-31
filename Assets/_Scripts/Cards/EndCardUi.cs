using UnityEngine;
using UnityEngine.UI;

public class EndCardUi : MonoBehaviour
{
    [SerializeField] protected Image i_frame;
    [SerializeField] protected Image i_icon;

    public void SetPower(PowerUp power)
    {
        i_frame.color = power.RarityColour;
        i_icon.sprite = i_icon.sprite;
    }
}
