using Domain.Gestures.Recognition.ErrorEvaluation;

namespace Domain.Gestures.Recognition;

public class GesturePerformingAssessment
{
	private readonly IDeltaErrorEvaluator _deltaErrorEvaluator;
	private readonly IFinalErrorEvaluator _finalErrorEvaluator;

	private State _state = State.InProgress;

	public Gesture Gesture { get; }
	public float Error { get; private set; }

	public GesturePerformingAssessment(Gesture gesture, IDeltaErrorEvaluator deltaErrorEvaluator, IFinalErrorEvaluator finalErrorEvaluator)
	{
		Assert(gesture != null, "Gesture must not be null");
		Assert(deltaErrorEvaluator != null, "Delta error evaluator must not be null");
		Assert(finalErrorEvaluator != null, "Final error evaluator must not be null");

		Gesture = gesture;
		_deltaErrorEvaluator = deltaErrorEvaluator;
		_finalErrorEvaluator = finalErrorEvaluator;
	}

	public void Advance(ManipulationDelta manipulationDelta)
	{
		Assert(_state == State.InProgress, "Can only advance an assessment that has not finished");

		var deltaError = _deltaErrorEvaluator.Evaluate(Gesture, manipulationDelta);
		Error += deltaError;
	}

	public void Finish(ManipulationState finalManipulationState)
	{
		Assert(_state == State.Finished, "Cannot finish an assessment that has never begun");

		var finalError = _finalErrorEvaluator.Evaluate(Gesture, Error, finalManipulationState);
		Error = finalError;

		_state = State.Finished;
	}

	private enum State
	{
		InProgress,
		Finished
	}
}