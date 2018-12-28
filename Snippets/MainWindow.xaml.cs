using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.IO;
using System.Xml;
using System.Xml.Serialization;
using System;
using System.Diagnostics;
using System.Windows.Interop;
using System.Threading;
using System.Text;

namespace Snippets
{
    class CategoryComparer : IEqualityComparer<Snippet>
    {
        public bool Equals(Snippet x, Snippet y)
        {
            return
                x.Category != null
                && y.Category != null
                && x.Category.ToLower() == y.Category.ToLower();
        }

        public int GetHashCode(Snippet snippet)
        {
            return 0;
        }
    };
    public class Snippet
    {
        public string CommandText { get; set; } = "";
        public string Description { get; set; } = "";
        public string Category { get; set; } = "";
        public string Interpreter { get; set; } = "";
        public string WorkingDirectory { get; set; } = "C:/";
        public string DisplayCommandText { get { return InsertEvery(CommandText, 5000, "\r\n"); } }
        public string DisplayDescription { get { return InsertEvery(Description, 5000, "\r\n"); } }

        private static int lastIndexOfAny(string s, string[] cs)
        {
            return
                cs
                    .Select(c => s.IndexOf(c))
                    .Where(index => index != -1)
                    .DefaultIfEmpty(-1)
                    .Min();
        }

        private static string InsertEvery(string s, int lineLength, string insertText)
        {
            if(s.Trim().StartsWith("run"))
            {
                ;
            }
            var restString = s.Trim() ?? "";
            var currentString = "";
            while(restString != "")
            {
                var lineString = String.Concat(restString.Take(lineLength));
                var lastWhitespaceIndex = lastIndexOfAny(lineString, new[] { " ", "\r\n", "\t"});
                var n =
                    lastWhitespaceIndex == -1 || lineString.Trim().Length < lineLength
                        ? lineString.Length
                        : lastWhitespaceIndex;
                currentString += String.Concat(restString.Take(n)) + "\r\n";
                restString = String.Concat(restString.Skip(n));
            }
            return currentString;
        }
    }


    public partial class MainWindow : Window
    {
        List<Snippet> snippets = new List<Snippet>();
        private FileSystemWatcher watcher = null;
        private string snippetsFile = System.AppDomain.CurrentDomain.BaseDirectory + "snippets.xml";

        private void OnSnippetsConfigChanged(object source, FileSystemEventArgs e)
        {
            if (e.ChangeType == WatcherChangeTypes.Changed)
            {
                loadSnippets("");
            }
        }

        private void loadSnippets(string categoryToSelect)
        {
            int retryCount = 0;
            while (true)
            {
                try
                {
                    //using (FileStream xmlStream = new FileStream(snippetsFile, FileMode.Open, FileAccess.Read))
                    using (StreamReader xmlStream = new StreamReader(snippetsFile, Encoding.Default, true))
                    {
                        using (XmlReader xmlReader = XmlReader.Create(xmlStream))
                        {
                            XmlSerializer serializer = new XmlSerializer(typeof(ProgramData));
                            ProgramData programData = serializer.Deserialize(xmlReader) as ProgramData;
                            snippets = new List<Snippet>();
                            Dispatcher.Invoke(() =>
                                {
                                    foreach (var snippet in programData.Snippet)
                                    {
                                        snippets.Add(
                                            new Snippet()
                                            {
                                                CommandText = snippet.CommandText.Replace("\n", "\r\n"),
                                                Description = snippet.Description,
                                                Category = snippet.Category,
                                                Interpreter =
                                                    (programData.Configuration.Interpreter.
                                                        FirstOrDefault(x => x.name == snippet.Interpreter)?.path
                                                    ?? programData.Configuration.Interpreter.First(x => x.@default)?.path)
                                                    ?? "",
                                                WorkingDirectory =
                                                    (snippet.workingDirectory
                                                    ?? programData.Configuration.DefaultWorkingDirectory)
                                                    ?? ""
                                            });
                                    }
                                    dgSnippets.ItemsSource = snippets;
                                    var s =
                                            snippets.Select(y => y.Category.Split(new[] { ',' })).SelectMany(y => y).Select(y => y.Trim()).OrderBy(x => x).Distinct();
                                    cmbCategories.ItemsSource =
                                        new[] { "Show all" }
                                            .Concat(s);
                                    cmbCategories.SelectedIndex = 0;
                                });
                        }
                    }
                    return;
                }
                catch (IOException ioErr)
                {
                    ++retryCount;
                    Thread.Sleep(20);
                    //MessageBox.Show(ioErr.ToString());
                    if(retryCount > 1000)
                    {
                        MessageBox.Show("Sorry, failed to reload XML data. Please restart the program manually.");
                        App.Current.Shutdown();
                        return;
                    }
                }
                catch (Exception err)
                {
                    MessageBox.Show("Sorry, an error occurred:\n" + err.ToString());
                    return;
                }
            }
        }


        public MainWindow(string categoryToSelect)
        {
            InitializeComponent();
            tbSearch.Focus();
            loadSnippets(categoryToSelect);
            watcher = new FileSystemWatcher();
            watcher.Path = Path.GetDirectoryName(snippetsFile);
            watcher.NotifyFilter = NotifyFilters.LastAccess | NotifyFilters.LastWrite | NotifyFilters.FileName | NotifyFilters.DirectoryName;
            watcher.Filter = Path.GetFileName(snippetsFile);
            watcher.EnableRaisingEvents = true;
            watcher.Changed += new FileSystemEventHandler(OnSnippetsConfigChanged);
        }

        protected override void OnSourceInitialized(EventArgs e)
        {
            base.OnSourceInitialized(e);
            HwndSource source = PresentationSource.FromVisual(this) as HwndSource;
            source.AddHook(WndProc);
        }

        private IntPtr WndProc(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
        {
            if (msg == NativeMethods.WM_SHOWME)
            {
                ShowMe();
            }
            return IntPtr.Zero;
        }

        private void ShowMe()
        {
            Show();
            Activate();
            Topmost = true;
            Topmost = false;
        }

        private void cmbCategories_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            UpdateCurrentSnippets();
        }

        private void dgSnippets_KeyDown(object sender, KeyEventArgs e)
        {
        }

        private void tbSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            UpdateCurrentSnippets();
        }

        private void UpdateCurrentSnippets()
        {
            var searchWords = tbSearch.Text.ToLower().Split(new [] { ' ' }).Where(x => x.Trim() != "");
            dgSnippets.ItemsSource =
                snippets.Where(snippet => 
                    (cmbCategories.SelectedIndex <= 0
                      || (snippet.Category.Contains(cmbCategories.SelectedItem as string)))
                    && searchWords.All(word =>
                        snippet.CommandText.ToLower().Contains(word)
                        || snippet.Description.ToLower().Contains(word)
                        || snippet.Category.ToLower().Contains(word)));
        }

        private void tbSearch_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.Key == Key.Down || e.Key == Key.Tab)
            {
                dgSnippets.Focus();
                if (dgSnippets.Items.Count > 0 && dgSnippets.SelectedIndex == -1)
                {
                        dgSnippets.SelectedIndex = 0;
                }
                var selectedRow = dgSnippets.ItemContainerGenerator.ContainerFromIndex(dgSnippets.SelectedIndex) as DataGridRow;
                if(selectedRow == null)
                {
                    return;
                }
                FocusManager.SetIsFocusScope(selectedRow, true);
                FocusManager.SetFocusedElement(selectedRow, selectedRow);
            }
        }

        private void tbSearch_GotFocus(object sender, RoutedEventArgs e)
        {
            tbSearch.SelectAll();
        }

        private void dgSnippets_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Tab)
            {
                e.Handled = true;
                if(Keyboard.IsKeyDown(Key.LeftShift) || Keyboard.IsKeyDown(Key.RightShift))
                {
                    tbSearch.Focus();
                }
                else
                {
                    cmbCategories.Focus();
                }
            }
            else if(e.Key == Key.Enter)
            {
                ExecuteSnippet(dgSnippets.SelectedItem as Snippet);
            }
            else if(e.Key == Key.Escape)
            {
                tbSearch.Focus();
                App.Current.MainWindow.WindowState = WindowState.Minimized;
            }
        }

        private void ExecuteSnippet(Snippet snippet)
        {
            if (snippet != null)
            {
                tbSearch.Focus();
                Hide();
                Directory.CreateDirectory(System.AppDomain.CurrentDomain.BaseDirectory + "/tmp");
                File.WriteAllText(System.AppDomain.CurrentDomain.BaseDirectory + "/tmp/ahk.ahk", snippet.CommandText);
                using (var p = new Process())
                {
                    p.StartInfo.Arguments = "\"" + System.AppDomain.CurrentDomain.BaseDirectory + "/tmp/ahk.ahk\"";
                    p.StartInfo.FileName = "c:/program files/AutoHotkey/AutoHotkey.exe";
                    Process.Start(p.StartInfo).WaitForExit();
                }
            }
        }

        private void dgSnippets_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            ExecuteSnippet(dgSnippets.SelectedItem as Snippet);
        }

        private void btnEditSnippets_Click(object sender, RoutedEventArgs e)
        {
            using (var p = new Process())
            {
                p.StartInfo.FileName = "notepad++.exe";
                p.StartInfo.Arguments = "snippets.xml";
                p.Start();
            }
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
        }

        private void Window_StateChanged(object sender, EventArgs e)
        {
            if(this.WindowState == WindowState.Minimized)
            {
                this.Hide();
            }
        }
    }
}
