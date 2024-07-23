using Microsoft.Xaml.Behaviors;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using WorkScheduleVisualizer.Models;

namespace WorkScheduleVisualizer.Behaviors
{
    public class DragDropBehavior : Behavior<FrameworkElement>
    {
        public static readonly DependencyProperty DropCommandProperty =
            DependencyProperty.Register("DropCommand", typeof(ICommand), typeof(DragDropBehavior), new PropertyMetadata(null));

        public ICommand DropCommand
        {
            get { return (ICommand)GetValue(DropCommandProperty); }
            set { SetValue(DropCommandProperty, value); }
        }

        public string ShiftType { get; set; }

        protected override void OnAttached()
        {
            base.OnAttached();
            AssociatedObject.AllowDrop = true;
            AssociatedObject.MouseMove += OnMouseMove;
            AssociatedObject.DragEnter += OnDragEnter;
            AssociatedObject.DragLeave += OnDragLeave;
            AssociatedObject.Drop += OnDrop;
        }

        protected override void OnDetaching()
        {
            base.OnDetaching();
            AssociatedObject.MouseMove -= OnMouseMove;
            AssociatedObject.DragEnter -= OnDragEnter;
            AssociatedObject.DragLeave -= OnDragLeave;
            AssociatedObject.Drop -= OnDrop;
        }

        private void OnMouseMove(object sender, MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed) {
                var draggedItem = sender as FrameworkElement;
                if (draggedItem != null) {
                    DragDrop.DoDragDrop(draggedItem, draggedItem.DataContext, DragDropEffects.Move);
                }
            }
        }

        private void OnDragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(typeof(Employee)) || e.Data.GetDataPresent(typeof(Shift))) {
                var border = sender as Border;
                if (border != null) {
                    border.Background = new SolidColorBrush(Colors.LightCoral);
                }
            }
        }

        private void OnDragLeave(object sender, DragEventArgs e)
        {
            var border = sender as Border;
            if (border != null) {
                border.Background = new SolidColorBrush(Colors.LightBlue);
            }
        }

        private void OnDrop(object sender, DragEventArgs e)
        {
            var border = sender as Border;
            if (border != null) {
                border.Background = new SolidColorBrush(Colors.LightBlue);
                var schedule = border.DataContext as Schedule;

                if (schedule != null) {
                    var shiftType = ShiftType;
                    var droppedData = e.Data.GetDataPresent(typeof(Employee)) ? e.Data.GetData(typeof(Employee)) : e.Data.GetData(typeof(Shift));

                    var dropParameter = new Tuple<Schedule, string, object>(schedule, shiftType, droppedData);
                    if (DropCommand != null && DropCommand.CanExecute(dropParameter)) {
                        DropCommand.Execute(dropParameter);
                    }
                }
            }
        }
    }
}
