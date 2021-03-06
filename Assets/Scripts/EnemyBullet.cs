﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// EnemyBulletクラス
/// </summary>
public class EnemyBullet : MonoBehaviour
{
    /// <summary>
    /// 速さ
    /// </summary>
    float speed = 1.0f;

    /// <summary>
    /// 弾の向き
    /// </summary>
    Vector3 direction = Vector2.down;

    void Start()
    {

    }

    void Update()
    {
        // 移動
        transform.position += direction * speed * Time.deltaTime;
    }

    void OnTriggerEnter(Collider other)
    {
        
    }

    // ----------------------------------------------------------------------------------------------------------
    // public関数
    // ----------------------------------------------------------------------------------------------------------
    /// <summary>
    /// 弾の初期化
    /// </summary>
    /// <param name="_direction">発射方向</param>
    public void initBullet(Vector2 _direction)
    {
        direction = _direction;
    }
}
