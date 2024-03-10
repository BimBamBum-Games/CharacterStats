using System;
using UnityEngine;

[DefaultExecutionOrder(-50)]
public class CharacterStats : MonoBehaviour {
    [SerializeField] private float _physicalDamageReduction;
    [SerializeField] private float _effectiveHealthPointByPhysicalReduction;
    [SerializeField] float _healthPointBakedPrevious, _healthPointCurrentAt;

    [SerializeField] StatContext _healthPoint;
    [SerializeField] StatContext _physicalArmor;

    IEffectiveHpCalculator _effective_hp_by_physical_reduction = new GetEffectiveHpByPhysicalArmor();
    IArmorReduction _armor_reduction_by_physical = new WarCraftStylePhysicalArmorReduction();

    public static CharacterStats Instance { get; private set; }

    public event EventHandler<CharacterStatsArgs> OnValueChange;

    public CharacterStatsArgs characterStatsArgs = new CharacterStatsArgs();

    public class CharacterStatsArgs : EventArgs {
        public float healthPointBaked;
        public float healthPointAt;
        public float physicalDamageReductionRatio;
        public float effectiveHealthPointByPhysicalReduction;
    }

    void Awake() {
        if(Instance == null) {
            Instance = this;
        }
        else {
            Destroy(Instance.gameObject);
        }

        //Ilk baslangica var sayilanlari getir.
        _healthPointCurrentAt = _healthPointBakedPrevious = _healthPoint.GetBakedValue();

    }


    [ContextMenu("Get Physical Armor Reduction")]
    public float GetPhysicalArmorReduction() {
        Debug.LogWarning("Physical Armor Reduction Hesaplandi!");
        return _physicalDamageReduction = _armor_reduction_by_physical.Execute(_physicalArmor);
    }


    [ContextMenu("Get Effective Health Point")]
    public float GetEffectiveHealthByPhysicalArmor() {
        return _effectiveHealthPointByPhysicalReduction = _effective_hp_by_physical_reduction.Execute(_healthPoint, _physicalDamageReduction);
    }

    public float GetFilteredDamage(float hpBaked, float pdReductionRatio) {
        return hpBaked - hpBaked * pdReductionRatio;
    }

    public void TakePhysicalDamage(StatContext physicalDamage) {

        //Su anki baked hp degeri degisirse gecmisteki baked hp degeri uzerindeki bir hp degerini de yenilenmis bake hp degerine gore uyarlayarak olcekler.
        if (_healthPointBakedPrevious != _healthPoint.GetBakedValue()) {
            _healthPointCurrentAt = ScaleHealthPointAt(_healthPoint.GetBakedValue(), _healthPointBakedPrevious, _healthPointCurrentAt);
            _healthPointBakedPrevious = _healthPoint.GetBakedValue();
        }

        float physicalDamageReductionRatio = GetPhysicalArmorReduction();
        float fiteredDamage = GetFilteredDamage(physicalDamage.GetBakedValue(), physicalDamageReductionRatio);
        float effectiveHPbyPA = GetEffectiveHealthByPhysicalArmor();

        _healthPointCurrentAt -= fiteredDamage;
        _healthPointCurrentAt = Mathf.Max(_healthPointCurrentAt, 0);
        if (_healthPointCurrentAt == 0) {
            Debug.LogWarning("Hero Has Been Killed!");
            _healthPointCurrentAt = _healthPointBakedPrevious;
        }

        //Debug.LogWarning($"Enemy Baked Physical Damage > {physicalDamage.GetBakedValue()}.");
        //Debug.LogWarning($"Player Damage Reduction Rate > {physicalDamageReductionRatio}.");
        //Debug.LogWarning($"Player Absorbed Damage (Reducted Damage) > {fiteredDamage}.");
        //Debug.LogWarning($"Player E.H.P. By Physical Reduction > {effectiveHPbyPA}.");
        //Debug.LogWarning($"Player H.P > {_healthPointCurrentAt}.");

        characterStatsArgs.healthPointBaked = _healthPoint.GetBakedValue();
        characterStatsArgs.healthPointAt = _healthPointCurrentAt;
        characterStatsArgs.physicalDamageReductionRatio = physicalDamageReductionRatio;
        characterStatsArgs.effectiveHealthPointByPhysicalReduction = effectiveHPbyPA;

        OnValueChange?.Invoke(this, characterStatsArgs);
    }

    public void AddModifier(Modifier gameAsset) {
        _healthPoint.AdditivePercentageModifierContainer.Add(gameAsset);
    }


    public float ScaleHealthPointAt(float currentMax, float currentAt, float nextMax) {
        return (currentAt * nextMax) / currentMax;
    }

}

public abstract class IEffectiveHpCalculator {
    public abstract float Execute(StatContext healthStat, float damageReduction);
}

public class GetEffectiveHpByPhysicalArmor : IEffectiveHpCalculator {
    public override float Execute(StatContext healthStat, float damageReduction) {
        return healthStat.GetBakedValue() / (1 - damageReduction);
    }
}

public abstract class IArmorReduction {
    public abstract float Execute(StatContext anyKindOfArmor);
}

public class CustomPhysicalArmorReduction : IArmorReduction {
    public override float Execute(StatContext anyKindOfArmorStat) {
        return (anyKindOfArmorStat.GetBakedValue() * 0.01f) / (1 + anyKindOfArmorStat.GetBakedValue() * 0.01f); 
    }
}

public class WarCraftStylePhysicalArmorReduction : IArmorReduction {
    public override float Execute(StatContext anyKindOfArmorStat) {
        return (anyKindOfArmorStat.GetBakedValue() * 0.06f) / (1 + anyKindOfArmorStat.GetBakedValue() * 0.06f);
    }
}
