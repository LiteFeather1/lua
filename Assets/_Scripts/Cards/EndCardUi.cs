using UnityEngine;
using UnityEngine.UI;

public class EndCardUi : MonoBehaviour
{
    [SerializeField] protected Image i_frame;
    [SerializeField] protected Image i_icon;
    [SerializeField] protected Image i_tierIcon;

    public void SetPower(PowerUp power)
    {
        i_frame.color = power.RarityColour;
        i_icon.sprite = power.Icon;
        i_tierIcon.sprite = power.TierIcon;
    }
}
