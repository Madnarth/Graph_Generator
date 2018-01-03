using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Text.RegularExpressions;
using System.Linq;

namespace Graph_Generator
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    /// 

    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        public int ellipseSize = 40;
        
        Vertices verticesList = new Vertices();
        Tuple<double, double, double> cords; //Item1 - x, Item2 - y, Item3 - stopień
        //
        //Przechodzenie z układu biegunowego na kartezjański
        public Tuple<double, double, double> GetVertexCords(int _n, double _deg) 
        {
            double shift = (360 / _n);
            double radius = int.Parse(canGraph.ActualWidth.ToString()) / 2 - ellipseSize / 2;
            double deg = _deg - shift;
            return Tuple.Create(radius * Math.Cos(deg * 0.01745329), radius * Math.Sin(deg * 0.01745329), deg);
        }
        decimal NullAsZero(string textboxText)
        {
            return Convert.ToDecimal(string.IsNullOrEmpty(textboxText) ? "0" : textboxText);
        }        
        public double ShiftForEdge()
        {
            return canGraph.ActualWidth / 2;
        }
        public double ShiftForVertex()
        {
            return ShiftForEdge() - ellipseSize / 2;
        }
        private void buttGraphGen_Click(object sender, RoutedEventArgs e)
        {
            buttGraphColor.IsEnabled = true;
            canGraph.Children.Clear();
            
            verticesList.VertexList = new List<Vertex>();
            txtBoxTest.Clear();
            string probability = int.Parse(txtBoxProbability0.Text) + System.Globalization.CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSepar‌​ator + int.Parse(txtBoxProbability1.Text);
            Adjacency_Matrix.N = int.Parse(txtBoxVerticesQuantity.Text);
            Adjacency_Matrix.P = Double.Parse(probability);

            var matrix = new Adjacency_Matrix();
            matrix.Fill_Half_Adjacency_Matrix();
            matrix.Reflect_Adjacency_Matrix();

            cords = new Tuple<double, double, double>(0.0, 0.0, 0.0);

            for (int i = 0; i < Adjacency_Matrix.N; i++)
            {
                var vertex = new Vertex(i + 1);                
                verticesList.VertexList.Add(vertex);
                cords = GetVertexCords(Adjacency_Matrix.N, cords.Item3);
                vertex.POSX = cords.Item1;
                vertex.POSY = cords.Item2;
            }
            DrawVerticesFromList(verticesList, ShiftForVertex());
            DrawEdgeFromList(verticesList, ShiftForEdge());
            FindNeighbors(verticesList);
        }
        //
        //Rysowanie wierzchołków z listy
        private void DrawVerticesFromList(Vertices _verticesList, double _shift)
        {
            foreach (var item in _verticesList)
            {
                DrawVertex(new Point(item.POSX + _shift, item.POSY + _shift), (SolidColorBrush)(new BrushConverter().ConvertFrom("#000000")), (SolidColorBrush)(new BrushConverter().ConvertFrom(item.COLOR)), item.ID.ToString());
            }
        }
        //
        //Ryosawnie wierzchołka
        private void DrawVertex(Point _point, SolidColorBrush _solidStrokeColorBrush, SolidColorBrush _solidFillColorBrush, string _vID)
        {
            Ellipse ellipse = new Ellipse();
            ellipse.Width = ellipseSize;
            ellipse.Height = ellipseSize;
            ellipse.Margin = new Thickness(_point.X, _point.Y, 0, 0);
            ellipse.Fill = _solidFillColorBrush;
            ellipse.StrokeThickness = 2;
            ellipse.Stroke = _solidStrokeColorBrush;
            Canvas.SetZIndex(ellipse, 2);
            canGraph.Children.Add(ellipse);

            TextBlock textBlock = new TextBlock();
            textBlock.Text = _vID;
            textBlock.Foreground = _solidStrokeColorBrush;
            if (_vID.Length > 1)
                textBlock.Margin = new Thickness(_point.X + 8, _point.Y + 6, 0, 0);
            else
                textBlock.Margin = new Thickness(_point.X + 14, _point.Y + 6, 0, 0);
            textBlock.FontSize = 20;
            Canvas.SetZIndex(textBlock, 3);
            canGraph.Children.Add(textBlock);
        }
        //
        //Rysowanie krawędzi z listy wierzchołków
        private void DrawEdgeFromList(Vertices _verticesList, double _shift)
        {
            for (int i = 0; i < Adjacency_Matrix.N; i++)
            {
                for (int j = 0; j < Adjacency_Matrix.N; j++)
                {
                    if (i < j && Adjacency_Matrix.AdjMatrix[i, j] == 1)
                    {
                        DrawEdge(_verticesList.FirstOrDefault(x1 => x1.ID == i + 1).POSX + _shift,
                            _verticesList.FirstOrDefault(x2 => x2.ID == j + 1).POSX + _shift,
                            _verticesList.FirstOrDefault(y1 => y1.ID == i + 1).POSY + _shift,
                            _verticesList.FirstOrDefault(y2 => y2.ID == j + 1).POSY + _shift,
                            (SolidColorBrush)(new BrushConverter().ConvertFrom("#000000")));
                    }
                }
            }
        }
        //
        //Rysowanie krawędzi
        private void DrawEdge(double _x1, double _x2, double _y1, double _y2, SolidColorBrush _solidStrokeColorBrush)
        {
            Line line = new Line();
            line.Stroke = _solidStrokeColorBrush;
            line.StrokeThickness = 2;
            line.X1 = Convert.ToDouble(_x1);
            line.X2 = Convert.ToDouble(_x2);
            line.Y1 = Convert.ToDouble(_y1);
            line.Y2 = Convert.ToDouble(_y2);
            Canvas.SetZIndex(line, 1);
            canGraph.Children.Add(line);
        }
        //
        //Znajdywanie sąsiadów
        private void FindNeighbors(Vertices _verticesList)
        {
            for (int i = 0; i < Adjacency_Matrix.N; i++)
            {
                for (int j = 0; j < Adjacency_Matrix.N; j++)
                {
                    if (i < j && Adjacency_Matrix.AdjMatrix[i, j] == 1)
                    {
                        _verticesList.First(x => x.ID == i + 1).NEIGHBORS.Add(j + 1);
                        _verticesList.First(x => x.ID == i + 1).DEGREE += 1;
                        _verticesList.First(x => x.ID == j + 1).NEIGHBORS.Add(i + 1);
                        _verticesList.First(x => x.ID == j + 1).DEGREE += 1;
                    }
                }
            }
        }
        //
        //Sprawdzanie, czy graf jest spójny
        private bool CheckIfConsistent(Vertices _verticesList)
        {
            bool consistent = true;
            foreach (var item in _verticesList)
            {
                bool isEmpty = !item.NEIGHBORS.Any();
                if (isEmpty)
                {
                    consistent = false;
                }
            }
            return consistent;
        }
        //
        //Kolorwanie grafu
        private void GraphColoring(Vertices _verticesList)
        {
            try
            {
                var rand = new Random();
                var masterIndex = rand.Next(1, _verticesList.Count() + 1);
                string tempColor = "#FFFFFF";
                List<string> taken = new List<string>();
                var tempList = _verticesList.Where(x => x.ID != masterIndex).ToList();

                List<string> setOfColors = new List<string>();
                if (_verticesList.Count() > HexColor.HEXCOLOR.Count)
                {
                    throw new Exception("Not enough colors in my palette to perform graph coloring\nSorry");
                }
                else
                { 
                    for (int i = 0; i < _verticesList.Count(); i++)
                    {
                        int k = rand.Next(0, HexColor.HEXCOLOR.Count);
                        if (!setOfColors.Contains(HexColor.HEXCOLOR[k]))
                        {
                            setOfColors.Add(HexColor.HEXCOLOR[k]);
                        }
                        else
                        {
                            i--;
                        }
                    }
                }

                _verticesList.FirstOrDefault(x => x.ID == masterIndex).COLOR = setOfColors[0];

                tempList = tempList.OrderByDescending(x => x.DEGREE).ToList();

                foreach (var item in tempList)
                {
                    taken.Clear();
                    foreach (var item2 in item.NEIGHBORS)
                    {
                        for (int i = 0; i < setOfColors.Count; i++)
                        {
                            if (_verticesList.FirstOrDefault(x => x.ID == item2).COLOR != setOfColors[i] && !taken.Contains(setOfColors[i]))
                            {
                                tempColor = setOfColors[i];
                                if (!taken.Contains(_verticesList.FirstOrDefault(x => x.ID == item2).COLOR))
                                {
                                    taken.Add(_verticesList.FirstOrDefault(x => x.ID == item2).COLOR);
                                }
                                break;
                            }
                            else
                            {
                                taken.Add(setOfColors[i]);
                            }
                        }
                    }
                    item.COLOR = tempColor;
                }
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
        }
        private void txtBoxProbability0_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (System.Text.RegularExpressions.Regex.IsMatch(txtBoxProbability0.Text, "[^0-1]"))
            {
                    txtBoxProbability0.Text = txtBoxProbability0.Text.Remove(txtBoxProbability0.Text.Length - 1); 
            }
            else
            {
                if (NullAsZero(txtBoxProbability0.Text) > 0 && NullAsZero(txtBoxProbability1.Text) > 0)
                {
                    MessageBox.Show("The probability value should be 0-1");
                    txtBoxProbability0.Text = txtBoxProbability0.Text.Remove(txtBoxProbability0.Text.Length - 1);
                }
            }
        }
        private void txtBoxProbability1_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (System.Text.RegularExpressions.Regex.IsMatch(txtBoxProbability1.Text, "^[^0-9]"))
            {
                txtBoxProbability1.Text = txtBoxProbability1.Text.Remove(txtBoxProbability1.Text.Length - 1);
            }
            else
            {
                if (NullAsZero(txtBoxProbability0.Text) > 0 && NullAsZero(txtBoxProbability1.Text) > 0)
                {
                    MessageBox.Show("The probability value should be 0-1");
                    txtBoxProbability1.Text = txtBoxProbability1.Text.Remove(txtBoxProbability1.Text.Length - 1);
                }
            }
        }

        private void txtBoxProbability0_LostFocus(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtBoxProbability0.Text))
            {
                txtBoxProbability0.Text = "0";
            }
        }

        private void txtBoxProbability1_LostFocus(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtBoxProbability1.Text))
            {
                txtBoxProbability1.Text = "0";
            }
        }

        private void buttGraphColor_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (CheckIfConsistent(verticesList) == true)
                {
                    GraphColoring(verticesList);
                    DrawVerticesFromList(verticesList, ShiftForVertex());
                }
                else
                {
                    throw new Exception("Graph must be consistent in order to color it\nPlease generate new graph");
                }
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
        }

        private void txtBoxVerticesQuantity_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (System.Text.RegularExpressions.Regex.IsMatch(txtBoxVerticesQuantity.Text, "[^0-9]"))
            {
                txtBoxVerticesQuantity.Text = txtBoxVerticesQuantity.Text.Remove(txtBoxVerticesQuantity.Text.Length - 1);
            }
        }
    }
}
