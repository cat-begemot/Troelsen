using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.IO;
using System.Drawing;



namespace DataParallelismWithForEach
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		private CancellationTokenSource cancelToken = new CancellationTokenSource();

		public MainWindow()
		{
			InitializeComponent();
		}

		private void cmdProcess_Click(object sender, RoutedEventArgs e)
		{
			Task.Factory.StartNew(() => ProcessFile());
		}

		private void cmdCancel_Click(object sender, RoutedEventArgs e)
		{
			cancelToken.Cancel();
		}

		private void ProcessFile()
		{
			ParallelOptions parOpts = new ParallelOptions();
			parOpts.CancellationToken = cancelToken.Token;
			parOpts.MaxDegreeOfParallelism = System.Environment.ProcessorCount;
			MessageBox.Show($"Processors number: {parOpts.MaxDegreeOfParallelism}");

			String path = System.IO.Path.GetFullPath(Environment.GetFolderPath(Environment.SpecialFolder.MyPictures) + @"\TestPictures");
			String[] files = Directory.GetFiles(path , "*.jpg", SearchOption.TopDirectoryOnly);
			String newDir = path + @"\EditedPictures";
			Directory.CreateDirectory(newDir);
			try
			{
				Parallel.ForEach(files, parOpts, currentFile =>
				{
					parOpts.CancellationToken.ThrowIfCancellationRequested();

					String fileName = System.IO.Path.GetFileName(currentFile);
					using (Bitmap bitmap = new Bitmap(currentFile))
					{
						bitmap.RotateFlip(RotateFlipType.Rotate180FlipNone);
						bitmap.Save(System.IO.Path.Combine(newDir, fileName));
						
						this.Dispatcher.Invoke((Action)delegate
						{
							this.Title = $"Processing {fileName} on thread {Thread.CurrentThread.ManagedThreadId}";
						});
					}
				});
				
			}
			catch(OperationCanceledException ex)
			{
				
			}
			
		}
	}
}
