using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class MonsterController : MonoBehaviour
{
    [SerializeField] Transform endPoint;
    [SerializeField] float speed;
    [SerializeField] PlayableDirector timeline;
    public Animator monsterAN;

    public void MoveMonster() => StartCoroutine(MoveToWall());

    public IEnumerator MoveToWall()
    {
        timeline.Pause();

        transform.SetParent(null);

        var posDelta = new Vector2(endPoint.position.x, endPoint.position.z) - new Vector2(transform.position.x, transform.position.z);
        float distance = posDelta.magnitude;
        float elapsedDistance = 0;

        float oao = transform.rotation.z;
        float y = Mathf.Atan2(posDelta.x, posDelta.y) * Mathf.Rad2Deg;
        while (elapsedDistance < distance)
        {
            float delta = Time.deltaTime * speed;
            transform.rotation = Quaternion.Euler(new Vector3(0f, y, oao + ((180 - oao) / distance) * elapsedDistance));
            transform.Translate(Vector3.forward * delta);
            elapsedDistance += delta;
            yield return new WaitForEndOfFrame();
        }
        transform.position = endPoint.position;
        monsterAN.Play("Bonk");
        GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;

        timeline.Resume();
    }

}
