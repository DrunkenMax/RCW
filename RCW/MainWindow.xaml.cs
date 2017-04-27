using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace RCW
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        System.Windows.Threading.DispatcherTimer timer = new System.Windows.Threading.DispatcherTimer();
        public MainWindow()
        {
            InitializeComponent();
        }
        const double c = 299792458;
        const double R = 6371000;
        const double G = 6.67408e-11;
        const double M = 5.9742e24;
        //  c - скорость света
        //  R - радиус Земли
        //  G - гравитационная постоянная
        //  M - масса Земли
        // da - расстояние от Земли до астероида
        // d1 - расстояние от Земли до ракеты
        // d2 - расстояние от ракеты до астероида
        // hp - здоровье экипажа
        // kt - вычисляемый промежуток времени
        // tp - прошло времени полета
        // M1 - масса астероида
        // R1 - радиус астероида
        // v0 - скорость всей ракеты перед выбросом
        // v1 - скорость всей ракеты после выброса
        // v2 - скорость вылета топлива относительно пространства
        // vf - скорость вылета топлива относительно ракеты
        // vc - максимальная скорость подачи топлива
        //vc2 - скорость подачи топлива снизу
        //vc3 - скорость подачи топлива сверху
        // p0 - импульс всей ракеты после выброса
        // p2 - импульс сброшенного топлива снизу
        // p3 - импульс сброшенного топлива сверху
        //  r - расстояние до центра Земли
        // r1 - растояние до центра астероида
        //  g - ускорение свободного падения на ракете
        // a0 - конечное укорение ракеты
        // m0 - масса всей ракеты перед выбросом
        // m1 - масса всей ракеты после выброса
        // m2 - масса сброшенного топлива снизу
        // m3 - масса сброшенного топлива сверху
        // mr - масса пустой ракеты
        // mf - текущая масса топлива
        // ol - перегрузка экипажа
        //  s - расстояние, пройденное за промежуток времени
        //fs0 - весь массив массы топлива в ступенях
        //rs0 - весь массив массы корпуса в ступенях
        //ks0 - изначальное количество ступеней
        // ks - текущее количество ступеней
        // so - сброшена ли ступень
        // ts - нажималась ли кнопка перезапуска
        // sp - запас прочности
        // ig - на земле и ракета
        //  S - площадь фронтальной поверхности ракеты
        // cx - коэффициент аэродинамического сопротивления ракеты
        // Sp - площадь фронтальной поверхности парашюта
        //cxp - коэффициент аэродинамического сопротивления парашюта
        //par - выпущен ли парашют
        double kt, M1, R1, v0, m0, m2, m3, m1, v1, v2, p0, p2, p3, r, r1, g, a0, hp, da, d1, d2, mr, mf, vf, vc, tp, vc3, vc2, ol, s, sp, S, cx, Sp, cxp;
        int ks0, ks; double[] fs0, rs0; bool so, ts, ig, par;
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            comboBox_FuelOut.SelectedIndex = 1; ts = false;
        }
        private void timerTick(object sender, EventArgs e)
        {
            if (!image6.IsVisible)
                kt = slider_Time.Value * slider_Time.Value * slider_Time.Value * 0.01;
            else
                kt = 0.01;
            //перерасчет массы топлива и ракеты, если сброшена ступень
            if (so)
            {
                so = false;
                if (ks > 1)
                {
                    double s1 = 0, s2 = 0;
                    ks -= 1;
                    for (int i = 0; i < ks; i++)
                    {
                        s1 += rs0[i];
                        s2 += fs0[i];
                    }
                    if (s1 != mr) mr = s1;
                    if (s2 < mf) mf = s2;
                }
            }
            m0 = mr + mf;
            //регенерация экипажа и починка ракеты
            if (sp > 0) sp += (kt / 60) * (hp / 1000);
            if (hp > 0) hp += kt / 60;
            if (sp > 1000) sp = 1000;
            if (hp > 1000) hp = 1000;
            //расчет всех величин за промежуток времени
            tp += kt;
            vc2 = vc*(Convert.ToDouble(slider_Down.Value)/1000);
            vc3 = vc*(Convert.ToDouble(slider_Top.Value)/1000);
            m2 = vc2 * kt;
            m3 = vc3 * kt;
            m1 = m0 - (m2 + m3);
            v2 = vf - v0;
            p0 = (m1 * v0) / Math.Sqrt(1 - ((v0 * v0) / (c * c)));
            p2 = (m2 * v2) / Math.Sqrt(1 - ((v2 * v2) / (c * c)));
            p3 = (m3 * v2) / Math.Sqrt(1 - ((v2 * v2) / (c * c)));
            v1 = 0;
            r = d1 + R;
            r1 = d2 + R1;
            g = ((G * M) / (r * r)) - ((G * M1) / (r1 * r1));
            if (p0 + p2 - p3 > 0) v1 = c / (Math.Sqrt(Math.Pow((c * m1) / (p0 + p2 - p3), 2) + 1));
            if (p0 + p2 - p3 < 0) v1 = -c / (Math.Sqrt(Math.Pow((c * m1) / (p0 + p2 - p3), 2) + 1));
            mf = mf - (m2 + m3);
            a0 = 0; ol = 0;
            m0 = m1;
            //проверяется, находится ли ракета в атмосфере, расчет аэродинамического замедления
            if (par)
            {
                par = false;
                cx = ((cxp * Sp) + (cx * S)) / (S + Sp);
                S += Sp;
            }
            double ae = 0;
            double ro = 0;
            if (d1 < (R / 50))
            {
                if (v1 > 0)
                {
                    ro = 1.3 * (((R / 50) - d1) / (R / 50));
                    ae = -(ro * S * cx * v0 * v0) / (m0 + m2 + m3);
                }
                if (v1 < 0)
                {
                    ro = 1.3 * (((R / 50) - d1) / (R / 50));
                    ae = (ro * S * cx * v0 * v0) / (m0 + m2 + m3);
                }
                if (image6.Visibility == Visibility.Hidden)
                    image6.Visibility = Visibility.Visible;
            }
            if (d2 < (R1 / 50))
            {
                if (v1 > 0)
                {
                    ro = 1.3 * (((R1 / 50) - d2) / (R1 / 50));
                    ae = -(ro * S * cx * v0 * v0) / (m0 + m2 + m3);
                }
                if (v1 < 0)
                {
                    ro = 1.3 * (((R1 / 50) - d2) / (R1 / 50));
                    ae = -(ro * S * cx * v0 * v0) / (m0 + m2 + m3);
                }
                if (image6.Visibility == Visibility.Hidden)
                    image6.Visibility = Visibility.Visible;
            }
            if ((d1 >= (R / 50)) && (d2 >= (R1 / 50)))
                image6.Visibility = Visibility.Hidden;
            //закончилось ли топливо + завершающие присваивания
            if (mf > 0)
            {
                a0 = ((v1 - v0) / kt) - g + ae;
                ol = Math.Abs((a0 + g) / 9.81);
            }
            else
            {
                mf = 0;
                image1.Visibility = Visibility.Visible;
                a0 = -g + ae;
                ol = Math.Abs((a0 + g) / 9.81);
                button2.Background = Brushes.LightGreen;
            }
            v1 = v0 + (a0 * kt);
            s = Convert.ToDouble((v0 * kt) + (a0 * kt * (kt / 2)));
            //оставляет ракету на земле, расчет прочности
            if (d1 + s < 0)
            {
                d1 = 0;
                v1 = 0;
                d2 = da;
                ol = Math.Abs(g / 9.81);
                if (!ig)
                {
                    ig = true;
                    if (Math.Abs(v0) > 1) sp -= Math.Abs(v0) * 10;
                    if (Math.Abs(v0) > 1) hp -= Math.Abs(v0) * 10;
                }
            }
            else
            {
                if (d2 - s < 0)
                {
                    d1 = da;
                    v1 = 0;
                    d2 = 0;
                    ol = Math.Abs(g / 9.81);
                    if (!ig)
                    {
                        ig = true;
                        if (Math.Abs(v0) > 1) sp -= Math.Abs(v0) * 10;
                        if (Math.Abs(v0) > 1) hp -= Math.Abs(v0) * 10;
                    }
                }
                else
                {
                    ig = false;
                    d2 = d2 - s;
                    d1 = d1 + s;
                }
            }
            if (sp < 0)
            {
                sp = 0;
                image4.Visibility = Visibility.Visible;
            }
            if (sp <= 200) label_StrPoints.Foreground = Brushes.DarkRed;
            if ((sp <= 400) && (sp > 200)) label_StrPoints.Foreground = Brushes.Red;
            if ((sp <= 600) && (sp > 400)) label_StrPoints.Foreground = Brushes.DarkOrange;
            if ((sp <= 800) && (sp > 600)) label_StrPoints.Foreground = Brushes.Orange;
            if ((sp < 1000) && (sp > 800)) label_StrPoints.Foreground = Brushes.Green;
            if (sp == 1000) label_StrPoints.Foreground = Brushes.DarkGreen;
            //учитывается перегрузка и уменьшается здоровье + окрашивание
            if (ol >= 5)
            {
                if (ol < 8)
                {
                    label_HP.Foreground= Brushes.Orange;
                    label_OverLoad.Foreground = Brushes.Orange;
                    hp -= 1;
                }
                if ((ol >= 8) && (ol < 11))
                {
                    label_HP.Foreground = Brushes.DarkOrange;
                    label_OverLoad.Foreground = Brushes.DarkOrange;
                    hp -= 2;
                }
                if ((ol >= 11) && (ol < 14))
                {
                    label_HP.Foreground = Brushes.Red;
                    label_OverLoad.Foreground = Brushes.Red;
                    hp -= 3;
                }
                if (ol >= 14)
                {
                    label_HP.Foreground = Brushes.DarkRed;
                    label_OverLoad.Foreground = Brushes.DarkRed;
                    hp -= 4;
                }
            }
            else
            {
                label_HP.Foreground = Brushes.DarkGreen;
                label_OverLoad.Foreground = Brushes.DarkGreen;
            }
            if (hp <= 0)
            {
                hp = 0;
                image2.Visibility = Visibility.Visible;
                button2.Background = Brushes.LightGreen;
            }
            //расчет количества топлива в последней ступени
            double mfs;
            if (ks > 1)
            {
                double s1 = 0;
                for (int i = 0; i < ks - 1; i++)
                {
                    s1 += fs0[i];
                }
                mfs = mf - s1;
                if (mfs < 0)
                {
                    mfs = 0;
                    button3.Background = Brushes.LightGreen;
                }
            }
            else mfs = mf;
            //вывод значений на экран
            label_FuelStage.Content = Mod.Mass(mfs);
            label_TimeOver.Content = Mod.Time(tp);            
            label_DistAst.Content = Mod.Dist(d2);
            label_DistEarth.Content = Mod.Dist(d1);
            label_RocketSpeed.Content = Mod.Vel(v1);
            label_RocketAcc.Content = Mod.Acc(a0);
            label_FuelMass.Content = Mod.Mass(mf);
            label_HP.Content = Mod.Hp(hp);
            label_OverLoad.Content = Mod.OL(ol);
            label_FuelUp.Content = Mod.Cons(vc3);
            label_FuelDown.Content = Mod.Cons(vc2);
            label_RocketMass.Content = Mod.Mass(mr);
            label_FuelSpeed.Content = Mod.Vel(vf);
            label_MaxFuelOut.Content = Mod.Cons(vc);
            label_StagesCount.Content = Convert.ToString(ks);
            label_StrPoints.Content = Mod.Hp(sp);
            v0 = v1; v1 = 0;
        }
        private void button_Start_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                switch (comboBox_Dist.SelectedIndex)
                {
                    case 0:
                        da = Convert.ToDouble(textBox_DistAst.Text);
                        break;
                    case 1:
                        da = Convert.ToDouble(textBox_DistAst.Text) * 1000;
                        break;
                    case 2:
                        da = Convert.ToDouble(textBox_DistAst.Text) * 1e6;
                        break;
                    case 3:
                        da = Convert.ToDouble(textBox_DistAst.Text) * c * 500;
                        break;
                    case 4:
                        da = Convert.ToDouble(textBox_DistAst.Text) * c;
                        break;
                    case 5:
                        da = Convert.ToDouble(textBox_DistAst.Text) * c * 60;
                        break;
                    case 6:
                        da = Convert.ToDouble(textBox_DistAst.Text) * c * 3600;
                        break;
                    case 7:
                        da = Convert.ToDouble(textBox_DistAst.Text) * c * 3600 * 24;
                        break;
                    case 8:
                        da = Convert.ToDouble(textBox_DistAst.Text) * c * 3600 * 24 * 365;
                        break;
                }
                hp = 1000; sp = 1000;
                double ind1 = 1, ind2 = 1;
                switch (comboBox_RocketMass.SelectedIndex)
                {
                    case 0:
                        ind1 = 1e-3;
                        break;
                    case 1:
                        ind1 = 1;
                        break;
                    case 2:
                        ind1 = 1e3;
                        break;
                    case 3:
                        ind1 = 1e6;
                        break;
                }
                switch (comboBox_FuelMass.SelectedIndex)
                {
                    case 0:
                        ind2 = 1e-3;
                        break;
                    case 1:
                        ind2 = 1;
                        break;
                    case 2:
                        ind2 = 1e3;
                        break;
                    case 3:
                        ind2 = 1e6;
                        break;
                }
                switch (comboBox_FuelSpeed.SelectedIndex)
                {
                    case 0:
                        vf = Convert.ToDouble(textBox_FuelSpeed.Text);
                        break;
                    case 1:
                        vf = Convert.ToDouble(textBox_FuelSpeed.Text) / 3.6;
                        break;
                    case 2:
                        vf = Convert.ToDouble(textBox_FuelSpeed.Text) * 1000;
                        break;
                    case 3:
                        vf = Convert.ToDouble(textBox_FuelSpeed.Text) * c;
                        break;
                }
                switch (comboBox_FuelOut.SelectedIndex)
                {
                    case 0:
                        vc = Convert.ToDouble(textBox_MaxFuelConsuption.Text) / 1000;
                        break;
                    case 1:
                        vc = Convert.ToDouble(textBox_MaxFuelConsuption.Text);
                        break;
                    case 2:
                        vc = Convert.ToDouble(textBox_MaxFuelConsuption.Text) * 1000;
                        break;
                }
                mf = 0; mr = 0; so = false; par = false;
                string[] fs01 = textBox_FuelStages.Text.Split(' ');
                string[] rs01 = textBox_RocketStages.Text.Split(' ');
                bool pass = true;
                if (rs01.Length != fs01.Length)
                {
                    textBox_RocketStages.Background = Brushes.LightCoral;
                    textBox_FuelStages.Background = Brushes.LightCoral;
                    pass = false;
                }
                rs0 = new double[rs01.Length + 1]; rs0[0] = ind1 * Convert.ToDouble(textBox_RocketMass.Text);
                fs0 = new double[fs01.Length + 1]; fs0[0] = ind2 * Convert.ToDouble(textBox_StartFuelMass.Text);
                mr += ind1 * Convert.ToDouble(textBox_RocketMass.Text);
                mf += ind2 * Convert.ToDouble(textBox_StartFuelMass.Text);
                ks0 = rs01.Length + 1; ks = ks0;
                for (int i = 1; i < ks0; i++)
                {
                    rs0[i] = ind1 * Convert.ToDouble(rs01[i - 1]);
                    mr += ind1 * Convert.ToDouble(rs01[i - 1]);
                    fs0[i] = ind2 * Convert.ToDouble(fs01[i - 1]);
                    mf += ind2 * Convert.ToDouble(fs01[i - 1]);
                }
                m0 = mf + mr;
                cx = Convert.ToDouble(textBox_Cx.Text);
                cxp = Convert.ToDouble(textBox_CxPar.Text);
                S = Convert.ToDouble(textBox_S.Text);
                Sp = Convert.ToDouble(textBox_SPar.Text);
                R1 = R * (Convert.ToDouble(textBox_AstRadius.Text));
                M1 = M * (Convert.ToDouble(textBox_AstMass.Text));
                tp = 0;
                d1 = 0; d2 = da;
                ig = true;
                if (pass)
                {
                    if (!ts)
                    {
                        timer.Tick += new EventHandler(timerTick);
                        timer.Interval = new TimeSpan(0, 0, 0, 0, 10);
                        timer.Start();
                    }
                    else
                    {
                        timer.Start();
                    }
                    tabControl1.SelectedIndex = 1;
                }
                else
                {
                    MessageBox.Show("Неправильно введенные данные! (отмечены красным)");
                }
            }
            catch
            {
                MessageBox.Show("Неправильно введенные данные! (отмечены красным)");
            }
        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            Random o = new Random();
            textBox_RocketStages.Text = ""; textBox_FuelStages.Text = "";
            for (int i = 1; i < o.Next(2, 6); i++)
            {
                textBox_RocketStages.Text += Convert.ToString(o.Next(10, 20)) + " ";
                textBox_FuelStages.Text += Convert.ToString(o.Next(50, 100)) + " ";
            }
            textBox_RocketStages.Text += Convert.ToString(o.Next(10, 20));
            textBox_FuelStages.Text += Convert.ToString(o.Next(50, 100));
            textBox_RocketMass.Text = Convert.ToString(o.Next(10, 20));
            textBox_StartFuelMass.Text = Convert.ToString(o.Next(50, 100));
            textBox_MaxFuelConsuption.Text = Convert.ToString(o.Next(600, 1400));
            textBox_FuelSpeed.Text = Convert.ToString(o.Next(6, 25));
            textBox_DistAst.Text = Convert.ToString(Convert.ToDouble(o.Next(10, 50)) / 10);
            textBox_AstMass.Text = Convert.ToString(Convert.ToDouble(o.Next(1, 5)) / 10);
            textBox_AstRadius.Text = Convert.ToString(Convert.ToDouble(o.Next(5, 30)) / 10);
            textBox_Cx.Text = Convert.ToString(Convert.ToDouble(o.Next(10, 50)) / 100);
            textBox_S.Text = Convert.ToString(o.Next(4, 40));
            textBox_CxPar.Text = Convert.ToString(Convert.ToDouble(o.Next(6, 12)));
            textBox_SPar.Text = Convert.ToString(o.Next(60, 120) * 10);
        }

        private void textBox_RocketMass_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                double d = Convert.ToDouble(textBox_RocketMass.Text);
                textBox_RocketMass.Background = Brushes.LightGreen;
            }
            catch
            {
                if (textBox_RocketMass.Text == "") textBox_RocketMass.Background = Brushes.White;
                else textBox_RocketMass.Background = Brushes.LightCoral;
            }
        }

        private void textBox_StartFuelMass_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                double d = Convert.ToDouble(textBox_StartFuelMass.Text);
                textBox_StartFuelMass.Background = Brushes.LightGreen;
            }
            catch
            {
                if (textBox_StartFuelMass.Text == "") textBox_StartFuelMass.Background = Brushes.White;
                else textBox_StartFuelMass.Background = Brushes.LightCoral;
            }
        }

        private void textBox_MaxFuelConsuption_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                double d = Convert.ToDouble(textBox_MaxFuelConsuption.Text);
                textBox_MaxFuelConsuption.Background = Brushes.LightGreen;
            }
            catch
            {
                if (textBox_MaxFuelConsuption.Text == "") textBox_MaxFuelConsuption.Background = Brushes.White;
                else textBox_MaxFuelConsuption.Background = Brushes.LightCoral;
            }
        }

        private void textBox_FuelSpeed_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                double d = Convert.ToDouble(textBox_FuelSpeed.Text);
                textBox_FuelSpeed.Background = Brushes.LightGreen;
            }
            catch
            {
                if (textBox_FuelSpeed.Text == "") textBox_FuelSpeed.Background = Brushes.White;
                else textBox_FuelSpeed.Background = Brushes.LightCoral;
            }
        }

        private void textBox_DistAst_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                double d = Convert.ToDouble(textBox_DistAst.Text);
                textBox_DistAst.Background = Brushes.LightGreen;
            }
            catch
            {
                if (textBox_DistAst.Text == "") textBox_DistAst.Background = Brushes.White;
                else textBox_DistAst.Background = Brushes.LightCoral;
            }
        }

        private void textBox_AstRadius_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                double d = Convert.ToDouble(textBox_AstRadius.Text);
                textBox_AstRadius.Background = Brushes.LightGreen;
            }
            catch
            {
                if (textBox_AstRadius.Text == "") textBox_AstRadius.Background = Brushes.White;
                else textBox_AstRadius.Background = Brushes.LightCoral;
            }
        }

        private void textBox_AstMass_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                double d = Convert.ToDouble(textBox_AstMass.Text);
                textBox_AstMass.Background = Brushes.LightGreen;
            }
            catch
            {
                if (textBox_AstMass.Text == "") textBox_AstMass.Background = Brushes.White;
                else textBox_AstMass.Background = Brushes.LightCoral;
            }
        }

        private void button2_Click(object sender, RoutedEventArgs e)
        {
            timer.Stop(); ts = true; par = false;
            da = 0; d1 = 0; d2 = 0; hp = 0; kt = 0; tp = 0; M1 = 0; R1 = 0;
            v0 = 0; v1 = 0; v2 = 0; vf = 0; vc = 0; vc2 = 0; vc3 = 0;
            p0 = 0; p2 = 0; p3 = 0; r = 0; r1 = 0; g = 0; a0 = 0; m1 = 0;
            m2 = 0; m3 = 0; mf = 0; ol = 0; s = 0; sp = 0; S = 0; cx = 0;
            fs0 = null; rs0 = null; ks0 = 0; ks = 0;
            Sp = 0; cxp = 0;
            image1.Visibility = Visibility.Hidden;
            image2.Visibility = Visibility.Hidden;
            image3.Visibility = Visibility.Hidden;
            image4.Visibility = Visibility.Hidden;
            image5.Visibility = Visibility.Hidden;
            image6.Visibility = Visibility.Hidden;
            slider_Down.Value = 0; slider_Top.Value = 0;
            slider_Time.Value = 1;
            button2.Background = Brushes.LightGray;
            tabControl1.SelectedIndex = 0; 
        }

        private void textBox1_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                string[] d = textBox_RocketStages.Text.Split(' ');
                double[] l = new double[d.Length];
                for (int i = 0; i < d.Length; i++)
                {
                    l[i] = Convert.ToDouble(d[i]);
                }
                textBox_RocketStages.Background = Brushes.LightGreen;
            }
            catch
            {
                if (textBox_RocketStages.Text == "") textBox_RocketStages.Background = Brushes.White;
                else textBox_RocketStages.Background = Brushes.LightCoral;
            }
        }

        private void textBox2_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                string[] d = textBox_FuelStages.Text.Split(' ');
                double[] l = new double[d.Length];
                for (int i = 0; i < d.Length; i++)
                {
                    l[i] = Convert.ToDouble(d[i]);
                }
                textBox_FuelStages.Background = Brushes.LightGreen;
            }
            catch
            {
                if (textBox_FuelStages.Text == "") textBox_FuelStages.Background = Brushes.White;
                else textBox_FuelStages.Background = Brushes.LightCoral;
            }
        }

        private void button3_Click(object sender, RoutedEventArgs e)
        {
            so = true;
            button3.Background = Brushes.LightGray;
        }

        private void textBox_Cx_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                double d = Convert.ToDouble(textBox_Cx.Text);
                textBox_Cx.Background = Brushes.LightGreen;
            }
            catch
            {
                if (textBox_Cx.Text == "") textBox_Cx.Background = Brushes.White;
                else textBox_Cx.Background = Brushes.LightCoral;
            }
        }

        private void textBox_S_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                double d = Convert.ToDouble(textBox_S.Text);
                textBox_S.Background = Brushes.LightGreen;
            }
            catch
            {
                if (textBox_S.Text == "") textBox_S.Background = Brushes.White;
                else textBox_S.Background = Brushes.LightCoral;
            }
        }

        private void button4_Click(object sender, RoutedEventArgs e)
        {
            if (!image3.IsVisible)
            {
                image3.Visibility = Visibility.Visible;
                par = true;
            }
        }

        private void textBox_CxPar_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                double d = Convert.ToDouble(textBox_CxPar.Text);
                textBox_CxPar.Background = Brushes.LightGreen;
            }
            catch
            {
                if (textBox_CxPar.Text == "") textBox_CxPar.Background = Brushes.White;
                else textBox_CxPar.Background = Brushes.LightCoral;
            }
        }

        private void textBox_SPar_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                double d = Convert.ToDouble(textBox_SPar.Text);
                textBox_SPar.Background = Brushes.LightGreen;
            }
            catch
            {
                if (textBox_SPar.Text == "") textBox_SPar.Background = Brushes.White;
                else textBox_SPar.Background = Brushes.LightCoral;
            }
        }
    }
}
