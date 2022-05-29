namespace Spire.Abilities;

public partial class PlayerAbility : Ability
{
	public virtual PlayerAbilityType Type { get; set; } = PlayerAbilityType.Standard;
	public BaseCharacter Character => Entity as BaseCharacter;
}
