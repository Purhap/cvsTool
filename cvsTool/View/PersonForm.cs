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
        private Label label2;
        private Label label3;
        private Label label4;
        public Chart chartForm;
        public PersonForm view;
        
        public PersonForm()
        {
            InitializeComponent();
            view = this;

        }

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
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
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
            this.button3.Location = new System.Drawing.Point(774, 9);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(102, 32);
            this.button3.TabIndex = 7;
            this.button3.Text = "Start Simulate";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // textBox3
            // 
            this.textBox3.Location = new System.Drawing.Point(513, 74);
            this.textBox3.Multiline = true;
            this.textBox3.Name = "textBox3";
            this.textBox3.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.textBox3.Size = new System.Drawing.Size(490, 137);
            this.textBox3.TabIndex = 8;
            this.textBox3.WordWrap = false;
            // 
            // richTextBox1
            // 
            this.richTextBox1.Location = new System.Drawing.Point(522, 252);
            this.richTextBox1.Name = "richTextBox1";
            this.richTextBox1.Size = new System.Drawing.Size(481, 96);
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
            this.button5.Location = new System.Drawing.Point(901, 9);
            this.button5.Name = "button5";
            this.button5.Size = new System.Drawing.Size(102, 32);
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
            this.comboBox1.Location = new System.Drawing.Point(901, 47);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(102, 20);
            this.comboBox1.TabIndex = 13;
            this.comboBox1.Text = "3";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(511, 9);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(41, 12);
            this.label2.TabIndex = 14;
            this.label2.Text = "label2";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(511, 32);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(41, 12);
            this.label3.TabIndex = 15;
            this.label3.Text = "label3";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(511, 55);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(41, 12);
            this.label4.TabIndex = 16;
            this.label4.Text = "label4";
            // 
            // PersonForm
            // 
            this.ClientSize = new System.Drawing.Size(1038, 371);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.comboBox1);
            this.Controls.Add(this.button5);
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
                //this.Invoke(Controllor.Model.simulationHouse.simulations[1].updateTradeLogParallelDelegate, new object[] { value });
                //this.Invoke(Controllor.Model.simulationHouse.simulations[2].updateTradeLogParallelDelegate, new object[] { value });

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
            this.textBox3.Text = "Simulation Start\r\n";
            UInt16 parallelRunNb = Convert.ToUInt16(this.comboBox1.Text);
            Controllor.Model.simulationHouse = new Model.SimulationHouse(parallelRunNb, ref view);
            //for (int i = 0; i < parallelRunNb; i++)
            //{
            //    Controllor.Model.simulationHouse.simulations[i].updateCurrentStatusDelegate = new Model.Simulation.UpdateCurrentStatusDelegate(updateCurrentStatus);
            //    Controllor.Model.simulationHouse.simulations[i].updateTradeLogParallelDelegate = new Model.Simulation.UpdateTradeLogParallelDelegate(updateTradeLogParallelTextBox);
                                                                
           // }
            Controllor.startParallelSimulation(ref Controllor.Model.simulationHouse);
        }

        public void updateCurrentStatus(string value, UInt16 index)
        {
            if (this.label2.InvokeRequired)
            {                
                this.Invoke(Controllor.Model.simulationHouse.simulations[0].updateCurrentStatusDelegate, new object[] { value, index });
                this.Invoke(Controllor.Model.simulationHouse.simulations[1].updateCurrentStatusDelegate, new object[] { value, index });
                this.Invoke(Controllor.Model.simulationHouse.simulations[2].updateCurrentStatusDelegate, new object[] { value, index });
            }
            else
            {
                switch (index)
                {
                    case 0:
                        this.label2.Text = value;
                        break;
                    case 1:
                        this.label3.Text = value;
                        break;
                    case 2:
                        this.label4.Text = value;
                        break;
                }     
            }
           
        }
    }
}
