using System;
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

        List<List<string>> items = new List<List<string>>();
        for (int i = 0; i < 10000; i++)
        {
            List<string> users = new List<string>();
            items.Add(users);
            if (i % 2 == 0)
            {
                users.Add("user" + i.ToString() + "@company.com");
                users.Add("user" + i.ToString() + "@company.com");
                users.Add("user" + i.ToString() + "@company.com");
                users.Add("user" + i.ToString() + "@company.com");
            }
            else
            {
                users.Add("user" + i.ToString() + "@company.com");
            }
        }

        FlatTreeDataGridSource<List<string>> treeDataGridSource = new FlatTreeDataGridSource<List<string>>(items);
        treeDataGridSource.Columns.Add(new TemplateColumn<List<string>>(
            "Item",
            new FuncDataTemplate<List<string>>((List<string> node, INameScope ns) =>
            {
                return new AvatarCellPanel();
            }, true)));

        mTreeDataGrid.Source = treeDataGridSource;

        mContainerPanel.Children.Add(mTreeDataGrid);

        //treeDataGridSource.Items = items;
    }

    void TreeDataGridOnCellPrepared(object? sender, TreeDataGridCellEventArgs e)
    {
        void CellLayoutUpdated(object? sender, EventArgs e)
        {
            var cc = (TreeDataGridCell)sender!;
            AvatarCellPanel? avatarCellPanel = cc.FindDescendantOfType<AvatarCellPanel>();
            List<string?> model = (List<string?>)mTreeDataGrid.Rows?[cc.RowIndex].Model!;
            avatarCellPanel!.SetData(model);
            cc.LayoutUpdated -= CellLayoutUpdated;
        }


        Control cellControl = e.Cell;


        AvatarCellPanel? avatarCellPanel =
            cellControl.FindDescendantOfType<AvatarCellPanel>();


        if (avatarCellPanel != null)
        {
            List<string?> model = (List<string?>)mTreeDataGrid.Rows?[e.RowIndex].Model!;
            avatarCellPanel?.SetData(model);
        }
        else
        {
            cellControl.LayoutUpdated += CellLayoutUpdated;
        }
    }

    readonly TreeDataGrid mTreeDataGrid;

    class AvatarCellPanel : DockPanel
    {
        internal void SetData(List<string?> data)
        {
            mTextBlock.Text = data[0];
            mAvatarGroup.SetAvatars(data);
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