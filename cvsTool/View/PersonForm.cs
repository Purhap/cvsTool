using System;
using System.Collections.Generic;
using System.Drawing;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using cvsTool.Controllor;
using System.Runtime.InteropServices;
using cvsTool.Model;
using static cvsTool.Model.BaseLibrary;

namespace cvsTool.View
{
     public partial class PersonForm : Form
   {
        [DllImport("kernel32.dll")]
        public static extern uint SetThreadExecutionState(uint esFlags);

        //禁止休眠和睡眠
        public static void DisableStandby()
        {
            SetThreadExecutionState(ES_CONTINUOUS | ES_SYSTEM_REQUIRED | ES_DISPLAY_REQUIRED);
        }
        //允许睡醒和休眠
        public static void EnableStandby()
        {
            SetThreadExecutionState(ES_CONTINUOUS);
        }

        const uint ES_SYSTEM_REQUIRED = 0x00000001;
        const uint ES_DISPLAY_REQUIRED = 0x00000002;
        const uint ES_CONTINUOUS = 0x80000000;
        private Button button3;
        private TextBox textBox3;
        private RichTextBox richTextBox1;
        private Button button4;
        private Button button5;
        private ComboBox comboBox1;
        public Chart chartForm;
        public PersonForm view;
        private ComboBox comboBox2;
        private Label label2;
        private ComboBox comboBox3;
        private Label label3;
        private Label label4;
        private TextBox textBox4;
        private TextBox textBox5;
        private TextBox textBox6;
        private Label label5;
        private GroupBox groupBox1;
        private GroupBox groupBox2;
        private TextBox textBox16;
        private TextBox textBox17;
        private TextBox textBox18;
        private Label label20;
        private TextBox textBox12;
        private TextBox textBox13;
        private TextBox textBox14;
        private Label label16;
        private TextBox textBox8;
        private TextBox textBox9;
        private TextBox textBox10;
        private Label label12;
        private List<Label> listLabel;
        
        public PersonForm()
        {
            
            InitializeComponent();          

            view = this;
            
        }
        private UInt16 ParallelNb;
        private UInt16 simulationStartNb;
        private UInt16 simulationEndNb;
        private TextBox textBox1;
        private TextBox textBox2;
        private Button button1;
        private Button button2;
        private ProgressBar progressBar1;
        private RichTextBox logTextBox;
        private Label label1;
        private PersonControllor _controllor;

        public PersonControllor Controllor
        {
            get { return _controllor; }

            set
            {
                this._controllor = value;

                //绑定一定只能写在给Controllor赋值以后而不能写在PersonForm的构造函数中(此时Controllor还未被实例化)  
                //因为我们这里采用的是Controllor-First而不是View-First，不然Controllor.Model为null会异常  
                //将View通过构造函数注入到Controllor中的属于Controllor-First,这时候Controllor先创建  
                //将Controllor通过构造函数注入到View中的属于View-First,这时候View先创建  
                this.textBox1.DataBindings.Add("Text", Controllor.Model, "ID");
                this.textBox2.DataBindings.Add("Text", Controllor.Model, "Name");  
            }

        }

        private void InitializeComponent()
        {
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.textBox2 = new System.Windows.Forms.TextBox();
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            this.logTextBox = new System.Windows.Forms.RichTextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.button3 = new System.Windows.Forms.Button();
            this.textBox3 = new System.Windows.Forms.TextBox();
            this.richTextBox1 = new System.Windows.Forms.RichTextBox();
            this.button4 = new System.Windows.Forms.Button();
            this.button5 = new System.Windows.Forms.Button();
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.comboBox2 = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.comboBox3 = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.textBox4 = new System.Windows.Forms.TextBox();
            this.textBox5 = new System.Windows.Forms.TextBox();
            this.textBox6 = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.textBox8 = new System.Windows.Forms.TextBox();
            this.textBox9 = new System.Windows.Forms.TextBox();
            this.textBox10 = new System.Windows.Forms.TextBox();
            this.label12 = new System.Windows.Forms.Label();
            this.textBox12 = new System.Windows.Forms.TextBox();
            this.textBox13 = new System.Windows.Forms.TextBox();
            this.textBox14 = new System.Windows.Forms.TextBox();
            this.label16 = new System.Windows.Forms.Label();
            this.textBox16 = new System.Windows.Forms.TextBox();
            this.textBox17 = new System.Windows.Forms.TextBox();
            this.textBox18 = new System.Windows.Forms.TextBox();
            this.label20 = new System.Windows.Forms.Label();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(109, 89);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(100, 21);
            this.textBox1.TabIndex = 0;
            // 
            // textBox2
            // 
            this.textBox2.Location = new System.Drawing.Point(109, 123);
            this.textBox2.Name = "textBox2";
            this.textBox2.Size = new System.Drawing.Size(100, 21);
            this.textBox2.TabIndex = 1;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(337, 89);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(129, 23);
            this.button1.TabIndex = 2;
            this.button1.Text = "Get History Prices";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(337, 121);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(133, 23);
            this.button2.TabIndex = 3;
            this.button2.Text = "Open Chart";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // progressBar1
            // 
            this.progressBar1.Location = new System.Drawing.Point(109, 45);
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(160, 23);
            this.progressBar1.TabIndex = 4;
            // 
            // logTextBox
            // 
            this.logTextBox.Location = new System.Drawing.Point(109, 162);
            this.logTextBox.Name = "logTextBox";
            this.logTextBox.Size = new System.Drawing.Size(361, 185);
            this.logTextBox.TabIndex = 5;
            this.logTextBox.Text = "";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(275, 50);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(17, 12);
            this.label1.TabIndex = 6;
            this.label1.Text = "0%";
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(368, 28);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(102, 32);
            this.button3.TabIndex = 7;
            this.button3.Text = "Start Simulate";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // textBox3
            // 
            this.textBox3.Location = new System.Drawing.Point(688, 320);
            this.textBox3.Multiline = true;
            this.textBox3.Name = "textBox3";
            this.textBox3.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.textBox3.Size = new System.Drawing.Size(444, 89);
            this.textBox3.TabIndex = 8;
            this.textBox3.WordWrap = false;
            // 
            // richTextBox1
            // 
            this.richTextBox1.Location = new System.Drawing.Point(688, 423);
            this.richTextBox1.Name = "richTextBox1";
            this.richTextBox1.Size = new System.Drawing.Size(444, 96);
            this.richTextBox1.TabIndex = 9;
            this.richTextBox1.Text = "";
            // 
            // button4
            // 
            this.button4.Location = new System.Drawing.Point(215, 121);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(116, 23);
            this.button4.TabIndex = 10;
            this.button4.Text = "Clean DataTable";
            this.button4.UseVisualStyleBackColor = true;
            this.button4.Click += new System.EventHandler(this.button4_Click);
            // 
            // button5
            // 
            this.button5.Location = new System.Drawing.Point(271, 20);
            this.button5.Name = "button5";
            this.button5.Size = new System.Drawing.Size(167, 53);
            this.button5.TabIndex = 11;
            this.button5.Text = "Start Parallel Simulation";
            this.button5.UseVisualStyleBackColor = true;
            this.button5.Click += new System.EventHandler(this.button5_Click);
            // 
            // comboBox1
            // 
            this.comboBox1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Items.AddRange(new object[] {
            "1",
            "2",
            "3",
            "4",
            "5",
            "6",
            "7",
            "8",
            "9",
            "10",
            "11",
            "12",
            "13",
            "14",
            "15",
            "16"});
            this.comboBox1.Location = new System.Drawing.Point(185, 55);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(51, 20);
            this.comboBox1.TabIndex = 13;
            this.comboBox1.Text = "3";
            // 
            // comboBox2
            // 
            this.comboBox2.FormattingEnabled = true;
            this.comboBox2.Items.AddRange(new object[] {
            "1",
            "2",
            "3",
            "4",
            "5",
            "6",
            "7",
            "8"});
            this.comboBox2.Location = new System.Drawing.Point(184, 25);
            this.comboBox2.Name = "comboBox2";
            this.comboBox2.Size = new System.Drawing.Size(51, 20);
            this.comboBox2.TabIndex = 14;
            this.comboBox2.Text = "2";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(42, 28);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(119, 12);
            this.label2.TabIndex = 15;
            this.label2.Text = "Max Parallel Thread";
            // 
            // comboBox3
            // 
            this.comboBox3.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.comboBox3.FormattingEnabled = true;
            this.comboBox3.Items.AddRange(new object[] {
            "1",
            "2",
            "3",
            "4",
            "5",
            "6",
            "7",
            "8",
            "9",
            "10",
            "11",
            "12",
            "13",
            "14",
            "15",
            "16"});
            this.comboBox3.Location = new System.Drawing.Point(84, 55);
            this.comboBox3.Name = "comboBox3";
            this.comboBox3.Size = new System.Drawing.Size(56, 20);
            this.comboBox3.TabIndex = 16;
            this.comboBox3.Text = "0";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(41, 59);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(29, 12);
            this.label3.TabIndex = 17;
            this.label3.Text = "From";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(162, 59);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(17, 12);
            this.label4.TabIndex = 18;
            this.label4.Text = "To";
            // 
            // textBox4
            // 
            this.textBox4.Location = new System.Drawing.Point(87, 18);
            this.textBox4.Name = "textBox4";
            this.textBox4.Size = new System.Drawing.Size(45, 21);
            this.textBox4.TabIndex = 19;
            this.textBox4.Text = "1";
            this.textBox4.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // textBox5
            // 
            this.textBox5.Location = new System.Drawing.Point(179, 18);
            this.textBox5.Name = "textBox5";
            this.textBox5.Size = new System.Drawing.Size(45, 21);
            this.textBox5.TabIndex = 20;
            this.textBox5.Text = "10";
            this.textBox5.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // textBox6
            // 
            this.textBox6.Location = new System.Drawing.Point(271, 18);
            this.textBox6.Name = "textBox6";
            this.textBox6.Size = new System.Drawing.Size(45, 21);
            this.textBox6.TabIndex = 21;
            this.textBox6.Text = "1";
            this.textBox6.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(40, 22);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(17, 12);
            this.label5.TabIndex = 23;
            this.label5.Text = "K1";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.textBox16);
            this.groupBox1.Controls.Add(this.textBox17);
            this.groupBox1.Controls.Add(this.textBox18);
            this.groupBox1.Controls.Add(this.label20);
            this.groupBox1.Controls.Add(this.textBox12);
            this.groupBox1.Controls.Add(this.textBox13);
            this.groupBox1.Controls.Add(this.textBox14);
            this.groupBox1.Controls.Add(this.label16);
            this.groupBox1.Controls.Add(this.textBox8);
            this.groupBox1.Controls.Add(this.textBox9);
            this.groupBox1.Controls.Add(this.textBox10);
            this.groupBox1.Controls.Add(this.label12);
            this.groupBox1.Controls.Add(this.textBox5);
            this.groupBox1.Controls.Add(this.textBox4);
            this.groupBox1.Controls.Add(this.textBox6);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Location = new System.Drawing.Point(688, 127);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(438, 175);
            this.groupBox1.TabIndex = 27;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Test Parameters";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.label2);
            this.groupBox2.Controls.Add(this.comboBox1);
            this.groupBox2.Controls.Add(this.button5);
            this.groupBox2.Controls.Add(this.label4);
            this.groupBox2.Controls.Add(this.comboBox2);
            this.groupBox2.Controls.Add(this.label3);
            this.groupBox2.Controls.Add(this.comboBox3);
            this.groupBox2.Location = new System.Drawing.Point(688, 10);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(444, 86);
            this.groupBox2.TabIndex = 28;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Run Settings";
            // 
            // textBox8
            // 
            this.textBox8.Location = new System.Drawing.Point(179, 56);
            this.textBox8.Name = "textBox8";
            this.textBox8.Size = new System.Drawing.Size(45, 21);
            this.textBox8.TabIndex = 28;
            this.textBox8.Text = "0.0010";
            this.textBox8.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // textBox9
            // 
            this.textBox9.Location = new System.Drawing.Point(87, 56);
            this.textBox9.Name = "textBox9";
            this.textBox9.Size = new System.Drawing.Size(45, 21);
            this.textBox9.TabIndex = 27;
            this.textBox9.Text = "0.0001";
            this.textBox9.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // textBox10
            // 
            this.textBox10.Location = new System.Drawing.Point(271, 56);
            this.textBox10.Name = "textBox10";
            this.textBox10.Size = new System.Drawing.Size(45, 21);
            this.textBox10.TabIndex = 29;
            this.textBox10.Text = "0.0001";
            this.textBox10.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(40, 60);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(17, 12);
            this.label12.TabIndex = 31;
            this.label12.Text = "K2";
            // 
            // textBox12
            // 
            this.textBox12.Location = new System.Drawing.Point(179, 94);
            this.textBox12.Name = "textBox12";
            this.textBox12.Size = new System.Drawing.Size(45, 21);
            this.textBox12.TabIndex = 36;
            this.textBox12.Text = "0.0010";
            this.textBox12.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // textBox13
            // 
            this.textBox13.Location = new System.Drawing.Point(87, 94);
            this.textBox13.Name = "textBox13";
            this.textBox13.Size = new System.Drawing.Size(45, 21);
            this.textBox13.TabIndex = 35;
            this.textBox13.Text = "0.0001";
            this.textBox13.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // textBox14
            // 
            this.textBox14.Location = new System.Drawing.Point(271, 94);
            this.textBox14.Name = "textBox14";
            this.textBox14.Size = new System.Drawing.Size(45, 21);
            this.textBox14.TabIndex = 37;
            this.textBox14.Text = "0.0001";
            this.textBox14.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // label16
            // 
            this.label16.AutoSize = true;
            this.label16.Location = new System.Drawing.Point(40, 98);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(17, 12);
            this.label16.TabIndex = 39;
            this.label16.Text = "K3";
            // 
            // textBox16
            // 
            this.textBox16.Location = new System.Drawing.Point(179, 132);
            this.textBox16.Name = "textBox16";
            this.textBox16.Size = new System.Drawing.Size(45, 21);
            this.textBox16.TabIndex = 44;
            this.textBox16.Text = "1";
            this.textBox16.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // textBox17
            // 
            this.textBox17.Location = new System.Drawing.Point(87, 132);
            this.textBox17.Name = "textBox17";
            this.textBox17.Size = new System.Drawing.Size(45, 21);
            this.textBox17.TabIndex = 43;
            this.textBox17.Text = "1";
            this.textBox17.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // textBox18
            // 
            this.textBox18.Location = new System.Drawing.Point(271, 132);
            this.textBox18.Name = "textBox18";
            this.textBox18.Size = new System.Drawing.Size(45, 21);
            this.textBox18.TabIndex = 45;
            this.textBox18.Text = "1";
            this.textBox18.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // label20
            // 
            this.label20.AutoSize = true;
            this.label20.Location = new System.Drawing.Point(40, 136);
            this.label20.Name = "label20";
            this.label20.Size = new System.Drawing.Size(17, 12);
            this.label20.TabIndex = 47;
            this.label20.Text = "K4";
            // 
            // PersonForm
            // 
            this.ClientSize = new System.Drawing.Size(1170, 527);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.button4);
            this.Controls.Add(this.richTextBox1);
            this.Controls.Add(this.textBox3);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.logTextBox);
            this.Controls.Add(this.progressBar1);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.textBox2);
            this.Controls.Add(this.textBox1);
            this.Name = "PersonForm";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        private void button1_Click(object sender, EventArgs e)
        {
            DisableStandby();
            Controllor.updateProcessDelegate = new Controllor.PersonControllor.UpdateUIDelegate(updateProcessBar);
            Controllor.updateLogDelegate = new Controllor.PersonControllor.UpdateLogDelegate(updateLogTextBox);
            Controllor.startGetHistroyPrice();
            
        }

        private void button2_Click(object sender, EventArgs e)
        {
            chartForm = new Chart(this, _controllor);
            chartForm.Show();
            
        }

        private void updateProcessBar(int value, int count)
        {
            if (this.progressBar1.InvokeRequired)
            {
                this.Invoke(Controllor.updateProcessDelegate, new object[] { value, count });
            }     
            else
            {
                this.progressBar1.Value = value;
                this.label1.Text = string.Format("{0}%  tick:{1}", value, count);           
            }
        }

        private void updateLogTextBox(string value)
        {
            if (this.logTextBox.InvokeRequired)
            {
                this.Invoke(Controllor.updateLogDelegate, new object[] { value });
            }
            else
            {
                this.logTextBox.Text += value;
                this.logTextBox.Select(this.logTextBox.TextLength, 0);//光标定位到文本最后
                this.logTextBox.ScrollToCaret();
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            DisableStandby();
            Controllor.Model.simulator.updateCurrentTickDelegate = new Model.Simulator.UpdateCurrentTickDelegate(updateCurrentTick);
            Controllor.Model.simulator.updateTradeLogDelegate = new Model.Simulator.UpdateTradeLogDelegate(updateTradeLogTextBox);
            Controllor.startSimulate();
        }

        private void updateCurrentTick(string value)
        {
            if (this.textBox3.InvokeRequired)
            {
                this.Invoke(Controllor.Model.simulator.updateCurrentTickDelegate, new object[] { value });
               
            }
            else
            {
                this.textBox3.Text += value;
                this.textBox3.Select(this.textBox3.TextLength, 0);//光标定位到文本最后
                this.textBox3.ScrollToCaret();

            }

        }


        private void updateTradeLogTextBox(string value)
        {
            if (this.richTextBox1.InvokeRequired)
            {
                this.Invoke(Controllor.Model.simulator.updateTradeLogDelegate, new object[] { value });
            }
            else
            {
                this.richTextBox1.Text += value;
                this.richTextBox1.Select(this.richTextBox1.TextLength, 0);//光标定位到文本最后
                this.richTextBox1.ScrollToCaret();
            }
           
        }
        public void updateTradeLogParallelTextBox(string value)
        {
            if (this.textBox3.InvokeRequired)
            {              
                this.Invoke(Controllor.Model.simulationHouse.simulations[0].updateTradeLogParallelDelegate, new object[] { value });
            }
            else
            {
                this.textBox3.Text += value;
                this.textBox3.Select(this.richTextBox1.TextLength, 0);//光标定位到文本最后
                this.textBox3.ScrollToCaret();
            }

        }
        

        private void button4_Click(object sender, EventArgs e)
        {
            DateTime d1 = DateTime.Now;
            Model.Csv.cleanfile("EUR2USD.csv", "EUR2USD1.csv");
            DateTime d2 = DateTime.Now;
            string ss;
            ss = string.Format("step1:{0:HH:mm:ss} ", d1);
            ss +=string.Format("step2:{0:HH:mm:ss} ", d2);
            MessageBox.Show(ss);

        }

        private void button5_Click(object sender, EventArgs e)
        {
            DisableStandby();
            ParallelNb = Convert.ToUInt16(this.comboBox2.Text);

            if (listLabel != null)
            {
                foreach (Label l in listLabel)
                {
                    this.Controls.Remove(l);
                }
            }

            listLabel = new List<Label>(ParallelNb);
                            
            TestParamsRange tp = new TestParamsRange();
            tp.k1 = Convert.ToUInt16(this.textBox4.Text);
            tp.k1_End = Convert.ToUInt16(this.textBox5.Text);
            tp.k1_Step = Convert.ToUInt16(this.textBox6.Text);
            tp.k2 = Convert.ToDouble(this.textBox9.Text);
            tp.k2_End = Convert.ToDouble(this.textBox8.Text);
            tp.k2_Step = Convert.ToDouble(this.textBox10.Text);

            tp.k3 = Convert.ToDouble(this.textBox13.Text);
            tp.k3_End = Convert.ToDouble(this.textBox12.Text);
            tp.k3_Step = Convert.ToDouble(this.textBox14.Text);

            tp.k4 = Convert.ToDouble(this.textBox17.Text);
            tp.k4_End = Convert.ToDouble(this.textBox16.Text);
            tp.k4_Step = Convert.ToDouble(this.textBox18.Text);

            UInt32 totalNum = Convert.ToUInt32((tp.k1_End - tp.k1) / tp.k1_Step);
            totalNum *= Convert.ToUInt32((tp.k2_End - tp.k2) / tp.k2_Step);
            totalNum *= Convert.ToUInt32((tp.k3_End - tp.k3) / tp.k3_Step);

            simulationStartNb = Convert.ToUInt16(this.comboBox3.Text);
            simulationEndNb = Convert.ToUInt16(this.comboBox1.Text);
            if ((simulationStartNb >= totalNum) || (simulationStartNb < 0) || (simulationEndNb > totalNum)||(simulationStartNb >= simulationEndNb))
            {
                MessageBox.Show("Start or End Parameter error!");
                return;
            }
            for (int i = 0; i< ParallelNb; i++)
            {
                Label lb = new Label();
                lb.Text = string.Format("Thread {0}:", i);
                lb.Location = new Point(500, 30 + 15 * i);
                lb.Size = new System.Drawing.Size(150, 10);
                listLabel.Add(lb);
                this.Controls.Add(listLabel[i]);                
            }
            
            this.textBox3.Text += string.Format("{0:yyyy-MM-dd HH:mm:ss}", DateTime.Now)+" Simulation Start\r\n";
            this.textBox3.Text += "TradeTimes, Profits, Good, Bad, K1, K2, K3, Thread ID, Time Cost, File Name\r\n";
            Controllor.Model.simulationHouse = new Model.SimulationHouse(ParallelNb, simulationStartNb, simulationEndNb,tp,ref view);
 
            Controllor.startParallelSimulation(ref Controllor.Model.simulationHouse);           
        }

        public void updateCurrentStatus(string value, UInt16 index)
        {
            UInt16 labelIndex = Convert.ToUInt16( index % ParallelNb);
            if (this.listLabel[labelIndex].InvokeRequired)
            {
                for (int i = 0; i < Controllor.Model.simulationHouse.currentThreadNum; i++)
                {
                   this.Invoke(Controllor.Model.simulationHouse.simulations[i].updateCurrentStatusDelegate, new object[] { value, labelIndex });
                }                               
            }
            else
            {
                listLabel[labelIndex].Text = value;           
            }
           
        }
    }
}
