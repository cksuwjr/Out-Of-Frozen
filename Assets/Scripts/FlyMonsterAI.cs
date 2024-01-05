using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyMonsterAI : MonsterAI
{
    [SerializeField] Vector2 X_minmax = Vector2.zero;
    [SerializeField] Vector2 Y_minmax = Vector2.zero;

    int X_dir = 1;
    int Y_dir = 1;
    protected override void StateRegister()
    {
        AddAct("Go", 100f);
    }
    IEnumerator Act()
    {
        bool Rest = false;
        while (!Rest)
        {
            ObstacleCheck();

            if (controller.Movable)
            {
                if (NowAct == "Go")
                {
                    float x = transform.position.x;
                    float y = transform.position.y;

                    if (X_minmax.x > x)
                        X_dir = 1;
                    else if (X_minmax.y < x)
                        X_dir = -1;


                    if (X_dir != 0)
                        transform.localScale = new Vector3(X_dir, 1, 1);


                    if (Y_minmax.x > y)
                        Y_dir = 1;
                    else if (Y_minmax.y < y)
                        Y_dir = -1;

                    rb.velocity = new Vector2(X_dir * status.Speed, Y_dir * status.Speed);
                }
            }
            yield return null;
        }
    }
}
