using System.Collections.Generic;
using UnityEngine;
using System;
using ExitGames.Client.Photon.StructWrapping;
using UnityEngine.UI;
using Unity.VisualScripting;

public class Player : MonoBehaviour, IDamageable
{
    public PlayerStat Stat;

    private Dictionary<Type, PlayerAbility> _abilitiesCache = new();
    
    

    public T GetAbility<T>() where T : PlayerAbility
    {
        var type = typeof(T);

        if (_abilitiesCache.TryGetValue(type, out PlayerAbility ability))
        {
            return ability as T;
        }

        // 게으른 초기화/로딩 -> 처음에 곧바로 초기화/로딩을 하는게 아니라
        //                    필요할때만 하는.. 뒤로 미루는 기법
        ability = GetComponentInChildren<T>();

        if (ability != null)
        {
            _abilitiesCache[ability.GetType()] = ability;

            return ability as T;
        }
        
        throw new Exception($"어빌리티 {type.Name}을 {gameObject.name}에서 찾을 수 없습니다.");
    }

    public void Damaged(float damage)
    {
        Stat.Health = Mathf.Max(0, Stat.Health - damage);
        GetAbility<PlayerHealthBar>().Refresh();

        Debug.Log($"남은체력: {Stat.Health}");
    }

private void Update()
    {
        if (!GetAbility<PlayerMove>().IsJumping && !GetAbility<PlayerMove>().IsRunning)
        {
            Stat.Stamina = Mathf.Min(Stat.MaxStamina, Stat.Stamina + Stat.StaminaRecovery * Time.deltaTime);
        }
    }
}



