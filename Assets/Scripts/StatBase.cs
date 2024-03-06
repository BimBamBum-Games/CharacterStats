using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class StatContext {
    //StatBase aslinda stat sinifidir. Diger deyis ile Modifier Strategy siniflarindan gelen degisimlere gore degeri update edilmektedir.
    public float StatBaseValue = 10f;
    public float BakedValue;

    public CumulativePercentageModifierContainer CumulativeModifierContainer;
    public DirectModifierContainer DirectModifierContainer;
    public AdditivePercentageModifierContainer AdditivePercentageModifierContainer;

    //[ContextMenu("Calculate Total Impact")]
    public void CalculateTotalModifierImpact() {
        //Contexte bagli metod. Bu siraya gore oncelikli hesaplama devreye alinir.
        BakedValue = DirectModifierContainer.CalculateTotalImpact(StatBaseValue);
        Debug.LogWarning($"Baked Value 01 {BakedValue}");
        BakedValue = AdditivePercentageModifierContainer.CalculateTotalImpact(BakedValue);
        Debug.LogWarning($"Baked Value 02 {BakedValue}");
        BakedValue = CumulativeModifierContainer.CalculateTotalImpact(BakedValue);
        Debug.LogWarning($"Baked Value 03 {BakedValue}");
    }
}
