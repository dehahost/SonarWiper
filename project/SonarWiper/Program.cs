/*
 * SonarWiper
 * ============
 * 
 * Wipe whole folder with files that each contains usage of Adobe's products.
 * 
 * Written in SharpDevelop by dehahost.
 * Release version is 1.2
 */
using System;
using System.IO;
using System.Security.Permissions;
using System.Timers;

namespace Sonar_Wiper {
	class Program {
		static Timer waiter;
		
		public static void Main(string[] args) {			
			if ( args.Length == 0 ) {
				FolderCheck();				
			} else if( args.Length == 1 ) {
				if ( args[0].ToLower() == "-stay" || args[0].ToLower() == "/stay" ) {
					FolderCheck();
					WatchSonar();
				} else
					WriteOutHelp();
			} else
				WriteOutHelp();
			
			Console.WriteLine( "\r\n[{0}] [GOOD/BYE] Exitting...", DateTime.Now.ToString() );
		}
		
		[PermissionSet(SecurityAction.Demand, Name="FullTrust")]
		private static void WatchSonar() {
			Console.Write( "[{0}] [WATCH/INFO] Setting up folder watcher...", DateTime.Now.ToString() );
			try {
				FileSystemWatcher watcher = new FileSystemWatcher();
				// Watch %AppData%\Adobe for changes
				watcher.Path = Environment.GetFolderPath( Environment.SpecialFolder.ApplicationData ) + "\\Adobe";
				// Fileter events
				watcher.NotifyFilter = NotifyFilters.LastAccess | NotifyFilters.LastWrite | NotifyFilters.DirectoryName;
				// Watch also subdirs
				watcher.IncludeSubdirectories = true;
				// Watch only for created and deleted things
				watcher.Created += watcher_Created;
				watcher.Deleted += watcher_Deleted;
				watcher.Changed += watcher_Changed;
				// Wake up, open your eyes and start watching
				watcher.EnableRaisingEvents = true;
				Console.WriteLine("  OK!");
				
				// Wait for the user to quit the program.
		        Console.WriteLine("Press [Q] then [Enter] to quit.");
		        while( Console.Read() != 'q' ) {}
			} catch (ArgumentException exar) {
				Console.WriteLine( "  FAILED!" );
				Console.WriteLine( "[{0}] [FOLDER_CHECK/MISSING] %AppData%\\Adobe is not present.", DateTime.Now.ToString() );
			}
		}

		static void watcher_Created(object sender, FileSystemEventArgs e) {
			if ( e.Name.StartsWith( "Sonar" ) ) {
				Console.WriteLine("[{0}] [WATCH/CREATED] Sometghing called \"{1}\" was just created.", DateTime.Now.ToString(), e.Name );
				HandleTheWaiter();
		    }
		}

		static void watcher_Changed(object sender, FileSystemEventArgs e) {
			if ( e.Name.StartsWith( "Sonar" ) ) {
				Console.WriteLine("[{0}] [WATCH/MODIFY] Something called \"{1}\" was just modified.", DateTime.Now.ToString(), e.Name );
				HandleTheWaiter();
		    }
		}
		
		static void watcher_Deleted(object sender, FileSystemEventArgs e) {
			if ( e.Name.StartsWith( "Sonar" ) )
				Console.WriteLine( "[{0}] [WATCH/DELETE] Something called \"{1}\" was just removed forever.", DateTime.Now.ToString(), e.Name );
		}

		static void waiter_Elapsed(object sender, ElapsedEventArgs e)
		{
			// Just delete Sonar folder
			Console.WriteLine( "[{0}] [WATCH/INFO] Deleting of Sonar folder in progress...", DateTime.Now.ToString() );
			Directory.Delete( Environment.GetFolderPath( Environment.SpecialFolder.ApplicationData ) + "\\Adobe\\Sonar\\", true );
			
			waiter.Close();
		}
		
		private static bool FolderCheck() {
			Console.WriteLine( "[{0}] [FOLDER_CHECK/INFO] Checking for Adobe's Sonar folder in %AppData%\\Adobe\\...", DateTime.Now.ToString() );
			if ( Directory.Exists( Environment.GetFolderPath( Environment.SpecialFolder.ApplicationData ) + "\\Adobe\\Sonar\\" ) ) {
				Console.Write("[{0}] [FOLDER_CHECK/INFO] Removing existing Sonar folder...", DateTime.Now.ToString() );
				Directory.Delete( Environment.GetFolderPath( Environment.SpecialFolder.ApplicationData ) + "\\Adobe\\Sonar\\", true );
				Console.WriteLine( "  OK!" );
				return true;
			}
			Console.WriteLine( "[{0}] [FOLDER_CHECK/MISSING] Sonar folder is not present.", DateTime.Now.ToString() );
			return false;
		}
		
		private static void HandleTheWaiter() {
			if ( waiter == null ) {
				waiter = new Timer( 50 );
				waiter.AutoReset = false;
				waiter.Elapsed += waiter_Elapsed;
			}
			
			if ( waiter.Enabled)
				waiter.Stop();
			waiter.Start();
		}
		
		private static void WriteOutHelp() {
			Console.WriteLine(
				"Sonar Wiper v1.2 by dehahost\r\n" +
				"======================\r\n" +
				"Simple wiper to hide out your usage of Adobe products.\r\n" +
				"\r\n" +
				"Command usage:\r\n" +
				"  sonarwiper.exe [/stay]\r\n"+
				"\r\n" +
				"Arguments:\r\n" +
				"  /stay   Sonar Wiper will stay running and automatically delete Sonar folder after changes was made."
			);
		}
	}
}