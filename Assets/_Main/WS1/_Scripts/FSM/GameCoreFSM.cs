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
			InitOnce,
			Init,
			Ready,
			Running,
			Pause,
			Win,
			Lose,
			End
		}

		public override void Initialize ()
		{
			CurrentState = new ReactiveProperty<State> (State.InitOnce);
			
			InitOnceOKTransition = CurrentState.Select (_ => _ == State.InitOnce).ToReactiveCommand ();
			InitOKTransition = CurrentState.Select (_ => _ == State.Init).ToReactiveCommand ();
			StartTransition = CurrentState.Select (_ => _ == State.Ready).ToReactiveCommand ();
			RestartTransition = CurrentState.Select (_ => _ == State.Ready || _ == State.Pause || _ == State.Win || _ == State.Lose).ToReactiveCommand ();
			PauseTransition = CurrentState.Select (_ => _ == State.Ready || _ == State.Running).ToReactiveCommand ();
			ResumeTransition = CurrentState.Select (_ => _ == State.Pause).ToReactiveCommand ();
			ResumeToReadyTransition = CurrentState.Select (_ => _ == State.Pause).ToReactiveCommand ();
			WinTransition = CurrentState.Select (_ => _ == State.Running).ToReactiveCommand ();
			LoseTransition = CurrentState.Select (_ => _ == State.Running).ToReactiveCommand ();
			FinishTransition = CurrentState.Select (_ => _ == State.Pause || _ == State.Win || _ == State.Lose).ToReactiveCommand ();

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
			
			StartTransition.Subscribe (_ => {
				if (CurrentState.Value == State.Ready)
					CurrentState.Value = State.Running;
			}).AddTo (this.baseAttachDisposables);
			
			RestartTransition.Subscribe (_ => {
				if (CurrentState.Value == State.Ready)
					CurrentState.Value = State.Init;
				else if (CurrentState.Value == State.Pause)
					CurrentState.Value = State.Init;
				else if (CurrentState.Value == State.Win)
					CurrentState.Value = State.Init;
				else if (CurrentState.Value == State.Lose)
					CurrentState.Value = State.Init;
			}).AddTo (this.baseAttachDisposables);
			
			PauseTransition.Subscribe (_ => {
				if (CurrentState.Value == State.Ready)
					CurrentState.Value = State.Pause;
				else if (CurrentState.Value == State.Running)
					CurrentState.Value = State.Pause;
			}).AddTo (this.baseAttachDisposables);
			
			ResumeTransition.Subscribe (_ => {
				if (CurrentState.Value == State.Pause)
					CurrentState.Value = State.Running;
			}).AddTo (this.baseAttachDisposables);
			
			ResumeToReadyTransition.Subscribe (_ => {
				if (CurrentState.Value == State.Pause)
					CurrentState.Value = State.Ready;
			}).AddTo (this.baseAttachDisposables);
			
			WinTransition.Subscribe (_ => {
				if (CurrentState.Value == State.Running)
					CurrentState.Value = State.Win;
			}).AddTo (this.baseAttachDisposables);
			
			LoseTransition.Subscribe (_ => {
				if (CurrentState.Value == State.Running)
					CurrentState.Value = State.Lose;
			}).AddTo (this.baseAttachDisposables);
			
			FinishTransition.Subscribe (_ => {
				if (CurrentState.Value == State.Pause)
					CurrentState.Value = State.End;
				else if (CurrentState.Value == State.Win)
					CurrentState.Value = State.End;
				else if (CurrentState.Value == State.Lose)
					CurrentState.Value = State.End;
			}).AddTo (this.baseAttachDisposables);
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