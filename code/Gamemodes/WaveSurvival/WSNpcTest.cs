using Spire.AI.Navigation;

namespace Spire.WaveSurvival;

public partial class WSNpcTest : BaseCharacter
{
	public override void Respawn()
	{
		base.Respawn();

		AIController = new WSAIController();
		AIController.SetAgent( this );
	}

	[Event.Tick.Server]
	protected void ServerTick()
	{
		AIController?.Tick();
	}
}
