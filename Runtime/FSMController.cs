using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LGP.FSM {
    /// <summary>
    /// Enabels the use of FSMStates inside a Behaviour.
    /// </summary>
    public class FSMController {

        #region Variables
        /// <summary>
        /// The monoBehaviour for this FSMController. It's used for Coroutines.
        /// </summary>
        public MonoBehaviour monoBehaviour;
        /// <summary>
        /// The manifest of the FSMController's FSMStates.
        /// </summary>
        public Dictionary<int, FSMState> states = new Dictionary<int, FSMState>();
        private int currentStateId = -1;
        private int previousStateId = -1;
		#endregion

		#region Property
        /// <summary>
        /// Gets the active FSMState.
        /// </summary>
		public FSMState State { get => (currentStateId >= 0 ? states[currentStateId] : null); }

        /// <summary>
        /// Gets the id of the active FSMState.
        /// </summary>
        public int StateId { get => currentStateId; }

        /// <summary>
        /// If true, the FSMController can't change the active FSMState.
        /// </summary>
        public bool IsStateLocked { get; set; }
		#endregion
		
        #region Constructor
		public FSMController() { }
        public FSMController(MonoBehaviour monoBehaviour, int initialState) {
            this.monoBehaviour = monoBehaviour;
            SetState(initialState);
        }
        #endregion

        #region Methods
        /// <summary>
        /// Adds a FSMState to the FSMController.
        /// </summary>
        public void AddState(int id, FSMState state) {
            id = Mathf.Max(id, 0);
            state.fsmController = this;
            state.id = id;
            states.Add(id, state);
        }

        /// <summary>
        /// Adds a FSMState to the FSMController.
        /// </summary>
        public void AddState(int id, OnStateEnterCallback enter, OnStateTickCallback tick, OnStateExitCallback exit) {
            AddState(id, new FSMState(enter, tick, exit));
        }

        /// <summary>
        /// Switches to a diffrent FSMState.
        /// </summary>
        /// <param name="id">The id of the FSMState.</param>
        /// <param name="forceSameState">Forces the FSMController to accept and procedure the same FSMState like normaly and doesnt ignore it.</param>
        /// <param name="useReenter">Determines whether the FSMController should use "Reenter" Method instead of the regular "Enter" Method when using the same State.</param>
        public virtual void SetState(int id, bool forceSameState = false, bool useReenter = false) {
            FSMState newState = states[id];

            // Handle state lock and empty state.
            if (newState == null || IsStateLocked) return;

            // Handle same State.
            if (id == currentStateId) {
                if (!forceSameState) return;
                if (useReenter) State.onStateReenter?.Invoke();
            }

            // Handle state Exiting.
            if (State != null) {
                State.SetTransition(true);
                State.onStateExit?.Invoke();
            }

            // Handle State Entering.
            previousStateId = currentStateId;
            currentStateId = id;
            if (id != currentStateId && !useReenter) State.onStateEnter?.Invoke();
            State.SetTransition(false);
        }

        /// <summary>
        /// Manually Updates the active FSMState for the regular Update Method.
        /// </summary>
        public void UpdateStateController() {
            if (State == null) return;
            if (State.onStateTick == null) return;
            if (State.IsFixedUpdate) return;
            if (!State.IsTransitioning) SetState(State.onStateTick.Invoke());
        }

        /// <summary>
        /// Manually Updates the active FSMState for the Fixed Update Method.
        /// </summary>
        public void UpdateStateControllerFixed() {
            if (State == null) return;
            if (State.onStateTick == null) return;
            if (!State.IsFixedUpdate) return;
            if (!State.IsTransitioning) SetState(State.onStateTick.Invoke());
        }
        #endregion
    }
}
