using Game;


public class OverlayDataManager
{
    private readonly Publisher<OverlayContent> overlay = new(OverlayContent.NONE);

    public void Init()
    {
        overlay.Update(OverlayContent.NONE);
    }

    public Subscriber<OverlayContent> CreateLastCheckPointPositionSubscriber()
    {
        return overlay.CreateSubscriber();
    }

    public void ShowGameOver()
    {
        overlay.Update(OverlayContent.GAME_OVER);
    }

    public void ShowInGameMenu()
    {
        overlay.Update(OverlayContent.IN_GAME_MENU);
    }

    public void HideOverlay()
    {
        overlay.Update(OverlayContent.NONE);
    }

    public OverlayContent GetActiveOverlay()
    {
        return overlay.currentValue;
    }
}
