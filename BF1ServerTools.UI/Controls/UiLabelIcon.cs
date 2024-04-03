namespace BF1ServerTools.UI.Controls;

public class UiLabelIcon : Label
{
    /// <summary>
    /// Icon图标
    /// </summary>
    public string Icon
    {
        get { return (string)GetValue(IconProperty); }
        set { SetValue(IconProperty, value); }
    }

    public Brush BorderColor {
        get { return (Brush)GetValue(BorderColorProperty); }
        set { SetValue(BorderColorProperty, value); }
    }

    public Brush BackgroundColor {
        get { return (Brush)GetValue(BackgroundColorProperty); }
        set { SetValue(BackgroundColorProperty, value); }
    }

    public static readonly DependencyProperty IconProperty =
        DependencyProperty.Register("Icon", typeof(string), typeof(UiLabelIcon), new PropertyMetadata("\xe63b"));
    public static readonly DependencyProperty BorderColorProperty =
        DependencyProperty.Register("BorderColor", typeof(Brush), typeof(UiLabelIcon), new PropertyMetadata(new SolidColorBrush(Color.FromRgb(0x67, 0xc2, 0x3a)))); // #67C23A
    public static readonly DependencyProperty BackgroundColorProperty =
        DependencyProperty.Register("BackgroundColor", typeof(Brush), typeof(UiLabelIcon), new PropertyMetadata(new SolidColorBrush(Color.FromRgb(0xB3, 0xe1, 0x9d)))); // #B3E19D
}
