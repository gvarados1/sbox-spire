namespace Spire.Buffs;

public class BaseHealingBuff : Buff
{
	public override string Name => "Minor Healing";
	public override string Description => "";
	public override float Duration => 10f;
	public override string IconPath => "Some icon path.";
	public override float TickInterval => 1f;

	//
	public virtual float HealAmountPerTick => 10f;

	public override void OnTick( BaseCharacter character )
	{
		base.OnTick( character );

		character.Health += HealAmountPerTick;
		character.Health = character.Health.Clamp( 0f, character.MaxHealth );
	}

	public override void OnDestroy( BaseCharacter character )
	{
		base.OnDestroy( character );
	}
}
