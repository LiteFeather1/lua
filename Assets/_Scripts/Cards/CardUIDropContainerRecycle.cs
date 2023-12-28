
namespace Lua.Cards
{
    public class CardUIDropContainerRecycle : CardUIDropContainer
    {
        public int CardsRecycled { get; private set; }

        protected override void UseCard(CardUIPowerUp card)
        {
            CardsRecycled++;
            card.ReturnToPile();
        }
    }
}
