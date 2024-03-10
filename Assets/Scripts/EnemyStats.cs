using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EnemyStats : MonoBehaviour
{
    [SerializeField] Button _attackButton;
    [SerializeField] TextMeshProUGUI _characterHealthPointTmp, _playerPhysicalArmorReduction, _playerEhpByPhysicalArmorReduction, _enemyPhysicalDamageTmp;
    [SerializeField] public StatContext _physicalDamage;
    [SerializeField] public Slider _healthBarSld;

    void Awake() {
        _attackButton.onClick.AddListener(AttackByPhysicalDamage);
    }

    private void OnEnable() {
        CharacterStats.Instance.OnValueChange += Update_CharacterStatsTmp_OnValueChange;
    }

    private void OnDisable() {
        CharacterStats.Instance.OnValueChange -= Update_CharacterStatsTmp_OnValueChange;
    }

    //Karakter Stat Health UI elementini gunceller. CharacterStats OnValueChange subscribe olmalidir.
    public void Update_CharacterStatsTmp_OnValueChange(object o, CharacterStats.CharacterStatsArgs csa) {
        _characterHealthPointTmp.text = csa.healthPointAt.ToString();
        _playerPhysicalArmorReduction.text = csa.physicalDamageReductionRatio.ToString();
        _playerEhpByPhysicalArmorReduction.text = csa.effectiveHealthPointByPhysicalReduction.ToString();
        _enemyPhysicalDamageTmp.text = _physicalDamage.GetBakedValue().ToString();
        _healthBarSld.value = csa.healthPointAt / csa.healthPointBaked;
    }

    //Attack yapacagi kisi PlayerCharacter oldugundan onu cagirir.
    public void AttackByPhysicalDamage() {
        CharacterStats.Instance.TakePhysicalDamage(_physicalDamage);
    }
}

