using System;
using System.Drawing;
using System.Windows.Forms;
using System.Media;

public class TimerApp : Form
{
	private Label timerLabel;
	private ComboBox minutesComboBox;
	private Button startButton;
	private Button stopButton;
	private System.Windows.Forms.Timer countdownTimer;
	private int totalSeconds;
	private int initialTotalSeconds;

	private struct TimeOption
	{
		public string Display { get; set; }
		public int Minutes { get; set; }
		public override string ToString() => Display;
	}

	public TimerApp()
	{
		InitializeComponents();
	}

	private void InitializeComponents()
	{
		InitializeForm();
		InitializeTimerLabel();
		InitializeMinutesComboBox();
		InitializeButtons();
		InitializeTimer();

		UpdateDisplay();
		minutesComboBox.SelectedIndexChanged += (s, e) => UpdateDisplay();
	}

	private void InitializeForm()
	{
		this.Text = "Elegant Timer";
		this.Size = new Size(420, 320);
		this.FormBorderStyle = FormBorderStyle.FixedSingle;
		this.MaximizeBox = false;
		this.StartPosition = FormStartPosition.CenterScreen;
		this.BackColor = ColorTranslator.FromHtml("#282c34");
		this.ForeColor = ColorTranslator.FromHtml("#abb2bf");
		this.Font = new Font("Segoe UI", 11F, FontStyle.Regular, GraphicsUnit.Point, ((byte)(0)));
	}

	private void InitializeTimerLabel()
	{
		timerLabel = new Label();
		timerLabel.Text = "00:00:00";
		timerLabel.Font = new Font("Segoe UI", 48F, FontStyle.Bold);
		timerLabel.TextAlign = ContentAlignment.MiddleCenter;
		timerLabel.AutoSize = false;
		timerLabel.Location = new Point(0, 30);
		timerLabel.Size = new Size(this.ClientSize.Width, 80);
		timerLabel.ForeColor = ColorTranslator.FromHtml("#61afef");
		this.Controls.Add(timerLabel);
	}

	private void InitializeMinutesComboBox()
	{
		minutesComboBox = new ComboBox();
		minutesComboBox.DropDownStyle = ComboBoxStyle.DropDownList;
		minutesComboBox.Width = 220;
		minutesComboBox.Location = new Point((this.ClientSize.Width - minutesComboBox.Width) / 2, 150);
		minutesComboBox.BackColor = ColorTranslator.FromHtml("#3a3f4b");
		minutesComboBox.ForeColor = Color.FromKnownColor(KnownColor.White);
		minutesComboBox.FlatStyle = FlatStyle.Flat;
		minutesComboBox.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, ((byte)(0)));

		minutesComboBox.Items.Add(new TimeOption { Display = "10 Minutes", Minutes = 10 });
		minutesComboBox.Items.Add(new TimeOption { Display = "20 Minutes", Minutes = 20 });
		minutesComboBox.Items.Add(new TimeOption { Display = "30 Minutes", Minutes = 30 });
		minutesComboBox.Items.Add(new TimeOption { Display = "60 Minutes (1 Hour)", Minutes = 60 });
		minutesComboBox.Items.Add(new TimeOption { Display = "120 Minutes (2 Hours)", Minutes = 120 });
		minutesComboBox.Items.Add(new TimeOption { Display = "480 Minutes (8 Hours)", Minutes = 480 });
		minutesComboBox.Items.Add(new TimeOption { Display = "1440 Minutes (24 Hours)", Minutes = 1440 });
		minutesComboBox.SelectedIndex = 0;
		this.Controls.Add(minutesComboBox);
	}

	private void InitializeButtons()
	{
		startButton = new Button();
		startButton.Text = "Start Timer";
		startButton.Location = new Point((this.ClientSize.Width - 180) / 2, 210);
		startButton.Size = new Size(180, 45);
		startButton.BackColor = ColorTranslator.FromHtml("#98c379");
		startButton.ForeColor = Color.Black;
		startButton.FlatAppearance.BorderSize = 0;
		startButton.FlatStyle = FlatStyle.Flat;
		startButton.Font = new Font("Segoe UI", 14F, FontStyle.Bold, GraphicsUnit.Point, ((byte)(0)));
		startButton.Click += new EventHandler(StartButton_Click);
		this.Controls.Add(startButton);

		stopButton = new Button();
		stopButton.Text = "Stop & Reset";
		stopButton.Location = new Point((this.ClientSize.Width - 180) / 2, 210);
		stopButton.Size = new Size(180, 45);
		stopButton.BackColor = ColorTranslator.FromHtml("#e06c75");
		stopButton.ForeColor = Color.Black;
		stopButton.FlatAppearance.BorderSize = 0;
		stopButton.FlatStyle = FlatStyle.Flat;
		stopButton.Font = new Font("Segoe UI", 14F, FontStyle.Bold, GraphicsUnit.Point, ((byte)(0)));
		stopButton.Click += new EventHandler(StopButton_Click);
		stopButton.Visible = false;
		this.Controls.Add(stopButton);
	}

	private void InitializeTimer()
	{
		countdownTimer = new System.Windows.Forms.Timer();
		countdownTimer.Interval = 1000;
		countdownTimer.Tick += new EventHandler(CountdownTimer_Tick);
	}

	private void UpdateDisplay()
	{
		if (!countdownTimer.Enabled)
		{
			TimeOption selected = (TimeOption)minutesComboBox.SelectedItem;
			totalSeconds = selected.Minutes * 60;
			timerLabel.Text = FormatTime(totalSeconds);
		}
	}

	private string FormatTime(int seconds)
	{
		TimeSpan ts = TimeSpan.FromSeconds(seconds);
		return $"{ts.Hours:D2}:{ts.Minutes:D2}:{ts.Seconds:D2}";
	}

	private void StartButton_Click(object sender, EventArgs e)
	{
		if (totalSeconds > 0)
		{
			initialTotalSeconds = totalSeconds;
			countdownTimer.Start();

			startButton.Visible = false;
			stopButton.Visible = true;
			minutesComboBox.Enabled = false;
		}
	}

	private void StopButton_Click(object sender, EventArgs e)
	{
		countdownTimer.Stop();
		ResetControls();
		UpdateDisplay();
	}

	private void CountdownTimer_Tick(object sender, EventArgs e)
	{
		if (totalSeconds > 0)
		{
			totalSeconds--;
			timerLabel.Text = FormatTime(totalSeconds);
		}
		else
		{
			countdownTimer.Stop();
			ResetControls();
			TimerFinished();
		}
	}

	private void TimerFinished()
	{
		SystemSounds.Exclamation.Play();
		this.TopMost = true;
		this.Activate();

		MessageBox.Show(this, "Time's up! Your timer has finished.", "Timer Finished!", MessageBoxButtons.OK, MessageBoxIcon.Information);

		this.TopMost = false;
	}

	private void ResetControls()
	{
		startButton.Visible = true;
		stopButton.Visible = false;
		minutesComboBox.Enabled = true;
	}
}