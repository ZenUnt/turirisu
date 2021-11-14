using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HariController : MonoBehaviour
{
    public GameManager gameManager;
    public GameObject pointEffectPrefab;

    void Start() {
        
    }

    void Update() {
        
    }

    // 釣り上げた魚を外しスコアに加算。魚が釣れたらtrueを返す
    public int Fish() {
        int retrnVal = 0;
        foreach (Transform ob in transform) {
            if (ob != null && ob.tag == "Fish") {
                retrnVal = 1;
                int point = 0;
                switch (ob.name.Substring(0,5)) {
                    case "fish1":
                        point = 50;
                        break;
                    case "fish2":
                        point = 100;
                        break;
                    case "fish3":
                        point = 200;
                        break;
                    case "fish4":
                        point = 500;
                        break;
                    case "fish5":
                        point = 2000;
                        break;
                    default: break;
                }
                // ポイントエフェクトを作成
                PointEffect pointEffect = Instantiate(pointEffectPrefab).GetComponent<PointEffect>();
                pointEffect.transform.localPosition = ob.transform.position;
                pointEffect.SetText(point.ToString());
                // ポイントを加算
                gameManager.AddScore(point);
                // 魚を消去
                GameObject.Destroy(ob.gameObject);
                gameManager.MinusFishNum();
                gameManager.AddScoreFishNum();
            }
        }
        // 魚を釣れるようにする
        GetComponent<CircleCollider2D>().isTrigger = false;
        return retrnVal;
    }

    private void OnCollisionEnter2D(Collision2D col) {
        if (col.gameObject.tag == "Fish") {
            // 魚が釣れたら2匹以上1つの針に釣れないようにする
            GetComponent<CircleCollider2D>().isTrigger = true;
        }
    }


}
