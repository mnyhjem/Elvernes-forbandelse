namespace ElvenCurse.Core.Model
{
    public class HunterNpc : Npc
    {
        public HunterNpc():base(5)
        {
            
        }

        public override void Attack(Character characterToAttack)
        {
            throw new System.NotImplementedException();
        }
    }
}