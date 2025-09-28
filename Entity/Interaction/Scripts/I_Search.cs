using Sentience;
using UnityEngine;

namespace MindTheatre
{
    [System.Serializable]
    [CreateAssetMenu(fileName = "I_Search", menuName = "Scaerth/Interaction/Search")]
    public class I_Search : EntityInteraction
    {
        protected override bool IsPossible(Body self, EntityData interactor, EntityData target)
        {
            Inventory inv = self.Data.Get<Inventory>();
            if (inv == null) return false;
            return true;
        }

        protected override bool TryExecute(Body self, EntityData interactor, EntityData target)
        {
            Body body = interactor.Get<Body>();
            if (body.Entity is Player player)
            {
                Hud.Instance.InventoryPanel.Show(true, self.Data);
                return true;
            }
            return false;
        }
    }
}