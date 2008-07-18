using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using Terrarium.Glass;

namespace Terrarium.Forms
{
    /// <summary>
    /// 
    /// </summary>
    public class TerrariumLed : GlassButton
    {
        private readonly GlassGradient _failedGradient = new GlassGradient(Color.FromArgb(255, 128, 128),
                                                                           Color.FromArgb(128, 0, 0));

        private readonly GlassGradient _idleGradient = new GlassGradient(Color.FromArgb(128, 255, 128),
                                                                         Color.FromArgb(0, 96, 0));

        private readonly GlassGradient _waitingGradient = new GlassGradient(Color.FromArgb(255, 255, 128),
                                                                            Color.FromArgb(128, 128, 0));

        /// <summary>
        ///  Adds to the activity of the LED.  Active LEDs are in the 
        ///  waiting state
        /// </summary>
        private int _activityCount;

        /// <summary>
        ///  Current state of the LED
        /// </summary>
        private LedStates _currentLedState;

        /// <summary>
        ///  The name of this LED used in the Tool Tip description
        /// </summary>
        private string _ledName = "";

        /// <summary>
        ///  Total number of completed LED activity requests.
        /// </summary>
        private int _numRequests;

        private ToolTip _toolTip;

        /// <summary>
        /// 
        /// </summary>
        public TerrariumLed()
        {
            InitializeComponent();
            _currentLedState = LedStates.Waiting;
            NormalGradient = _waitingGradient;
        }

        /// <summary>
        ///  Name for this LED.
        /// </summary>
        [Browsable(true)]
        [Description("Set a LED name used in the tool tip")]
        public string LedName
        {
            get { return _ledName; }
            set { _ledName = value; }
        }

        /// <summary>
        ///  Current state of the LED, Idle, Waiting or Failed
        ///  See LedStates for more details or to add states.
        /// </summary>
        [Browsable(true)]
        public LedStates LedState
        {
            get { return _currentLedState; }

            set
            {
                if (Created == false)
                {
                    SetLedStateSynchronous(value);
                }
                else
                {
                    BeginInvoke(new SetLedStateSynchronousCallback(SetLedStateSynchronous), new object[] {value});
                }
            }
        }

        /// <summary>
        ///  Provides access to the current activity count for an LED
        /// </summary>
        [Browsable(false)]
        public int ActivityCount
        {
            get { return _activityCount; }
        }

        /// <summary>
        ///  Delegate implementation for asynchronous LedState changing
        /// </summary>
        /// <param name="newState">The new state of the LED</param>
        public void SetLedStateSynchronous(LedStates newState)
        {
            if (newState == LedStates.Waiting)
            {
                _numRequests++;
            }

            _currentLedState = newState;

            updateGradient();

            setToolTip();
            Invalidate();
            Update();
        }

        /// <summary>
        ///  Adds to the activity counter for the LED putting it into the
        ///  Waiting state.
        /// </summary>
        public void AddActivityCount()
        {
            BeginInvoke(new AddActivityCountSynchronousCallback(addActivityCountSynchronous));
        }

        /// <summary>
        ///  Delegate implementation for async activity adds
        /// </summary>
        private void addActivityCountSynchronous()
        {
            _activityCount++;
            LedState = LedStates.Waiting;
            setToolTip();
        }

        /// <summary>
        ///  Used to subtract from the activity count asynchronously
        /// </summary>
        public void RemoveActivityCount()
        {
            BeginInvoke(new RemoveActivityCountSynchronousCallback(removeActivityCountSynchronous));
        }

        /// <summary>
        ///  Delegate implementation for async activity subtractions
        /// </summary>
        private void removeActivityCountSynchronous()
        {
            _activityCount--;
            if (_activityCount <= 0)
            {
                _activityCount = 0;
                LedState = LedStates.Idle;
            }
            setToolTip();
        }

        /// <summary>
        ///  Sets a detailed tool tip based on the led name, current state,
        ///  and activity.
        /// </summary>
        private void setToolTip()
        {
            string extraMessage = "";

            switch (LedState)
            {
                case LedStates.Idle:
                    extraMessage = "";
                    break;
                case LedStates.Waiting:
                    extraMessage = "[Active:";
                    if (_activityCount > 0)
                    {
                        extraMessage += " " + _activityCount + " requests";
                    }
                    extraMessage += "]";
                    break;
                case LedStates.Failed:
                    extraMessage = "[Last Request Failed]";
                    break;
            }

            if (_toolTip != null)
            {
                _toolTip.SetToolTip(this, _ledName + " " + extraMessage + " [Total Requests: " + _numRequests + "]");
            }
        }

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            _toolTip = new ToolTip();
        }

        private void updateGradient()
        {
            switch (_currentLedState)
            {
                case LedStates.Idle:
                    DisabledGradient = _idleGradient;
                    break;
                case LedStates.Waiting:
                    DisabledGradient = _waitingGradient;
                    break;
                case LedStates.Failed:
                    DisabledGradient = _failedGradient;
                    break;
            }
        }

        /// <summary>
        ///  Asynchronous delegate for use with LedState property
        /// </summary>
        private delegate void SetLedStateSynchronousCallback(LedStates newState);

        /// <summary>
        ///  Asynchronous delegate for adding to the activity counter
        /// </summary>
        private delegate void AddActivityCountSynchronousCallback();

        /// <summary>
        ///  Asynchronous delegate for subtracting from the activity counter
        /// </summary>
        private delegate void RemoveActivityCountSynchronousCallback();
    }
}