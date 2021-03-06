﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Stareater.AppData;
using Stareater.Localization;
using Stareater.Controllers;
using Stareater.Controllers.Views;
using Stareater.Galaxy;
using Stareater.GuiUtils;
using Stareater.Utils.NumberFormatters;

namespace Stareater.GUI
{
	partial class FormNewGame : Form
	{
		private NewGameController controller;

		private int customStartIndex;

		public FormNewGame()
		{
			InitializeComponent();
			
			setLanguage();
		}

		public void Initialize()
		{
			if (NewGameController.CanCreateGame) {
				controller = new NewGameController();
				updatePlayerViews();

				foreach (var start in MapAssets.Starts)
					setupStartSelector.Items.Add(new Tag<StartingConditions>(start, start.Name));
				this.customStartIndex = setupStartSelector.Items.Count;
				setupStartSelector.Items.Add(new Tag<StartingConditions>(controller.CustomStart, SettingsWinforms.Get.Language["StartingConditions"][NewGameController.CustomStartNameKey].Text()));
				setupStartSelector.Items.Add(new Tag<StartingConditions>(null, SettingsWinforms.Get.Language["StartingConditions"]["customize"].Text()));

				if (NewGameController.LastStartingCondition != null)					
					setupStartSelector.SelectedIndex = setupStartSelector.Items.Count - 1;
				else
					setupStartSelector.SelectedItem = new Tag<StartingConditions>(NewGameController.DefaultStartingCondition, null);
				updateMapDescription();
			}
			else
				DialogResult = System.Windows.Forms.DialogResult.Cancel;
		}

		public void CreateGame(GameController gameController)
		{
			gameController.CreateGame(controller);
		}

		private void setLanguage()
		{
			Context context = SettingsWinforms.Get.Language["FormNewGame"];

			this.Font = SettingsWinforms.Get.FormFont;
			this.Text = context["FormTitle"].Text();

			setupPlayersButton.Text = context["setupPlayersButton"].Text();
			setupMapButton.Text = context["setupMapButton"].Text();
			startButton.Text = context["startButton"].Text();
		}

		protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
		{
			if (keyData == Keys.Escape) 
				this.Close();
			return base.ProcessCmdKey(ref msg, keyData);
		}
		
		private void updatePlayerViews()
		{
			var players = controller.PlayerList;
			
			playerViewsLayout.Controls.Clear();
			while (playerViewsLayout.Controls.Count < players.Count)
				playerViewsLayout.Controls.Add(new NewGamePlayerView());

			for (int i = 0; i < players.Count; i++)
				(playerViewsLayout.Controls[i] as NewGamePlayerView).SetData(players[i]);
		}

		private void updateMapDescription()
		{
			mapDescription.Text = controller.StarPositioner.Description + Environment.NewLine +
				controller.StarConnector.Description + Environment.NewLine +
				controller.StarPopulator.Description;
		}

		private void setupPlayersButton_Click(object sender, EventArgs e)
		{
			using (var playersForm = new FormSetupPlayers(controller)) {
				playersForm.ShowDialog();
				updatePlayerViews();
			}
		}

		private void setupStartSelector_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (setupStartSelector.SelectedItem == null)
				return;

			StartingConditions start = (setupStartSelector.SelectedItem as Tag<StartingConditions>).Value;
			if (start != null) {
				StringBuilder sb = new StringBuilder();
				var formatter = new ThousandsFormatter(start.Population, start.Infrastructure);

				sb.AppendLine("Colonies: " + start.Colonies);
				sb.AppendLine("Population: " + formatter.Format(start.Population));
				sb.Append("Infrastructure: " + formatter.Format(start.Infrastructure));

				startingDescription.Text = sb.ToString();
				controller.SelectedStart = start;
			}
			else
				using (var form = new FormStartingConditions()) {
					form.Initialize(controller.CustomStart);
					if (form.ShowDialog() == DialogResult.OK && form.IsValid) {
						controller.CustomStart = form.GetResult();
						setupStartSelector.Items[customStartIndex] = new Tag<StartingConditions>(controller.CustomStart, controller.CustomStart.Name);
						setupStartSelector.SelectedIndex = customStartIndex;
						controller.SelectedStart = controller.CustomStart;
					}
				}
		}

		private void setupMapButton_Click(object sender, EventArgs e)
		{
			using (var form = new FormSetupMap()) {
				form.Initialize(controller);
				form.ShowDialog();
				updateMapDescription();
			}
		}

		private void startButton_Click(object sender, EventArgs e)
		{
			DialogResult = DialogResult.OK;
		}
	}
}
