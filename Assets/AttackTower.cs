using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackTower : MonoBehaviour
{
    public LineRenderer laser;

    private MinionPlacer raidController;
    
    void Start()
    {
        raidController = FindObjectOfType<MinionPlacer>();
    }

    private float timeUntilNextAttack = 5;
    public float shotDelay = 20;
    void Update()
    {
        if (raidController && raidController.raidActive)
        {
            timeUntilNextAttack -= Time.deltaTime;

            if (timeUntilNextAttack <= 0)
            {
                Minion closestMinion = null;
                var closestDistance = float.MaxValue;
                foreach (var minion in raidController.minions)
                {
                    if(!minion)
                        continue;
                    var distance = (transform.position - minion.transform.position).magnitude;
                    if (distance < closestDistance)
                    {
                        closestDistance = distance;
                        closestMinion = minion;
                    }
                }

                if (closestDistance > 5)
                    closestMinion = null;
                
                if (closestMinion)
                {
                    StartCoroutine(FireLaser(closestMinion));
                    timeUntilNextAttack = shotDelay;
                }
            }
        }
    }

    public IEnumerator FireLaser(Minion target)
    {
        laser.SetPosition(0, transform.position + new Vector3(0, 2f, 0));
        laser.SetPosition(1, target.transform.position + new Vector3(0, 0.7f, 0));
        laser.gameObject.SetActive(true);
        yield return new WaitForSeconds(1);
        Destroy(target.gameObject);
        yield return new WaitForSeconds(1);
        laser.gameObject.SetActive(false);
    }
}
