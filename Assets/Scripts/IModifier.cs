using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[Serializable]
public abstract class IModifier {
    //Abstract IModifier
    public float modifierBaseValue;
}

[Serializable]
public class Modifier : IModifier {
    //Concrete Modifier
}


[Serializable]
public abstract class IModifierContainer {
    //Concrete Modifier Iceren Abstract Modifier Container Class.
    public List<Modifier> modifiers;
    public void Add(Modifier modifier) {
        modifiers.Add(modifier);
    }

    public void Remove(Modifier modifier) {
        modifiers.Remove(modifier);
    }

    //Onemli metod ve strategy saglamaktadir.
    public abstract float CalculateTotalImpact(float statBaseValue);
}

[Serializable]
public class CumulativePercentageModifierContainer : IModifierContainer {
    public override float CalculateTotalImpact(float statBaseValue) {
        float totalModifierImpact = statBaseValue;
        foreach(Modifier modifier in modifiers) {
            totalModifierImpact += (totalModifierImpact * modifier.modifierBaseValue * 0.01f);
        }
        return totalModifierImpact;
    }
}

[Serializable]
public class DirectModifierContainer : IModifierContainer {
    public override float CalculateTotalImpact(float statBaseValue) {
        float totalModifierImpact = statBaseValue;
        foreach (Modifier modifier in modifiers) {
            totalModifierImpact += modifier.modifierBaseValue;
        }
        return totalModifierImpact;
    }
}

[Serializable]
public class AdditivePercentageModifierContainer : IModifierContainer {
    public override float CalculateTotalImpact(float statBaseValue) {
        float totalModifierImpact, cumulativeModifierImpact;
        totalModifierImpact = cumulativeModifierImpact = statBaseValue;
        foreach (Modifier modifier in modifiers) {
            totalModifierImpact = (totalModifierImpact * modifier.modifierBaseValue * 0.01f);
            cumulativeModifierImpact += totalModifierImpact;
        }
        //Cumulative sifir donmemesi gerek.
        return cumulativeModifierImpact;
    }
}