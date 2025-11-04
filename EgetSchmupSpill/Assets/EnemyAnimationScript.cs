using UnityEngine;

public class EnemyAnimationScript : MonoBehaviour
{
    private Animator anim;

    void Start()
    {
        anim = GetComponent<Animator>();
    }

    public void PauseAnimation()
    {
        anim.speed = 0f;  //  Pauses animation
    }

    public void ResumeAnimation()
    {
        anim.speed = 1f;  //  Resumes animation
    }
}
