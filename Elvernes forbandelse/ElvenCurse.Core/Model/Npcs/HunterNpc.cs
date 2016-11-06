namespace ElvenCurse.Core.Model.Npcs
{
    public class HunterNpc : Npc
    {
        public HunterNpc():base(5, 5, Npctype.Hunter)
        {
            
        }

        public override void Attack(Character characterToAttack)
        {
            throw new System.NotImplementedException();
        }
    }
}