using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterStats : MonoBehaviour {

    [SerializeField] private float _effectiveHealthByPhysicalArmor, _effectiveHealthByMagicArmor, _effectiveHealthByTrueArmor;

    [SerializeField] StatContext _health;
    [SerializeField] StatContext _magicArmor, _trueArmor, _physicalArmor;
    [SerializeField] StatContext _rangeAttack, _rangeAttackSpeed, _rangeAttackRange;

    IEffectiveHealthPointCalculator _get_ehp_by_physical_armor = new Get_EHP_ByPhysicalArmor();
    IEffectiveHealthPointCalculator _get_ehp_by_magical_armor = new Get_EHP_ByMagicalArmor();
    IEffectiveHealthPointCalculator _get_ehp_by_true_armor = new Get_EHP_ByTrueArmor();

    [ContextMenu("Get Effective Health By Physical Armor")]
    public void Get_EffectiveHealth_By_PhysicalArmor() {
        //Physical armor, physical rezistansi anlamina gelmektedir.
        _effectiveHealthByPhysicalArmor = _get_ehp_by_physical_armor.Execute(_health, _physicalArmor);
        Debug.LogWarning("Effective Health Physical Armor Ile Hesaplandi!");
    }

    [ContextMenu("Get Effective Health By Magic Armor")]
    public void Get_EffectiveHealth_By_MagicArmor() {
        _get_ehp_by_magical_armor.Execute(_health, _magicArmor);
        Debug.LogWarning("Effective Health Magic Armor Ile Hesaplandi!");
    }

    [ContextMenu("Get Effective Health By True Armor")]
    public void Get_EffectiveHealth_By_TrueArmor() {
        _effectiveHealthByTrueArmor = _get_ehp_by_true_armor.Execute(_health, _trueArmor);
        Debug.LogWarning("Effective Health True Armor Ile Hesaplandi!");
    }
}


public abstract class IEffectiveHealthPointCalculator {
    public abstract float Execute(StatContext healthStat, StatContext oneOfArmorStat);
}

public class Get_EHP_ByPhysicalArmor : IEffectiveHealthPointCalculator {
    public override float Execute(StatContext healthStat, StatContext physicalArmor) {
        //Magic armor, magic rezistansi anlamina gelmektedir.
        healthStat.CalculateTotalModifierImpact();
        physicalArmor.CalculateTotalModifierImpact();
        return healthStat.BakedValue * (1 + physicalArmor.BakedValue * 0.01f);
    }
}

public class Get_EHP_ByMagicalArmor : IEffectiveHealthPointCalculator {
    public override float Execute(StatContext healthStat, StatContext magicalArmor) {
        //Magic armor, magic rezistansi anlamina gelmektedir.
        healthStat.CalculateTotalModifierImpact();
        magicalArmor.CalculateTotalModifierImpact();
        return healthStat.BakedValue * (1 + magicalArmor.BakedValue * 0.01f) * 0.5f;
    }
}

public class Get_EHP_ByTrueArmor : IEffectiveHealthPointCalculator {
    public override float Execute(StatContext healthStat, StatContext trueArmor) {
        //True armor, true damage rezistansi anlamina gelmektedir.
        healthStat.CalculateTotalModifierImpact();
        trueArmor.CalculateTotalModifierImpact();
        return healthStat.BakedValue * (1 + trueArmor.BakedValue * 0.001f);
    }
}