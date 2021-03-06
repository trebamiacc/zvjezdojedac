﻿using System.Collections.Generic;
using System.Drawing;
using Ikadn;
using Ikadn.Ikon.Types;
using Ikadn.Utilities;

namespace Stareater.AppData
{
	class SettingsWinforms : Settings
	{
		#region Singleton
		static SettingsWinforms instance = null;

		public static new SettingsWinforms Get
		{
			get
			{
				if (instance == null)
					instance = new SettingsWinforms(loadFile());
				return instance;
			}
		}
		#endregion

		public float GuiScale { get; set; }

		public SettingsWinforms(TaggableQueue<object, IkadnBaseObject> data)
			:base(data)
		{
			if (data.CountOf(WinformsSettingsTag) > 0)
			{
				IkonComposite wfSpecificData = data.Dequeue(WinformsSettingsTag).To<IkonComposite>();
				GuiScale = wfSpecificData[GuiScaleKey].To<float>();
			}
			else
			{
				GuiScale = 1;
			}
		}

		public Font FormFont
		{
			get
			{
				return new Font(SystemFonts.DefaultFont.FontFamily, SystemFonts.DefaultFont.Size * GuiScale);
			}
		}

		#region Attribute keys
		const string WinformsSettingsTag = "winforms";
		const string GuiScaleKey = "guiscale";
		#endregion

		protected override void buildSaveData(IkadnWriter writer)
		{
			base.buildSaveData(writer);

			IkonComposite settings = new IkonComposite(WinformsSettingsTag);
			settings.Add(GuiScaleKey, new IkonFloat(GuiScale));
			settings.Compose(writer);
		}
	}
}
