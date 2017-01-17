using System;
using System.Net;
using System.Text;
using System.Collections.Generic;

using Slacker2;
using Slacker2.Models;

using BuildsFlightCore;

namespace SampleBotApp
{

	public class BuildsFlightBot : BotService
	{
		[Subscribe("^!bf-create-app (.+)$")]
		[Usage("!bf-create-app APPNAME")]
		public void OnCreateApp(SlackMessage message, string app)
		{
			BuildsFlight.CreateApp(app);

			message.Reply($"{app} created.");
		}

		[Subscribe("^!bf-list-builds (.+)")]
		[Usage("^!bf-list-builds APPNAME")]
		public void OnListBuilds(SlackMessage message, string app)
		{
			var appInfo = BuildsFlight.GetApp(app);

			if (appInfo == null)
				message.Reply($"{app} not found.");
			else
			{
				var sb = new StringBuilder();

				sb.Append($"[{appInfo.Name}]\r\n");
				foreach (var build in appInfo.Builds)
				{
					sb.Append($"   - {build.Version}\r\n");
					sb.Append($"     {build.Url}\r\n");
				}

				message.Reply(sb.ToString());
			}
		}

		[Subscribe("^!bf-add-build (.+) (.+) (.+)$")]
		[Usage("!bf-add-build APPNAME VERSION URL")]
		public void OnAddBuild(SlackMessage message, string app, string version, string url)
		{
			var appInfo = BuildsFlight.GetApp(app);

			if (appInfo == null)
				message.Reply($"{app} not found.");
			else
			{
				appInfo.AddBuild(version, url, "");

				message.AddReaction("ok");
				message.Reply($"OK");
			}
		}

		[Subscribe("^!bf-set-target-version (.+) (.+)$")]
		[Usage("!bf-set-target-version APPNAME VERSION")]
		public void OnSetTargetVersion(SlackMessage message, string app, string version)
		{
			var appInfo = BuildsFlight.GetApp(app);

			if (appInfo == null)
				message.Reply($"{app} not found.");
			else
			{
				appInfo.SetTargetVersion(version);
				message.AddReaction("ok");
				message.Reply($"OK");
			}
		}
	}
}
