using UnityEngine;
using UnityEngine.UI; 

public class Abilities : MonoBehaviour
{
    Image abilityImage;
    private float cooldownTime;
    private float currentTime;
    private bool isCooldown = false;

    void Start()
    {
        abilityImage = GetComponent<Image>();
        abilityImage.fillAmount = 0;
    }

    public void StartCooldown(float cd)
    {
        cooldownTime = cd;
        currentTime = cd;
        isCooldown = true;
        abilityImage.fillAmount = 1;
    }

    void Update()
    {
        if (!isCooldown) return;

        currentTime -= Time.deltaTime;

        abilityImage.fillAmount = currentTime / cooldownTime;

        if (currentTime <= 0)
        {
            currentTime = 0;
            isCooldown = false;
            abilityImage.fillAmount = 0;
        }
    }
}