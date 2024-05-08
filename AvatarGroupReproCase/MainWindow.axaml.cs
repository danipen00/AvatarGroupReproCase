using System.Collections.Generic;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Models.TreeDataGrid;
using Avalonia.Controls.Primitives;
using Avalonia.Controls.Templates;
using Avalonia.Layout;
using Avalonia.VisualTree;

namespace AvatarGroupReproCase;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();

        mTreeDataGrid = new TreeDataGrid();
        mTreeDataGrid.CellPrepared += TreeDataGridOnCellPrepared;

        List<string> items = new List<string>();
        for (int i = 0; i < 10000; i++)
        {
            items.Add("user" + i.ToString() + "@company.com");
        }

        FlatTreeDataGridSource<string> treeDataGridSource = new FlatTreeDataGridSource<string>(items);
        treeDataGridSource.Columns.Add(new TemplateColumn<string>(
            "Item",
            new FuncDataTemplate<string>((string node, INameScope ns) =>
            {
                return new AvatarCellPanel();
            }, true)));

        mTreeDataGrid.Source = treeDataGridSource;

        mContainerPanel.Children.Add(mTreeDataGrid);

        //treeDataGridSource.Items = items;
    }

    void TreeDataGridOnCellPrepared(object? sender, TreeDataGridCellEventArgs e)
    {
        Control cellControl = e.Cell;

        AvatarCellPanel? avatarCellPanel =
            cellControl.FindDescendantOfType<AvatarCellPanel>();

        string? model = (string)mTreeDataGrid.Rows?[e.RowIndex].Model!;

        avatarCellPanel?.SetData(model);
    }

    readonly TreeDataGrid mTreeDataGrid;

    class AvatarCellPanel : DockPanel
    {
        internal void SetData(string? data)
        {
            mTextBlock.Text = data;
            mAvatarGroup.SetAvatars(new List<string?> { data });
        }

        internal AvatarCellPanel()
        {
            mAvatarGroup = new AvatarGroup()
            {
                MaxAvatars = 3,
                OverlapPercentage = 0.2,
            };

            mTextBlock = new TextBlock();
            mTextBlock.Margin = new Thickness(5, 0, 0, 0);

            DockPanel.SetDock(mAvatarGroup, Dock.Left);

            Children.Add(mAvatarGroup);
            Children.Add(mTextBlock);
        }

        AvatarGroup mAvatarGroup;
        TextBlock mTextBlock = new TextBlock();
    }
}