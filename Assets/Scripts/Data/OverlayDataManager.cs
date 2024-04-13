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

    public void HideOverlay()
    {
        overlay.Update(OverlayContent.NONE);
    }
}
