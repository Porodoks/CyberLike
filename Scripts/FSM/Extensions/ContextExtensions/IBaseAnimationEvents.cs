using System;

public interface IBaseAnimationEvents
{
    public event Action OnAnimationStart;
    public event Action OnAnimationEnd;
    public void OnAnimationStartHandler();
    public void OnAnimationEndHandler();
}
