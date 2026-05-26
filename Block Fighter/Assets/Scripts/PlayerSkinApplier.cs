using UnityEngine;

public static class PlayerSkinApplier
{
    private const string SkinDatabaseResourcePath = "SkinDatabase";

    public static void ApplySelectedSkin(Animator animator)
    {
        if (animator == null)
        {
            return;
        }

        SkinDatabase database = Resources.Load<SkinDatabase>(SkinDatabaseResourcePath);
        if (database == null)
        {
            return;
        }

        SkinDefinition skin = database.GetSkin(PlayerSkinService.SelectedSkinId);
        if (skin == null || skin.Id == database.DefaultSkinId)
        {
            return;
        }

        AnimatorOverrideController overrideController = new AnimatorOverrideController(animator.runtimeAnimatorController);

        if (skin.IdleClip != null)
        {
            overrideController["playerIdle"] = skin.IdleClip;
        }

        if (skin.RunClip != null)
        {
            overrideController["playerRun"] = skin.RunClip;
        }

        animator.runtimeAnimatorController = overrideController;
    }
}
