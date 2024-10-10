using System.Collections;
using UnityEngine;

public class Mob_Move : MonoBehaviour
{
    public float move_speed;

    [SerializeField] SpriteRenderer spriteRenderer;

    Mob_Manager.Mob_Move_State Move_State;

    public IEnumerator Move()
    {
        move_speed = 1;
        Vector3 pos = Mob_Manager.inst.spawn_pos.position;
        Vector3 inverted_pos = new Vector3(-pos.x, pos.y - 7.8f, 0f);
        Move_State = Mob_Manager.Mob_Move_State.Down;

        while (true)
        {
            float speed = (move_speed * RuleManager.inst.minus_speed) * 0.05f;
            if (speed < 0) speed = 0;

            switch (Move_State)
            {
                case Mob_Manager.Mob_Move_State.Down:
                    transform.position += new Vector3(0, -speed, 0) * Time.deltaTime * 50;
                    spriteRenderer.flipX = false;

                    if (transform.position.y <= inverted_pos.y)
                    {
                        transform.position = new Vector3(pos.x, inverted_pos.y, 0);
                        Move_State = Mob_Manager.Mob_Move_State.Right;
                    } break;

                case Mob_Manager.Mob_Move_State.Right:
                    transform.position += new Vector3(speed, 0, 0) * Time.deltaTime * 50;
                    spriteRenderer.flipX = false;

                    if (transform.position.x >= inverted_pos.x)
                    {
                        transform.position = inverted_pos;
                        Move_State = Mob_Manager.Mob_Move_State.Up;
                    } break;

                case Mob_Manager.Mob_Move_State.Up:
                    transform.position += new Vector3(0, speed, 0) * Time.deltaTime * 50;
                    spriteRenderer.flipX = true;

                    if (transform.position.y >= pos.y)
                    {
                        transform.position = new Vector3(inverted_pos.x, pos.y, 0);
                        Move_State = Mob_Manager.Mob_Move_State.Left;
                    } break;

                case Mob_Manager.Mob_Move_State.Left:
                    transform.position += new Vector3(-speed, 0, 0) * Time.deltaTime * 50;
                    spriteRenderer.flipX = true;

                    if (transform.position.x <= pos.x)
                    {
                        transform.position = pos;
                        Move_State = Mob_Manager.Mob_Move_State.Down;
                    } break;
            }

            yield return new WaitForSeconds(0.01f);
        }
    }
}
