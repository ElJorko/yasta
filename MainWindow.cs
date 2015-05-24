using System;
using Gtk;

using Yasta;

public partial class MainWindow: Gtk.Window
{	

	protected int currentTime;
	protected int timeLeft;
	private float workHours;
	private float lunchTime;
	private float coffee1Time;
	private float coffee2Time;
	private float startupTime;



	bool runTimer = true;

	public MainWindow (): base (Gtk.WindowType.Toplevel)
	{
		Build ();

		// Initial time

		startStopButton.Label = "Stop";
		workHours = float.Parse(workTimeInputFieldText.Text);
		lunchTime = float.Parse(lunchTimeInputFieldText.Text);
		coffee1Time = float.Parse(coffee1TimeInputFieldText.Text);
		coffee2Time = float.Parse(coffee2TimeInputFieldText.Text);
		startupTime = float.Parse(startupTimeInputFieldText.Text);
		timeLeft = (int) (( workHours * 60f + coffee1Time + coffee2Time + lunchTime + startupTime)*60f);
		UpdateTimeText ();
		UpdateETF ();
		StartClock ();
	}
	
	protected void OnDeleteEvent (object sender, DeleteEventArgs a)
	{
		Application.Quit ();
		a.RetVal = true;
	}

	protected void OnOpen (object o, ButtonPressEventArgs args)
	{
		throw new NotImplementedException ();


	}

	protected void StartStopButtonPress (object o, ButtonPressEventArgs args)
	{
		throw new NotImplementedException ();
	}

	void StartClock ()
	{
		// Every second call `update_status' (1000 milliseconds)
		
		GLib.Timeout.Add (1000, new GLib.TimeoutHandler (update_status));
	}
	
	bool update_status ()
	{

//		timeText.Text = DateTime.Now.ToString ();
		timeLeft -= 1;
		UpdateTimeText ();

		CheckIfDone ();
		
		// returning true means that the timeout routine should be invoked
		// again after the timeout period expires.   Returning false would
		// terminate the timeout.
		
		return runTimer;
	}

	private void UpdateCurrentTime(){
		currentTime = DateTime.Now.Hour * 3600 + DateTime.Now.Minute * 60 + DateTime.Now.Second;
	}



	void UpdateTimeText(){
		int hoursLeft = timeLeft / 3600;
		int minutesLeft = timeLeft % 3600 / 60;
		int secondsLeft = (timeLeft % 3600 )% 60;
		timeText.Text = "" + hoursLeft.ToString().PadLeft(2,'0') + ":" + minutesLeft.ToString().PadLeft(2,'0') + ":" + secondsLeft.ToString().PadLeft(2,'0');
	}

	void UpdateETF(){
		UpdateCurrentTime ();
		int etfTime = currentTime + timeLeft;
		int hours = (etfTime / 3600) % 24;
		int minutes = etfTime % 3600 / 60;
		int seconds = (etfTime % 3600 )% 60;
		etfText.Text = "ETF: " + hours.ToString().PadLeft(2,'0') + ":" + minutes.ToString().PadLeft(2,'0') + ":" + seconds.ToString().PadLeft(2,'0');
	}

	private void CheckIfDone(){
		if (timeLeft <= 0) {
//			efxSource.clip = dayOverSound;			
//			//Play the clip.
//			efxSource.Play ();
			runTimer = false;
			Window freedomWindow = new Window(WindowType.Toplevel);
			freedomWindow.SetPosition(WindowPosition.Center);
			freedomWindow.Resize(200,200);
//			freedomWindow.Title = "Yasta!!";
			Label myLabel = new Label("Yasta !!");
			myLabel.ModifyFont(Pango.FontDescription.FromString("Monospace 30"));
			freedomWindow.Add (myLabel);
			freedomWindow.ShowAll();
			freedomWindow.Present();
			this.Present();
		}
	}

	protected void WorkTimeInputFieldCallback (object sender, EventArgs e)
	{
		float newTime = float.Parse (workTimeInputFieldText.Text);
		UpdateTimeAllocation(workHours*3600f, newTime*3600f);
		workHours = newTime;
//		Console.WriteLine ("olakase");

	}

	protected void LunchTimeInputFieldCallback (object o, TextInsertedArgs args)
	{
		float newTime = float.Parse (lunchTimeInputFieldText.Text);
		UpdateTimeAllocation(lunchTime*60f, newTime*60f);
		lunchTime = newTime;
	}

	protected void Coffee1TimeInputFieldCallback (object o, TextInsertedArgs args)
	{
		float newTime = float.Parse (coffee1TimeInputFieldText.Text);
		UpdateTimeAllocation(coffee1Time*60f, newTime*60f);
		coffee1Time = newTime;
	}

	protected void Coffee2TimeInputFieldCallback (object o, TextInsertedArgs args)
	{
		float newTime = float.Parse (coffee2TimeInputFieldText.Text);
		UpdateTimeAllocation(coffee2Time*60f, newTime*60f);
		coffee2Time = newTime;
	}

	protected void StartupTimeInputFieldCallback (object o, TextInsertedArgs args)
	{
		float newTime = float.Parse (startupTimeInputFieldText.Text);
		UpdateTimeAllocation(startupTime*60f, newTime*60f);
		startupTime = newTime;;
	}

	
	private void UpdateTimeAllocation(float oldTime, float newTime)
	{
		timeLeft = timeLeft + (int)  (newTime - oldTime);
		UpdateTimeText ();
		UpdateETF ();
	}

	protected void OnAbout (object sender, System.EventArgs e)
	{
		string[] authors = { "ElJorko" };
		string license = "TBD";
		
		AboutDialog dialog = new AboutDialog();
		
		dialog.ProgramName = "Yasta";
		dialog.Version = "0.1";
		dialog.Authors = authors;
		dialog.Copyright = "(C) 2015";
		dialog.License = license;
		
		dialog.TransientFor = this;
		
		dialog.Run();
		
		dialog.Destroy();
	}
	protected void OnWorkingHoursPlan (object sender, System.EventArgs e)
	{
		WorkingHoursPlanDialog dialog = new WorkingHoursPlanDialog();

		dialog.TransientFor = this;

		dialog.Run();

		dialog.Destroy();
	}
}
