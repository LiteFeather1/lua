using UnityEngine;

public class ProjectileMoon : Projectile
{
    protected override void Bounce(Collider2D collision, bool isScreen)
    {
        if (isScreen && collision.transform.localPosition.y < transform.position.y)
            Deactivate();
        else
            base.Bounce(collision, isScreen);
    }
}
