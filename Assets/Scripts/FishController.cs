using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishController : MonoBehaviour
{
    private float speed = 0.04f;
    private int moveDirection = -1;
    private bool isFished;

    void Start() {
        isFished = false;
    }

    void Update() {
        if (!isFished) {
            Vector3 scale = transform.localScale;
            if (transform.position.x > 3f) {
                speed = Random.Range(0.02f, 0.06f);
                moveDirection = -1;
                if (transform.name.Substring(0, 5) == "fish5") {
                    scale.x = 0.3f;
                } else {
                    scale.x = 0.2f;
                }
            } else if (transform.position.x < -3f) {
                speed = Random.Range(0.02f, 0.06f);
                moveDirection = 1;
                if (transform.name.Substring(0, 5) == "fish5") {
                    scale.x = -0.3f;
                } else {
                    scale.x = -0.2f;
                }
            }
            transform.localScale = scale;
            this.transform.Translate(moveDirection * speed, 0, 0);
        }
    }

    private void OnCollisionEnter2D(Collision2D col) {
        if (col.gameObject.transform.childCount == 0) {
            if (col.gameObject.tag == "Hari") {
                isFished = true;
                this.transform.Translate(0, 0.2f, 0);
                if (transform.name.Substring(0, 5) == "fish5") {
                    GetComponent<CapsuleCollider2D>().enabled = false;
                } else {
                    GetComponent<CircleCollider2D>().enabled = false;
                }
                transform.parent = col.gameObject.transform;
            }
        }
    }
}
