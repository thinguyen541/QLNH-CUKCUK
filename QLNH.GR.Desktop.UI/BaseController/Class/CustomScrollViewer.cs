using System.Windows.Controls;
using System.Windows.Input;
using System.Windows;
using System.Windows.Media;

namespace QLNH.GR.Desktop.UI
{

    public class CustomScrollView : ScrollViewer
    {

        private bool isDragging;
        private Point lastMousePosition;

        public CustomScrollView()
        {
            // Attach event handlers
            this.PreviewMouseDown += OnMouseDown;
            this.PreviewMouseMove += OnMouseMove;
            this.PreviewMouseUp += OnMouseUp;
            this.PreviewMouseWheel += OnMouseWheel;
        }

        private void OnMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (true)
            {
                isDragging = true;
                lastMousePosition = e.GetPosition(this);
               
            }
        }

        private void OnMouseMove(object sender, MouseEventArgs e)
        {
            if (isDragging)
            {
                Point currentMousePosition = e.GetPosition(this);
                Vector delta = currentMousePosition - lastMousePosition;

                this.ScrollToVerticalOffset(this.VerticalOffset - delta.Y);
                this.ScrollToHorizontalOffset(this.HorizontalOffset - delta.X);

                lastMousePosition = currentMousePosition;
            }
        }

        private void OnMouseUp(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Released)
            {
                isDragging = false;
               
            }
        }


        private void OnMouseWheel(object sender, MouseWheelEventArgs e)
        {
            if (true)
            {
                // Scroll the CustomScrollView if the mouse wheel event occurred elsewhere
                this.ScrollToVerticalOffset(this.VerticalOffset - e.Delta);
                e.Handled = true;

            }
        }

        private bool IsMouseOverListView(MouseEventArgs e)
        {
            DependencyObject source = e.OriginalSource as DependencyObject;
            return source != null && FindVisualParent<ListView>(source) != null;
        }

        private T FindVisualParent<T>(DependencyObject child) where T : DependencyObject
        {
            DependencyObject parentObject = VisualTreeHelper.GetParent(child);
            if (parentObject == null) return null;

            if (parentObject is T parent)
                return parent;
            else
                return FindVisualParent<T>(parentObject);
        }
    }
}

