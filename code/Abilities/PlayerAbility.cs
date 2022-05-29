namespace Spire.Abilities;

public partial class PlayerAbility : Ability
{
	public PlayerAbilityType Type { get; set; } = PlayerAbilityType.Standard;

	public new BaseCharacter Entity => base.Entity as BaseCharacter;
}
