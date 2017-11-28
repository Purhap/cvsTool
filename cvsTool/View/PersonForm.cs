using System;
using System.Collections.Generic;
using System.Drawing;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using cvsTool.Controllor;

namespace cvsTool.View
{
  
    public partial class PersonForm : Form
   {
        public PersonForm()
        {
            InitializeComponent();

        }

        private TextBox textBox1;
        private TextBox textBox2;
        private Button button1;
        private Button button2;
        private ProgressBar progressBar1;
        private PersonControllor _controllor;

       // delegate void AsynUpdateUI(int step);
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
            this.SuspendLayout();
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(96, 58);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(100, 21);
            this.textBox1.TabIndex = 0;
            // 
            // textBox2
            // 
            this.textBox2.Location = new System.Drawing.Point(116, 121);
            this.textBox2.Name = "textBox2";
            this.textBox2.Size = new System.Drawing.Size(100, 21);
            this.textBox2.TabIndex = 1;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(381, 45);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 2;
            this.button1.Text = "button1";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(381, 121);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(75, 23);
            this.button2.TabIndex = 3;
            this.button2.Text = "button2";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // progressBar1
            // 
            this.progressBar1.Location = new System.Drawing.Point(260, 192);
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(160, 23);
            this.progressBar1.TabIndex = 4;
            // 
            // PersonForm
            // 
            this.ClientSize = new System.Drawing.Size(502, 255);
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
            this.textBox1.Text = "2";
            this.textBox2.Text = "jacky";
            this.button1.Enabled = false;
            // updateProcess = new cvsTool.Controllor.UpdateUI(UpdataUIStatus);
            Controllor.updateProcessDelegate = new Controllor.PersonControllor.UpdateUIDelegate(updateProcessBar);
            Controllor.startGetHistroyPrice();
            
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Controllor.Model.ID = "2";
            Controllor.Model.Name = "jacky";

            Controllor.stopGetHistroyPrice();
        }

        private void updateProcessBar(int value)
        {
            if (this.progressBar1.InvokeRequired)
            {
                this.Invoke(Controllor.updateProcessDelegate, new object[] { value });
            }     
            else
            {
                this.progressBar1.Value += value;               
            }
        }
    }
}
