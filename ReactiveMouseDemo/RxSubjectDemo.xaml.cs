using System;
using System.Diagnostics;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Windows;

namespace ReactiveMouseDemo
{
    /// <summary>
    /// RxSubjectDemo.xaml 的交互逻辑
    /// </summary>
    public partial class RxSubjectDemo : Window
    {
        public RxSubjectDemo()
        {
            InitializeComponent();
            Subject<string> sbj = new Subject<string>();

            Observable.Interval(TimeSpan.FromSeconds(1))
             .Select(x => "温度: " + x)
            .Subscribe(x => {
                this.Dispatcher.Invoke(() => List.Items.Add("温度数据1秒一个：" + x));
            });

            Observable.Interval(TimeSpan.FromSeconds(2))
             .Select(x => "位移: " + x)  
             .Do((x)=>
             {
                 Console.WriteLine($"位移日志，当前的位移数据为：{x}");             
             })
             .Subscribe(x => {
                 this.Dispatcher.Invoke(() => List2.Items.Add("位移数据2秒一个：" + x));  
             });
            Observable.Interval(TimeSpan.FromSeconds(2))
            .Select(x => "位移: " + x).Subscribe(sbj);

            Observable.Interval(TimeSpan.FromSeconds(1))
            .Select(x => "温度: " + x)
           .Subscribe(sbj);


            sbj.Subscribe(x=> {
                this.Dispatcher.Invoke(() => List3.Items.Add("综合显示："+x));
            });

            



        }


    }
}
