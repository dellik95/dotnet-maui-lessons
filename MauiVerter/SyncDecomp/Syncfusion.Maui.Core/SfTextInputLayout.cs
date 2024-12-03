using System;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Maui;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Devices;
using Microsoft.Maui.Graphics;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Syncfusion.Maui.Core.Internals;
using Syncfusion.Maui.Graphics.Internals;

namespace Syncfusion.Maui.Core;

[ContentProperty("Content")]
public class SfTextInputLayout : SfContentView, ITouchListener
{
	private static readonly BindableProperty IsLayoutFocusedProperty = BindableProperty.Create(nameof(IsLayoutFocused), typeof(bool), typeof(SfTextInputLayout), false, BindingMode.Default, null, OnIsLayoutFocusedChanged);

	public static readonly BindableProperty ContainerTypeProperty = BindableProperty.Create(nameof(ContainerType), typeof(ContainerType), typeof(SfTextInputLayout), ContainerType.Filled, BindingMode.Default, null, OnContainerTypePropertyChanged);

	public static readonly BindableProperty LeadingViewProperty = BindableProperty.Create(nameof(LeadingView), typeof(View), typeof(SfTextInputLayout), null, BindingMode.Default, null, OnLeadingViewChanged);

	public static readonly BindableProperty TrailingViewProperty = BindableProperty.Create(nameof(TrailingView), typeof(View), typeof(SfTextInputLayout), null, BindingMode.Default, null, OnTrailingViewChanged);

	public static readonly BindableProperty ShowLeadingViewProperty = BindableProperty.Create(nameof(ShowLeadingView), typeof(bool), typeof(SfTextInputLayout), true, BindingMode.Default, null, OnShowLeadingViewPropertyChanged);

	public static readonly BindableProperty ShowTrailingViewProperty = BindableProperty.Create(nameof(ShowTrailingView), typeof(bool), typeof(SfTextInputLayout), true, BindingMode.Default, null, OnShowTrailingViewPropertyChanged);

	public static readonly BindableProperty ShowHintProperty = BindableProperty.Create(nameof(ShowHint), typeof(bool), typeof(SfTextInputLayout), true, BindingMode.Default, null, OnPropertyChanged);

	internal static readonly BindableProperty ShowCharCountProperty = BindableProperty.Create(nameof(ShowCharCount), typeof(bool), typeof(SfTextInputLayout), false, BindingMode.Default, null, OnPropertyChanged);

	public static readonly BindableProperty ShowHelperTextProperty = BindableProperty.Create(nameof(ShowHelperText), typeof(bool), typeof(SfTextInputLayout), true, BindingMode.TwoWay, null, OnPropertyChanged);

	public static readonly BindableProperty HasErrorProperty = BindableProperty.Create(nameof(HasError), typeof(bool), typeof(SfTextInputLayout), false, BindingMode.Default, null, OnHasErrorPropertyChanged);

	public static readonly BindableProperty IsHintAlwaysFloatedProperty = BindableProperty.Create(nameof(IsHintAlwaysFloated), typeof(bool), typeof(SfTextInputLayout), false, BindingMode.Default, null, OnHintAlwaysFloatedPropertyChanged);

	public static readonly BindableProperty StrokeProperty = BindableProperty.Create(nameof(Stroke), typeof(Color), typeof(SfTextInputLayout), Color.FromRgba("79747E"), BindingMode.Default, null, OnPropertyChanged);

	public static readonly BindableProperty ContainerBackgroundProperty = BindableProperty.Create(nameof(ContainerBackground), typeof(Brush), typeof(SfTextInputLayout), new SolidColorBrush(Color.FromRgba("E7E0EC")), BindingMode.Default, null, OnPropertyChanged);

	public static readonly BindableProperty OutlineCornerRadiusProperty = BindableProperty.Create(nameof(OutlineCornerRadius), typeof(double), typeof(SfTextInputLayout), 3.5, BindingMode.Default, null, OnPropertyChanged);

	public static readonly BindableProperty FocusedStrokeThicknessProperty = BindableProperty.Create(nameof(FocusedStrokeThickness), typeof(double), typeof(SfTextInputLayout), 2.0, BindingMode.Default, null, OnPropertyChanged);

	public static readonly BindableProperty UnfocusedStrokeThicknessProperty = BindableProperty.Create(nameof(UnfocusedStrokeThickness), typeof(double), typeof(SfTextInputLayout), 1.0, BindingMode.Default, null, OnPropertyChanged);

	public static readonly BindableProperty CharMaxLengthProperty = BindableProperty.Create(nameof(CharMaxLength), typeof(int), typeof(SfTextInputLayout), int.MaxValue, BindingMode.Default, null, OnCharMaxLengthPropertyChanged);

	public static readonly BindableProperty HintProperty = BindableProperty.Create(nameof(Hint), typeof(string), typeof(SfTextInputLayout), string.Empty, BindingMode.Default, null, OnPropertyChanged);

	public static readonly BindableProperty HelperTextProperty = BindableProperty.Create(nameof(HelperText), typeof(string), typeof(SfTextInputLayout), string.Empty, BindingMode.Default, null, OnPropertyChanged);

	public static readonly BindableProperty ErrorTextProperty = BindableProperty.Create(nameof(ErrorText), typeof(string), typeof(SfTextInputLayout), string.Empty, BindingMode.Default, null, OnPropertyChanged);

	public static readonly BindableProperty ReserveSpaceForAssistiveLabelsProperty = BindableProperty.Create(nameof(ReserveSpaceForAssistiveLabels), typeof(bool), typeof(SfTextInputLayout), true, BindingMode.Default, null, OnReserveSpacePropertyChanged);

	public static readonly BindableProperty LeadingViewPositionProperty = BindableProperty.Create(nameof(LeadingViewPosition), typeof(ViewPosition), typeof(SfTextInputLayout), ViewPosition.Inside, BindingMode.Default, null, OnPropertyChanged);

	public static readonly BindableProperty TrailingViewPositionProperty = BindableProperty.Create(nameof(TrailingViewPosition), typeof(ViewPosition), typeof(SfTextInputLayout), ViewPosition.Inside, BindingMode.Default, null, OnPropertyChanged);

	public static readonly BindableProperty InputViewPaddingProperty = BindableProperty.Create(nameof(InputViewPadding), typeof(Microsoft.Maui.Thickness), typeof(SfTextInputLayout), new Microsoft.Maui.Thickness(-1.0, -1.0, -1.0, -1.0), BindingMode.Default, null, OnInputViewPaddingPropertyChanged);

	public static readonly BindableProperty EnablePasswordVisibilityToggleProperty = BindableProperty.Create(nameof(EnablePasswordVisibilityToggle), typeof(bool), typeof(SfTextInputLayout), false, BindingMode.Default, null, OnEnablePasswordTogglePropertyChanged);

	internal static readonly BindableProperty ShowClearButtonProperty = BindableProperty.Create(nameof(ShowClearButton), typeof(bool), typeof(SfTextInputLayout), false, BindingMode.Default, null, OnPropertyChanged);

	public static readonly BindableProperty EnableFloatingProperty = BindableProperty.Create(nameof(EnableFloating), typeof(bool), typeof(SfTextInputLayout), true, BindingMode.Default, null, OnPropertyChanged);

	public static readonly BindableProperty EnableHintAnimationProperty = BindableProperty.Create(nameof(EnableHintAnimation), typeof(bool), typeof(SfTextInputLayout), true, BindingMode.Default, null, OnPropertyChanged);

	internal static readonly BindableProperty ShowDropDownButtonProperty = BindableProperty.Create(nameof(ShowDropDownButton), typeof(bool), typeof(SfTextInputLayout), false, BindingMode.Default, null, OnPropertyChanged);

	internal static readonly BindableProperty ClearButtonColorProperty = BindableProperty.Create(nameof(ClearButtonColor), typeof(Color), typeof(SfTextInputLayout), Colors.Grey, BindingMode.Default, null, OnPropertyChanged);

	internal static readonly BindableProperty DropDownButtonColorProperty = BindableProperty.Create(nameof(DropDownButtonColor), typeof(Color), typeof(SfTextInputLayout), Colors.Grey, BindingMode.Default, null, OnPropertyChanged);

	public static readonly BindableProperty HintLabelStyleProperty = BindableProperty.Create(nameof(HintLabelStyle), typeof(LabelStyle), typeof(SfTextInputLayout), null, BindingMode.TwoWay, null, OnHintLableStylePropertyChanged, null, null, (BindableObject bindale) => GetHintLabelStyleDefaultValue());

	public static readonly BindableProperty HelperLabelStyleProperty = BindableProperty.Create(nameof(HelperLabelStyle), typeof(LabelStyle), typeof(SfTextInputLayout), null, BindingMode.TwoWay, null, OnHelperLableStylePropertyChanged, null, null, (BindableObject bindale) => GetHelperLabelStyleDefaultValue());

	public static readonly BindableProperty ErrorLabelStyleProperty = BindableProperty.Create(nameof(ErrorLabelStyle), typeof(LabelStyle), typeof(SfTextInputLayout), null, BindingMode.TwoWay, null, OnErrorLableStylePropertyChanged, null, null, (BindableObject bindale) => GetErrorLabelStyleDefaultValue());

	internal static readonly BindableProperty CounterLabelStyleProperty = BindableProperty.Create(nameof(CounterLabelStyle), typeof(LabelStyle), typeof(SfTextInputLayout), null, BindingMode.TwoWay, null, OnCounterLableStylePropertyChanged, null, null, (BindableObject bindale) => GetCounterLabelStyleDefaultValue());

	internal static readonly BindablePropertyKey CurrentActiveColorKey = BindableProperty.CreateReadOnly(nameof(CurrentActiveColor), typeof(Color), typeof(SfTextInputLayout), Color.FromRgba("79747E"), BindingMode.Default);

	internal static readonly BindableProperty IsHintFloatedProperty = BindableProperty.Create(nameof(IsHintFloated), typeof(bool), typeof(SfTextInputLayout), false, BindingMode.Default, null, OnIsHintFloatedPropertyChanged);

	public static readonly BindableProperty CurrentActiveColorProperty = CurrentActiveColorKey.BindableProperty;

	public new static readonly BindableProperty IsEnabledProperty = BindableProperty.Create(nameof(IsEnabled), typeof(bool), typeof(SfTextInputLayout), true, BindingMode.Default, null, OnEnabledPropertyChanged);

	private const double DefaultAssistiveLabelPadding = 4.0;

	private const double DefaultAssistiveTextHeight = 16.0;

	private const double DefaultHintTextHeight = 24.0;

	private const double FilledTopPadding = 24.0;

	private const double FilledBottomPadding = 10.0;

	private const double NoneBottomPadding = 5.0;

	private const double NoneTopPadding = 32.0;

	private const double OutlinedPadding = 16.0;

	private const double EdgePadding = 16.0;

	private const double RightPadding = 16.0;

	private const float StartX = 12f;

	private const float DefaultHintFontSize = 16f;

	private const float FloatedHintFontSize = 12f;

	private const double DefaultAnimationDuration = 167.0;

	private const int IconSize = 32;

	private const float CounterTextPadding = 12f;

	private string counterText = "0";

	private bool isAnimating = false;

	private bool isNonEditableContent = false;

	private double animatingFontSize;

	private double leadingViewLeftPadding = 8.0;

	private double leadingViewRightPadding = 12.0;

	private double trailingViewLeftPadding = 12.0;

	private double trailingViewRightPadding = 8.0;

	private double defaultLeadAndTrailViewWidth = 24.0;

	private double leadViewWidth = 0.0;

	private double trailViewWidth = 0.0;

	private float passwordToggleIconEdgePadding = 8f;

	private float passwordToggleCollapsedTopPadding = 9f;

	private float passwordToggleVisibleTopPadding = 10f;

	private string toggleVisibleIconPath = "M8,3.3000005C9.2070007,3.3000005 10.181999,4.2819988 10.181999,5.5000006 10.181999,6.7179991 9.2070007,7.6999975 8,7.6999975 6.7929993,7.6999975 5.8180008,6.7179991 5.8180008,5.5000006 5.8180008,4.2819988 6.7929993,3.3000005 8,3.3000005z M8,1.8329997C5.993,1.8329997 4.3639984,3.475999 4.3639984,5.5000006 4.3639984,7.5249983 5.993,9.1669999 8,9.1669999 10.007,9.1669999 11.636002,7.5249983 11.636002,5.5000006 11.636002,3.475999 10.007,1.8329997 8,1.8329997z M8,0C11.636002,-1.1919138E-07 14.742001,2.2800001 16,5.5000006 14.742001,8.7199975 11.636002,11 8,11 4.3639984,11 1.2579994,8.7199975 0,5.5000006 1.2579994,2.2800001 4.3639984,-1.1919138E-07 8,0z";

	private string toggleCollapsedIconPath = "M4.7510004,4.9479995C4.5109997,5.4359995 4.3660002,5.9739994 4.3660002,6.5489993 4.3660002,8.5569992 5.9949999,10.186999 8.0030003,10.186999 8.5780001,10.186999 9.1160002,10.040999 9.6040001,9.8009992L8.4760003,8.6729991C8.3239999,8.7099991 8.1630001,8.7319992 8.0030003,8.7319992 6.796,8.7319992 5.8210001,7.7569993 5.8210001,6.5489993 5.8210001,6.3889993 5.8429999,6.2289994 5.8790002,6.0759995z M8.0110002,4.3729997C9.2189999,4.3729995,10.194,5.3479995,10.194,6.5559993L10.179,6.6719995 7.8870001,4.3809996z M7.9960003,1.092C11.634,1.092 14.741,3.3549995 16,6.5489993 15.469,7.9019992 14.603,9.0879991 13.504,10.004999L11.38,7.8799992C11.547,7.4659992 11.641,7.0219994 11.641,6.5489993 11.641,4.5399996 10.012,2.9099996 8.0030003,2.9099996 7.5310001,2.9099996 7.0869999,3.0049996 6.6719999,3.1729996L5.1000004,1.6009998C6.0030003,1.2739997,6.9770002,1.092,7.9960003,1.092z M1.6520004,0L14.552,12.900999 13.628,13.823998 11.496,11.699999 11.19,11.394999C10.208,11.786999 9.131,12.005999 8.0030003,12.005999 4.3660002,12.005999 1.2589998,9.7429991 0,6.5489993 0.56700039,5.1089995 1.5130005,3.8569996 2.7209997,2.9179997L2.3870001,2.5829997 0.72700024,0.92399979z";

	private bool isPasswordTextVisible = false;

	private bool isHintDownToUp = true;

	private double translateStart = 0.0;

	private double translateEnd = 1.0;

	private double fontSizeStart = 16.0;

	private double fontSizeEnd = 12.0;

	private float fontsize = 16f;

	private EffectsRenderer effectsenderer;

	private PathBuilder pathBuilder = new PathBuilder();

	private RectF primaryIconRectF = default(RectF);

	private RectF secondaryIconRectF = default(RectF);

	private RectF outlineRectF = default(RectF);

	private RectF hintRect = default(RectF);

	private RectF backgroundRectF = default(RectF);

	private RectF clipRect = default(RectF);

	private RectF viewBounds = default(RectF);

	private PointF startPoint = default(PointF);

	private PointF endPoint = default(PointF);

	private RectF helperTextRect = default(RectF);

	private RectF errorTextRect = default(RectF);

	private RectF counterTextRect = default(RectF);

	private LabelStyle internalHintLabelStyle = new LabelStyle();

	private LabelStyle internalHelperLabelStyle = new LabelStyle();

	private LabelStyle internalErrorLabelStyle = new LabelStyle();

	private LabelStyle internalCounterLabelStyle = new LabelStyle();

	private IDropdownRenderer? tilRenderer;

	private SfDropdownEntry? dropdownEntry;

	private Rect? oldBounds;

	public bool ShowHint
	{
		get
		{
			return (bool)GetValue(ShowHintProperty);
		}
		set
		{
			SetValue(ShowHintProperty, value);
		}
	}

	internal bool ShowCharCount
	{
		get
		{
			return (bool)GetValue(ShowCharCountProperty);
		}
		set
		{
			SetValue(ShowCharCountProperty, value);
		}
	}

	public bool ShowHelperText
	{
		get
		{
			return (bool)GetValue(ShowHelperTextProperty);
		}
		set
		{
			SetValue(ShowHelperTextProperty, value);
		}
	}

	public bool HasError
	{
		get
		{
			return (bool)GetValue(HasErrorProperty);
		}
		set
		{
			SetValue(HasErrorProperty, value);
		}
	}

	public Color Stroke
	{
		get
		{
			return (Color)GetValue(StrokeProperty);
		}
		set
		{
			SetValue(StrokeProperty, value);
		}
	}

	internal float HintFontSize { get; set; }

	public Brush ContainerBackground
	{
		get
		{
			return (Brush)GetValue(ContainerBackgroundProperty);
		}
		set
		{
			SetValue(ContainerBackgroundProperty, value);
		}
	}

	public double OutlineCornerRadius
	{
		get
		{
			return (double)GetValue(OutlineCornerRadiusProperty);
		}
		set
		{
			SetValue(OutlineCornerRadiusProperty, value);
		}
	}

	public double FocusedStrokeThickness
	{
		get
		{
			return (double)GetValue(FocusedStrokeThicknessProperty);
		}
		set
		{
			SetValue(FocusedStrokeThicknessProperty, value);
		}
	}

	public double UnfocusedStrokeThickness
	{
		get
		{
			return (double)GetValue(UnfocusedStrokeThicknessProperty);
		}
		set
		{
			SetValue(UnfocusedStrokeThicknessProperty, value);
		}
	}

	public int CharMaxLength
	{
		get
		{
			return (int)GetValue(CharMaxLengthProperty);
		}
		set
		{
			SetValue(CharMaxLengthProperty, value);
		}
	}

	public View LeadingView
	{
		get
		{
			return (View)GetValue(LeadingViewProperty);
		}
		set
		{
			SetValue(LeadingViewProperty, value);
		}
	}

	public View TrailingView
	{
		get
		{
			return (View)GetValue(TrailingViewProperty);
		}
		set
		{
			SetValue(TrailingViewProperty, value);
		}
	}

	public bool ShowLeadingView
	{
		get
		{
			return (bool)GetValue(ShowLeadingViewProperty);
		}
		set
		{
			SetValue(ShowLeadingViewProperty, value);
		}
	}

	public bool ShowTrailingView
	{
		get
		{
			return (bool)GetValue(ShowTrailingViewProperty);
		}
		set
		{
			SetValue(ShowTrailingViewProperty, value);
		}
	}

	public ViewPosition LeadingViewPosition
	{
		get
		{
			return (ViewPosition)GetValue(LeadingViewPositionProperty);
		}
		set
		{
			SetValue(LeadingViewPositionProperty, value);
		}
	}

	public ViewPosition TrailingViewPosition
	{
		get
		{
			return (ViewPosition)GetValue(TrailingViewPositionProperty);
		}
		set
		{
			SetValue(TrailingViewPositionProperty, value);
		}
	}

	public Microsoft.Maui.Thickness InputViewPadding
	{
		get
		{
			return (Microsoft.Maui.Thickness)GetValue(InputViewPaddingProperty);
		}
		set
		{
			SetValue(InputViewPaddingProperty, value);
		}
	}

	private bool IsLayoutFocused
	{
		get
		{
			return (bool)GetValue(IsLayoutFocusedProperty);
		}
		set
		{
			SetValue(IsLayoutFocusedProperty, value);
		}
	}

	public bool IsHintAlwaysFloated
	{
		get
		{
			return (bool)GetValue(IsHintAlwaysFloatedProperty);
		}
		set
		{
			SetValue(IsHintAlwaysFloatedProperty, value);
		}
	}

	public string Hint
	{
		get
		{
			return (string)GetValue(HintProperty);
		}
		set
		{
			SetValue(HintProperty, value);
		}
	}

	public string HelperText
	{
		get
		{
			return (string)GetValue(HelperTextProperty);
		}
		set
		{
			SetValue(HelperTextProperty, value);
		}
	}

	public string ErrorText
	{
		get
		{
			return (string)GetValue(ErrorTextProperty);
		}
		set
		{
			SetValue(ErrorTextProperty, value);
		}
	}

	public bool ReserveSpaceForAssistiveLabels
	{
		get
		{
			return (bool)GetValue(ReserveSpaceForAssistiveLabelsProperty);
		}
		set
		{
			SetValue(ReserveSpaceForAssistiveLabelsProperty, value);
		}
	}

	public ContainerType ContainerType
	{
		get
		{
			return (ContainerType)GetValue(ContainerTypeProperty);
		}
		set
		{
			SetValue(ContainerTypeProperty, value);
		}
	}

	public bool EnablePasswordVisibilityToggle
	{
		get
		{
			return (bool)GetValue(EnablePasswordVisibilityToggleProperty);
		}
		set
		{
			SetValue(EnablePasswordVisibilityToggleProperty, value);
		}
	}

	public Color CurrentActiveColor
	{
		get
		{
			return (Color)GetValue(CurrentActiveColorProperty);
		}
		internal set
		{
			SetValue(CurrentActiveColorKey, value);
		}
	}

	public new bool IsEnabled
	{
		get
		{
			return (bool)GetValue(IsEnabledProperty);
		}
		set
		{
			SetValue(IsEnabledProperty, value);
		}
	}

	internal bool ShowDropDownButton
	{
		get
		{
			return (bool)GetValue(ShowDropDownButtonProperty);
		}
		set
		{
			SetValue(ShowDropDownButtonProperty, value);
		}
	}

	internal Color DropDownButtonColor
	{
		get
		{
			return (Color)GetValue(DropDownButtonColorProperty);
		}
		set
		{
			SetValue(DropDownButtonColorProperty, value);
		}
	}

	internal bool ShowClearButton
	{
		get
		{
			return (bool)GetValue(ShowClearButtonProperty);
		}
		set
		{
			SetValue(ShowClearButtonProperty, value);
		}
	}

	public bool EnableFloating
	{
		get
		{
			return (bool)GetValue(EnableFloatingProperty);
		}
		set
		{
			SetValue(EnableFloatingProperty, value);
		}
	}

	public bool EnableHintAnimation
	{
		get
		{
			return (bool)GetValue(EnableHintAnimationProperty);
		}
		set
		{
			SetValue(EnableHintAnimationProperty, value);
		}
	}

	internal Color ClearButtonColor
	{
		get
		{
			return (Color)GetValue(ClearButtonColorProperty);
		}
		set
		{
			SetValue(ClearButtonColorProperty, value);
		}
	}

	public LabelStyle HintLabelStyle
	{
		get
		{
			return (LabelStyle)GetValue(HintLabelStyleProperty);
		}
		set
		{
			SetValue(HintLabelStyleProperty, value);
		}
	}

	public LabelStyle HelperLabelStyle
	{
		get
		{
			return (LabelStyle)GetValue(HelperLabelStyleProperty);
		}
		set
		{
			SetValue(HelperLabelStyleProperty, value);
		}
	}

	public LabelStyle ErrorLabelStyle
	{
		get
		{
			return (LabelStyle)GetValue(ErrorLabelStyleProperty);
		}
		set
		{
			SetValue(ErrorLabelStyleProperty, value);
		}
	}

	internal LabelStyle CounterLabelStyle
	{
		get
		{
			return (LabelStyle)GetValue(CounterLabelStyleProperty);
		}
		set
		{
			SetValue(CounterLabelStyleProperty, value);
		}
	}

	public string? Text { get; private set; }

	internal bool IsOutlined => ContainerType == ContainerType.Outlined;

	internal bool IsNone => ContainerType == ContainerType.None;

	internal bool IsFilled => ContainerType == ContainerType.Filled;

	private bool IsHintFloated
	{
		get
		{
			return (bool)GetValue(IsHintFloatedProperty);
		}
		set
		{
			SetValue(IsHintFloatedProperty, value);
		}
	}

	private Color DisabledColor => Color.FromUint(1107296256u);

	internal SizeF HintSize
	{
		get
		{
			if (string.IsNullOrEmpty(Hint) || HintLabelStyle == null)
			{
				return new Size(0.0, 24.0);
			}
			internalHintLabelStyle.FontSize = HintFontSize;
			Size size = Hint.Measure(internalHintLabelStyle);
			size.Height = 24.0;
			size.Width += 8.0;
			return size;
		}
	}

	internal SizeF FloatedHintSize
	{
		get
		{
			if (string.IsNullOrEmpty(Hint) || HintLabelStyle == null)
			{
				return new Size(0.0, 16.0);
			}
			internalHintLabelStyle.FontSize = 12.0;
			Size size = Hint.Measure(internalHintLabelStyle);
			size.Height = 16.0;
			size.Width += 8.0;
			return size;
		}
	}

	internal SizeF CounterTextSize
	{
		get
		{
			if (string.IsNullOrEmpty(counterText) || CounterLabelStyle == null)
			{
				return GetLabelSize(new Size(0.0, 16.0));
			}
			Size size = counterText.Measure(internalCounterLabelStyle);
			size.Height = 16.0;
			size.Width += 4.0;
			return GetLabelSize(size);
		}
	}

	internal SizeF HelperTextSize
	{
		get
		{
			if (string.IsNullOrEmpty(HelperText) || HelperLabelStyle == null)
			{
				return GetLabelSize(new Size(0.0, 16.0));
			}
			Size size = HelperText.Measure(internalHelperLabelStyle);
			size.Height = 16.0;
			return GetLabelSize(size);
		}
	}

	internal SizeF ErrorTextSize
	{
		get
		{
			if (string.IsNullOrEmpty(ErrorText) || ErrorLabelStyle == null)
			{
				return GetLabelSize(new Size(0.0, 16.0));
			}
			Size size = ErrorText.Measure(internalErrorLabelStyle);
			size.Height = 16.0;
			return GetLabelSize(size);
		}
	}

	internal double BaseLineMaxHeight => Math.Max(FocusedStrokeThickness, UnfocusedStrokeThickness);

	private double TopPadding => IsOutlined ? (16.0 + (double)(FloatedHintSize.Height / 2f)) : (IsFilled ? 24.0 : 32.0);

	private double BottomPadding => (IsFilled ? 10.0 : (IsOutlined ? 16.0 : 5.0)) + ((!ReserveSpaceForAssistiveLabels) ? 0.0 : (string.IsNullOrEmpty(HelperText) ? ((double)ErrorTextSize.Height) : ((double)HelperTextSize.Height + 4.0)));

	private double LeftPadding => IsNone ? 0.0 : 16.0;

	private double AssistiveLabelPadding => ReserveSpaceForAssistiveLabels ? 4.0 : BaseLineMaxHeight;

	private PathF ToggleIconPath => isPasswordTextVisible ? pathBuilder.BuildPath(toggleVisibleIconPath) : pathBuilder.BuildPath(toggleCollapsedIconPath);

	private bool isRTL => (((IVisualElementController)this).EffectiveFlowDirection & EffectiveFlowDirection.RightToLeft) == EffectiveFlowDirection.RightToLeft;

	private bool IsDropDownIconVisible => ShowDropDownButton && base.Content is SfDropdownEntry;

	private bool IsClearIconVisible => ShowClearButton && IsLayoutFocused && !isNonEditableContent && !string.IsNullOrEmpty(Text);

	private bool IsPassowordToggleIconVisible => (IsLayoutFocused || IsHintFloated) && EnablePasswordVisibilityToggle && base.Content is Entry;

	private static LabelStyle GetHintLabelStyleDefaultValue()
	{
		return new LabelStyle
		{
			FontSize = 16.0
		};
	}

	private static LabelStyle GetHelperLabelStyleDefaultValue()
	{
		return new LabelStyle();
	}

	private static LabelStyle GetErrorLabelStyleDefaultValue()
	{
		return new LabelStyle();
	}

	private static LabelStyle GetCounterLabelStyleDefaultValue()
	{
		return new LabelStyle();
	}

	public SfTextInputLayout()
	{
		base.DrawingOrder = DrawingOrder.BelowContent;
		HintFontSize = 16f;
		SetRendererBasedOnPlatform();
		this.AddTouchListener(this);
		effectsenderer = new EffectsRenderer(this);
		base.PropertyChanged += SfTextInputLayout_PropertyChanged;
		AddDefaultVSM();
		UpdateViewBounds();
	}

	async void ITouchListener.OnTouch(PointerEventArgs e)
	{
		Point touchPoint;
		if (isRTL && DeviceInfo.Platform == DevicePlatform.WinUI)
		{
			touchPoint = e.TouchPoint;
			touchPoint.X = base.Width - touchPoint.X;
		}
		else
		{
			touchPoint = e.TouchPoint;
		}
		if (e.Action != PointerActions.Released)
		{
			return;
		}
		if ((EnablePasswordVisibilityToggle || ShowDropDownButton) && primaryIconRectF.Contains(touchPoint))
		{
			ToggleIcon();
		}
		else if (IsClearIconVisible && secondaryIconRectF.Contains(touchPoint) && IsLayoutFocused)
		{
			ClearText();
		}
		else
		{
			if (base.Content == null || base.Content.IsFocused || ((!IsOutlined || !outlineRectF.Contains(touchPoint)) && ((!IsFilled && !IsNone) || !backgroundRectF.Contains(touchPoint))))
			{
				return;
			}
			SfDropdownEntry dropdownEntry = default(SfDropdownEntry);
			int num;
			View content;
			if (isNonEditableContent)
			{
				content = base.Content;
				dropdownEntry = content as SfDropdownEntry;
				num = ((dropdownEntry != null) ? 1 : 0);
			}
			else
			{
				num = 0;
			}
			if (num != 0)
			{
				dropdownEntry.IsDropDownOpen = true;
				return;
			}
			await Task.Delay(1);
			content = base.Content;
			if (content is InputView inputView)
			{
				inputView.Focus();
				return;
			}
			content = base.Content;
			SfDropdownEntry dropdownentry = content as SfDropdownEntry;
			if (dropdownentry != null && dropdownentry.InputView != null)
			{
				dropdownentry.InputView.Focus();
			}
		}
	}

	public new void Focus()
	{
		if (base.Content is InputView inputView)
		{
			inputView.Focus();
		}
		if (base.Content is SfDropdownEntry sfDropdownEntry)
		{
			sfDropdownEntry.Focus();
		}
	}

	public new void Unfocus()
	{
		if (base.Content is InputView inputView)
		{
			inputView.Unfocus();
		}
		if (base.Content is SfDropdownEntry sfDropdownEntry)
		{
			sfDropdownEntry.Unfocus();
		}
	}

	internal double GetLeftPadding()
	{
		return (InputViewPadding.Left < 0.0) ? LeftPadding : InputViewPadding.Left;
	}

	internal double GetTopPadding()
	{
		double num = TopPadding;
		if (DeviceInfo.Platform == DevicePlatform.WinUI && !(base.Content is Editor))
		{
			num -= 6.0;
		}
		return (InputViewPadding.Top < 0.0) ? num : (InputViewPadding.Top + GetDefaultTopPadding());
	}

	internal double GetRightPadding()
	{
		return (!(InputViewPadding.Right < 0.0)) ? InputViewPadding.Right : (isNonEditableContent ? BaseLineMaxHeight : 16.0);
	}

	internal double GetBottomPadding()
	{
		double num = BottomPadding;
		if (DeviceInfo.Platform == DevicePlatform.WinUI && !(base.Content is Editor))
		{
			num -= 10.0;
		}
		return (InputViewPadding.Bottom < 0.0) ? num : (InputViewPadding.Bottom + GetDefaultBottomPadding());
	}

	private double GetDefaultTopPadding()
	{
		return (IsFilled ? 16.0 : 8.0) + ((DeviceInfo.Platform != DevicePlatform.WinUI) ? BaseLineMaxHeight : 0.0);
	}

	private double GetDefaultBottomPadding()
	{
		return ReserveSpaceForAssistiveLabels ? 20.0 : ((DeviceInfo.Platform != DevicePlatform.WinUI) ? BaseLineMaxHeight : 0.0);
	}

	private void UpdateIconVisibility()
	{
		if (base.Content is InputView)
		{
			ShowClearButton = false;
			ShowDropDownButton = false;
		}
	}

	private void UpdateContentMargin(View view)
	{
		if (view != null)
		{
			base.Content.Margin = new Microsoft.Maui.Thickness
			{
				Top = GetTopPadding(),
				Bottom = GetBottomPadding(),
				Left = GetLeftPadding(),
				Right = GetRightPadding()
			};
		}
	}

	protected override void OnContentChanged(object oldValue, object newValue)
	{
		if ((newValue != null && newValue is InputView) || (newValue != null && newValue is SfDropdownEntry sfDropdownEntry && sfDropdownEntry.InputView != null))
		{
			InputView inputView = newValue as InputView;
			if (newValue is SfDropdownEntry sfDropdownEntry2)
			{
				dropdownEntry = sfDropdownEntry2;
				isNonEditableContent = !sfDropdownEntry2.IsEditableMode;
				inputView = dropdownEntry.InputView;
			}
			if (inputView != null)
			{
				if (!isNonEditableContent)
				{
					inputView.Focused += InputViewFocused;
					inputView.Unfocused += InputViewUnfocused;
				}
				inputView.BackgroundColor = Colors.Transparent;
				inputView.TextChanged += InputViewTextChanged;
				inputView.HandlerChanged += InputView_HandlerChanged;
				if (!string.IsNullOrEmpty(inputView.Text))
				{
					IsHintFloated = true;
					isHintDownToUp = false;
					Text = inputView.Text;
				}
			}
			if (newValue is Entry entry && EnablePasswordVisibilityToggle)
			{
				entry.IsPassword = true;
				isPasswordTextVisible = false;
			}
			if (dropdownEntry != null)
			{
				dropdownEntry.BackgroundColor = Colors.Transparent;
				if (dropdownEntry.DropDownView != null)
				{
					dropdownEntry.DropDownView.AnchorView = this;
					dropdownEntry.DropDownView.PopupOpened += DropDownView_PopupOpened;
					dropdownEntry.DropDownView.PopupClosed += DropDownView_PopupClosed;
				}
			}
		}
		else if (newValue != null)
		{
			IsHintAlwaysFloated = true;
		}
		if (newValue is View view)
		{
			UpdateContentMargin(view);
		}
		if (newValue is InputView inputView2)
		{
			inputView2.Opacity = (IsHintFloated ? 1.0 : ((DeviceInfo.Platform == DevicePlatform.iOS || DeviceInfo.Platform == DevicePlatform.MacCatalyst || DeviceInfo.Platform == DevicePlatform.macOS) ? 0.02 : 0.0));
		}
		if (newValue is SfDropdownEntry sfDropdownEntry3 && sfDropdownEntry3.InputView != null)
		{
			if (DeviceInfo.Platform == DevicePlatform.Android)
			{
				sfDropdownEntry3.Opacity = (IsHintFloated ? 1 : 0);
			}
			else
			{
				sfDropdownEntry3.InputView.Opacity = (IsHintFloated ? 1.0 : ((DeviceInfo.Platform == DevicePlatform.iOS || DeviceInfo.Platform == DevicePlatform.MacCatalyst || DeviceInfo.Platform == DevicePlatform.macOS) ? 0.02 : 0.0));
			}
		}
		UpdateIconVisibility();
		UpdateViewBounds();
		base.OnContentChanged(oldValue, newValue);
		if (!IsEnabled)
		{
			OnEnabledPropertyChanged(IsEnabled);
		}
	}

	protected override Size MeasureContent(double widthConstraint, double heightConstraint)
	{
		base.MeasureContent(widthConstraint, heightConstraint);
		double num = 0.0;
		double num2 = 0.0;
		Size size = new Size(0.0, 0.0);
		if (base.Content != null)
		{
			size = base.Content.Measure(widthConstraint, heightConstraint, MeasureFlags.IncludeMargins);
		}
		num = ((widthConstraint != -1.0 && widthConstraint != double.PositiveInfinity) ? widthConstraint : size.Width);
		num2 = ((heightConstraint != -1.0 && heightConstraint != double.PositiveInfinity) ? heightConstraint : size.Height);
		return new Size(num, num2);
	}

	protected override Size ArrangeContent(Rect bounds)
	{
		if (!oldBounds.HasValue)
		{
			oldBounds = bounds;
			UpdateViewBounds();
		}
		else if (oldBounds?.Width != bounds.Width || oldBounds?.Height != bounds.Height || oldBounds?.X != bounds.X || oldBounds?.Y != bounds.Y)
		{
			UpdateViewBounds();
		}
		return base.ArrangeContent(bounds);
	}

	protected override void OnDraw(ICanvas canvas, RectF dirtyRect)
	{
		base.OnDraw(canvas, dirtyRect);
		canvas.SaveState();
		canvas.Antialias = true;
		UpdateIconRectF();
		DrawBorder(canvas, dirtyRect);
		DrawHintText(canvas, dirtyRect);
		DrawAssistiveText(canvas, dirtyRect);
		DrawClearIcon(canvas, dirtyRect);
		DrawPasswordToggleIcon(canvas, dirtyRect);
		DrawDropDownIcon(canvas, dirtyRect);
		UpdatePopupPositions();
		effectsenderer.DrawEffects(canvas);
		UpdateContentPosition();
		canvas.ResetState();
	}

	private void UpdatePopupPositions()
	{
		if (dropdownEntry?.DropDownView != null)
		{
			dropdownEntry.DropDownView.PopupX = GetPopupX();
			dropdownEntry.DropDownView.PopupY = GetPopupY();
			dropdownEntry.DropDownView.PopupWidth = GetPopupWidth();
		}
	}

	protected override void ChangeVisualState()
	{
		base.ChangeVisualState();
		string name = (HasError ? "Error" : (IsLayoutFocused ? "Focused" : "Normal"));
		Microsoft.Maui.Controls.VisualStateManager.GoToState(this, name);
		if (!HasError)
		{
			CurrentActiveColor = (IsEnabled ? Stroke : DisabledColor);
		}
	}

	protected override void OnHandlerChanged()
	{
		base.OnHandlerChanged();
		UnhookEvents();
		if (base.Handler != null)
		{
			HookEvents();
		}
	}

	private static void OnLeadingViewChanged(BindableObject bindable, object oldValue, object newValue)
	{
		(bindable as SfTextInputLayout)?.AddView(oldValue, newValue);
		(bindable as SfTextInputLayout)?.InvalidateDrawable();
	}

	private static void OnTrailingViewChanged(BindableObject bindable, object oldValue, object newValue)
	{
		(bindable as SfTextInputLayout)?.AddView(oldValue, newValue);
		(bindable as SfTextInputLayout)?.InvalidateDrawable();
	}

	private static void OnHintAlwaysFloatedPropertyChanged(BindableObject bindable, object oldValue, object newValue)
	{
		if (!(bindable is SfTextInputLayout sfTextInputLayout) || !(newValue is bool))
		{
			return;
		}
		bool flag = (bool)newValue;
		if (true)
		{
			if (!flag && !string.IsNullOrEmpty(sfTextInputLayout.Text))
			{
				sfTextInputLayout.IsHintFloated = true;
				sfTextInputLayout.isHintDownToUp = !sfTextInputLayout.IsHintFloated;
				sfTextInputLayout.InvalidateDrawable();
			}
			else
			{
				sfTextInputLayout.IsHintFloated = flag;
				sfTextInputLayout.isHintDownToUp = !sfTextInputLayout.IsHintFloated;
				sfTextInputLayout.InvalidateDrawable();
			}
		}
	}

	private static void OnShowLeadingViewPropertyChanged(BindableObject bindable, object oldValue, object newValue)
	{
		if (newValue is bool)
		{
			bool showLeadingView = (bool)newValue;
			if (true)
			{
				(bindable as SfTextInputLayout)?.UpdateLeadingViewVisibility(showLeadingView);
				(bindable as SfTextInputLayout)?.UpdateViewBounds();
			}
		}
	}

	private static void OnShowTrailingViewPropertyChanged(BindableObject bindable, object oldValue, object newValue)
	{
		if (newValue is bool)
		{
			bool showTrailingView = (bool)newValue;
			if (true)
			{
				(bindable as SfTextInputLayout)?.UpdateTrailingViewVisibility(showTrailingView);
				(bindable as SfTextInputLayout)?.UpdateViewBounds();
			}
		}
	}

	private static void OnIsLayoutFocusedChanged(BindableObject bindable, object oldValue, object newValue)
	{
		(bindable as SfTextInputLayout)?.ChangeVisualState();
		(bindable as SfTextInputLayout)?.StartAnimation();
	}

	private static void OnHasErrorPropertyChanged(BindableObject bindable, object oldValue, object newValue)
	{
		(bindable as SfTextInputLayout)?.ChangeVisualState();
		(bindable as SfTextInputLayout)?.InvalidateDrawable();
	}

	private static void OnCharMaxLengthPropertyChanged(BindableObject bindable, object oldValue, object newValue)
	{
		if (bindable is SfTextInputLayout sfTextInputLayout && newValue is int)
		{
			int value = (int)newValue;
			if (true)
			{
				sfTextInputLayout.counterText = $"0/{value}";
				sfTextInputLayout.InvalidateDrawable();
			}
		}
	}

	private static void OnEnablePasswordTogglePropertyChanged(BindableObject bindable, object oldValue, object newValue)
	{
		if (bindable is SfTextInputLayout sfTextInputLayout && sfTextInputLayout.Content is Entry entry && newValue is bool)
		{
			bool isPassword = (bool)newValue;
			if (true)
			{
				entry.IsPassword = isPassword;
				sfTextInputLayout.isPasswordTextVisible = false;
				sfTextInputLayout.UpdateViewBounds();
			}
		}
	}

	private static void OnIsHintFloatedPropertyChanged(BindableObject bindable, object oldValue, object newValue)
	{
		if (!(bindable is SfTextInputLayout sfTextInputLayout))
		{
			return;
		}
		if (newValue is bool flag && sfTextInputLayout.Content is InputView)
		{
			sfTextInputLayout.Content.Opacity = (flag ? 1.0 : ((DeviceInfo.Platform == DevicePlatform.iOS || DeviceInfo.Platform == DevicePlatform.MacCatalyst || DeviceInfo.Platform == DevicePlatform.macOS) ? 0.02 : 0.0));
		}
		if (!(newValue is bool))
		{
			return;
		}
		bool flag2 = (bool)newValue;
		if (sfTextInputLayout.Content is SfDropdownEntry sfDropdownEntry && sfDropdownEntry.InputView != null)
		{
			if (DeviceInfo.Platform == DevicePlatform.Android)
			{
				sfDropdownEntry.Opacity = (flag2 ? 1 : 0);
			}
			else
			{
				sfDropdownEntry.InputView.Opacity = (flag2 ? 1.0 : ((DeviceInfo.Platform == DevicePlatform.iOS || DeviceInfo.Platform == DevicePlatform.MacCatalyst || DeviceInfo.Platform == DevicePlatform.macOS) ? 0.02 : 0.0));
			}
		}
	}

	private static void OnInputViewPaddingPropertyChanged(BindableObject bindable, object oldValue, object newValue)
	{
		(bindable as SfTextInputLayout)?.UpdateContentMargin((bindable as SfTextInputLayout)?.Content);
		(bindable as SfTextInputLayout)?.UpdateViewBounds();
	}

	private static void OnEnabledPropertyChanged(BindableObject bindable, object oldValue, object newValue)
	{
		(bindable as SfTextInputLayout)?.OnEnabledPropertyChanged((bool)newValue);
	}

	private static void OnHintLableStylePropertyChanged(BindableObject bindable, object oldValue, object newValue)
	{
		if (bindable is SfTextInputLayout sfTextInputLayout)
		{
			if (oldValue is LabelStyle labelStyle)
			{
				labelStyle.PropertyChanged -= sfTextInputLayout.HintLabelStyle_PropertyChanged;
			}
			if (newValue is LabelStyle labelStyle2)
			{
				sfTextInputLayout.internalHintLabelStyle.TextColor = labelStyle2.TextColor;
				sfTextInputLayout.internalHintLabelStyle.FontFamily = labelStyle2.FontFamily;
				sfTextInputLayout.internalHintLabelStyle.FontAttributes = labelStyle2.FontAttributes;
				sfTextInputLayout.HintFontSize = (float)((labelStyle2.FontSize < 12.0) ? 12.0 : labelStyle2.FontSize);
				labelStyle2.PropertyChanged += sfTextInputLayout.HintLabelStyle_PropertyChanged;
				sfTextInputLayout.UpdateViewBounds();
			}
		}
	}

	private static void OnHelperLableStylePropertyChanged(BindableObject bindable, object oldValue, object newValue)
	{
		if (bindable is SfTextInputLayout sfTextInputLayout)
		{
			if (oldValue is LabelStyle labelStyle)
			{
				labelStyle.PropertyChanged -= sfTextInputLayout.HelperLabelStyle_PropertyChanged;
			}
			if (newValue is LabelStyle labelStyle2)
			{
				sfTextInputLayout.internalHelperLabelStyle.TextColor = labelStyle2.TextColor;
				sfTextInputLayout.internalHelperLabelStyle.FontFamily = labelStyle2.FontFamily;
				sfTextInputLayout.internalHelperLabelStyle.FontAttributes = labelStyle2.FontAttributes;
				sfTextInputLayout.internalHelperLabelStyle.FontSize = labelStyle2.FontSize;
				labelStyle2.PropertyChanged += sfTextInputLayout.HelperLabelStyle_PropertyChanged;
				sfTextInputLayout.UpdateViewBounds();
			}
		}
	}

	private static void OnErrorLableStylePropertyChanged(BindableObject bindable, object oldValue, object newValue)
	{
		if (bindable is SfTextInputLayout sfTextInputLayout)
		{
			if (oldValue is LabelStyle labelStyle)
			{
				labelStyle.PropertyChanged -= sfTextInputLayout.ErrorLabelStyle_PropertyChanged;
			}
			if (newValue is LabelStyle labelStyle2)
			{
				sfTextInputLayout.internalErrorLabelStyle.TextColor = labelStyle2.TextColor;
				sfTextInputLayout.internalErrorLabelStyle.FontFamily = labelStyle2.FontFamily;
				sfTextInputLayout.internalErrorLabelStyle.FontAttributes = labelStyle2.FontAttributes;
				sfTextInputLayout.internalErrorLabelStyle.FontSize = labelStyle2.FontSize;
				labelStyle2.PropertyChanged += sfTextInputLayout.ErrorLabelStyle_PropertyChanged;
				sfTextInputLayout.UpdateViewBounds();
			}
		}
	}

	private static void OnCounterLableStylePropertyChanged(BindableObject bindable, object oldValue, object newValue)
	{
		if (bindable is SfTextInputLayout sfTextInputLayout)
		{
			if (oldValue is LabelStyle labelStyle)
			{
				labelStyle.PropertyChanged -= sfTextInputLayout.CounterLabelStyle_PropertyChanged;
			}
			if (newValue is LabelStyle labelStyle2)
			{
				sfTextInputLayout.internalCounterLabelStyle.TextColor = labelStyle2.TextColor;
				sfTextInputLayout.internalCounterLabelStyle.FontFamily = labelStyle2.FontFamily;
				sfTextInputLayout.internalCounterLabelStyle.FontAttributes = labelStyle2.FontAttributes;
				sfTextInputLayout.internalCounterLabelStyle.FontSize = labelStyle2.FontSize;
				labelStyle2.PropertyChanged += sfTextInputLayout.CounterLabelStyle_PropertyChanged;
				sfTextInputLayout.UpdateViewBounds();
			}
		}
	}

	private static void OnReserveSpacePropertyChanged(BindableObject bindable, object oldValue, object newValue)
	{
		if (bindable is SfTextInputLayout sfTextInputLayout && sfTextInputLayout.Content != null)
		{
			sfTextInputLayout.UpdateContentMargin(sfTextInputLayout.Content);
			sfTextInputLayout.UpdateViewBounds();
		}
	}

	private static void OnContainerTypePropertyChanged(BindableObject bindable, object oldValue, object newValue)
	{
		if (bindable is SfTextInputLayout sfTextInputLayout && sfTextInputLayout.Content != null)
		{
			sfTextInputLayout.UpdateContentMargin(sfTextInputLayout.Content);
			sfTextInputLayout.UpdateViewBounds();
		}
	}

	private static void OnPropertyChanged(BindableObject bindable, object oldValue, object newValue)
	{
		(bindable as SfTextInputLayout)?.UpdateViewBounds();
	}

	private void SfTextInputLayout_PropertyChanged(object? sender, PropertyChangedEventArgs e)
	{
		if (e.PropertyName == "FlowDirection" || e.PropertyName == "Height")
		{
			UpdateViewBounds();
		}
	}

	private void HookLabelStylePropertyChanged()
	{
		if (HelperLabelStyle != null)
		{
			HelperLabelStyle.PropertyChanged += HelperLabelStyle_PropertyChanged;
		}
		if (ErrorLabelStyle != null)
		{
			ErrorLabelStyle.PropertyChanged += ErrorLabelStyle_PropertyChanged;
		}
		if (HintLabelStyle != null)
		{
			HintLabelStyle.PropertyChanged += HintLabelStyle_PropertyChanged;
		}
		if (CounterLabelStyle != null)
		{
			CounterLabelStyle.PropertyChanged += CounterLabelStyle_PropertyChanged;
		}
	}

	private void UnHookLabelStylePropertyChanged()
	{
		if (HelperLabelStyle != null)
		{
			HelperLabelStyle.PropertyChanged -= HelperLabelStyle_PropertyChanged;
		}
		if (ErrorLabelStyle != null)
		{
			ErrorLabelStyle.PropertyChanged -= ErrorLabelStyle_PropertyChanged;
		}
		if (HintLabelStyle != null)
		{
			HintLabelStyle.PropertyChanged -= HintLabelStyle_PropertyChanged;
		}
		if (CounterLabelStyle != null)
		{
			CounterLabelStyle.PropertyChanged -= CounterLabelStyle_PropertyChanged;
		}
	}

	private void HintLabelStyle_PropertyChanged(object? sender, PropertyChangedEventArgs e)
	{
		if (sender is LabelStyle labelStyle && !string.IsNullOrEmpty(e.PropertyName))
		{
			HintFontSize = (float)((labelStyle.FontSize < 12.0) ? 12.0 : labelStyle.FontSize);
			MatchLabelStyleProperty(internalHintLabelStyle, labelStyle, e.PropertyName);
			InvalidateDrawable();
		}
	}

	private void HelperLabelStyle_PropertyChanged(object? sender, PropertyChangedEventArgs e)
	{
		if (sender is LabelStyle labelStyle && !string.IsNullOrEmpty(e.PropertyName))
		{
			MatchLabelStyleProperty(internalHelperLabelStyle, labelStyle, e.PropertyName);
			InvalidateDrawable();
		}
	}

	private void ErrorLabelStyle_PropertyChanged(object? sender, PropertyChangedEventArgs e)
	{
		if (sender is LabelStyle labelStyle && !string.IsNullOrEmpty(e.PropertyName))
		{
			MatchLabelStyleProperty(internalErrorLabelStyle, labelStyle, e.PropertyName);
			InvalidateDrawable();
		}
	}

	private void CounterLabelStyle_PropertyChanged(object? sender, PropertyChangedEventArgs e)
	{
		if (sender is LabelStyle labelStyle && !string.IsNullOrEmpty(e.PropertyName))
		{
			MatchLabelStyleProperty(internalCounterLabelStyle, labelStyle, e.PropertyName);
			InvalidateDrawable();
		}
	}

	private void MatchLabelStyleProperty(LabelStyle internalLabelStyle, LabelStyle labelStyle, string propertyName)
	{
		if (propertyName == "FontAttributes")
		{
			internalLabelStyle.FontAttributes = labelStyle.FontAttributes;
		}
		if (propertyName == "TextColor")
		{
			internalLabelStyle.TextColor = labelStyle.TextColor;
		}
		if (propertyName == "FontSize")
		{
			internalLabelStyle.FontSize = labelStyle.FontSize;
		}
		if (propertyName == "FontFamily")
		{
			internalLabelStyle.FontFamily = labelStyle.FontFamily;
		}
	}

	private void SetRendererBasedOnPlatform()
	{
		tilRenderer = new FluentDropdownEntryRenderer();
	}

	private void OnEnabledPropertyChanged(bool isEnabled)
	{
		base.IsEnabled = isEnabled;
		if (base.Content != null)
		{
			base.Content.IsEnabled = isEnabled;
		}
		InvalidateDrawable();
	}

	private void UpdateViewBounds()
	{
		UpdateLeadingViewPosition();
		UpdateTrailingViewPosition();
		UpdateContentPosition();
		InvalidateDrawable();
	}

	private void DropDownView_PopupOpened(object? sender, EventArgs e)
	{
		if (DeviceInfo.Platform == DevicePlatform.WinUI)
		{
			dropdownEntry?.DropdownContent?.SetValue(VisualElement.WidthRequestProperty, GetPopupWidth());
		}
		if (isNonEditableContent)
		{
			IsLayoutFocused = true;
		}
	}

	private void DropDownView_PopupClosed(object? sender, EventArgs e)
	{
		if (isNonEditableContent)
		{
			IsLayoutFocused = false;
		}
	}

	private int GetPopupX()
	{
		int num = (IsOutlined ? ((int)outlineRectF.X) : ((int)startPoint.X));
		if (isRTL)
		{
			num = ((!(DeviceInfo.Platform == DevicePlatform.WinUI)) ? (-num) : ((int)(base.Width - (double)num - GetPopupWidth())));
		}
		return num;
	}

	private int GetPopupY()
	{
		return ReserveSpaceForAssistiveLabels ? ((int)(0.0 - ((double)HelperTextSize.Height + 4.0 - BaseLineMaxHeight))) : 0;
	}

	private double GetPopupWidth()
	{
		int num = 0;
		num = ((!IsOutlined) ? ((int)(endPoint.X - startPoint.X)) : ((int)outlineRectF.Width));
		return num;
	}

	private SizeF GetLabelSize(SizeF size)
	{
		if (ReserveSpaceForAssistiveLabels)
		{
			return size;
		}
		size.Height = 0f;
		size.Width = 0f;
		return size;
	}

	private void InputView_HandlerChanged(object? sender, EventArgs e)
	{
		if ((sender as InputView)?.Handler != null && (sender as InputView)?.Handler?.PlatformView is TextBox textBox)
		{
			textBox.BorderThickness = new Microsoft.UI.Xaml.Thickness(0.0);
			textBox.Padding = new Microsoft.UI.Xaml.Thickness(0.0);
			textBox.Resources["TextControlBorderThemeThicknessFocused"] = textBox.BorderThickness;
		}
	}

	private void InputViewFocused(object? sender, FocusEventArgs e)
	{
		IsLayoutFocused = true;
	}

	private void InputViewUnfocused(object? sender, FocusEventArgs e)
	{
		IsLayoutFocused = false;
	}

	private void InputViewTextChanged(object? sender, Microsoft.Maui.Controls.TextChangedEventArgs e)
	{
		if (sender is InputView || sender is SfInputView)
		{
			Text = e.NewTextValue;
			if (string.IsNullOrEmpty(Text) && !IsLayoutFocused)
			{
				if (!IsHintAlwaysFloated)
				{
					IsHintFloated = false;
					isHintDownToUp = true;
					InvalidateDrawable();
				}
			}
			else if (!string.IsNullOrEmpty(Text) && !IsHintFloated)
			{
				IsHintFloated = true;
				isHintDownToUp = false;
				InvalidateDrawable();
			}
			string? text = Text;
			if (text == null || text.Length != 1)
			{
				string? text2 = Text;
				if (text2 == null || text2.Length != 0)
				{
					goto IL_00e4;
				}
			}
			InvalidateDrawable();
		}
		goto IL_00e4;
		IL_00e4:
		if (DeviceInfo.Platform == DevicePlatform.WinUI && base.Content is Editor)
		{
			InvalidateMeasure();
		}
	}

	private void ClearText()
	{
		if (base.Content is InputView inputView)
		{
			inputView.Text = string.Empty;
		}
		else if (base.Content is SfDropdownEntry sfDropdownEntry && sfDropdownEntry.InputView != null)
		{
			sfDropdownEntry.InputView.Text = string.Empty;
		}
	}

	private void AddDefaultVSM()
	{
		VisualStateGroupList visualStateGroupList = new VisualStateGroupList();
		Microsoft.Maui.Controls.VisualStateGroup visualStateGroup = new Microsoft.Maui.Controls.VisualStateGroup();
		visualStateGroup.Name = "CommonStates";
		Microsoft.Maui.Controls.VisualState visualState = new Microsoft.Maui.Controls.VisualState
		{
			Name = "Focused"
		};
		Microsoft.Maui.Controls.Setter item = new Microsoft.Maui.Controls.Setter
		{
			Property = StrokeProperty,
			Value = Color.FromArgb("#6750A4")
		};
		visualState.Setters.Add(item);
		Microsoft.Maui.Controls.VisualState visualState2 = new Microsoft.Maui.Controls.VisualState
		{
			Name = "Error"
		};
		Microsoft.Maui.Controls.Setter item2 = new Microsoft.Maui.Controls.Setter
		{
			Property = StrokeProperty,
			Value = Color.FromArgb("#B3261E")
		};
		visualState2.Setters.Add(item2);
		Microsoft.Maui.Controls.VisualState visualState3 = new Microsoft.Maui.Controls.VisualState
		{
			Name = "Normal"
		};
		Microsoft.Maui.Controls.Setter item3 = new Microsoft.Maui.Controls.Setter
		{
			Property = StrokeProperty,
			Value = Color.FromArgb("#79747E")
		};
		visualState3.Setters.Add(item3);
		visualStateGroup.States.Add(visualState3);
		visualStateGroup.States.Add(visualState);
		visualStateGroup.States.Add(visualState2);
		visualStateGroupList.Add(visualStateGroup);
		if (!this.HasVisualStateGroups())
		{
			Microsoft.Maui.Controls.Setter item4 = new Microsoft.Maui.Controls.Setter
			{
				Property = Microsoft.Maui.Controls.VisualStateManager.VisualStateGroupsProperty,
				Value = visualStateGroupList
			};
			Microsoft.Maui.Controls.Style style = new Microsoft.Maui.Controls.Style(typeof(SfTextInputLayout));
			style.Setters.Add(item4);
			base.Resources = new Microsoft.Maui.Controls.ResourceDictionary { style };
			base.Style = style;
		}
	}

	private void UnhookEvents()
	{
		if (base.Content != null && base.Content is InputView inputView)
		{
			inputView.Focused -= InputViewFocused;
			inputView.Unfocused -= InputViewUnfocused;
			inputView.TextChanged -= InputViewTextChanged;
			inputView.HandlerChanged -= InputView_HandlerChanged;
		}
		if (base.Content != null && base.Content is SfDropdownEntry sfDropdownEntry)
		{
			InputView inputView2 = sfDropdownEntry.InputView;
			if (inputView2 != null)
			{
				if (!isNonEditableContent)
				{
					inputView2.Focused -= InputViewFocused;
					inputView2.Unfocused -= InputViewUnfocused;
				}
				inputView2.TextChanged -= InputViewTextChanged;
				inputView2.HandlerChanged -= InputView_HandlerChanged;
			}
			SfDropdownView dropDownView = sfDropdownEntry.DropDownView;
			if (dropDownView != null)
			{
				dropDownView.PopupOpened -= DropDownView_PopupOpened;
				dropDownView.PopupClosed -= DropDownView_PopupClosed;
			}
		}
		base.PropertyChanged -= SfTextInputLayout_PropertyChanged;
		UnHookLabelStylePropertyChanged();
	}

	private void HookEvents()
	{
		if (base.Content != null && base.Content is InputView inputView)
		{
			inputView.Focused += InputViewFocused;
			inputView.Unfocused += InputViewUnfocused;
			inputView.TextChanged += InputViewTextChanged;
			inputView.HandlerChanged += InputView_HandlerChanged;
		}
		if (base.Content != null && base.Content is SfDropdownEntry sfDropdownEntry)
		{
			InputView inputView2 = sfDropdownEntry.InputView;
			if (inputView2 != null)
			{
				if (!isNonEditableContent)
				{
					inputView2.Focused += InputViewFocused;
					inputView2.Unfocused += InputViewUnfocused;
				}
				inputView2.TextChanged += InputViewTextChanged;
				inputView2.HandlerChanged += InputView_HandlerChanged;
			}
			SfDropdownView dropDownView = sfDropdownEntry.DropDownView;
			if (dropDownView != null)
			{
				dropDownView.PopupOpened += DropDownView_PopupOpened;
				dropDownView.PopupClosed += DropDownView_PopupClosed;
			}
		}
		base.PropertyChanged += SfTextInputLayout_PropertyChanged;
		HookLabelStylePropertyChanged();
	}

	private void ToggleIcon()
	{
		if (base.Content is SfDropdownEntry sfDropdownEntry && !isNonEditableContent)
		{
			if (IsLayoutFocused && !sfDropdownEntry.IsDropDownOpen)
			{
				sfDropdownEntry.IsDropDownOpen = false;
			}
			else
			{
				sfDropdownEntry.IsDropDownOpen = true;
			}
		}
		if (base.Content is Entry entry)
		{
			isPasswordTextVisible = !isPasswordTextVisible;
			entry.IsPassword = !isPasswordTextVisible;
			InvalidateDrawable();
		}
	}

	private void AddView(object oldValue, object newValue)
	{
		View view = (View)oldValue;
		if (view != null && this.Contains(view))
		{
			Remove(view);
		}
		View view2 = (View)newValue;
		if (view2 != null)
		{
			Add(view2);
		}
		UpdateLeadingViewVisibility(ShowLeadingView);
		UpdateTrailingViewVisibility(ShowTrailingView);
	}

	private void UpdateContentPosition()
	{
		UpdateLeadViewWidthForContent();
		UpdateTrailViewWidthForContent();
		if (base.Content != null)
		{
			viewBounds.X = (int)leadViewWidth;
			viewBounds.Y = 0f;
			viewBounds.Width = (int)(base.Width - leadViewWidth - trailViewWidth);
			viewBounds.Height = (int)base.Height;
			if (((!isNonEditableContent && ShowClearButton) || ShowDropDownButton || EnablePasswordVisibilityToggle) && !isNonEditableContent)
			{
				viewBounds.Width -= (float)(((ShowDropDownButton || EnablePasswordVisibilityToggle) && !isNonEditableContent && ShowClearButton) ? 52.0 : 20.0);
			}
			AbsoluteLayout.SetLayoutBounds((BindableObject)base.Content, (Rect)viewBounds);
		}
	}

	private void UpdateLeadingViewPosition()
	{
		if (ShowLeadingView && LeadingView != null)
		{
			viewBounds.X = (float)(leadingViewLeftPadding + BaseLineMaxHeight);
			viewBounds.Y = (float)(IsOutlined ? BaseLineMaxHeight : 0.0);
			viewBounds.Width = (float)((LeadingView.WidthRequest != -1.0) ? LeadingView.WidthRequest : ((LeadingView.Width == -1.0 || LeadingView.Width == base.Width) ? defaultLeadAndTrailViewWidth : LeadingView.Width));
			viewBounds.Height = (float)(base.Height - AssistiveLabelPadding - (double)HelperTextSize.Height);
			if (IsOutlined || IsFilled)
			{
				LeadingView.VerticalOptions = LayoutOptions.Center;
			}
			if (IsNone)
			{
				viewBounds.Height = (float)((double)viewBounds.Height - BaseLineMaxHeight - 5.0);
				LeadingView.VerticalOptions = LayoutOptions.End;
			}
			AbsoluteLayout.SetLayoutBounds((BindableObject)LeadingView, (Rect)viewBounds);
		}
	}

	private void UpdateTrailingViewPosition()
	{
		if (ShowTrailingView && TrailingView != null)
		{
			viewBounds.Width = (float)((TrailingView.WidthRequest != -1.0) ? TrailingView.WidthRequest : ((TrailingView.Width == -1.0 || TrailingView.Width == base.Width) ? defaultLeadAndTrailViewWidth : TrailingView.Width));
			viewBounds.X = (float)(base.Width - (double)viewBounds.Width - trailingViewRightPadding);
			viewBounds.Y = (float)(IsOutlined ? BaseLineMaxHeight : 0.0);
			viewBounds.Height = (float)(base.Height - AssistiveLabelPadding - (double)HelperTextSize.Height);
			if (IsOutlined || IsFilled)
			{
				TrailingView.VerticalOptions = LayoutOptions.Center;
			}
			if (IsNone)
			{
				viewBounds.Height = (float)((double)viewBounds.Height - BaseLineMaxHeight - 5.0);
				TrailingView.VerticalOptions = LayoutOptions.End;
			}
			AbsoluteLayout.SetLayoutBounds((BindableObject)TrailingView, (Rect)viewBounds);
		}
	}

	private void UpdateLeadingViewVisibility(bool showLeadingView)
	{
		if (LeadingView != null)
		{
			LeadingView.IsVisible = showLeadingView;
		}
	}

	private void UpdateTrailingViewVisibility(bool showTrailingView)
	{
		if (TrailingView != null)
		{
			TrailingView.IsVisible = showTrailingView;
		}
	}

	private void UpdateLeadViewWidthForContent()
	{
		leadViewWidth = 0.0;
		if (ShowLeadingView && LeadingView != null)
		{
			leadViewWidth = ((LeadingView.Width == -1.0 || LeadingView.Width == base.Width) ? defaultLeadAndTrailViewWidth : LeadingView.Width) + ((LeadingViewPosition == ViewPosition.Outside) ? (leadingViewLeftPadding + leadingViewRightPadding) : (IsNone ? (leadingViewLeftPadding + leadingViewRightPadding) : leadingViewLeftPadding));
		}
	}

	private void UpdateTrailViewWidthForContent()
	{
		trailViewWidth = 0.0;
		if (ShowTrailingView && TrailingView != null)
		{
			trailViewWidth = ((TrailingView.Width == -1.0 || TrailingView.Width == base.Width) ? defaultLeadAndTrailViewWidth : TrailingView.Width) + ((TrailingViewPosition == ViewPosition.Outside) ? (trailingViewLeftPadding + trailingViewRightPadding) : trailingViewLeftPadding);
		}
	}

	private void UpdateLeadViewWidthForBorder()
	{
		leadViewWidth = 0.0;
		if (ShowLeadingView && LeadingView != null && LeadingViewPosition == ViewPosition.Outside)
		{
			leadViewWidth = LeadingView.Width + leadingViewLeftPadding + leadingViewRightPadding;
		}
	}

	private void UpdateTrailViewWidthForBorder()
	{
		trailViewWidth = 0.0;
		if (ShowTrailingView && TrailingView != null && TrailingViewPosition == ViewPosition.Outside)
		{
			trailViewWidth = TrailingView.Width + trailingViewLeftPadding + trailingViewRightPadding;
		}
	}

	private void UpdateEffectsRendererBounds()
	{
		if (effectsenderer != null)
		{
			effectsenderer.RippleBoundsCollection.Clear();
			effectsenderer.HighlightBoundsCollection.Clear();
			if (IsClearIconVisible && IsLayoutFocused)
			{
				effectsenderer.RippleBoundsCollection.Add(secondaryIconRectF);
				effectsenderer.HighlightBoundsCollection.Add(secondaryIconRectF);
			}
			if (IsPassowordToggleIconVisible || IsDropDownIconVisible)
			{
				effectsenderer.RippleBoundsCollection.Add(primaryIconRectF);
				effectsenderer.HighlightBoundsCollection.Add(primaryIconRectF);
			}
		}
	}

	private void UpdateCounterLabelText()
	{
		if (ShowCharCount)
		{
			int value = ((!string.IsNullOrEmpty(Text)) ? Text.Length : 0);
			counterText = ((CharMaxLength == int.MaxValue) ? $"{value}" : $"{value}/{CharMaxLength}");
			InvalidateDrawable();
		}
	}

	private void UpdateNoneContainerHintPosition()
	{
		if (IsHintFloated)
		{
			hintRect.X = 0f;
			hintRect.Y = 4f;
			hintRect.Width = FloatedHintSize.Width;
			hintRect.Height = FloatedHintSize.Height;
		}
		else
		{
			hintRect.X = 0f;
			hintRect.Y = (int)(base.Height - (double)HintSize.Height - BaseLineMaxHeight - AssistiveLabelPadding - (double)HelperTextSize.Height);
			hintRect.Width = HintSize.Width;
			hintRect.Height = HintSize.Height;
		}
	}

	private void UpdateFilledContainerHintPosition()
	{
		if (IsHintFloated)
		{
			hintRect.X = 16f;
			hintRect.Y = 8f;
			hintRect.Width = FloatedHintSize.Width;
			hintRect.Height = FloatedHintSize.Height;
		}
		else
		{
			hintRect.X = 16f;
			hintRect.Y = (float)((base.Height - (double)HelperTextSize.Height - AssistiveLabelPadding) / 2.0) - HintSize.Height / 2f;
			hintRect.Width = HintSize.Width;
			hintRect.Height = HintSize.Height;
		}
	}

	private void UpdateOutlinedContainerHintPosition()
	{
		if (IsHintFloated)
		{
			hintRect.X = 12f;
			hintRect.Y = 0f;
			hintRect.Width = (int)FloatedHintSize.Width;
			hintRect.Height = (int)FloatedHintSize.Height;
		}
		else
		{
			hintRect.X = 12f;
			hintRect.Y = outlineRectF.Center.Y - HintSize.Height / 2f;
			hintRect.Width = HintSize.Width;
			hintRect.Height = HintSize.Height;
		}
	}

	private void UpdateHintPosition()
	{
		if (isAnimating)
		{
			if (IsOutlined)
			{
				hintRect.X = 16f;
				if (ShowLeadingView && LeadingView != null)
				{
					leadViewWidth = LeadingView.Width + ((LeadingViewPosition == ViewPosition.Outside) ? (leadingViewLeftPadding + leadingViewRightPadding) : (IsNone ? (leadingViewLeftPadding + leadingViewRightPadding) : leadingViewLeftPadding));
					hintRect.X += (float)leadViewWidth;
				}
				if (isRTL)
				{
					hintRect.X = (float)(base.Width - (double)hintRect.X - (double)hintRect.Width);
				}
			}
			return;
		}
		if (IsNone)
		{
			UpdateNoneContainerHintPosition();
		}
		if (IsFilled)
		{
			UpdateFilledContainerHintPosition();
		}
		if (IsOutlined)
		{
			UpdateOutlinedContainerHintPosition();
		}
		if (ShowLeadingView && LeadingView != null)
		{
			leadViewWidth = LeadingView.Width + ((LeadingViewPosition == ViewPosition.Outside) ? (leadingViewLeftPadding + leadingViewRightPadding) : (IsNone ? (leadingViewLeftPadding + leadingViewRightPadding) : leadingViewLeftPadding));
			hintRect.X += (float)leadViewWidth;
		}
		if (isRTL)
		{
			hintRect.X = (float)(base.Width - (double)hintRect.X - (double)hintRect.Width);
		}
	}

	private void UpdateHelperTextPosition()
	{
		UpdateLeadViewWidthForContent();
		UpdateTrailViewWidthForBorder();
		double num = (IsNone ? 0.0 : 16.0);
		helperTextRect.X = (int)(num + leadViewWidth);
		helperTextRect.Y = (int)(base.Height - (double)HelperTextSize.Height);
		helperTextRect.Width = (int)(base.Width - num - 12.0 - 4.0 - (double)(ShowCharCount ? (CounterTextSize.Width + 12f) : 0f) - trailViewWidth - leadViewWidth);
		helperTextRect.Height = HelperTextSize.Height;
		if (isRTL)
		{
			helperTextRect.X = (int)(base.Width - (double)helperTextRect.X - (double)helperTextRect.Width);
		}
	}

	private void UpdateErrorTextPosition()
	{
		UpdateLeadViewWidthForContent();
		UpdateTrailViewWidthForBorder();
		double num = (IsNone ? 0.0 : 16.0);
		errorTextRect.X = (int)(num + leadViewWidth);
		errorTextRect.Y = (int)(base.Height - (double)ErrorTextSize.Height);
		errorTextRect.Width = (int)(base.Width - num - 12.0 - 4.0 - (double)(ShowCharCount ? (CounterTextSize.Width + 12f) : 0f) - trailViewWidth - leadViewWidth);
		errorTextRect.Height = (int)ErrorTextSize.Height;
		if (isRTL)
		{
			errorTextRect.X = (int)(base.Width - (double)errorTextRect.X - (double)errorTextRect.Width);
		}
	}

	private void UpdateCounterTextPosition()
	{
		UpdateTrailViewWidthForBorder();
		counterTextRect.X = (int)(base.Width - (double)CounterTextSize.Width - 12.0 - trailViewWidth);
		counterTextRect.Y = (int)(base.Height - (double)CounterTextSize.Height);
		counterTextRect.Width = (int)CounterTextSize.Width;
		counterTextRect.Height = (int)CounterTextSize.Height;
		if (isRTL)
		{
			counterTextRect.X = (float)(base.Width - (double)counterTextRect.X - (double)counterTextRect.Width);
		}
	}

	private void UpdateBaseLinePoints()
	{
		UpdateLeadViewWidthForBorder();
		UpdateTrailViewWidthForBorder();
		startPoint.X = (float)leadViewWidth;
		startPoint.Y = (float)(base.Height - (double)HelperTextSize.Height - AssistiveLabelPadding);
		endPoint.X = (float)(base.Width - trailViewWidth);
		endPoint.Y = (float)(base.Height - (double)HelperTextSize.Height - AssistiveLabelPadding);
	}

	private void UpdateOutlineRectF()
	{
		UpdateLeadViewWidthForBorder();
		UpdateTrailViewWidthForBorder();
		outlineRectF.X = (float)(BaseLineMaxHeight + leadViewWidth);
		outlineRectF.Y = (float)((BaseLineMaxHeight > (double)(FloatedHintSize.Height / 2f)) ? BaseLineMaxHeight : ((double)(FloatedHintSize.Height / 2f)));
		outlineRectF.Width = (float)(base.Width - BaseLineMaxHeight * 2.0 - leadViewWidth - trailViewWidth);
		outlineRectF.Height = (float)(base.Height - (double)outlineRectF.Y - (double)HelperTextSize.Height - AssistiveLabelPadding);
	}

	private void UpdateBackgroundRectF()
	{
		UpdateLeadViewWidthForBorder();
		UpdateTrailViewWidthForBorder();
		backgroundRectF.X = (float)leadViewWidth;
		backgroundRectF.Y = 0f;
		backgroundRectF.Width = (float)(base.Width - leadViewWidth - trailViewWidth);
		backgroundRectF.Height = (float)(base.Height - (double)HelperTextSize.Height - AssistiveLabelPadding);
	}

	private void CalculateClipRect()
	{
		if (!string.IsNullOrEmpty(Hint) && EnableFloating)
		{
			clipRect.X = (float)((double)(outlineRectF.X + 12f) - BaseLineMaxHeight);
			clipRect.Y = 0f;
			clipRect.Width = FloatedHintSize.Width;
			clipRect.Height = FloatedHintSize.Height;
			if (ShowLeadingView && LeadingView != null && LeadingViewPosition == ViewPosition.Inside)
			{
				clipRect.X = (float)((double)clipRect.X + LeadingView.Width + leadingViewLeftPadding);
			}
		}
		else
		{
			clipRect = new Rect(0.0, 0.0, 0.0, 0.0);
		}
	}

	private void UpdateIconRectF()
	{
		UpdateOutlineRectF();
		UpdateBackgroundRectF();
		UpdateTrailViewWidthForContent();
		UpdatePrimaryIconRectF();
		UpdateSecondaryIconRectF();
		UpdateEffectsRendererBounds();
	}

	private void UpdatePrimaryIconRectF()
	{
		primaryIconRectF.X = (float)(base.Width - trailViewWidth - (IsOutlined ? (BaseLineMaxHeight * 2.0) : BaseLineMaxHeight) - 32.0);
		if (IsNone)
		{
			primaryIconRectF.Y = (float)(base.Content.Y + base.Content.Height / 2.0 - 16.0);
		}
		else if (IsOutlined)
		{
			primaryIconRectF.Y = outlineRectF.Center.Y - 16f;
		}
		else
		{
			primaryIconRectF.Y = (float)((base.Height - AssistiveLabelPadding - (double)HelperTextSize.Height) / 2.0 - 16.0);
		}
		primaryIconRectF.Width = ((IsPassowordToggleIconVisible || IsDropDownIconVisible) ? 32 : 0);
		primaryIconRectF.Height = ((IsPassowordToggleIconVisible || IsDropDownIconVisible) ? 32 : 0);
		if (isRTL)
		{
			primaryIconRectF.X = (float)(base.Width - (double)primaryIconRectF.X - (double)primaryIconRectF.Width);
		}
	}

	private void UpdateSecondaryIconRectF()
	{
		secondaryIconRectF.X = ((primaryIconRectF.Width != 0f) ? (primaryIconRectF.X - 32f) : primaryIconRectF.X);
		secondaryIconRectF.Y = primaryIconRectF.Y;
		secondaryIconRectF.Width = 32f;
		secondaryIconRectF.Height = 32f;
		if (DeviceInfo.Platform == DevicePlatform.Android)
		{
			secondaryIconRectF.X += 2f;
			secondaryIconRectF.Y += 2f;
			secondaryIconRectF.Width = 28f;
			secondaryIconRectF.Height = 28f;
		}
		if (isRTL)
		{
			secondaryIconRectF.X -= secondaryIconRectF.Width;
		}
	}

	private void UpdateHintColor()
	{
		internalHintLabelStyle.TextColor = ((!IsEnabled) ? DisabledColor : ((IsLayoutFocused || HasError) ? Stroke : HintLabelStyle.TextColor));
	}

	private void UpdateHelperTextColor()
	{
		internalHelperLabelStyle.TextColor = (IsEnabled ? HelperLabelStyle.TextColor : DisabledColor);
	}

	private void UpdateErrorTextColor()
	{
		internalErrorLabelStyle.TextColor = ((!IsEnabled) ? DisabledColor : (ErrorLabelStyle.TextColor.Equals(Color.FromRgba(0.0, 0.0, 0.0, 0.87)) ? Stroke : ErrorLabelStyle.TextColor));
	}

	private void UpdateCounterTextColor()
	{
		internalCounterLabelStyle.TextColor = ((!IsEnabled) ? DisabledColor : ((!HasError) ? CounterLabelStyle.TextColor : (ErrorLabelStyle.TextColor.Equals(Color.FromRgba(0.0, 0.0, 0.0, 0.87)) ? Stroke : ErrorLabelStyle.TextColor)));
	}

	private void DrawBorder(ICanvas canvas, RectF dirtyRect)
	{
		canvas.SaveState();
		if (isRTL)
		{
			canvas.Translate((float)base.Width, 0f);
			canvas.Scale(-1f, 1f);
		}
		canvas.StrokeSize = (float)(IsLayoutFocused ? FocusedStrokeThickness : UnfocusedStrokeThickness);
		canvas.StrokeColor = (IsEnabled ? Stroke : DisabledColor);
		if (!IsOutlined)
		{
			UpdateBaseLinePoints();
			if (IsFilled)
			{
				canvas.SaveState();
				if (ContainerBackground is SolidColorBrush solidColorBrush)
				{
					canvas.FillColor = solidColorBrush.Color;
				}
				UpdateBackgroundRectF();
				canvas.FillRoundedRectangle(backgroundRectF, OutlineCornerRadius, OutlineCornerRadius, 0.0, 0.0);
				canvas.RestoreState();
			}
			canvas.DrawLine(startPoint.X, startPoint.Y, endPoint.X, endPoint.Y);
		}
		else
		{
			UpdateOutlineRectF();
			if (((IsLayoutFocused && !string.IsNullOrEmpty(Hint)) || IsHintFloated) && ShowHint)
			{
				CalculateClipRect();
				canvas.SubtractFromClip(clipRect);
			}
			if (ContainerBackground is SolidColorBrush solidColorBrush2)
			{
				canvas.FillColor = solidColorBrush2.Color;
			}
			canvas.FillRoundedRectangle(outlineRectF, OutlineCornerRadius);
			canvas.DrawRoundedRectangle(outlineRectF, OutlineCornerRadius);
		}
		canvas.RestoreState();
	}

	private void DrawHintText(ICanvas canvas, RectF dirtyRect)
	{
		if ((!IsHintFloated || EnableFloating) && (IsHintFloated || string.IsNullOrEmpty(Text)) && ShowHint && !string.IsNullOrEmpty(Hint) && HintLabelStyle != null)
		{
			canvas.SaveState();
			UpdateOutlineRectF();
			UpdateHintPosition();
			UpdateHintColor();
			internalHintLabelStyle.FontSize = (isAnimating ? ((double)(float)animatingFontSize) : (IsHintFloated ? 12.0 : HintLabelStyle.FontSize));
			Microsoft.Maui.Graphics.HorizontalAlignment horizontalAlignment = ((!IsNone && !isAnimating && !IsFilled) ? Microsoft.Maui.Graphics.HorizontalAlignment.Center : (isRTL ? Microsoft.Maui.Graphics.HorizontalAlignment.Right : Microsoft.Maui.Graphics.HorizontalAlignment.Left));
			Microsoft.Maui.Graphics.VerticalAlignment verticalAlignment = Microsoft.Maui.Graphics.VerticalAlignment.Center;
			canvas.DrawText(Hint, hintRect, horizontalAlignment, verticalAlignment, internalHintLabelStyle);
			canvas.RestoreState();
		}
	}

	private void DrawAssistiveText(ICanvas canvas, RectF dirtyRect)
	{
		if (HasError)
		{
			DrawErrorText(canvas, dirtyRect);
		}
		else
		{
			DrawHelperText(canvas, dirtyRect);
		}
		DrawCounterText(canvas, dirtyRect);
	}

	private void DrawHelperText(ICanvas canvas, RectF dirtyRect)
	{
		if (ShowHelperText && !string.IsNullOrEmpty(HelperText) && HelperLabelStyle != null)
		{
			canvas.SaveState();
			UpdateHelperTextPosition();
			UpdateHelperTextColor();
			canvas.DrawText(HelperText, helperTextRect, isRTL ? Microsoft.Maui.Graphics.HorizontalAlignment.Right : Microsoft.Maui.Graphics.HorizontalAlignment.Left, Microsoft.Maui.Graphics.VerticalAlignment.Top, internalHelperLabelStyle);
			canvas.RestoreState();
		}
	}

	private void DrawErrorText(ICanvas canvas, RectF dirtyRect)
	{
		if (ShowHelperText && !string.IsNullOrEmpty(ErrorText) && ErrorLabelStyle != null)
		{
			canvas.SaveState();
			UpdateErrorTextPosition();
			UpdateErrorTextColor();
			canvas.DrawText(ErrorText, errorTextRect, isRTL ? Microsoft.Maui.Graphics.HorizontalAlignment.Right : Microsoft.Maui.Graphics.HorizontalAlignment.Left, Microsoft.Maui.Graphics.VerticalAlignment.Top, internalErrorLabelStyle);
			canvas.RestoreState();
		}
	}

	private void DrawCounterText(ICanvas canvas, RectF dirtyRect)
	{
		if (ShowCharCount && !string.IsNullOrEmpty(counterText) && CounterLabelStyle != null)
		{
			canvas.SaveState();
			UpdateCounterTextPosition();
			UpdateCounterTextColor();
			canvas.DrawText(counterText, counterTextRect, isRTL ? Microsoft.Maui.Graphics.HorizontalAlignment.Right : Microsoft.Maui.Graphics.HorizontalAlignment.Left, Microsoft.Maui.Graphics.VerticalAlignment.Top, internalCounterLabelStyle);
			canvas.RestoreState();
		}
	}

	private void DrawClearIcon(ICanvas canvas, RectF dirtyRect)
	{
		if (IsClearIconVisible)
		{
			canvas.SaveState();
			canvas.StrokeColor = Stroke;
			canvas.StrokeSize = 1.5f;
			tilRenderer?.DrawClearButton(canvas, secondaryIconRectF);
			canvas.RestoreState();
		}
	}

	private void DrawPasswordToggleIcon(ICanvas canvas, RectF dirtyRect)
	{
		if (IsPassowordToggleIconVisible)
		{
			canvas.SaveState();
			canvas.FillColor = Stroke;
			canvas.Translate(primaryIconRectF.X + passwordToggleIconEdgePadding, primaryIconRectF.Y + (isPasswordTextVisible ? passwordToggleVisibleTopPadding : passwordToggleCollapsedTopPadding));
			if (DeviceInfo.Platform == DevicePlatform.Android)
			{
				canvas.Scale(1.2f, 1.2f);
			}
			canvas.FillPath(ToggleIconPath);
			canvas.RestoreState();
		}
	}

	public void DrawDropDownIcon(ICanvas canvas, RectF rectF)
	{
		if (IsDropDownIconVisible)
		{
			canvas.SaveState();
			canvas.FillColor = Stroke;
			canvas.StrokeColor = Stroke;
			canvas.StrokeSize = 2f;
			tilRenderer?.DrawDropDownButton(canvas, primaryIconRectF);
			canvas.RestoreState();
		}
	}

	private void UpdateSizeStartAndEndValue()
	{
		fontSizeStart = (IsHintFloated ? 12f : HintFontSize);
		fontSizeEnd = (IsHintFloated ? HintFontSize : 12f);
	}

	private void UpdateStartAndEndValue()
	{
		if (IsNone && base.Width > 0.0 && base.Height > 0.0)
		{
			if (IsHintFloated)
			{
				translateStart = 4.0;
				translateEnd = base.Height - (double)HintSize.Height - BaseLineMaxHeight - AssistiveLabelPadding - (double)HelperTextSize.Height - 4.0;
			}
			else
			{
				translateStart = base.Height - (double)HintSize.Height - BaseLineMaxHeight - AssistiveLabelPadding - (double)HelperTextSize.Height - 4.0;
				translateEnd = 4.0;
			}
		}
		if (IsOutlined && base.Width > 0.0 && base.Height > 0.0)
		{
			if (IsHintFloated)
			{
				translateStart = 0.0;
				translateEnd = outlineRectF.Center.Y - HintSize.Height / 2f;
			}
			else
			{
				translateStart = outlineRectF.Center.Y - HintSize.Height / 2f;
				translateEnd = 0.0;
			}
		}
		if (IsFilled && base.Width > 0.0 && base.Height > 0.0)
		{
			if (IsHintFloated)
			{
				translateStart = 8.0;
				translateEnd = (base.Height - (double)HelperTextSize.Height - AssistiveLabelPadding) / 2.0 - (double)(HintSize.Height / 2f);
			}
			else
			{
				translateStart = (base.Height - (double)HelperTextSize.Height - AssistiveLabelPadding) / 2.0 - (double)(HintSize.Height / 2f);
				translateEnd = 8.0;
			}
		}
	}

	private void StartAnimation()
	{
		if (IsHintFloated && IsLayoutFocused)
		{
			InvalidateDrawable();
			return;
		}
		if (string.IsNullOrEmpty(Text) && !IsLayoutFocused && !EnableFloating)
		{
			IsHintFloated = false;
			isHintDownToUp = true;
			InvalidateDrawable();
			return;
		}
		if (!string.IsNullOrEmpty(Text) || IsHintAlwaysFloated || !EnableHintAnimation)
		{
			IsHintFloated = true;
			isHintDownToUp = false;
			if (!EnableHintAnimation && !IsLayoutFocused && string.IsNullOrEmpty(Text))
			{
				IsHintFloated = false;
				isHintDownToUp = true;
			}
			InvalidateDrawable();
			return;
		}
		animatingFontSize = (IsHintFloated ? 12f : HintFontSize);
		UpdateStartAndEndValue();
		UpdateSizeStartAndEndValue();
		IsHintFloated = !IsHintFloated;
		if (!IsHintFloated)
		{
			UpdateHintPosition();
		}
		fontsize = (float)fontSizeStart;
		Animation animation = new Animation(OnScalingAnimationUpdate, fontSizeStart, fontSizeEnd, Easing.Linear);
		Animation animation2 = new Animation(OnTranslateAnimationUpdate, translateStart, translateEnd, Easing.SinIn);
		animation2.WithConcurrent(animation);
		animation2.Commit(this, "showAnimator", 7u, 167u, null, OnTranslateAnimationEnded, () => false);
	}

	private void OnScalingAnimationUpdate(double value)
	{
		fontsize = (float)value;
	}

	private void OnTranslateAnimationUpdate(double value)
	{
		isAnimating = true;
		hintRect.Y = (float)value;
		animatingFontSize = fontsize;
		InvalidateDrawable();
	}

	private void OnTranslateAnimationEnded(double value, bool isCompleted)
	{
		isAnimating = false;
		isHintDownToUp = !isHintDownToUp;
	}
}
