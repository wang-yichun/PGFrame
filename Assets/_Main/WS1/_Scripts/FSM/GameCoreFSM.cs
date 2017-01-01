using System.Linq;
using System;
using System.Collections;
using System.Collections.Generic;

namespace WS1
{

	using PGFrame;
	using Newtonsoft.Json;
	using Newtonsoft.Json.Linq;
	using UniRx;

	public class GameCoreFSM : FSMBase<GameCoreFSM.State>
	{
		public enum State
		{
			Running,
			Win,
			Lose,
			Pause,
			Init,
			Ready,
			InitOnce,
			End
		}

		public override void Initialize ()
		{
			CurrentState = new ReactiveProperty<State> (State.InitOnce);

			InitOnceOKTransition = CurrentState.Select (_ => _ == State.InitOnce).ToReactiveCommand ();
			InitOKTransition = CurrentState.Select (_ => _ == State.Init).ToReactiveCommand ();
			StartTransition = CurrentState.Select (_ => _ == State.Ready).ToReactiveCommand ();
			RestartTransition = CurrentState.Select (_ => _ == State.Ready || _ == State.Pause || _ == State.Lose || _ == State.Win).ToReactiveCommand ();
			PauseTransition = CurrentState.Select (_ => _ == State.Running).ToReactiveCommand ();
			ResumeTransition = CurrentState.Select (_ => _ == State.Pause).ToReactiveCommand ();
			ResumeToReadyTransition = CurrentState.Select (_ => _ == State.Pause).ToReactiveCommand ();
			WinTransition = CurrentState.Select (_ => _ == State.Running).ToReactiveCommand ();
			LoseTransition = CurrentState.Select (_ => _ == State.Running).ToReactiveCommand ();
			FinishTransition = CurrentState.Select (_ => _ == State.Pause || _ == State.Lose || _ == State.Win).ToReactiveCommand ();

			base.Initialize ();
		}

		public override void Attach ()
		{
			base.Attach ();

			InitOnceOKTransition.Subscribe (_ => {
				if (CurrentState.Value == State.InitOnce)
					CurrentState.Value = State.Init;
			}).AddTo (this.baseAttachDisposables);

			InitOKTransition.Subscribe (_ => {
				if (CurrentState.Value == State.Init)
					CurrentState.Value = State.Ready;
			}).AddTo (this.baseAttachDisposables);

			// ...
		}

		public override void Detach ()
		{
			base.Detach ();
		}

		public ReactiveCommand InitOnceOKTransition;
		public ReactiveCommand InitOKTransition;
		public ReactiveCommand StartTransition;
		public ReactiveCommand RestartTransition;
		public ReactiveCommand PauseTransition;
		public ReactiveCommand ResumeTransition;
		public ReactiveCommand ResumeToReadyTransition;
		public ReactiveCommand WinTransition;
		public ReactiveCommand LoseTransition;
		public ReactiveCommand FinishTransition;
	}

}