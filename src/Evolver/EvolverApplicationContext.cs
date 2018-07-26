using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.SQLite;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Runtime.InteropServices;


namespace Evolver
{
    internal static class NativeMethods
    {
        // Import SetThreadExecutionState Win32 API and necessary flags
        [DllImport("kernel32.dll")]
        public static extern uint SetThreadExecutionState(uint esFlags);
        public const uint ES_AWAYMODE_REQUIRED = 0x00000040;
        public const uint ES_CONTINUOUS = 0x80000000;
        public const uint ES_SYSTEM_REQUIRED = 0x00000001;

        public static void PreventSystemSleep()
        {
            SetThreadExecutionState(ES_CONTINUOUS | ES_SYSTEM_REQUIRED | ES_AWAYMODE_REQUIRED);
        }
        public static void AllowSystemSleep()
        {
            SetThreadExecutionState(ES_CONTINUOUS);
        }
    }

    class EvolverApplicationContext : ApplicationContext
    {
        private readonly string worldPath;
        private readonly string dbPath;
        private WorldDbContext dbContext;
        private MutantManager mutantManager;
        private EvolutionRunner runner;

        public EvolverApplicationContext()
        {
            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;
            worldPath = Path.Combine(Application.StartupPath, "world");
            dbPath = Path.Combine(worldPath, "world.db");

            if (Directory.Exists(worldPath))
            {
                try
                {
                    Directory.CreateDirectory(worldPath);
                    var connectionString = new SQLiteConnectionStringBuilder() { DataSource = dbPath }.ConnectionString;
                    var conn = new SQLiteConnection(connectionString);
                    conn.Open();
                    dbContext = new WorldDbContext(conn, true);
                    mutantManager = new MutantManager(dbContext, worldPath);
                    mutantManager.LoadMutants();
                    ShowWorld();
                }
                catch (Exception e)
                {
                    while (e.InnerException != null)
                        e = e.InnerException;
                    MessageBox.Show("Failed to load world: " + e.Message, "Fatal Error");
                    Environment.Exit(-1);
                }
            }
            else
            {
                var createWorldForm = new CreateWorldForm();
                createWorldForm.Closed += OnCreateWorldFormClosed;
                createWorldForm.CreateWorldHandler = CreateWorld;
                createWorldForm.Show();
            }
        }

        private void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            try
            {
                MessageBox.Show("Unhandled exception: " + e.ExceptionObject.ToString());
            }
            catch
            {
            }
            Environment.Exit(-1);
        }

        private void CreateWorld(CreateWorldForm form, int numMutants, int mutantSize)
        {
            try
            {
                Directory.CreateDirectory(worldPath);
                var connectionString = new SQLiteConnectionStringBuilder() { DataSource = dbPath }.ConnectionString;
                var conn = new SQLiteConnection(connectionString);
                conn.Open();
                dbContext = new WorldDbContext(conn, false);
                dbContext.Create(numMutants, mutantSize);
                mutantManager = new MutantManager(dbContext, worldPath);
                mutantManager.CreateMutants(
                    x => {
                        form.Progress = x;
                        Application.DoEvents();
                    });
                form.Closed -= OnCreateWorldFormClosed;
                ShowWorld();
                form.Close();
            }
            catch (Exception e)
            {
                DestroyWorld();
                while (e.InnerException != null)
                    e = e.InnerException;
                MessageBox.Show("Coulnd't create or open db file, because " + e.Message, "Fatal Error");
                Environment.Exit(-1);
            }
        }

        private void DestroyWorld()
        {
            if (dbContext != null)
            {
                mutantManager = null;
                dbContext.Database.Connection.Close();
                dbContext.Dispose();
                dbContext = null;
                GC.Collect();
                GC.WaitForPendingFinalizers();
            }
            Directory.Delete(worldPath, true);
        }

        private void ShowWorld()
        {
            var worldForm = new WorldForm()
            {
                DbContext = dbContext
            };
            worldForm.RunOnMap = RunOnMap;
            worldForm.ViewMutant = ViewMutant;
            worldForm.LoadMutant = LoadMutant;
            worldForm.RunEvolution = RunEvolution;
            worldForm.StopEvolution = StopEvolution;
            worldForm.ResizeMutants = ResizeMutants;
            worldForm.Closed += (sender, e) =>
            {
                dbContext.SaveChanges();
                ExitThread();
            };
            worldForm.Show();
        }

        private void ViewMutant(WorldForm form, int mutantId)
        {
            var mutant = mutantManager.GetMutant(mutantId);
            var mutantForm = new MutantForm(mutant);
            mutantForm.Show(form);
        }

        private void LoadMutant(WorldForm form, string path)
        {
            try
            {
                var mutant = mutantManager.LoadMutant(path);
                var mutantForm = new MutantForm(mutant);
                mutantForm.Show(form);
            }
            catch (Exception e)
            {
                MessageBox.Show(form, e.Message, "Failed");
            }
        }

        private void RunEvolution(WorldForm form)
        {
            runner = new EvolutionRunner(dbContext, mutantManager, form.EvolverSettings);
            runner.UpdateEvent += form.Runner_UpdateEvent;
            runner.Run();
        }

        private void Runner_UpdateEvent(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        private void RunOnMap(WorldForm form, int[] mutants)
        {
            var mapForm = new MapForm();
            mapForm.SetRunner(new EvolutionRunner(dbContext, mutantManager, form.EvolverSettings, mutants));
            mapForm.ShowDialog(form);
        }

        private void StopEvolution(WorldForm form)
        {
            if (runner != null)
            {
                runner.Stop();
                runner.UpdateEvent -= form.Runner_UpdateEvent;
                runner = null;
            }
        }

        private void ResizeMutants(WorldForm form)
        {
            var changeWorldForm = new ChangeWorldForm();
            changeWorldForm.ChangeWorldHandler = ChangeWorld;
            changeWorldForm.ShowDialog(form);
        }

        private void ChangeWorld(ChangeWorldForm form, int mutantSize)
        {
            mutantManager.ResizeMutants(mutantSize,
                x => {
                    form.Progress = x;
                    Application.DoEvents();
                });
            form.Close();
        }

        private void OnCreateWorldFormClosed(object sender, EventArgs e)
        {
            DestroyWorld();
            ExitThread();
        }
    }
}
