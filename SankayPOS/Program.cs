using SankayPOS.Database;
using SankayPOS.Forms;
using SankayPOS.Utils;

namespace SankayPOS;

static class Program
{
    /// <summary>
    ///  The main entry point for the application.
    /// </summary>
    [STAThread]
    static void Main()
    {
        // Initialize database
        DatabaseHelper.InitializeDatabase();
        
        // Initialize sample data on first run
        SampleDataInitializer.InitializeSampleData();
        
        // To customize application configuration such as set high DPI settings or default font,
        // see https://aka.ms/applicationconfiguration.
        ApplicationConfiguration.Initialize();
        Application.Run(new MainForm());
    }    
}