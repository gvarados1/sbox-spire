namespace Spire.Abilities;

public partial class PlayerAbility : Ability
{
	public virtual PlayerAbilityType Type { get; set; } = PlayerAbilityType.Standard;
	public BaseCharacter Character => Entity as BaseCharacter;

	public static InputButton GetInputButtonFromSlot( int slotIndex )
	{
		return slotIndex switch
		{
			0 => InputButton.Slot1,
			1 => InputButton.Slot2,
			2 => InputButton.Slot3,
			_ => InputButton.Slot1
		};
	}
}
