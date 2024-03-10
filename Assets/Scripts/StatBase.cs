using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class StatContext {
    //StatBase aslinda stat sinifidir. Diger deyis ile Modifier Strategy siniflarindan gelen degisimlere gore degeri update edilmektedir.
    public float StatLevel;
    [SerializeField] float _bakedValue;

    public DirectModifierContainer DirectModifierContainer;
    public AdditivePercentageModifierContainer AdditivePercentageModifierContainer;
    public CumulativePercentageModifierContainer CumulativeModifierContainer;

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

    public float GetBaseValue(float anyKindOfArmorStat) {
        return (anyKindOfArmorStat * 0.06f) / (1 + anyKindOfArmorStat * 0.06f);
    }

    public void AddModifier(Modifier modifier) {
        if(modifier.ModifierType == ModifierType.Direct) {

        }else if (modifier.ModifierType == ModifierType.Direct) {

        }else if (modifier.ModifierType == ModifierType.Direct) {

        }
    }
}