using DG.Tweening;

namespace ClerkShadow.Data
{
    public static class Constants 
    {
        public const Ease DoTweenDefaultEase = Ease.InSine;

        public static class AnimationState
        {
            public const string Idle = "Idle";
            public const string Jump = "Jump";
            public const string Run = "IsRunning";
        }
    }
}
