namespace Spire.UI;

public partial class BaseNameplate : Panel
{
	protected virtual bool StayOnScreen => false;
	protected virtual float Health => Character?.Health ?? 100f;
	protected virtual float MaxHealth => Character?.MaxHealth ?? 100f;
	protected virtual float HealthFraction => Health / MaxHealth;

	protected virtual Vector2 SafetyBounds => new( 0.1f, 0.1f );

	public BaseCharacter Character { get; protected set; }

	public virtual string NameplateName => "Nameplate";

	public BaseNameplate( BaseCharacter character )
	{
		Character = character;
	}

	/// <summary>
	/// Set up nameplate position. 
	/// </summary>
	/// <returns>bool for if the nameplate is visible on screen</returns>
	protected bool SetupNameplatePosition()
	{
		var screenpos = GetScreenPoint();

		var cachedX = screenpos.x;
		var cachedY = screenpos.y;

		if ( StayOnScreen )
		{
			var safetyX = SafetyBounds.x;
			var safetyY = SafetyBounds.y;

			screenpos.x = screenpos.x.Clamp( safetyX, 1 - safetyX );
			screenpos.y = screenpos.y.Clamp( safetyY, 1 - safetyY );
		}

		Style.Left = Length.Fraction( screenpos.x );
		Style.Top = Length.Fraction( screenpos.y );

		return cachedX < 0 || cachedX > 1 || cachedY < 0 || cachedY > 1;
	}

	public virtual Vector3 GetWorldPoint()
	{
		return (Character?.Position ?? Vector3.Zero) + Vector3.Up * 100f;
	}

	public virtual Vector3 GetScreenPoint()
	{
		var worldPoint = GetWorldPoint();
		var screenPoint = worldPoint.ToScreen();

		return screenPoint;
	}

	public virtual void Update()
	{
		SetupNameplatePosition();
	}
}
