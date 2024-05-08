using System;
using System.Collections.Generic;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Shapes;
using Avalonia.Layout;
using Avalonia.Media;

namespace AvatarGroupReproCase;

public class AvatarGroup : Border
{
    public static readonly StyledProperty<int> MaxAvatarsProperty =
        AvaloniaProperty.Register<AvatarGroup, int>(nameof(MaxAvatars), 5, coerce: CoerceMaxAvatars);

    public int MaxAvatars
    {
        get => GetValue(MaxAvatarsProperty);
        set => SetValue(MaxAvatarsProperty, value);
    }

    public static readonly StyledProperty<double> OverlapPercentageProperty =
        AvaloniaProperty.Register<AvatarGroup, double>(nameof(OverlapPercentage), 0.2, coerce: CoerceOverlapPercentage);

    public double OverlapPercentage
    {
        get => GetValue(OverlapPercentageProperty);
        set => SetValue(OverlapPercentageProperty, value);
    }

    public static readonly StyledProperty<IBrush> AvatarBorderBrushProperty =
        AvaloniaProperty.Register<AvatarGroup, IBrush>(nameof(AvatarBorderBrush), Brushes.White);

    public IBrush AvatarBorderBrush
    {
        get => GetValue(AvatarBorderBrushProperty);
        set => SetValue(AvatarBorderBrushProperty, value);
    }

    public static readonly StyledProperty<double> AvatarBorderThicknessProperty =
        AvaloniaProperty.Register<AvatarGroup, double>(nameof(AvatarBorderThickness), 0);

    public double AvatarBorderThickness
    {
        get => GetValue(AvatarBorderThicknessProperty);
        set => SetValue(AvatarBorderThicknessProperty, value);
    }

    public void SetAvatars(List<string?> avatars)
    {
        mAvatars.Clear();
        mAvatars.AddRange(avatars);
        UpdatePanel();
    }

    public void AddAvatar(string? avatar)
    {
        mAvatars.Add(avatar);
        UpdatePanel();
    }

    public void AddAvatars(List<string> avatars)
    {
        mAvatars.AddRange(avatars);
        UpdatePanel();
    }

    public void RemoveAvatar(string? avatar)
    {
        mAvatars.Remove(avatar);
        UpdatePanel();
    }

    public void RemoveAvatars(List<string?> avatars)
    {
        foreach (var avatar in avatars)
        {
            mAvatars.Remove(avatar);
        }

        UpdatePanel();
    }

    public AvatarGroup()
    {
        mContentPanel = new StackPanel();
        mContentPanel.Orientation = Orientation.Horizontal;
        Child = mContentPanel;
    }

    // internal for testing purposes
    internal StackPanel ContentPanel => mContentPanel;

    protected override void OnSizeChanged(SizeChangedEventArgs e)
    {
        base.OnSizeChanged(e);

        UpdateSizesAndMargins();
    }

    void UpdateSizesAndMargins()
    {
        for (int i = 0; i < mContentPanel.Children.Count; i++)
        {
            AvatarImage? avatar = mContentPanel.Children[i] as AvatarImage;

            if (avatar == null)
                continue;

            avatar.Width = Bounds.Height;
            avatar.Height = Bounds.Height;

            if (i == 0)
                continue;

            double overlapValue = Bounds.Height * OverlapPercentage;
            avatar.Margin = new Thickness(-overlapValue, 0, 0, 0);
        }
    }

    void UpdatePanel()
    {
        mContentPanel.Children.Clear();

        int rest = mAvatars.Count - MaxAvatars;

        for (int i = 0; i < MaxAvatars; i++)
        {
            if (i >= mAvatars.Count)
                break;

            AvatarImage avatarImage = new AvatarImage()
            {
                /*UserName = mAvatars[i],
                BorderBrush = AvatarBorderBrush,
                BorderThickness = AvatarBorderThickness,*/
            };

            mContentPanel.Children.Add(avatarImage);
        }

        if (rest > 0)
        {
            AvatarImage avatarImage = new AvatarImage()
            {
                /*UserName = "+" + rest,
                BorderBrush = AvatarBorderBrush,
                BorderThickness = AvatarBorderThickness,
                Background = Brushes.Gray,*/
            };

            mContentPanel.Children.Add(avatarImage);
        }

        if (mAvatars.Count > 0)
        {
            ToolTip.SetTip(this, string.Join(Environment.NewLine, mAvatars));
        }

        UpdateSizesAndMargins();
    }

    protected override void OnPropertyChanged(AvaloniaPropertyChangedEventArgs change)
    {
        base.OnPropertyChanged(change);

        if (change.Property == MaxAvatarsProperty)
        {
            UpdatePanel();
        }

        if (change.Property == OverlapPercentageProperty)
        {
            UpdateSizesAndMargins();
        }

        if (change.Property == AvatarBorderBrushProperty ||
            change.Property == AvatarBorderThicknessProperty)
        {
            UpdateAvatarBorders();
        }
    }

    void UpdateAvatarBorders()
    {
        foreach (var child in mContentPanel.Children)
        {
            if (child is AvatarImage avatar)
            {
                //avatar.BorderBrush = AvatarBorderBrush;
                //avatar.BorderThickness = AvatarBorderThickness;
            }
        }
    }

    static int CoerceMaxAvatars(AvaloniaObject sender, int value)
    {
        return value < 1 ? 1 : value;
    }

    static double CoerceOverlapPercentage(AvaloniaObject sender, double value)
    {
        return value < 0 ? 0 : value > 1 ? 1 : value;
    }

    List<string?> mAvatars = new List<string?>();
    StackPanel mContentPanel;
}

class AvatarImage : Ellipse
{
    public AvatarImage()
    {
        Fill = Brushes.Red;
    }
}