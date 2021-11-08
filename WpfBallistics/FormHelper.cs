using System.Windows;
using System.Windows.Controls;
using System.Windows.Shapes;

namespace WpfBallistics
{
    /// <summary>
    /// Класс для помощи при работе с элементами формы
    /// </summary>
    static public class FormHelper
    {
        /// <summary>
        /// ПОлучение ListBox'a
        /// </summary>
        /// <param name="MainRoot">Grid в котором надо искать элемент</param>
        /// <param name="elementName">Название элемента</param>
        /// <returns>Возврат нужного ListBox'a</returns>
        public static ListView GetListView(Grid MainRoot, string elementName)
        {
            foreach (UIElement el in MainRoot.Children)
            {
                if (el is ListView view)
                {
                    if (view.Name == elementName)
                    {
                        return view;
                    }
                }
            }
            return null;
        }
        /// <summary>
        /// ПОлучение DataGrid'a
        /// </summary>
        /// <param name="MainRoot">Grid в котором надо искать элемент</param>
        /// <param name="elementName">Название элемента</param>
        /// <returns>Возврат нужного DataGrid'a</returns>
        public static DataGrid GetDataGrid(Grid MainRoot, string elementName)
        {
            foreach (UIElement el in MainRoot.Children)
            {
                if (el is DataGrid grid)
                {
                    if (grid.Name == elementName)
                    {
                        return grid;
                    }
                }
            }
            return null;
        }
        /// <summary>
        /// ПОлучение Button'a
        /// </summary>
        /// <param name="MainRoot">Grid в котором надо искать элемент</param>
        /// <param name="elementName">Название элемента</param>
        /// <returns>Возврат нужного Button'a</returns>
        public static Button GetButton(Grid MainRoot, string elementName)
        {
            foreach (UIElement el in MainRoot.Children)
            {
                if (el is Button button)
                {
                    if (button.Name == elementName)
                    {
                        return button;
                    }
                }
            }
            return null;
        }
        /// <summary>
        /// ПОлучение TextBox'a
        /// </summary>
        /// <param name="MainRoot">Grid в котором надо искать элемент</param>
        /// <param name="elementName">Название элемента</param>
        /// <returns>Возврат нужного TextBox'a</returns>
        public static TextBox GetTextBox(Grid MainRoot, string elementName)
        {
            foreach (UIElement el in MainRoot.Children)
            {
                if (el is TextBox box)
                {
                    if (box.Name == elementName)
                    {
                        return box;
                    }
                }
            }
            return null;
        }
        /// <summary>
        /// Метод для удаления нужного TextBlock элемента
        /// </summary>
        /// <param name="MainRoot">Grid в котором есть нужный элемент</param>
        /// <param name="name">Название элемента</param>
        /// <returns>true - если удален элемент</returns>
        public static bool DeleteTextBlock(Grid MainRoot, string name)
        {
            foreach (UIElement el in MainRoot.Children)
            {
                if (el is TextBlock block)
                {
                    if (block.Name.Contains(name))
                    {
                        MainRoot.Children.Remove((UIElement)block);
                        return true;
                    }
                }
            }
            return false;
        }
        /// <summary>
        /// Метод для удаления нужного TextBox элемента
        /// </summary>
        /// <param name="MainRoot">Grid в котором есть нужный элемент</param>
        /// <param name="name">Название элемента</param>
        /// <returns>true - если удален элемент</returns>
        public static bool DeleteTextBox(Grid MainRoot, string name)
        {
            foreach (UIElement el in MainRoot.Children)
            {
                if (el is TextBox box)
                {
                    if (box.Name.Contains(name))
                    {
                        MainRoot.Children.Remove((UIElement)box);
                        return true;
                    }
                }
            }
            return false;
        }
        /// <summary>
        /// Метод для удаления нужного Rectangle элемента
        /// </summary>
        /// <param name="MainRoot">Grid в котором есть нужный элемент</param>
        /// <param name="name">Название элемента</param>
        /// <returns>true - если удален элемент</returns>
        public static bool DeleteRectangle(Grid MainRoot, string name)
        {
            foreach (UIElement el in MainRoot.Children)
            {
                if (el is Rectangle rectangle)
                {
                    if (rectangle.Name.Contains(name))
                    {
                        MainRoot.Children.Remove((UIElement)rectangle);
                        return true;
                    }
                }
            }
            return false;
        }
    }
}