using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItoController : MonoBehaviour
{
    public GameManager gameManager;
    public GameObject pointEffectPrefab;

    private const float speed = 0.08f; // 糸が上下するスピード

    private int moveDirection = 1;
    private bool isMovingFlag;
    private bool reachBottom; // 画面がタップされてから底に付いたらTrue
    private int currentHariNum; // 現在有効な針の数

    void Start() {
        isMovingFlag = false;
        reachBottom = false;
        currentHariNum = 2;
    }

    void FixedUpdate() {
        if (gameManager.GetGameMode() == GameManager.GAME_MODE.PLAY) {
            if (Input.GetMouseButton(0) && isMovingFlag == false) {
                moveDirection = -1;
                isMovingFlag = true;
                reachBottom = false;
            }

            if (isMovingFlag) {
                if (reachBottom && transform.position.y > 5.3f) {
                    // 糸が陸に上がったら魚を外しスコアを加算
                    int fishNum = 0; // つり上がった魚の数
                    isMovingFlag = false;
                    Transform children = GetComponentInChildren<Transform>();
                    foreach (Transform child in children) {
                        if (child.tag == "Hari") {
                            fishNum += child.GetComponent<HariController>().Fish();
                        } else {
                            GameObject gChild = child.transform.Find("hari").gameObject;
                            fishNum += gChild.GetComponent<HariController>().Fish();
                        }
                    }
                    // 全ての針で釣れたら針追加
                    if (fishNum == currentHariNum && fishNum < 5) {
                        currentHariNum++;
                        switch (fishNum) {
                            case 2:
                                GameObject hariito2 = transform.Find("hariito2").gameObject;
                                hariito2.SetActive(true);
                                break;
                            case 3:
                                GameObject hariito3 = transform.Find("hariito3").gameObject;
                                hariito3.SetActive(true);
                                break;
                            case 4:
                                GameObject hariito4 = transform.Find("hariito4").gameObject;
                                hariito4.SetActive(true);
                                gameManager.ChangeRespanTime(600);
                                break;
                            default:
                                break;
                        }
                        // 針追加テキストを生成
                        PointEffect pointEffect = Instantiate(pointEffectPrefab).GetComponent<PointEffect>();
                        pointEffect.transform.localPosition = new Vector3(-1, 2, 0);
                        pointEffect.SetText("針追加!");
                    }
                } else if (transform.position.y < 0.9f) {
                    reachBottom = true;
                    moveDirection = 1;
                }
                this.transform.Translate(0, moveDirection * speed, 0);
            }
        }
    }
}
