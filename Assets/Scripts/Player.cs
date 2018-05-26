﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// プレイヤークラス
/// </summary>
public class Player : MonoBehaviour {
	private GameManager GM;			// GameManagerオブジェクトをアクセスするため
									// ＊＊＊＊GameManagerシーンはゲームシーンと一緒にロードしているのは必要です！！
	public int playerNumber;		// 0: P1 (青) ; 1: P2 (赤)
	public float speed = .2f;

	private string controlAxisX;
	private string controlAxisY;
	private GameObject assignedBulletPrefab;

	private bool controllerEnabled = true;

	public int HP = 3;

    private PlayerHealthUI playerHealthUI;

	// Property
	public bool ControllerEnabled {
		get { return controllerEnabled; }
		set { controllerEnabled = value; }
	}

	void Start () {
		GM = GameObject.Find ("GameManager").GetComponent<GameManager> ();

		//コントロールをロードする
		switch (playerNumber) {
			case 0:
				controlAxisX = "Horizontal_P1";
				controlAxisY = "Vertical_P1";
				assignedBulletPrefab = GM.playerBulletPrefab_Blue;
                playerHealthUI = GameObject.Find("Player1HP").GetComponent<PlayerHealthUI> ();
				break;
			case 1:
				controlAxisX = "Horizontal_P2";
				controlAxisY = "Vertical_P2";
				assignedBulletPrefab = GM.playerBulletPrefab_Red;
                playerHealthUI = GameObject.Find("Player2HP").GetComponent<PlayerHealthUI>();
                break;
		}


	}

	void Update () {
		if (controllerEnabled) {
			Move ();
			Direction ();
		}

	}

	// コントロールで動く
	void Move() {
		//　行動コントロール
		transform.Translate (Input.GetAxis (controlAxisX) * speed, Input.GetAxis (controlAxisY) * speed, 0);

		// 攻撃コントロール
		switch (playerNumber) {
		case 0 :
			if (Input.GetKeyDown(KeyCode.LeftShift)) FireBullet();
			break;
		case 1:
			if (Input.GetKeyDown(KeyCode.RightShift)) FireBullet();
			break;
		}

	}

	// スプライトの向こう
	void Direction() {
		switch (playerNumber) {
			case 0:
				if (Input.GetKeyDown (KeyCode.W)) {
					this.gameObject.transform.GetChild (0).gameObject.GetComponent<SpriteRenderer>().enabled = false;
					this.gameObject.transform.GetChild (1).gameObject.GetComponent<SpriteRenderer>().enabled = false;
					this.gameObject.transform.GetChild (2).gameObject.GetComponent<SpriteRenderer>().enabled = false;
					this.gameObject.transform.GetChild (3).gameObject.GetComponent<SpriteRenderer>().enabled = true;
				}
				if (Input.GetKeyDown(KeyCode.A)) {
					this.gameObject.transform.GetChild (0).gameObject.GetComponent<SpriteRenderer>().enabled = false;
					this.gameObject.transform.GetChild (1).gameObject.GetComponent<SpriteRenderer>().enabled = true;
					this.gameObject.transform.GetChild (2).gameObject.GetComponent<SpriteRenderer>().enabled = false;
					this.gameObject.transform.GetChild (3).gameObject.GetComponent<SpriteRenderer>().enabled = false;
				}
				if (Input.GetKeyDown(KeyCode.S)) {
					this.gameObject.transform.GetChild (0).gameObject.GetComponent<SpriteRenderer>().enabled = true;
					this.gameObject.transform.GetChild (1).gameObject.GetComponent<SpriteRenderer>().enabled = false;
					this.gameObject.transform.GetChild (2).gameObject.GetComponent<SpriteRenderer>().enabled = false;
					this.gameObject.transform.GetChild (3).gameObject.GetComponent<SpriteRenderer>().enabled = false;
				}
				if (Input.GetKeyDown(KeyCode.D))  {
					this.gameObject.transform.GetChild (0).gameObject.GetComponent<SpriteRenderer>().enabled = false;
					this.gameObject.transform.GetChild (1).gameObject.GetComponent<SpriteRenderer>().enabled = false;
					this.gameObject.transform.GetChild (2).gameObject.GetComponent<SpriteRenderer>().enabled = true;
					this.gameObject.transform.GetChild (3).gameObject.GetComponent<SpriteRenderer>().enabled = false;
				}
				break;
			case 1:
				if (Input.GetKeyDown (KeyCode.I)) {
					this.gameObject.transform.GetChild (0).gameObject.GetComponent<SpriteRenderer>().enabled = false;
					this.gameObject.transform.GetChild (1).gameObject.GetComponent<SpriteRenderer>().enabled = false;
					this.gameObject.transform.GetChild (2).gameObject.GetComponent<SpriteRenderer>().enabled = false;
					this.gameObject.transform.GetChild (3).gameObject.GetComponent<SpriteRenderer>().enabled = true;
				}
				if (Input.GetKeyDown(KeyCode.J)) {
					this.gameObject.transform.GetChild (0).gameObject.GetComponent<SpriteRenderer>().enabled = false;
					this.gameObject.transform.GetChild (1).gameObject.GetComponent<SpriteRenderer>().enabled = true;
					this.gameObject.transform.GetChild (2).gameObject.GetComponent<SpriteRenderer>().enabled = false;
					this.gameObject.transform.GetChild (3).gameObject.GetComponent<SpriteRenderer>().enabled = false;
				}
				if (Input.GetKeyDown(KeyCode.K)) {
					this.gameObject.transform.GetChild (0).gameObject.GetComponent<SpriteRenderer>().enabled = true;
					this.gameObject.transform.GetChild (1).gameObject.GetComponent<SpriteRenderer>().enabled = false;
					this.gameObject.transform.GetChild (2).gameObject.GetComponent<SpriteRenderer>().enabled = false;
					this.gameObject.transform.GetChild (3).gameObject.GetComponent<SpriteRenderer>().enabled = false;
				}
				if (Input.GetKeyDown(KeyCode.L))  {
					this.gameObject.transform.GetChild (0).gameObject.GetComponent<SpriteRenderer>().enabled = false;
					this.gameObject.transform.GetChild (1).gameObject.GetComponent<SpriteRenderer>().enabled = false;
					this.gameObject.transform.GetChild (2).gameObject.GetComponent<SpriteRenderer>().enabled = true;
					this.gameObject.transform.GetChild (3).gameObject.GetComponent<SpriteRenderer>().enabled = false;
				}
				break;
			}
	}


	// 玉を撃つ
	void FireBullet() {
		// GMに玉の量を減る
		if (GM.ammo <= 0) {
			Debug.Log ("玉が足りない");
		} else {
			GM.ammo--;
			Instantiate (assignedBulletPrefab, transform.position, transform.rotation);
		}
		// プレイヤーの玉プリファブの生成

		Debug.Log ("撃ている");
	}

	// トリガーとの衝突
	void OnTriggerEnter2D(Collider2D other) {
		//　敵、 敵の玉と
		if (other.tag == "Enemy") {
			HP--;
		}

		// 玉と
		switch (playerNumber) {
		case 0:
			if (other.tag == "Enemy_BlueBullet")
				HP--;
                playerHealthUI.OnDamage(HP);
			if (other.tag == "Enemy_RedBullet")
				Destroy(other.gameObject);
			break;
		case 1:
			if (other.tag == "Enemy_RedBullet")
				HP--;
                playerHealthUI.OnDamage(HP);
			if (other.tag == "Enemy_BlueBullet")
				Destroy(other.gameObject);
			break;
		}
	}

	// HPチェック
	public bool IsDead(){
		return (HP <= 0) ? true : false;
	}



}
