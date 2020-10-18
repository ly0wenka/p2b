using UnityEngine;
using UnityEngine.UI;

public class PlayerInventoryDisplay : MonoBehaviour
{
    public Text starText;
    public Text lifeText;
    public void OnChangeStarTotal(int totalStars)
    {
        starText.text = $"stars = {totalStars}";
    }
    public void OnChangeLifeTotal(int totalLifes)
    {
        lifeText.text = $"lifes = {totalLifes}";
    }
}