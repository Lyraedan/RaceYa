using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rock : MonoBehaviour
{

    public Animator anim;

    public void BeginRock() => anim.SetBool("Rocking", true);

    public void EndRock() => anim.SetBool("Rocking", false);
}
