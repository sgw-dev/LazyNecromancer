using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelUpScreen : MonoBehaviour
{
    public Button HealthUp;
    public Button HealthDown;
    public Button ManaUp;
    public Button ManaDown;
    public Button StrengthUp;
    public Button StrengthDown;
    public Button MagicUp;
    public Button MagicDown;
    public Button StaminaUp;
    public Button StaminaDown;
    public Text HealthCost;
    public Text ManaCost;
    public Text StrengthCost;
    public Text MagicCost;
    public Text StaminaCost;
    public Text Health;
    public Text Mana;
    public Text Strength;
    public Text Magic;
    public Text Stamina;
    public Text EXP;
    public Button Keep;
    public Button Revert;


    //Will need a method to take these values in from the player
    public int healthValue;
    public int manaValue;
    public int strengthValue;
    public int magicValue;
    public int staminaValue;
    public int expValue;

    public int adjustedHealthValue;
    public int adjustedManaValue;
    public int adjustedStrengthValue;
    public int adjustedMagicValue;
    public int adjustedStaminaValue;
    public int adjustedExpValue;

    // Start is called before the first frame update
    void Start()
    {
        healthValue = 10;
        manaValue = 10;
        strengthValue = 10;
        magicValue = 10;
        staminaValue = 10;

        this.Health.text = healthValue.ToString();
        this.Mana.text = manaValue.ToString();
        this.Strength.text = strengthValue.ToString();
        this.Magic.text = magicValue.ToString();
        this.Stamina.text = staminaValue.ToString();

        expValue = 1000;

        this.EXP.text = "EXP: " + expValue.ToString();

        //TODO: Will need functions to calculate these, placeholder
        this.HealthCost.text = "To level: " + 100;
        this.ManaCost.text = "To level: " + 100;
        this.StrengthCost.text = "To level: " + 100;
        this.MagicCost.text = "To level: " + 100;
        this.StaminaCost.text = "To level: " + 100;

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
