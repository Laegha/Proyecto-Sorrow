using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class MonsterController : MonoBehaviour
{
    [SerializeField] Transform endPoint;
    [SerializeField] float speed;
    [SerializeField] PlayableDirector timeline;

    public void MoveMonster() => StartCoroutine(MoveToWall());

    public IEnumerator MoveToWall()
    {
        timeline.Pause();

        transform.SetParent(null);
        float distance = Vector3.Distance(transform.position, endPoint.position);
        Vector3 direction = (transform.position- endPoint.position).normalized;
        float elapsedDistance = 0;
        while(elapsedDistance < distance)
        {
            float delta = Time.deltaTime * speed;
            transform.Translate(direction * delta);
            elapsedDistance += delta;
            yield return new WaitForEndOfFrame();
        }
        timeline.Resume();
    }
}
