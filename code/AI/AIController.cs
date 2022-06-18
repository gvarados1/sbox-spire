using Spire.AI.Navigation;

namespace Spire.AI;

public partial class AIController : BaseNetworkable
{
	[Net, Predicted]
	public Entity Agent { get; protected set; }

	[Net, Predicted]
	protected Navigator Navigator { get; set; }

	public AIController()
	{
	}

	public void SetAgent( Entity agent )
	{
		Agent = agent;
	}

	public virtual void Tick()
	{
		Navigator?.Tick( Agent.Position );
	}
}
