//------------------------------------------------------------------------------
//      Copyright (c) Microsoft Corporation.  All rights reserved.
//------------------------------------------------------------------------------

using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Windows.Forms;

using Terrarium.Metal;

namespace Terrarium.Forms
{
	/// <summary>
	/// 
	/// </summary>
	public class TerrariumLed : Terrarium.Metal.MetalPanel
	{
		private System.Windows.Forms.ToolTip toolTip;

		/// <summary>
		///  Current state of the LED
		/// </summary>
		private LedStates state;
		/// <summary>
		///  Adds to the activity of the LED.  Active LEDs are in the 
		///  waiting state
		/// </summary>
		private int activityCount = 0;
		/// <summary>
		///  Total number of completed LED activity requests.
		/// </summary>
		private int numRequests = 0;
		/// <summary>
		///  The name of this LED used in the Tool Tip description
		/// </summary>
		private string ledName = "";

		/// <summary>
		/// 
		/// </summary>
		public TerrariumLed()
		{
			InitializeComponent();
			
			this.UseStyles = false;
			this.state = LedStates.Idle;
		}

		/// <summary>
		///  Name for this LED.
		/// </summary>
		[Browsable(true)]
		[Description("Set a LED name used in the tool tip")]
		public string LedName
		{
			get
			{
				return ledName;
			}

			set
			{
				ledName = value;
			}
		}

		/// <summary>
		///  Current state of the LED, Idle, Waiting or Failed
		///  See LedStates for more details or to add states.
		/// </summary>
		[Browsable(true)]
		public LedStates LedState
		{
			get 
			{
				return state;
			}
        
			set
			{
				if ( this.Handle == IntPtr.Zero )
				{
					SetLedStateSynchronous(value);
				}
				else
				{
					this.BeginInvoke(new SetLedStateSynchronousCallback(SetLedStateSynchronous), new object[] {value});
				}
			}
		}

		/// <summary>
		///  Asynchronous delegate for use with LedState property
		/// </summary>
		delegate void SetLedStateSynchronousCallback(LedStates newState);

		/// <summary>
		///  Delegate implementation for asynchronous LedState changing
		/// </summary>
		/// <param name="newState">The new state of the LED</param>
		public void SetLedStateSynchronous(LedStates newState)
		{
			if (newState == LedStates.Waiting)
			{
				numRequests++;
			}

			state = newState;

			this.UpdateGradient();

			SetToolTip();
			this.Invalidate();
			this.Update();
		}

		/// <summary>
		///  Adds to the activity counter for the LED putting it into the
		///  Waiting state.
		/// </summary>
		public void AddActivityCount()
		{
			this.BeginInvoke(new AddActivityCountSynchronousCallback(AddActivityCountSynchronous));
		}

		/// <summary>
		///  Asynchronous delegate for adding to the activity counter
		/// </summary>
		delegate void AddActivityCountSynchronousCallback();

		/// <summary>
		///  Delegate implementation for async activity adds
		/// </summary>
		void AddActivityCountSynchronous() 
		{
			activityCount++;
			LedState = LedStates.Waiting;
			SetToolTip();
		}

		/// <summary>
		///  Used to subtract from the activity count asynchronously
		/// </summary>
		public void RemoveActivityCount() 
		{
			this.BeginInvoke(new RemoveActivityCountSynchronousCallback(RemoveActivityCountSynchronous));
		}

		/// <summary>
		///  Asynchronous delegate for subtracting from the activity counter
		/// </summary>
		delegate void RemoveActivityCountSynchronousCallback();

		/// <summary>
		///  Delegate implementation for async activity subtractions
		/// </summary>
		void RemoveActivityCountSynchronous() 
		{
			activityCount--;
			if (activityCount <= 0)
			{
				activityCount = 0;
				LedState = LedStates.Idle;
			}
			SetToolTip();
		}

		/// <summary>
		///  Provides access to the current activity count for an LED
		/// </summary>
		[Browsable(false)]
		public int ActivityCount
		{
			get
			{
				return activityCount;
			}
		}

		/// <summary>
		///  Sets a detailed tool tip based on the led name, current state,
		///  and activity.
		/// </summary>
		private void SetToolTip()
		{
			string extraMessage = "";

			switch (LedState)
			{
				case LedStates.Idle:
					extraMessage = "";
					break;
				case LedStates.Waiting:
					extraMessage = "[Active:";
					if (activityCount > 0)
					{
						extraMessage += " " + activityCount.ToString() + " requests";
					}
					extraMessage += "]";
					break;
				case LedStates.Failed:
					extraMessage = "[Last Request Failed]";
					break;

			}

			if ( this.toolTip != null )
			{
				this.toolTip.SetToolTip(this, ledName + " " + extraMessage + " [Total Requests: " + numRequests + "]");
			}
		}

		#region Component Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.toolTip = new System.Windows.Forms.ToolTip();
		}
		#endregion

		/// <summary>
		///  Method for resetting the activity count and idle states for an LED
		/// </summary>
		private void Reset()
		{
			activityCount = 0;
			LedState = LedStates.Idle;
			SetToolTip();
		}

		/// <summary>
		/// Override this to update our gradient, then let the base class
		/// paint as normal.
		/// </summary>
		/// <param name="e"></param>
		protected override void OnPaint(PaintEventArgs e)
		{
			switch( this.state )
			{
				case LedStates.Idle:
					this.gradient = MetalStyleManager.Active.LedIdle;
					break;
				case LedStates.Waiting:
					this.gradient = MetalStyleManager.Active.LedWaiting;
					break;
				case LedStates.Failed:
					this.gradient = MetalStyleManager.Active.LedFailed;
					break;
			}
			base.OnPaint (e);
		}

		private void UpdateGradient()
		{
			switch( this.state )
			{
				case LedStates.Idle:
					this.gradient = MetalStyleManager.Active.LedIdle;
					break;
				case LedStates.Waiting:
					this.gradient = MetalStyleManager.Active.LedWaiting;
					break;
				case LedStates.Failed:
					this.gradient = MetalStyleManager.Active.LedFailed;
					break;
			}
		}

	}
}
