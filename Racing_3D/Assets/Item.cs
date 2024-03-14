using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;

public class Item : MonoBehaviour
{
    private void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player"))
            return;
        float randomValue = Random.value * 100f; // 0에서 100 사이의 랜덤 값 생성

        switch (randomValue)
        {
            case < 5f:// 코인 1000
                Debug.Log("10% 확률로 동작 1");
                break;
            case < 15f:// 코인 500
                Debug.Log("10% 확률로 동작 2");
                break;
            case < 60f: // 랜덤 사용 아이템 - 미사일, 범퍼- 부딮히면 상대차 뒤집힙.
                Debug.Log("20% 확률로 동작 3");
                break;
            default: //코인 100
                Debug.Log("30% 확률로 동작 5");
                break;
        }

        GetComponent<MeshRenderer>().enabled = false;
        StartCoroutine(Regen());
    }

    IEnumerator Regen()
    {
        yield return new WaitForSeconds(2);
        GetComponent<MeshRenderer>().enabled = true;
    }
}

