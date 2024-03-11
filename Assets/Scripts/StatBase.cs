using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class StatContext {
    //StatBase aslinda stat sinifidir. Diger deyis ile Modifier Strategy siniflarindan gelen degisimlere gore degeri update edilmektedir.
    public float StatLevel;
    [SerializeField] float _bakedValue;

    public ModifierDirect DirectModifierContainer;
    public ModifierAdditiveRate AdditivePercentageModifierContainer;
    public ModifierCumulativeRate CumulativeModifierContainer;

    //[ContextMenu("Calculate Total Impact")]
    private void CalculateTotalModifierImpact() {
        //Contexte bagli metod. Bu siraya gore oncelikli hesaplama devreye alinir.
        _bakedValue = DirectModifierContainer.CalculateTotalImpact(StatLevel);
        //Debug.LogWarning($"Baked Value 01 {BakedValue}");
        _bakedValue = AdditivePercentageModifierContainer.CalculateTotalImpact(_bakedValue);
        //Debug.LogWarning($"Baked Value 02 {BakedValue}");
        _bakedValue = CumulativeModifierContainer.CalculateTotalImpact(_bakedValue);
        //Debug.LogWarning($"Baked Value 03 {BakedValue}");
    }

    public float GetBakedValue() {
        //Property ile cagrilirsa diger hesaplamalarda pespese cagrildiginda foreach stack overlflow yaparak cakisir.
        CalculateTotalModifierImpact();
        return _bakedValue;
    }

    public void SetBakedValue(float bakedValueFromOutside) {
        //Islemler sonucunda update edilmesi istenebilir.
        _bakedValue = bakedValueFromOutside;
    }

    public void AddModifier(Modifier modifier) {
        //Modifierler burada secim almalidir. //Referans kaybolursa dýsaridan calismaz. (Surekli new lenen bir yapi olursa.)
        if(modifier.ModifierType == ModifierType.Direct) {
            DirectModifierContainer.Add(modifier);
        }
        else if (modifier.ModifierType == ModifierType.Direct) {
            AdditivePercentageModifierContainer.Add(modifier);
        }
        else if (modifier.ModifierType == ModifierType.Direct) {
            CumulativeModifierContainer.Add(modifier);
        }
    }
}