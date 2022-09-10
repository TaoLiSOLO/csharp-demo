using System;
using System.Linq;
using System.Reactive.Linq;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Reactive.Subjects;

namespace ReactiveMouseDemo
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// 这是一个样例，展示Reactive  的优势
    /// </summary>
    public partial class MainWindow : Window
    {

        private IDisposable _subscription;
        public MainWindow()
        {
            InitializeComponent();


            //  事件转Observable  Rx提供的方法为：FromEventPattern
            var mouseDowns = Observable.FromEventPattern<MouseButtonEventArgs>(this, "MouseDown");
            var mouseUp = Observable.FromEventPattern<MouseButtonEventArgs>(this, "MouseUp");
            var movements = Observable.FromEventPattern<MouseEventArgs>(this, "MouseMove");

            Polyline line = null;
            _subscription =
                 movements
                 .Do(x => Console.WriteLine($"鼠标移动时的坐标：" +
                                            $"X_{x.EventArgs.GetPosition(this).X}," +
                                            $"Y_{x.EventArgs.GetPosition(this).Y}"))
                 .SkipUntil(
                     mouseDowns.Do(_ =>
                     {
                         line = new Polyline() { Stroke = Brushes.Black, StrokeThickness = 3 };
                         canvas.Children.Add(line);
                     }))   // movements 不会被订阅，除非接收到 mouseDowns
                 .TakeUntil(mouseUp) //一直被订阅，直到 mouseUp
                 .Select(m => m.EventArgs.GetPosition(this))
                 .Repeat()
                 .Subscribe(pos => line.Points.Add(pos));


            mouseDowns.Select(m => m.EventArgs.GetPosition(this)).Repeat().Subscribe(x =>
            {
                this.Dispatcher.Invoke(() => List.Items.Add($"鼠标点击时的坐标:X_{x.X},Y_{x.Y}"));
            });
            mouseUp.Select(m => m.EventArgs.GetPosition(this)).Repeat().Subscribe(x =>
            {
                this.Dispatcher.Invoke(() => List.Items.Add($"鼠标抬起时的坐标:X_{x.X},Y_{x.Y}"));
            });


























            //SkipUntil   从后面的委托中的条件开始订阅

            //IObservable<string> messages = ...
            //IObservable<string> controlChannel = ...
            //
            // messages.SkipUntil(controlChannel.Where(m => m == "START"))  
            // .Subscribe(
            // msg => {/* add to message screen */ },
            // ex => { /* error handling */},
            // () => { /* completion handling */});


            //Do  执行当前Observable的，在side  Effect 

            // Observable.Range(1, 5)
            //.Do(x => { Console.WriteLine("{0} was emitted", x); })
            //.Where(x => x % 2 == 0)
            //.Do(x => { Console.WriteLine("{0} survived the Where()", x); })
            //.Select(x => x * 3)
            //.SubscribeConsole("final");

            //输出结果：
            //1 was emitted
            //2 was emitted
            //2 survived the Where()
            //final - OnNext(6)
            //3 was emitted
            //4 was emitted
            //4 survived the Where()
            //final - OnNext(12)
            //5 was emitted
            //final - OnCompleted()

            //Select  筛选当前的位置

            //Repeat  重新订阅
            //  Observable.Range(1, 3)
            // .Repeat(2)
            // .SubscribeConsole("Repeat(2)");
            // 输出如下：
            //Repeat(2) - OnNext(1)
            //Repeat(2) - OnNext(2)
            //Repeat(2) - OnNext(3)
            //Repeat(2) - OnNext(1)
            //Repeat(2) - OnNext(2)
            //Repeat(2) - OnNext(3)
            //Repeat(2) - OnCompleted()


        }
    }
}
