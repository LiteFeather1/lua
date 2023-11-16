public class CardUIDropContainerRecycle : CardUIDropContainer
{
    protected override void UseCard(CardUIPowerUp card)
    {
        card.ReturnToPile();
    }
}
