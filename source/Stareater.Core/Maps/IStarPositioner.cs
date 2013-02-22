﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stareater.Utils.PluginParameters;

namespace Stareater.Maps
{
	public interface IStarPositioner
	{
		string Name { get; }
		string Description { get; }
		ParameterList Parameters { get; }
	}
}
