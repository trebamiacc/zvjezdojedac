﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using Stareater.AppData;
using Stareater.Controllers;
using Stareater.Localization;
using Stareater.Utils.Collections;
using Stareater.Utils.NumberFormatters;

namespace Stareater.GUI
{
	public partial class FormColonyDetails : Form
	{
		private ColonyController controller;

		public FormColonyDetails()
		{
			InitializeComponent();
		}

		public FormColonyDetails(ColonyController controller) : this()
		{
			this.controller = controller;
			
			Context context = SettingsWinforms.Get.Language["FormColony"];
			//TODO(v0.5): set form title
			
			buildingsGroup.Text = context["buildingsGroup"].Text();
			planetInfoGroup.Text = context["planetGroup"].Text();
			popInfoGroup.Text = context["popGroup"].Text();
			productivityGroup.Text = context["productivityGroup"].Text();

			var vars = new TextVar();
			var popFormat = new ThousandsFormatter(controller.PopulationMax, controller.Population);
			var percentFormat = new DecimalsFormatter(0, 1);
			vars.And("pop", popFormat.Format(controller.Population));
			vars.And("popGrowth", popFormat.Format(controller.PopulationGrowth));
			vars.And("popMax", popFormat.Format(controller.PopulationMax));
			vars.And("popOrg", percentFormat.Format(controller.Organization * 100));
			
			populationInfo.Text = context["populationInfo"].Text(vars.Get);
			growthInfo.Text = context["growthInfo"].Text(vars.Get); //TODO(v0.5): add sign
			infrastructureInfo.Text = context["infrastructureInfo"].Text(vars.Get);
			
			var prefixFormat = new ThousandsFormatter();
			vars.And("planetSize", prefixFormat.Format(controller.PlanetSize));
			vars.And("planetEnv", percentFormat.Format(controller.PlanetEnvironment * 100));
			
			sizeInfo.Text = context["sizeInfo"].Text(vars.Get);
			environmentInfo.Text = context["environmentInfo"].Text(vars.Get);
			
			vars.And("prodFood", percentFormat.Format(controller.FoodPerPop));
			vars.And("prodOre", percentFormat.Format(controller.OrePerPop));
			vars.And("prodInd", percentFormat.Format(controller.IndustryPerPop));
			vars.And("prodDev", percentFormat.Format(controller.DevelopmentPerPop));
			vars.And("totalInd", prefixFormat.Format(controller.IndustryTotal));
			vars.And("totalDev", prefixFormat.Format(controller.DevelopmentTotal));
			
			foodInfo.Text = context["foodInfo"].Text(vars.Get);
			miningInfo.Text = context["miningInfo"].Text(vars.Get);
			industryInfo.Text = context["industryInfo"].Text(vars.Get);
			developmentInfo.Text = context["developmentInfo"].Text(vars.Get);
			industryTotalInfo.Text = context["industryTotalInfo"].Text(vars.Get);
			developmentTotalInfo.Text = context["developmentTotalInfo"].Text(vars.Get);
			
			foreach (var data in controller.Buildings) {
				var itemView = new BuildingItem();
				itemView.Data = data;
				buildingsList.Controls.Add(itemView);
			}
		}
		
		protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
		{
			if (keyData == Keys.Escape) 
				this.Close();
			return base.ProcessCmdKey(ref msg, keyData);
		}
	}
}
