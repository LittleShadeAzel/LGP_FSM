# LGP_FSM
Finite State Machine Package for Unity. It creates a Finite State Machine
inside a MonoBehaviour for you to control.

=CLASSES=

-FSMController-
This Class Handles all the State Processing and Changing. It has the
following Properties:
  - State         => Shows the active FSMState.
  - StateId       => Shows the active Id of a FSMState.
  - IsStateLocked => If TRUE the controller wont't change the active State.

Following Methods:
  - AddState(int stateId, FSMState state):void
  - SetState(int stateId, bool forceSameState, 
               [bool useReenter = false]):void

-FSMState- 
This Class represents a State. It works with delegates and call the defined
Methods inside the MonoBehaviour. A FSMState needs to be added to a 
FSMController in order to work. A State has the following delegates:
  - Enter   => Called when the state is entered for the first time.
  - Tick    => Called every time UpdateStateController/
               UpdateStateController Fixed from FSMController gets called
  - Exit    => Called when we exit a state.
  - Reenter => If enabled, the state will call this delagate, if the 
               Controller enters the same state again.
               
A state has also these following flags:
  - IsFixedUdpate   => Determines wheter the Tick delegate gets called
                       by the Controller in Normal or Fixed Update.
  - IsTransitioning => Determines wheter the State is currently
                       tranisitioning into another one.   


=HOW TO USE=

Step 1: Define your States and add FSMController to your Behaviour.
States can be acces via integers. Simply create integer constants to 
define your states. Like:

        // State Controller
        public FSMController fsm;
        
        // State IDs
        public const int STATE_NORMAL = 0;
        public const int STATE_DASH = 1;
        ...

Step 2: Instatiate FSMController and add the first states with your Methods

        // States
        fsm = new FSMController();
        
        // Add Normal State
        FSMState normalState = new FSMState(null, UpdateNormalState, null);
        normalState.IsFixedUpdate = true;
        StateController.AddState(STATE_NORMAL, normalState);

Step 3: Now Set your State to start

         // Set State
         StateController.SetState(STATE_NORMAL);

