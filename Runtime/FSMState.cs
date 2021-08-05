using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LGP.FSM {

    #region Delegates
    public delegate void OnStateEnterCallback();
    public delegate int OnStateTickCallback();
    public delegate void OnStateExitCallback();
    public delegate void OnStateReenterCallback();
    #endregion

    /// <summary>
    /// A FSMState used by the FSMController.
    /// </summary>
    public class FSMState {
        #region Variables
        public int id;
        public FSMController fsmController;
        public OnStateEnterCallback onStateEnter;
        public OnStateTickCallback onStateTick;
        public OnStateExitCallback onStateExit;
        public OnStateReenterCallback onStateReenter;
        private bool isTransitioning;
        /// <summary>
        /// Determines whether or not the state is going to be updated in the Fixed Update or not.
        /// </summary>
        public bool isFixedUpdate = false;
		#endregion

		#region Properties
        /// <summary>
        /// It shows if the FSMState is transitioning into another FSMState. It's used by the FSMController to determine when a FSMState is done and shouldn't be updated anymore.
        /// </summary>
        public bool IsTransitioning { get => isTransitioning; }
		#endregion

		#region Contructors
		public FSMState() { }
        public FSMState(OnStateEnterCallback enter, OnStateTickCallback tick, OnStateExitCallback exit, OnStateReenterCallback reenter = null) {
            onStateEnter = enter;
            onStateTick = tick;
            onStateExit = exit;
            onStateReenter = reenter;
        }
        #endregion

        #region Methods
        /// <summary>
        /// Sets the flag for the FSMState to end and transition into another one.
        /// </summary>
        /// <param name="value"></param>
        public void SetTransitioning(bool value) {
            isTransitioning = value;
		}
		#endregion
	}
}

