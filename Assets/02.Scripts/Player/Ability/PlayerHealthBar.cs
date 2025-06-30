using UnityEngine;
using UnityEngine.UI;

public class PlayerHealthBar : PlayerAbility
{
    public Slider HealthBarSlider;

    private void Start()
    {
        Refresh();

        //_owner.OnDataChanged += Refresh;
    }

    public void Refresh()
    {
        HealthBarSlider.value = _owner.Stat.Health / _owner.Stat.MaxHealth;
    }
}
