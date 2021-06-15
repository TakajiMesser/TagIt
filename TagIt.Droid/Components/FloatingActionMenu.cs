﻿using Android.Animation;
using Android.Content;
using Android.Content.Res;
using Android.Graphics;
using Android.Graphics.Drawables;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;
using AndroidX.Core.Content.Resources;
using Google.Android.Material.FloatingActionButton;
using Java.Lang;
using System;
using System.Collections.Generic;
using System.Linq;

namespace TagIt.Droid.Components
{
    [Register("com.tagit.droid.components.FloatingActionMenu")]
    public class FloatingActionMenu : ViewGroup
    {
        private const int ANIMATION_DURATION = 250;
        private const int ANIMATION_DELAY = 100;

        private const int OPEN_UP = 0;
        private const int OPEN_DOWN = 1;
        private const int OPEN_LEFT = 2;
        private const int OPEN_RIGHT = 3;

        private const int LABEL_LEFT = 0;
        private const int LABEL_TOP = 1;
        private const int LABEL_RIGHT = 2;
        private const int LABEL_BOTTOM = 3;

        private FloatingActionButton _menuButton;
        private List<View> _originalChildren = new List<View>();

        private bool _alignLeft = false;
        private bool _alignTop = false;
        private bool _alignRight = false;
        private bool _alignBottom = false;

        public bool IsOpened { get; private set; }
        public Drawable Icon { get; private set; }
        public Color BackgroundColor { get; private set; }
        public float ButtonElevation { get; private set; }
        public int ButtonSpacing { get; private set; }
        public bool OpenOnHold { get; private set; }
        public int OpenDirection { get; private set; }

        public event EventHandler ButtonClick;
        public event EventHandler<LongClickEventArgs> ButtonLongClick;

        public FloatingActionMenu(Context context) : base(context) { }
        public FloatingActionMenu(Context context, IAttributeSet attrs) : base(context, attrs) { InitializeFromAttributes(context, attrs); }
        public FloatingActionMenu(Context context, IAttributeSet attrs, int defStyle) : base(context, attrs, defStyle) { InitializeFromAttributes(context, attrs); }

        public FloatingActionMenu(IntPtr javaReference, JniHandleOwnership transfer) : base(javaReference, transfer) { }

        private void InitializeFromAttributes(Context context, IAttributeSet attrs)
        {
            var attr = context.ObtainStyledAttributes(attrs, Resource.Styleable.FloatingActionMenu, 0, 0);

            Icon = attr.GetDrawable(Resource.Styleable.FloatingActionMenu_buttonIcon)
                ?? ResourcesCompat.GetDrawable(Resources, Resource.Drawable.fab_add, null);
            BackgroundColor = attr.GetColor(Resource.Styleable.FloatingActionMenu_buttonBackgroundColor, unchecked((int)0xFF263238));
            ButtonElevation = attr.GetFloat(Resource.Styleable.FloatingActionMenu_buttonElevation, 0.0f);
            ButtonSpacing = attr.GetInteger(Resource.Styleable.FloatingActionMenu_buttonSpacing, 30);
            OpenOnHold = attr.GetBoolean(Resource.Styleable.FloatingActionMenu_openOnHold, false);
            OpenDirection = attr.GetInteger(Resource.Styleable.FloatingActionMenu_openDirection, OPEN_UP);

            /*_alignLeft = attrs.GetAttributeBooleanValue(Android.Resource.Attribute.LayoutAlignParentLeft, false);
            _alignTop = attrs.GetAttributeBooleanValue(Android.Resource.Attribute.LayoutAlignParentTop, false);
            _alignRight = attrs.GetAttributeBooleanValue(Android.Resource.Attribute.LayoutAlignParentRight, false);
            _alignBottom = attrs.GetAttributeBooleanValue(Android.Resource.Attribute.LayoutAlignParentBottom, false);*/

            if (bool.TryParse(attrs.GetAttributeValue("http://schemas.android.com/apk/res/android", "layout_alignParentLeft"), out bool alignLeft))
            {
                _alignLeft = alignLeft;
            }

            if (bool.TryParse(attrs.GetAttributeValue("http://schemas.android.com/apk/res/android", "layout_alignParentTop"), out bool alignTop))
            {
                _alignTop = alignTop;
            }

            if (bool.TryParse(attrs.GetAttributeValue("http://schemas.android.com/apk/res/android", "layout_alignParentRight"), out bool alignRight))
            {
                _alignRight = alignRight;
            }

            if (bool.TryParse(attrs.GetAttributeValue("http://schemas.android.com/apk/res/android", "layout_alignParentBottom"), out bool alignBottom))
            {
                _alignBottom = alignBottom;
            }


            CreateMenuButton();
        }

        private void CreateMenuButton()
        {
            _menuButton = new FloatingActionButton(Context)
            {
                BackgroundTintList = ColorStateList.ValueOf(BackgroundColor),
                Elevation = ButtonElevation,
                LongClickable = OpenOnHold
            };
            _menuButton.SetImageDrawable(Icon);

            _menuButton.Click += (s, e) =>
            {
                if (IsOpened)
                {
                    CloseMenu();
                }
                else
                {
                    ButtonClick?.Invoke(s, e);

                    if (!OpenOnHold)
                    {
                        OpenMenu();
                    }
                }
            };

            if (OpenOnHold)
            {
                _menuButton.LongClick += (s, e) =>
                {
                    if (IsOpened)
                    {
                        CloseMenu();
                    }
                    else
                    {
                        OpenMenu();
                    }
                };
            }
            else
            {
                _menuButton.LongClick += (s, e) => ButtonLongClick?.Invoke(s, e);
            }
        }

        protected override void OnFinishInflate()
        {
            base.OnFinishInflate();

            FocusableInTouchMode = true;

            for (var i = 0; i < ChildCount; i++)
            {
                var child = GetChildAt(i);

                child.Visibility = ViewStates.Gone;
                child.ScaleX = 0.0f;
                child.ScaleY = 0.0f;

                _originalChildren.Add(child);
            }

            AddView(_menuButton, GenerateDefaultLayoutParams());
            BringChildToFront(_menuButton);
            CreateLabels();
        }

        private void CreateLabels()
        {
            foreach (var child in _originalChildren)
            {
                var layoutParameters = child.LayoutParameters as LayoutParams;

                if (!string.IsNullOrEmpty(layoutParameters.LabelText))
                {
                    var label = new TextView(Context)
                    {
                        Text = layoutParameters.LabelText,
                        TextSize = layoutParameters.LabelTextSize,
                        BackgroundTintList = ColorStateList.ValueOf(layoutParameters.LabelBackgroundColor),
                        Background = ResourcesCompat.GetDrawable(Resources, Resource.Drawable.fab_label_background, null),
                        Visibility = ViewStates.Gone,
                        Alpha = 0.0f,
                        TranslationX = 50.0f,
                        Clickable = true
                    };
                    label.SetTextColor(layoutParameters.LabelTextColor);
                    label.Click += (s, e) => child.CallOnClick();

                    AddView(label);
                    child.SetTag(Resource.Id.fab_label, label);
                }
            }
        }

        public void OpenMenu()
        {
            if (!IsOpened)
            {
                _menuButton.Enabled = false;
                _menuButton.Animate()
                    .Rotation(135.0f)
                    .SetDuration(ANIMATION_DURATION)
                    .Start();

                var backgroundAnimation = ObjectAnimator.OfInt(this, "backgroundColor", unchecked((int)0x00000000), unchecked((int)0xA0000000));
                backgroundAnimation.SetEvaluator(new ArgbEvaluator());
                backgroundAnimation.SetDuration(ANIMATION_DURATION)
                    .Start();

                int delay = 0;

                var children = (OpenDirection == OPEN_DOWN || OpenDirection == OPEN_RIGHT) ? _originalChildren : _originalChildren.AsEnumerable().Reverse().ToList();
                for (var i = 0; i < children.Count; i++)
                {
                    var child = children[i];
                    child.Visibility = ViewStates.Visible;

                    var animation = child.Animate()
                        .ScaleX(1.0f)
                        .ScaleY(1.0f)
                        .SetDuration(ANIMATION_DURATION)
                        .SetStartDelay(delay);

                    if (i == children.Count - 1)
                    {
                        animation = animation.WithEndAction(new Runnable(() => _menuButton.Enabled = true));
                    }

                    animation.Start();

                    if (child.GetTag(Resource.Id.fab_label) is TextView label)
                    {
                        label.Visibility = ViewStates.Visible;
                        label.Animate()
                            .Alpha(1.0f)
                            .TranslationX(0.0f)
                            .SetDuration(ANIMATION_DURATION)
                            .SetStartDelay(delay)
                            .Start();
                    }

                    delay += ANIMATION_DELAY;
                }
            }

            IsOpened = true;
        }

        public void CloseMenu()
        {
            if (IsOpened)
            {
                _menuButton.Enabled = false;
                _menuButton.Animate()
                    .Rotation(0.0f)
                    .SetDuration(ANIMATION_DURATION)
                    .Start();

                var backgroundAnimation = ObjectAnimator.OfInt(this, "backgroundColor", unchecked((int)0xA0000000), unchecked((int)0x00000000));
                backgroundAnimation.SetEvaluator(new ArgbEvaluator());
                backgroundAnimation.SetDuration(ANIMATION_DURATION)
                    .Start();

                int delay = 0;

                var children = (OpenDirection == OPEN_DOWN || OpenDirection == OPEN_RIGHT) ? _originalChildren.AsEnumerable().Reverse().ToList() : _originalChildren;
                for (var i = 0; i < children.Count; i++)
                {
                    var child = children[i];

                    var animation = child.Animate()
                        .ScaleX(0.0f)
                        .ScaleY(0.0f)
                        .SetDuration(ANIMATION_DURATION)
                        .SetStartDelay(delay);

                    if (i == children.Count - 1)
                    {
                        animation = animation.WithEndAction(new Runnable(() =>
                        {
                            child.Visibility = ViewStates.Gone;
                            _menuButton.Enabled = true;
                            IsOpened = false;
                        }));
                    }
                    else
                    {
                        animation = animation.WithEndAction(new Runnable(() => child.Visibility = ViewStates.Gone));
                    }

                    animation.Start();

                    if (child.GetTag(Resource.Id.fab_label) is TextView label)
                    {
                        var layoutParameters = child.LayoutParameters as LayoutParams;

                        label.Animate()
                            .Alpha(0.0f)
                            .TranslationX(layoutParameters.LabelDirection == LABEL_RIGHT ? -50.0f : 50.0f)
                            .SetDuration(ANIMATION_DURATION)
                            .SetStartDelay(delay)
                            .WithEndAction(new Runnable(() => label.Visibility = ViewStates.Gone))
                            .Start();
                    }

                    delay += ANIMATION_DELAY;
                }
            }
        }

        public override bool OnKeyDown([GeneratedEnum] Keycode keyCode, KeyEvent e)
        {
            if (IsOpened && _menuButton.Enabled && keyCode == Keycode.Back)
            {
                CloseMenu();
                return true;
            }
            else
            {
                return false;
            }
        }

        public override bool OnTouchEvent(MotionEvent @event)
        {
            if (IsOpened && _menuButton.Enabled)
            {
                CloseMenu();
            }

            return true;
        }

        protected override void OnMeasure(int widthMeasureSpec, int heightMeasureSpec)
        {
            for (var i = 0; i < ChildCount; i++)
            {
                var child = GetChildAt(i);
                if (child.Visibility != ViewStates.Gone)
                {
                    MeasureChildWithMargins(child, widthMeasureSpec, 0, heightMeasureSpec, 0);
                }
            }

            var marginLayoutParameters = (MarginLayoutParams)LayoutParameters;

            int width = (IsOpened || LayoutParameters.Width == ViewGroup.LayoutParams.MatchParent)
                ? GetDefaultSize(SuggestedMinimumWidth, widthMeasureSpec) + marginLayoutParameters.LeftMargin + marginLayoutParameters.RightMargin
                : _menuButton.MeasuredWidth + PaddingLeft + PaddingRight;

            int height = (IsOpened || LayoutParameters.Height == ViewGroup.LayoutParams.MatchParent)
                ? GetDefaultSize(SuggestedMinimumHeight, heightMeasureSpec) + marginLayoutParameters.TopMargin + marginLayoutParameters.BottomMargin
                : _menuButton.MeasuredHeight + PaddingTop + PaddingBottom;

            SetMeasuredDimension(width, height);
        }

        protected override void OnLayout(bool changed, int l, int t, int r, int b)
        {
            int menuLeft = PaddingLeft;
            int menuTop = PaddingTop;

            if (IsOpened)
            {
                if (!_alignLeft && _alignRight)
                {
                    menuLeft += (r - l) - _menuButton.MeasuredWidth - PaddingRight;
                }

                if (!_alignTop && _alignBottom)
                {
                    menuTop += (b - t) - _menuButton.MeasuredHeight - PaddingBottom;
                }
            }

            int menuRight = menuLeft + _menuButton.MeasuredWidth;
            int menuBottom = menuTop + _menuButton.MeasuredHeight;

            _menuButton.Layout(menuLeft, menuTop, menuRight, menuBottom);

            int childLeft = menuLeft;
            int childTop = menuTop;
            int childRight = menuRight;
            int childBottom = menuBottom;

            var children = (OpenDirection == OPEN_DOWN || OpenDirection == OPEN_RIGHT) ? _originalChildren : _originalChildren.AsEnumerable().Reverse();
            foreach (var child in children)
            {
                if (child.Visibility != ViewStates.Gone)
                {
                    if (OpenDirection == OPEN_UP || OpenDirection == OPEN_DOWN)
                    {
                        childLeft = menuLeft + (_menuButton.MeasuredWidth - child.MeasuredWidth) / 2;
                        childRight = childLeft + child.MeasuredWidth;

                        if (OpenDirection == OPEN_UP)
                        {
                            childTop = childTop - ButtonSpacing - child.MeasuredHeight;
                            childBottom = childTop + child.MeasuredHeight;
                        }
                        else
                        {
                            childBottom = childBottom + ButtonSpacing + child.MeasuredHeight;
                            childTop = childBottom - child.MeasuredHeight;
                        }
                    }
                    else
                    {
                        childTop = menuTop + (_menuButton.MeasuredHeight - child.MeasuredHeight) / 2;
                        childBottom = childTop + child.MeasuredHeight;

                        if (OpenDirection == OPEN_LEFT)
                        {
                            childLeft = childLeft - ButtonSpacing - child.MeasuredWidth;
                            childRight = childLeft + child.MeasuredWidth;
                        }
                        else
                        {
                            childRight = childRight + ButtonSpacing + child.MeasuredWidth;
                            childLeft = childRight - child.MeasuredWidth;
                        }
                    }

                    child.Layout(childLeft, childTop, childRight, childBottom);

                    if (child.GetTag(Resource.Id.fab_label) is View label)
                    {
                        int labelLeft = 0, labelTop = 0;

                        int childCenter = childLeft + (childRight - childLeft) / 2;
                        var layoutParameters = child.LayoutParameters as LayoutParams;

                        switch (layoutParameters.LabelDirection)
                        {
                            case LABEL_LEFT:
                                labelLeft = childLeft - ButtonSpacing - label.MeasuredWidth;
                                labelTop = childTop + child.MeasuredHeight / 2 - label.MeasuredHeight / 2;
                                break;
                            case LABEL_TOP:
                                labelLeft = childCenter - label.MeasuredWidth / 2;
                                labelTop = childTop - ButtonSpacing - label.MeasuredHeight;
                                break;
                            case LABEL_RIGHT:
                                labelLeft = childRight + ButtonSpacing;
                                labelTop = childTop + child.MeasuredHeight / 2 - label.MeasuredHeight / 2;
                                break;
                            case LABEL_BOTTOM:
                                labelLeft = childCenter - label.MeasuredWidth / 2;
                                labelTop = childBottom + ButtonSpacing;
                                break;
                        }

                        int labelRight = labelLeft + label.MeasuredWidth;
                        int labelBottom = labelTop + label.MeasuredHeight;

                        label.Layout(labelLeft, labelTop, labelRight, labelBottom);
                    }
                }
            }
        }

        public override ViewGroup.LayoutParams GenerateLayoutParams(IAttributeSet attrs) => new LayoutParams(Context, attrs);

        protected override ViewGroup.LayoutParams GenerateLayoutParams(ViewGroup.LayoutParams p) => new LayoutParams(p);

        protected override ViewGroup.LayoutParams GenerateDefaultLayoutParams() => new LayoutParams(ViewGroup.LayoutParams.WrapContent, ViewGroup.LayoutParams.WrapContent);

        protected override bool CheckLayoutParams(ViewGroup.LayoutParams p) => p is LayoutParams;

        public new class LayoutParams : MarginLayoutParams
        {
            public string LabelText { get; set; }
            public int LabelTextSize { get; set; }
            public Color LabelTextColor { get; set; }
            public Color LabelBackgroundColor { get; set; }
            public int LabelDirection { get; set; }

            public LayoutParams(ViewGroup.LayoutParams source) : base(source) { }
            public LayoutParams(int width, int height) : base(width, height) { }
            public LayoutParams(Context c, IAttributeSet attrs) : base(c, attrs)
            {
                var typedArray = c.ObtainStyledAttributes(attrs, Resource.Styleable.FloatingActionMenu, 0, 0);

                LabelText = typedArray.GetString(Resource.Styleable.FloatingActionMenu_layout_labelText);
                LabelTextSize = typedArray.GetInteger(Resource.Styleable.FloatingActionMenu_layout_labelTextSize, 12);
                LabelTextColor = typedArray.GetColor(Resource.Styleable.FloatingActionMenu_layout_labelTextColor, unchecked((int)0xFFFFFFFF));
                LabelBackgroundColor = typedArray.GetColor(Resource.Styleable.ViewBackgroundHelper_backgroundTint, unchecked((int)0xFF263238));
                LabelDirection = typedArray.GetInteger(Resource.Styleable.FloatingActionMenu_layout_labelDirection, LABEL_RIGHT);
            }

            protected LayoutParams(IntPtr javaReference, JniHandleOwnership transfer) : base(javaReference, transfer) { }
        }
    }
}
