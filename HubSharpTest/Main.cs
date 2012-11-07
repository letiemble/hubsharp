using System;
using System.Collections.Generic;
using System.IO;
using HubSharp.Core;

namespace HubSharpTest
{
	/// <summary>
	/// Sample application that demonstrate the use of the hubsharp library.
	/// </summary>
	class MainClass
	{
		public static void Main (string[] args)
		{
			// Read credentials from a file
			String[] lines = File.ReadAllLines("Credentials.txt");
			String username = lines[0];
			String password = lines[1];

			// Or provide them directly
			//username = "letiemble";
			//password = "XXX";

			// Create the main GitHub access
			GitHub gh = new GitHub(username, password);

			// Retrieve a user or an organization
			User user = gh.GetUser("letiemble");
			Organization org = gh.GetOrganization("github");

			// Enumerate the repositories or get a specific one
			IEnumerable<Repository> reps = org.GetRepositories(RepositoryType.Public);
			Repository rep = org.GetRepository("android");

			// Retrieve the labels and the milestones of the repository
			IEnumerable<Label> labels = rep.GetLabels();
			IEnumerable<Milestone> milestones = rep.GetMilestones();
		}
	}
}
