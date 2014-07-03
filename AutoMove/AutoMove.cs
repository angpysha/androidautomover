using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Concurrent;
using System.Threading;
using AutoMove;
using System.Reflection;
//using System.Windows.Forms;
namespace AutoMove
{
    
    public class AutoMove
    {
        //private string _extensions;
        private IEnumerable<string> _extentions;
        private string _output;
        private List<FileSystemWatcher> _watcher;
        private ConcurrentDictionary<uint, DateTime> _filesh = new ConcurrentDictionary<uint, DateTime>();
        private Timer _timer;

        public AutoMove(string configpath)
        {
            var inifil = new IniFile(configpath);

            var configmonitor = new FileSystemWatcher(Path.GetDirectoryName(configpath), Path.GetFileName(configpath));

            var readconfig = FunctionTools.Recreate(() => new
            {
                Outfolder = inifil["Output"].First(),
                Input = inifil["Input"],
                Extentions = inifil["Extentions"].FirstOrDefault().With(x => x.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
            });
            configmonitor.Changed += delegate
    {
        Stop();
        inifil = new IniFile(configpath);
        var newconfig = readconfig();
        Init(newconfig.Input, newconfig.Outfolder, newconfig.Extentions);
        Start();
    };
            var config = readconfig();
           // MessageBox.Show(config.Input);
          //  showinfo(config.Outfolder);
            Init(config.Input, config.Outfolder, config.Extentions);
            configmonitor.EnableRaisingEvents = true;
        }

    /*    private void showinfo(IniFile.IniFileSection iniFileSection)
        {
            throw new NotImplementedException();
        }*/

       /* public string showinfo(string a)
        {
            
        }*/

        public void Stop()
        {
            _watcher.ForEach(x => x.EnableRaisingEvents = false);
        }

        public void Start()
        {
            _watcher.ForEach(x => x.EnableRaisingEvents = true);
        }

        public AutoMove(IEnumerable<string> monitorngfolders, string outputfldr, IEnumerable<string> extentions = null)
        {
            Init(monitorngfolders, outputfldr, extentions);
        }

        private void Init(IEnumerable<string> monitorfldr, string outfldr, IEnumerable<string> extentions)
        {
            _output = outfldr;
            _extentions = extentions ?? new[] {".apk"};
            _watcher = new List<FileSystemWatcher>();
            _timer = new Timer(delegate {
              var now =  DateTime.Now;
              var keys = _filesh.Where(pair => (now - pair.Value).Hours > 1).Select(pair => pair.Key).ToArray();
              keys.ForEach(x => _filesh.TryRemove(x, out now));
            },null,TimeSpan.Zero,new TimeSpan(0,0,10,0));
            foreach (var watcher in monitorfldr.Where(Directory.Exists).Select(folder => new FileSystemWatcher(folder) {IncludeSubdirectories=true}))
            {
                watcher.Created += watcherhandl;
                watcher.Changed += watcherhandl2;
                _watcher.Add(watcher);
            }

        }

        private void watcherhandl2(object sender, FileSystemEventArgs e)
        {
            ((Action<string>)Move2).BeginInvoke(e.FullPath, null, null);
        }

        private void Move2(string obj)
        {
            var filename = Path.GetFileNameWithoutExtension(obj);
                var fulfilenae = Path.GetFullPath(obj);
                var filename2 = Path.GetFileName(obj);
                var newfile = Path.Combine(_output, filename2);
            FileInfo fin = new FileInfo(obj);
            if (!File.Exists(obj) || !_extentions.Any(obj.EndsWith))
            {
                return;
            }
            if (File.Exists(newfile))
            {
                File.Delete(newfile);
            }
           //if (fin.Length == 0)
           {
         
                Task mv = Task.Run(() =>
                {
                    /*  if (!File.Exists(newfile))
                      {
                          return;
                      }*/

                    File.Move(@obj, @newfile);
                    //   StreamWriter str = new StreamWriter("d:\\34.txt");
                    //    str.Write(newfile);
                    //   str.Close();
                });
                mv.Wait();
            }
        }

        private void watcherhandl(object sender, FileSystemEventArgs fileSystemEventArgs)
        {
            ((Action<string>)Move).BeginInvoke(fileSystemEventArgs.FullPath, null, null);
        }

        private void Move(string obj)
        {
            if (!File.Exists(obj) || !_extentions.Any(obj.EndsWith))
            {
                return;
            }
            var filename = Path.GetFileNameWithoutExtension(obj);
            var fulfilenae = Path.GetFullPath(obj);
            var filename2 = Path.GetFileName(obj);
            var newfile = Path.Combine(_output, filename2);
            //string infile = string.Format("\"{0}\" \"{1}\"", obj);
            //  try
            //{
            byte[] filecont = null;
            IOUtils.WrapSharingViolations(() => filecont = File.ReadAllBytes(obj), null, int.MaxValue, 1000);
            FileInfo fin = new FileInfo(obj);
            //if (File.GetAccessControl(obj) == FileAccess.Read) { };
            //IOUtils.WrapSharingViolations(() => { using (File.OpenRead(obj));}, null, int.MaxValue, 1000);
            Task mv = Task.Run(() =>
            {
              /*  if (!File.Exists(newfile))
                {
                    return;
                }*/

                File.Move(@obj, @newfile);
             //   StreamWriter str = new StreamWriter("d:\\34.txt");
            //    str.Write(newfile);
             //   str.Close();
            });
            mv.Wait();
            //}
            /*catch (IOException u)
            {
                return;
            }*/
        }


    }
}
