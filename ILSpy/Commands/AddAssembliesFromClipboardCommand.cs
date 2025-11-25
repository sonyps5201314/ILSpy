using System;
using System.Composition;
using System.Linq;
using System.Windows;

using ICSharpCode.ILSpy;
using ICSharpCode.ILSpy.AssemblyTree;
using ICSharpCode.ILSpyX;

namespace ICSharpCode.ILSpy.Commands
{
	[ExportMainMenuCommand(ParentMenuID = "_File", Header = "_Add Assemblies from Clipboard", MenuCategory = "Open", MenuOrder = 2.1)]
	[Shared]
	public class AddAssembliesFromClipboardCommand : SimpleCommand
	{
		private readonly AssemblyTreeModel _assemblyTreeModel;

		public AddAssembliesFromClipboardCommand(AssemblyTreeModel assemblyTreeModel)
		{
			_assemblyTreeModel = assemblyTreeModel;
		}

		public override void Execute(object parameter)
		{
			string clipboardText = Clipboard.GetText();
			if (string.IsNullOrWhiteSpace(clipboardText))
				return;

			var paths = clipboardText
				.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries)
				.Select(p => p.Trim())
				.Where(p => !string.IsNullOrEmpty(p));

			var assemblyList = _assemblyTreeModel.AssemblyList;
			foreach (var path in paths)
			{
				try
				{
					assemblyList.OpenAssembly(path);
				}
				catch (Exception ex)
				{
					MessageBox.Show($"无法添加程序集: {path}\n{ex.Message}", "添加程序集失败", MessageBoxButton.OK, MessageBoxImage.Warning);
				}
			}
		}
	}
}
