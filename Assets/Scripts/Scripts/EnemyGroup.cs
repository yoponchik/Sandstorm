using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyGroup : MonoBehaviour
{
    public Enemy leader;
    public Enemy[] bodyGuards;
    public Enemy[] rangers;

    // Start is called before the first frame update
    void Start()
    {
        // 그룹에 속한 적들을 생성하고싶다.
        // 생성된 Enemy에게 this를 알려주고싶다.
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
