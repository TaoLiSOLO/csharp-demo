using System;
using System.Reactive.Linq;
using System.Windows;

namespace ReactiveMouseDemo
{
    /// <summary>
    /// SearchWindow.xaml 的交互逻辑
    /// </summary>
    public partial class SearchWindow : Window
    {
        public SearchWindow()
        {
            InitializeComponent();
            Observable.FromEventPattern(SearchTerm, "TextChanged")
            .Select(_ => SearchTerm.Text)
            .Throttle(TimeSpan.FromMilliseconds(1000))
            .DistinctUntilChanged()
            .ObserveOnDispatcher()
            .Subscribe(s => Terms.Items.Add(s));
        }
    }
}
